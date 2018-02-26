using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSharpTest.Net.RpcLibrary
{
    /// <summary>
    /// </summary>
    public class mFile
    {
        /// <summary>
        /// </summary>
        public static void CreateFileBlank(string file, int SizeDesired_MB) {
            //string file = @"e:\ifc.db";
            //long length_add = 1024L * 1024L * 1L; // 1MB
            long length_add = 1024L * 1024L * SizeDesired_MB; // SizeDesired_MB MB

            // create blank file of desired size (nice and quick!)
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
            fs.Seek(length_add, SeekOrigin.Begin);
            fs.WriteByte(0);
            fs.Close();
        }

    }


}
