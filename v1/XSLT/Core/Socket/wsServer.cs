using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Linq;
using System.Xml.Serialization;

namespace Dom
{
    public class wsServer
    {
        public static int Port = 9999;
        static TcpListener socketServer;
        static List<SocketClient> ClientList = new List<SocketClient>();
        static readonly object _lock = new object();

        public static void Run()
        {
            socketServer = new TcpListener(IPAddress.Any, Port);
            socketServer.Start();
            Port = ((IPEndPoint)socketServer.LocalEndpoint).Port;
            while (true)
            {
                TcpClient socketConnection = socketServer.AcceptTcpClient();
                SocketClient socketClient = new SocketClient(socketConnection);
            }
        }

        public static void CloseClient(SocketClient client)
        {
            lock (_lock)
                ClientList.Remove(client);

            client.Client.Close();
            client.Dispose();
            client = null;
            Console.WriteLine("Client Disconnected\r\n");
        }

        public static void AddClient(SocketClient client)
        {
            lock (_lock)
                ClientList.Add(client);
        }

        public static void SendBroadcast(string text)
        {
            lock (_lock)
            {
                foreach (SocketClient client in ClientList)
                {
                    if (client.Client.Connected)
                    {
                        try
                        {
                            NetworkStream l_Stream = client.Client.GetStream();
                            List<byte> lb = new List<byte>();
                            lb.Add(0x81);
                            int size = text.Length;//get the message's size
                            lb.Add((byte)size); //get the size in bytes
                            lb.AddRange(Encoding.UTF8.GetBytes(text));
                            l_Stream.Write(lb.ToArray(), 0, size + 2); //I do size+2 because we have 2 bytes plus 0x81 and (byte)size
                        }
                        catch
                        {
                            CloseClient(client);
                        }
                    }
                }
            }
        }

        public static void Send(SocketClient client, string text)
        {
            lock (_lock)
            {
                if (client.Client.Connected)
                {
                    try
                    {
                        NetworkStream l_Stream = client.Client.GetStream();
                        List<byte> lb = new List<byte>();
                        lb.Add(0x81);
                        int size = text.Length;//get the message's size
                        lb.Add((byte)size); //get the size in bytes
                        lb.AddRange(Encoding.UTF8.GetBytes(text));
                        l_Stream.Write(lb.ToArray(), 0, size + 2); //I do size+2 because we have 2 bytes plus 0x81 and (byte)size
                    }
                    catch
                    {
                        CloseClient(client);
                    }
                }
            }
        }
    }


    public class SocketClient
    {
        public TcpClient Client;
        StreamReader reader;
        StreamWriter writer;

        public SocketClient(TcpClient client)
        {
            Client = client;
            Thread clientThread = new Thread(new ThreadStart(StartClient));
            clientThread.Start();
        }

        private void StartClient()
        {
            reader = new StreamReader(Client.GetStream());
            var headers = new Dictionary<string, string>();

            int kLine = 0;
            bool IsWebSocketPacket = false;
            string line = "", url = "";
            while ((line = reader.ReadLine()) != string.Empty)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    if (line != null) line = line.Trim();
                    if (kLine == 0) url = line;

                    var tokens = line.Split(new char[] { ':' }, 2);
                    if (!string.IsNullOrEmpty(line) && tokens.Length > 1)
                    {
                        string key = tokens[0];
                        headers[key] = tokens[1].Trim();
                        if (key.Contains("WebSocket"))
                            IsWebSocketPacket = true;
                    }

                    kLine++;
                }
            }

            //if (url.Contains("/favicon.ico"))
            //{
            //    reader.Close();
            //    Client.Close();
            //    return;
            //}

