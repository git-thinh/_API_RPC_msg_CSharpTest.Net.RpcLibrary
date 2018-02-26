using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Threading;

namespace Dom
{
    public static class wsHTTP
    {
        public static void ProcessRequest(this Socket socket, string info, Dictionary<string, string> header)
        {
            string[] a = info.Split(' ');
            if (a.Length == 3)
            {
                string path = a[1].Substring(1);
                switch (path)
                {
                    case "login":
                        break;
                    case "message":
                        socket.Response_SSE();
                        break;
                    default:
                        if (path == string.Empty) path = "_index.html";
                        if (path.Length < 4) return;
                        string ext = path.Substring(path.Length - 3, 3);
                        switch (ext)
                        {
                            case "tml":
                                socket.Response_File(path, "text/html");
                                break;
                            case "css":
                                socket.Response_File(path, "text/css");
                                break;
                            case ".js":
                                socket.Response_File(path, "text/javascript");
                                break;
                            case "ttf":
                                socket.Response_Font(path, "application/x-font-ttf");
                                break;
                            case "ico":
                                socket.Response_ICO();
                                break;
                        }
                        break;
                }
            }
        }

        static void Response_ICO(this Socket socket)
        {
            string s =
                "HTTP/1.1 200 OK\r\n" +
                "Content-Type: image/x-icon\r\n" +
                "Content-Length: 0\r\n" +
                "\r\n";
            byte[] buf = Encoding.UTF8.GetBytes(s);
            var stream = new NetworkStream(socket);
            stream.Write(buf, 0, buf.Length);
            stream.Close();
        }

        static void Response_File(this Socket socket, string file_name, string content_type)
        {
            string data = ResourceHelper.GetEmbeddedResource("Asset/" + file_name);
            string len = Encoding.UTF8.GetBytes(data).Length.ToString();

            var stream = new NetworkStream(socket);
            string s =
                "HTTP/1.1 200 OK\r\n" +
                "Content-Type: " + content_type + "\r\n" +
                "Content-Length: " + len + "\r\n" +
                "\r\n" + data;

            byte[] buf = Encoding.UTF8.GetBytes(s);
            stream.Write(buf, 0, buf.Length);
            stream.Close();
        }

        static void Response_Font(this Socket socket, string file_name, string content_type)
        {
            var assembly = Assembly.GetExecutingAssembly();
            byte[] data = ResourceHelper.GetBytes("Asset/" + file_name);
            string len = data.Length.ToString();

            var stream = new NetworkStream(socket);
            string s =
                "HTTP/1.1 200 OK\r\n" +
                "Content-Type: " + content_type + "\r\n" +
                "Content-Length: " + len + "\r\n" +
                "\r\n";

            byte[] buf = Encoding.UTF8.GetBytes(s);
            stream.Write(buf, 0, buf.Length);

            stream.Write(data, 0, data.Length);

            stream.Close();
        }

        static void Response_SSE(this Socket socket)
        {
            string data = DateTime.Now.ToString();
            string len = Encoding.UTF8.GetBytes(data).Length.ToString();
            var stream = new NetworkStream(socket);
            string s =
                "HTTP/1.1 200 OK\r\n" +
                "Content-Type: text/event-stream\r\n" +
                "Content-Length: " + len + "\r\n" +
                "\r\n" + data;
            byte[] buf = Encoding.UTF8.GetBytes(s);
            stream.Write(buf, 0, buf.Length);
            stream.Flush();
            Thread.Sleep(3000);
            //stream.Close();
        }




























        static void Response_Home(this Socket socket, Dictionary<string, string> header)
        {
            var stream = new NetworkStream(socket);
            string s =
                "HTTP/1.1 200 OK\r\n" +
                "Content-Type: application/json\r\n" +
                "Content-Length: 15\r\n" +
                "\r\n" +
                "{\"test\":\"test\"}";

            byte[] buf = Encoding.UTF8.GetBytes(s);
            stream.Write(buf, 0, buf.Length);
            stream.Close();
        }

        static void Response_Demo_Socket(this Socket socket, Dictionary<string, string> header)
        {
            string data = ResourceHelper.GetEmbeddedResource("Asset/ws.html");
            var stream = new NetworkStream(socket);
            string s =
                "HTTP/1.1 200 OK\r\n" +
                "Content-Type: text/html\r\n" +
                "Content-Length: " + data.Length + "\r\n" +
                "\r\n" +
                data;

            byte[] buf = Encoding.UTF8.GetBytes(s);
            stream.Write(buf, 0, buf.Length);
            stream.Close();
        }






    }
}
