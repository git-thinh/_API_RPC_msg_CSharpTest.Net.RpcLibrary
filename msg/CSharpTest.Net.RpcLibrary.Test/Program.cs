using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpTest.Net.RpcLibrary.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = msg.BuildHeader(msgDataEncode.string_ascii, new byte[0],
                new msgSender(RpcProtseq.ncacn_ip_tcp, "11001", "table", "thinh", "12345"));


        }
    }
}
