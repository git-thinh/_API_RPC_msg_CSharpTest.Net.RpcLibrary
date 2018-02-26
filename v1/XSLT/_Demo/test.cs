using System;

using System.Runtime.InteropServices;
using System.Threading;

namespace Dom
{
      public class Test
    { 
        public static void demo2()
        {
            ////int size = Marshal.SizeOf(typeof(oTagRaw));

            ////oTagRaw o1 = (oTagRaw)new oTag(1, "A", "select", "select", "a", "a", "giá trị = Tiếng Việt");

            ////oTag e1 = (oTag)o1;

            ////byte[] b1 = o1.Serialize_Marshal();
            ////oTagRaw o11 = b1.Deserialize_Marshal<oTagRaw>();
            ////oTag e11 = (oTag)o11;

            ////oTagRaw o2 = (oTagRaw)new oTag(1, "B", "input", "check", "b", "b", "value = English");
            ////oTag e2 = (oTag)o2;

            ////byte[] b2 = o2.Serialize_Marshal();
            ////oTagRaw o22 = b2.Deserialize_Marshal<oTagRaw>();
            ////oTag e22 = (oTag)o22;

            ////cacheTag[0] = e1;
            ////cacheTag[1] = e2;

            //////cacheTag.Export_XML("data.xml");
            ////cacheTag.Export_XLST("out.xml");

            ////cacheTag.Clear();
        }

        public static void demo1()
        { 

        }



        public static void demo3()
        {

            string url = "";
            url = "ws://localhost:8080/service"; //v1
            url = "ws://127.0.0.1:9393/Laputa"; // v2
            url = "ws://127.0.0.1:8181/"; // v2
            url = "ws://127.0.0.1:8181/websession"; //v4
            url = "ws://localhost:8181/websession"; //v4
                                                    //url = "ws://103.28.37.96/"; // v 4.5.2
                                                    //url = "ws://localhost:8080/"; //v4

            //WebClient webClient = new WebClient() { Proxy = null };
            //var response2 = webClient.DownloadString("http://localhost:8181/demo.html");
            //Console.WriteLine(response2);

            using (var ws = new WebSocketSharp.WebSocket(url))
            {
                ws.OnMessage += (sender, e) =>
                {
                    Console.WriteLine("Server says: " + e.Data);
                };

                ws.Connect();

                //Console.WriteLine("enter to send ......");
                //Console.ReadKey();

                ws.Send("1");
                Thread.Sleep(10);
                ws.Send("456");
                Thread.Sleep(10);
                ws.Send("789");
                Thread.Sleep(10);

            }
        }

    }
}
