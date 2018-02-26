using CSharpTest.Net.RpcLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading;

namespace msgBotTest
{
    public class mRpcClient
    {
        static mRpcClient()
        {
            AppDomain.CurrentDomain.AssemblyResolve +=
                (sender, args) =>
                {
                    var resourceName = string.Format(@"msgBotTest.DLL.{0}.dll",
                        new AssemblyName(args.Name).Name);

                    using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                    {
                        if (stream == null) return null;

                        var assemblyData = new Byte[stream.Length];
                        stream.Read(assemblyData, 0, assemblyData.Length);

                        return Assembly.Load(assemblyData);
                    }
                };
        }

        public void demo1()
        {

            Thread.Sleep(3000);

            // The client and server must agree on the interface id to use:
            var iid = new Guid("{1B617C4B-BF68-4B8C-AE2B-A77E6A3ECEC5}");

            bool attempt = true;
            while (attempt)
            {
                attempt = false;
                // Open the connection based on the endpoint information and interface IID
                //using (var client = new RpcClientApi(iid, RpcProtseq.ncalrpc, null, "RpcExampleClientServer"))
                using (var client = new RpcClientApi(iid, RpcProtseq.ncacn_ip_tcp, null, @"18081"))
                {
                    // Provide authentication information (not nessessary for LRPC)
                    client.AuthenticateAs(RpcClientApi.Self);

                    client.Execute(new byte[1] { 0xEC });

                    // Send the request and get a response
                    try
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            var response = client.Execute(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
                            Console.WriteLine("Server response: {0}", Encoding.UTF8.GetString(response));
                        }

                        //client.Execute(new byte[0]);

                        //byte[] bytes = new byte[1 * 1024 * 1024]; //1mb in/out
                        //new Random().NextBytes(bytes);

                        //Stopwatch stopWatch = new Stopwatch();
                        //stopWatch.Start();

                        //for (int i = 0; i < 2; i++)
                        //    client.Execute(bytes);

                        //stopWatch.Stop();
                        //TimeSpan ts = stopWatch.Elapsed;
                        //string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                        //Console.WriteLine(elapsedTime + " ncalrpc-large-timming");
                    }
                    catch (RpcException rx)
                    {
                        if (rx.RpcError == RpcError.RPC_S_SERVER_UNAVAILABLE || rx.RpcError == RpcError.RPC_S_SERVER_TOO_BUSY)
                        {
                            //Use a wait handle if your on the same box...
                            Console.Error.WriteLine("Waiting for server...");
                            System.Threading.Thread.Sleep(1000);
                            attempt = true;
                        }
                        else
                            Console.Error.WriteLine(rx);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex);
                    }
                }
            }
        }
    }
}
