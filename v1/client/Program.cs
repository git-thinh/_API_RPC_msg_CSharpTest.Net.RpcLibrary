using System;
using System.Net.Sockets;
using System.Text;

class MyClient
{
    static void Main(string[] args)
    {
        TcpClient tcpClient;
        int port = 80;
        NetworkStream stream = null;

        tcpClient = new TcpClient("103.28.37.96", port);
        Console.WriteLine("Connection was established....");

        stream = tcpClient.GetStream();

        Byte[] response = new Byte[tcpClient.ReceiveBufferSize];
        stream.Read(response, 0, (int)tcpClient.ReceiveBufferSize);

        String returnData = Encoding.UTF8.GetString(response);
        Console.WriteLine("Server Response " + returnData);

        tcpClient.Close();
        stream.Close();

    }
}