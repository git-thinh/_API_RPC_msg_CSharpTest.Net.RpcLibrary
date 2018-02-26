namespace msg
{
    using CSharpTest.Net.RpcLibrary;
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Simple HTTP-based server to receive strings from the IpcClient and send back
    /// strings in response.
    /// </summary>
    public class mRpc : IDisposable
    {
        private string _localHost;
        private RpcServerApi _server;

        static mRpc()
        {
            AppDomain.CurrentDomain.AssemblyResolve +=
                (sender, args) =>
                {
                    var resourceName = string.Format(@"msg.DLL.{0}.dll",
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


        /// <summary>
        /// Start listening at 127.0.0.1:port.
        /// </summary>
        public void Start(string codeAPI)
        {
            if (_server != null) throw new Exception("Server already initialized.");


            // The client and server must agree on the interface id to use:
            var iid = new Guid("{1B617C4B-BF68-4B8C-AE2B-A77E6A3ECEC5}");

            // Create the server instance, adjust the defaults to your needs.
            _server = new RpcServerApi(iid, 100, ushort.MaxValue, allowAnonTcp: false);

            try
            {
                // Add an endpoint so the client can connect, this is local-host only:
                _server.AddProtocol(RpcProtseq.ncalrpc, codeAPI, 100);
                // If you want to use TCP/IP uncomment the following, make sure your client authenticates or allowAnonTcp is true
                _server.AddProtocol(RpcProtseq.ncacn_np, @"\pipe\" + codeAPI, 25);
                // If you want to use TCP/IP uncomment the following, make sure your client authenticates or allowAnonTcp is true
                _server.AddProtocol(RpcProtseq.ncacn_ip_tcp, @"18081", 25);

                // Add the types of authentication we will accept
                _server.AddAuthentication(RpcAuthentication.RPC_C_AUTHN_GSS_NEGOTIATE);
                _server.AddAuthentication(RpcAuthentication.RPC_C_AUTHN_WINNT);
                _server.AddAuthentication(RpcAuthentication.RPC_C_AUTHN_NONE);

                // Subscribe the code to handle requests on this event:
                _server.OnExecute += _server_OnExecute;
                //_server.OnExecute += delegate (IRpcClientInfo client, byte[] bytes) { return new byte[0]; };

                // Start Listening 
                _server.StartListening();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }



        }

        private byte[] _server_OnExecute(IRpcClientInfo client, byte[] input)
        {
            //Impersonate the caller:
            using (client.Impersonate())
            {
                var reqBody = Encoding.UTF8.GetString(input);
                Console.WriteLine("Received '{0}' from {1}", reqBody, client.ClientUser.Name);

                return Encoding.UTF8.GetBytes(
                    String.Format(
                        "Hello {0}, I received your message '{1}'.",
                        client.ClientUser.Name,
                        reqBody
                        )
                    );
            }
            ////==============================================================
            //if (input.Length > 0)
            //{
            //    msgDataEncode code = (msgDataEncode)input[0];
            //    switch (code)
            //    {
            //        case msgDataEncode.ping:  // 0
            //            break;
            //        case msgDataEncode.update_node:  // 255
            //            break;
            //        case msgDataEncode.number_byte:  // 9
            //            Console.WriteLine(input.Length);
            //            break;
            //        case msgDataEncode.string_ascii:  // 1
            //            break;
            //        case msgDataEncode.string_utf8:  // 2
            //            break;
            //        case msgDataEncode.string_base64:  // 3
            //            break;
            //        case msgDataEncode.number_decimal:  // 4
            //            break;
            //        case msgDataEncode.number_long:  // 5
            //            break;
            //        case msgDataEncode.number_double:  // 6
            //            break;
            //        case msgDataEncode.number_int:  // 8
            //            break;
            //        default:
            //            #region
            //            //Impersonate the caller:
            //            using (client.Impersonate())
            //            {
            //                var reqBody = Encoding.UTF8.GetString(input);
            //                Console.WriteLine("Received '{0}' from {1}", reqBody, client.ClientUser.Name);

            //                return Encoding.UTF8.GetBytes(
            //                    String.Format(
            //                        "Hello {0}, I received your message '{1}'.",
            //                        client.ClientUser.Name,
            //                        reqBody
            //                        )
            //                    );
            //            }
            //            #endregion
            //    }//end switch
            //}
            //return new byte[0];
        }

        /// <summary>
        /// Stop listening, free resources.
        /// </summary>
        public void Stop()
        {
            if (_server != null)
            {
                var listener = _server;
                _server = null;

                GC.Collect(0, GCCollectionMode.Forced);
                GC.WaitForPendingFinalizers();

                listener.StopListening();
            }
        }
        
        public void Dispose()
        {
            Stop();
        }
    }
}