using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpTest.Net.RpcLibrary
{
    /// <summary>
    /// </summary>
    //[System.Diagnostics.DebuggerNonUserCode]
    public class msg
    {
        /// <summary> 
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="data"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static byte[] BuildHeader(msgDataEncode dataType, byte[] data, msgSender sender)
        {
            int sizeHeader = 500;
            byte[] b = new byte[data.Length + sizeHeader];
            b[0] = (byte)dataType;
            
            #region // msgSender

            int posSender = 100;
            b[posSender] = (byte)sender.Authentication;
            b[posSender + 1] = (byte)sender.Protocol;

            int sizeSenderInfo = 99;
            byte[] aEndPoint = GetBytesASCII(sender.EndPoint, sizeSenderInfo);
            byte[] aCodeAPI = GetBytesASCII(sender.CodeAPI, sizeSenderInfo);
            byte[] aUsername = GetBytesASCII(sender.Username, sizeSenderInfo);
            byte[] aPassword = GetBytesASCII(sender.Password, sizeSenderInfo);
            aEndPoint.CopyTo(b, posSender + 2);
            aCodeAPI.CopyTo(b, posSender + 2 + sizeSenderInfo);
            aUsername.CopyTo(b, posSender + 2 + (sizeSenderInfo * 2));
            aPassword.CopyTo(b, posSender + 2 + (sizeSenderInfo * 3));

            #endregion

            data.CopyTo(b, sizeHeader);

            return b;
        }

        #region [ BUILDER ]

        /// <summary> 
        /// </summary>
        public static byte[] Builder(msgDataEncode dataType, string data, msgSender sender)
        {
            byte[] a = new byte[0];
            switch (dataType)
            {
                case msgDataEncode.string_utf8:
                    a = Encoding.UTF8.GetBytes(data);
                    break;
                case msgDataEncode.string_ascii:
                    a = Encoding.ASCII.GetBytes(data);
                    break;
            }
            byte[] b = BuildHeader(dataType, a, sender);
            return b;
        }

        /// <summary> 
        /// </summary>
        public static byte[] Builder(msgDataEncode dataType, decimal data, msgSender sender)
        {
            byte[] a = GetBytes(data);
            byte[] b = BuildHeader(dataType, a, sender);
            return b;
        }

        /// <summary> 
        /// </summary>
        public static byte[] Builder(msgDataEncode dataType, double data, msgSender sender)
        {
            byte[] a = BitConverter.GetBytes(data);
            byte[] b = BuildHeader(dataType, a, sender);
            return b;
        }

        /// <summary> 
        /// </summary>
        public static byte[] Builder(msgDataEncode dataType, long data, msgSender sender)
        {
            byte[] a = BitConverter.GetBytes(data);
            byte[] b = BuildHeader(dataType, a, sender);
            return b;
        }

        /// <summary> 
        /// </summary>
        public static byte[] Builder(msgDataEncode dataType, int data, msgSender sender)
        {
            byte[] a = BitConverter.GetBytes(data);
            byte[] b = BuildHeader(dataType, a, sender);
            return b;
        }

        #endregion

        #region [ FUNCTION ]


        /// <summary> 
        /// </summary>
        private static byte[] CreateMsgID()
        {
            byte[] b = new byte[100];
            string id = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1, 9999);
            byte[] a = Encoding.ASCII.GetBytes(id);
            a.CopyTo(b, 0);
            //for (int i = a.Length; i < 100; i++)
            //    b[i] = 0x00;
            return b;
        }

        /// <summary> 
        /// </summary>
        private static byte[] GetBytesASCII(string text_ASCII, int sizeByteArray)
        {
            byte[] b = new byte[sizeByteArray];
            if (string.IsNullOrEmpty(text_ASCII)) return b;
            byte[] a = Encoding.ASCII.GetBytes(text_ASCII);
            a.CopyTo(b, 0);
            //for (int i = a.Length; i < sizeByteArray; i++)
            //    b[i] = 0x00;//0xFF
            return b;
        }

        /// <summary> 
        /// </summary>
        public static byte[] GetBytes(decimal dec)
        {
            //Load four 32 bit integers from the Decimal.GetBits function
            Int32[] bits = decimal.GetBits(dec);
            //Create a temporary list to hold the bytes
            List<byte> bytes = new List<byte>();
            //iterate each 32 bit integer
            foreach (Int32 i in bits)
            {
                //add the bytes of the current 32bit integer
                //to the bytes list
                bytes.AddRange(BitConverter.GetBytes(i));
            }
            //return the bytes list as an array
            return bytes.ToArray();
        }

        /// <summary> 
        /// </summary>
        public static decimal ToDecimal(byte[] bytes)
        {
            //check that it is even possible to convert the array
            if (bytes.Length != 16)
                throw new Exception("A decimal must be created from exactly 16 bytes");

            //make an array to convert back to int32's
            Int32[] bits = new Int32[4];
            for (int i = 0; i <= 15; i += 4)
            {
                //convert every 4 bytes into an int32
                bits[i / 4] = BitConverter.ToInt32(bytes, i);
            }
            //Use the decimal's new constructor to
            //create an instance of decimal
            return new decimal(bits);
        }

        #endregion
    } //end class

    /// <summary>
    /// </summary>
    public class msgSender
    {
        /// <summary>
        /// </summary>
        public msgSender() { }

        /// <summary>
        /// </summary>
        public msgSender(RpcProtseq protocol, string endPoint, string codeAPI)
        {
            Protocol = protocol;
            EndPoint = endPoint;
            CodeAPI = codeAPI;
            Authentication = msgAuthentication.none;
        }

        /// <summary>
        /// </summary>
        public msgSender(RpcProtseq protocol, string endPoint, string codeAPI, string token)
        {
            Protocol = protocol;
            EndPoint = endPoint;
            CodeAPI = codeAPI;
            Authentication = msgAuthentication.token;
            Token = token;
        }

        /// <summary>
        /// </summary>
        public msgSender(RpcProtseq protocol, string endPoint, string codeAPI, string username, string password)
        {
            Protocol = protocol;
            EndPoint = endPoint;
            CodeAPI = codeAPI;
            Authentication = msgAuthentication.login;
            Username = username;
            Password = password;
        }

        /// <summary>
        /// </summary>
        public RpcProtseq Protocol { set; get; }

        /// <summary>
        /// </summary>
        public string EndPoint { set; get; }

        /// <summary>
        /// </summary>
        public string CodeAPI { set; get; }

        /// <summary>
        /// </summary>
        public msgAuthentication Authentication { set; get; }

        /// <summary>
        /// </summary>
        public string Username { set; get; }


        /// <summary>
        /// </summary>
        public string Password { set; get; }

        /// <summary>
        /// </summary>
        public string Token { set; get; }
    }

    /// <summary>
    /// </summary>
    public enum msgDataEncode
    {
        /// <summary>
        /// </summary>
        ping = 0,
        /// <summary>
        /// </summary>
        string_ascii = 1,
        /// <summary>
        /// </summary>
        string_utf8 = 2,
        /// <summary>
        /// </summary>
        string_base64 = 3,
        /// <summary>
        /// </summary>
        number_decimal = 4,
        /// <summary>
        /// </summary>
        number_long = 5,
        /// <summary>
        /// </summary>
        number_double = 6,
        /// <summary>
        /// </summary>
        number_int = 8,
        /// <summary>
        /// </summary>
        number_byte = 9,
        /// <summary>
        /// </summary>
        update_node = 255
    }

    /// <summary>
    /// </summary>
    public enum msgType
    {
        /// <summary>
        /// </summary>
        string_ascii = 0,
        /// <summary>
        /// </summary>
        string_utf8 = 1
    }

    /// <summary>
    /// </summary>
    public enum msgAuthentication
    {
        /// <summary>
        /// </summary>
        none = 0,
        /// <summary>
        /// </summary>
        login = 1,
        /// <summary>
        /// </summary>
        token = 2
    }

    /// <summary> 
    /// </summary>
    static partial class msgConst
    {
        //public const byte 
    }
}
