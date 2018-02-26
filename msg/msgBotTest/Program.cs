using System;
using System.Collections.Generic;
using System.Text;

namespace msgBotTest
{
    class Program
    {
        static void Main(string[] args)
        {
            new mRpcClient().demo1();

            // Wait until you are done...
            Console.WriteLine("Press [Enter] to exit...");
            Console.ReadLine();
        }
    }
}
