using model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Xsl;
using System.Xml;
using System.Runtime.InteropServices;
using System.Reflection;

namespace db
{
    unsafe public class Cache2
    {
        const int header_ = (1024 * 1024 * 1); // 1MB
        const int capacity_ = 1000;
        static MemoryMappedFile map;
        static Dictionary<long, object> dicKeyStore = new Dictionary<long, object>() { };
        static Dictionary<long, int> dicKeyIndexFile = new Dictionary<long, int>() { };
        static readonly object lock_ = new object();

        int Capacity = 0;
        int Count = 0;
        int Count_OK = 0;

        string path_File = string.Empty;
        string FileName = string.Empty;
        int TotalSize = 0;
        
        int ItemSize = 0;
        Type ItemType;
        string ItemTypeName;
        
        public Cache2(Type type)
        { 
        }

        public void set_Cache(int key, object o)
        {
            lock (lock_)
            {
                if (dicKeyStore.ContainsKey(key) == false)
                {
                    dicKeyStore.Add(key, o);
                }
            }
        }

        public bool Add1(object o)
        {
            Type t = o.GetType();
            if (o != null && t.Name == ItemTypeName)
            {
                FieldInfo fd = t.GetField("_key");
                if (fd == null) return false;
                object id = fd.GetValue(o);
                if (id != null)
                {
                    long key = 0;
                    if (long.TryParse(id.ToString(), out key) & key > 0)
                    {
                        lock (lock_)
                        {
                            if (dicKeyStore.ContainsKey(key) == false)
                            {
                                dicKeyStore.Add(key, o);
                                file_Append(Count, o);
                                Count++;
                                Count_OK++;
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private void file_Append(int index, object o)
        {
            Byte[] buf = new Byte[ItemSize];
            IntPtr ptr = Marshal.AllocHGlobal(ItemSize);
            Marshal.StructureToPtr(o, ptr, true);
            Marshal.Copy(ptr, buf, 0, ItemSize);
            Marshal.FreeHGlobal(ptr);

            byte[] bCount = BitConverter.GetBytes(Count + 1);

            int offset = header_ + (index * ItemSize);
            using (Stream view = map.MapView(MapAccess.FileMapWrite, 0, offset + ItemSize))
            {
                view.Seek(offset, SeekOrigin.Begin);
                view.Write(buf, 0, ItemSize);

                view.Seek(0, SeekOrigin.Begin);
                view.Write(bCount, 0, 4);
            }
        }
    }
}
