using CSharpTest.Net.RpcLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace msg
{
    class Program
    {
        static void Main(string[] args)
        {
            new mRpc().Start("RpcExampleClientServer");

            // Wait until you are done...
            Console.WriteLine("Press [Enter] to exit...");
            Console.ReadLine();
        }
    }
}