            if (IsWebSocketPacket)
            {
                #region

                wsServer.AddClient(this);

                // create a writer for this client
                writer = new StreamWriter(Client.GetStream());

                String secWebSocketAccept = ComputeWebSocketHandshakeSecurityHash09(headers["Sec-WebSocket-Key"]);

                // send handshake to this client only
                writer.WriteLine("HTTP/1.1 101 Web Socket Protocol Handshake");
                writer.WriteLine("Upgrade: WebSocket");
                writer.WriteLine("Connection: Upgrade");
                writer.WriteLine("WebSocket-Origin: http://localhost:63422/");
                writer.WriteLine("WebSocket-Location: ws://localhost:8181/websession");
                writer.WriteLine("Sec-WebSocket-Accept: " + secWebSocketAccept);
                writer.WriteLine("");
                writer.Flush();

                //wsServer.SendBroadcast("New Client Connected");
                wsServer.Send(this, "LOGIN");

                Thread clientRun = new Thread(new ThreadStart(RunClient));
                clientRun.Start();
                #endregion
            }
            else
            {
                // is http request
                Socket socket = Client.Client;
                try
                {
                    socket.ProcessRequest(url, headers);

                    ////// Get stream
                    ////var networkStream = new NetworkStream(socket);

                    ////// Dummy response HTTP
                    ////String responseString =
                    ////    "HTTP/1.1 200 OK\r\n" +
                    ////    "Content-Type: application/json\r\n" +
                    ////    "Content-Length: 15\r\n" +
                    ////    "\r\n" +
                    ////    "{\"test\":\"test\"}";

                    ////// Write response
                    ////byte[] respBuffer = Encoding.UTF8.GetBytes(responseString);
                    ////networkStream.Write(respBuffer, 0, respBuffer.Length);
                    ////networkStream.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    // Disconnect and close the socket.
                    if (socket != null)
                    {
                        if (socket.Connected)
                        {
                            socket.Close();
                        }
                    }
                }
            }
        }

        public static String ComputeWebSocketHandshakeSecurityHash09(String secWebSocketKey)
        {
            const String MagicKEY = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
            String secWebSocketAccept = String.Empty;

            // 1. Combine the request Sec-WebSocket-Key with magic key.
            String ret = secWebSocketKey + MagicKEY;

            // 2. Compute the SHA1 hash
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] sha1Hash = sha.ComputeHash(Encoding.UTF8.GetBytes(ret));

            // 3. Base64 encode the hash
            secWebSocketAccept = Convert.ToBase64String(sha1Hash);

            return secWebSocketAccept;
        }


        private void RunClient()
        {
            try
            {
                NetworkStream ns;
                while (true)
                {
                    ns = Client.GetStream();
                    while (!ns.DataAvailable) ;

                    byte[] b = new byte[Client.Available];
                    ns.Read(b, 0, b.Length);

                    string tem = GetDecodedData(b, b.Length);
                    Console.WriteLine("===>" + tem);
                    //wsServer.SendBroadcast("server received " + tem);
                }
            }
            catch
            {
                Console.WriteLine("Client Disconnected\r\n");
                wsServer.CloseClient(this);
            }
        }
        public static string GetDecodedData(byte[] buffer, int length)
        {
            byte b = buffer[1];
            int dataLength = 0;
            int totalLength = 0;
            int keyIndex = 0;

            if (b - 128 <= 125)
            {
                dataLength = b - 128;
                keyIndex = 2;
                totalLength = dataLength + 6;
            }

            if (b - 128 == 126)
            {
                dataLength = BitConverter.ToInt16(new byte[] { buffer[3], buffer[2] }, 0);
                keyIndex = 4;
                totalLength = dataLength + 8;
            }

            if (b - 128 == 127)
            {
                dataLength = (int)BitConverter.ToInt64(new byte[] { buffer[9], buffer[8], buffer[7], buffer[6], buffer[5], buffer[4], buffer[3], buffer[2] }, 0);
                keyIndex = 10;
                totalLength = dataLength + 14;
            }

            if (totalLength > length)
                throw new Exception("The buffer length is small than the data length");

            byte[] key = new byte[] { buffer[keyIndex], buffer[keyIndex + 1], buffer[keyIndex + 2], buffer[keyIndex + 3] };

            int dataIndex = keyIndex + 4;
            int count = 0;
            for (int i = dataIndex; i < totalLength; i++)
            {
                buffer[i] = (byte)(buffer[i] ^ key[count % 4]);
                count++;
            }

            //string bs = string.Join(" ", buffer.Select(x => x.ToString()).ToArray());
            //Console.WriteLine("===>" + bs);

            if (buffer[buffer.Length - 2] == 3 && buffer[buffer.Length - 1] == 233)
                return "closed";

            return Encoding.ASCII.GetString(buffer, dataIndex, dataLength);
        }

        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }
    }
}
