using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Linq;
using System.Threading;

namespace db
{
    unsafe public class Store1
    {
        const string key_ = "_key";
        const string index_ = "_index";
        const int header_ = (1024 * 1024 * 1); // 1MB
        const int capacity_ = 1000;

        #region // VARIABLE ...

        static oColumn[] column = new oColumn[] { };

        static MemoryMappedFile MMF;

        static readonly object lock_ = new object();
        static Dictionary<int, IntPtr> dicIndex = new Dictionary<int, IntPtr>() { };
        static List<long> listKEY = new List<long>() { };
        static Dictionary<long, object> dicKeyStore = new Dictionary<long, object>() { };

        int Index_Last = 0;
        
        int Capacity = 0;
        int Count_Item = 0;

        string path_File = string.Empty;
        string FileName = string.Empty;
        int TotalSize = 0;

        int ItemSize = 0;
        Type ItemType;
        string ItemTypeName;

        public int Count { get { return Count_Item; } }

        #endregion

        #region // STORE: CONTRACTOR, INIT, RESET, CLEAR, DROP ...

        public Store1(Type type, string src)
        {
            store_Init(type, src);
        }

        private void store_Init(Type type, string src = "")
        {
            bool load_Data = false;
            if (!string.IsNullOrEmpty(src)) column_Init(src);
            if (type == null) return;

            #region // ATRIBUTE: MMF, ItemType, ItemTypeName, ItemSize, FileName, Capacity, TotalSize...

            ItemType = type;
            ItemTypeName = type.Name;
            ItemSize = Marshal.SizeOf(type);
            FileName = type.Name;
            path_File = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, FileName + ".dat");

            if (!File.Exists(path_File))
            {
                Capacity = capacity_;
                TotalSize = header_ + (Capacity * ItemSize);
                file_CreateNew();
            }
            else
            {
                FileInfo fi = new FileInfo(path_File);
                TotalSize = (int)fi.Length;
                long size = TotalSize - (header_ + 1);
                if (size > 0 && size % ItemSize == 0)
                {
                    Capacity = (int)(size / ItemSize);
                    load_Data = true;
                }
            }

            index_Init_Column();

            MMF = MemoryMappedFile.Create(path_File, MapProtection.PageReadWrite, TotalSize);

            #endregion

            if (load_Data) file_Load_Init();
        }

        /// <summary>
        /// Clear all data, index. Total size file do not change. Cache in memory(RAM) clear empty.
        /// </summary>
        public void store_Clear()
        {
            file_Truncate();
            lock (lock_)
            {
                listKEY.Clear();
                dicKeyStore.Clear();
            }
            store_Init(ItemType);
        }

        /// <summary>
        /// Clear all data, index. Delete file data current, create new file(1MB ~ 10MB). Cache in memory(RAM) clear empty.
        /// </summary>
        public void store_Drop()
        {

        }

        #endregion

        #region // COLUMN: ...

        private void column_Init(string src)
        {
            string[] a = src.Split(new string[] { "//#//" }, StringSplitOptions.None)[0]
                .Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                .Select(x => x.Trim().ToLower())
                .Where(x => x.StartsWith("public ") && x.Contains(";"))
                .Select(x => x.Replace(";", string.Empty))
                .ToArray();
            List<oColumn> ls = new List<oColumn>() { };
            for (int k = 0; k < a.Length; k++)
            {
                string[] c = a[k].Split(' ').Select(x => x.Trim()).Where(x => x != string.Empty).ToArray();
                if (c.Length == 8)
                {
                    try
                    {
                        oColumn o = default(oColumn);
                        o.Type = c[1];
                        o.Name = c[2];
                        o.Id = Convert.ToInt32(c[4]);
                        o.Offset = Convert.ToInt32(c[5]);
                        o.Size = Convert.ToInt32(c[6]);
                        o.Index = Convert.ToByte(c[7]);
                        ls.Add(o);
                    }
                    catch { }
                }
            }
            column = ls.ToArray();
        }


        #endregion

        #region // FILE: CREATE_NEW, APPEND, ...

        private void file_Truncate()
        {
            FileStream fs = new FileStream(path_File, FileMode.Truncate);
            fs.Seek(TotalSize, SeekOrigin.Begin);
            fs.WriteByte(0);
            fs.Close();
        }

        private void file_CreateNew()
        {
            FileStream fs = new FileStream(path_File, FileMode.OpenOrCreate);
            fs.Seek(TotalSize, SeekOrigin.Begin);
            fs.WriteByte(0);
            fs.Close();
        }

        private void file_Load_Init()
        {
            using (Stream view = MMF.MapView(MapAccess.FileMapRead, 0, TotalSize))
            {
                byte[] buf = new byte[4];
                view.Seek(0, SeekOrigin.Begin);
                view.Read(buf, 0, 4);
                if (buf[0] == 0) return;

                int offset = header_;
                buf = new byte[ItemSize];


                var colIndex = column.Where(x => x.Index == 1).ToArray();
                for (int index = 0; index < Capacity; index++)
                {
                    view.Seek(offset, SeekOrigin.Begin);
                    view.Read(buf, 0, ItemSize);

                    bool ok = false;
                    if (buf[0] != 0)
                    {
                        long key = BitConverter.ToInt64(buf.Take(8).ToArray(), 0);
                        if (key > 0)
                        {
                            object item = get_Item(buf);
                            if (item != null && dicKeyStore.ContainsKey(key) == false)
                            {
                                ok = true;

                                listKEY.Add(key);
                                dicKeyStore.Add(key, item);

                                index_Item_Add(index, item, colIndex);
                                Count_Item++;
                                Index_Last = index;
                            }
                        }
                    }
                    if (ok == false) listKEY.Add(0);

                    offset += ItemSize;
                }
            }
        }

        private void file_Append(int index, object o)
        {
            Byte[] buf = new Byte[ItemSize];
            IntPtr ptr = Marshal.AllocHGlobal(ItemSize);
            Marshal.StructureToPtr(o, ptr, true);
            Marshal.Copy(ptr, buf, 0, ItemSize);
            Marshal.FreeHGlobal(ptr);

            file_Append(index, buf);
        }

        private void file_Append(int index, byte[] buf, int Count_Item_ = -1)
        {
            if (Count_Item_ == -1) Count_Item_ = index + 1;

            byte[] bCount = BitConverter.GetBytes(Count_Item_);

            int offset = header_ + (index * ItemSize);
            using (Stream view = MMF.MapView(MapAccess.FileMapWrite, 0, offset + ItemSize))
            {
                view.Seek(offset, SeekOrigin.Begin);
                view.Write(buf, 0, ItemSize);

                view.Seek(0, SeekOrigin.Begin);
                view.Write(bCount, 0, 4);
            }
        }

        #endregion

        #region // INDEX ...

        private void index_Init_Column()
        {
            var cs = column.Where(x => x.Index == 1).ToArray();
            if (cs.Length > 0)
            {
                for (int i = 0; i < cs.Length; i++)
                {
                    oColumn c = cs[i];
                    IntPtr ptr = IntPtr.Zero;
                    switch (c.Type)
                    {
                        case "byte":
                            byte* p1 = (byte*)ArrayPointer.NewAndInit<byte>(Capacity);
                            ptr = (IntPtr)p1;
                            dicIndex.Add(c.Id, ptr);
                            break;
                        case "int":
                            int* p2 = (int*)ArrayPointer.NewAndInit<int>(Capacity);
                            ptr = (IntPtr)p2;
                            dicIndex.Add(c.Id, ptr);
                            break;
                        case "long":
                            long* p3 = (long*)ArrayPointer.NewAndInit<long>(Capacity);
                            ptr = (IntPtr)p3;
                            dicIndex.Add(c.Id, ptr);
                            break;
                    }
                }
            }
        }

        private void index_Item_Add(int index, object item)
        {
            var colIndex = column.Where(x => x.Index == 1).ToArray();
            if (colIndex.Length > 0)
                index_Item_Add(index, item, colIndex);
        }

        private void index_Item_Add(int index, object item, oColumn[] colIndex)
        {
            Type ti = item.GetType();
            for (int i = 0; i < colIndex.Length; i++)
            {
                oColumn c = colIndex[i];
                object val = ti.GetField(c.Name).GetValue(item);
                if (val == null || val == string.Empty || val == "0") continue;

                IntPtr ptr = IntPtr.Zero;
                if (dicIndex.TryGetValue(c.Id, out ptr))
                {
                    switch (c.Type)
                    {
                        case "byte":
                            byte* p1 = (byte*)ptr;
                            p1[index] = Convert.ToByte(val);
                            break;
                        case "int":
                            int* p2 = (int*)ptr;
                            p2[index] = Convert.ToInt32(val);
                            break;
                        case "long":
                            long* p3 = (long*)ptr;
                            p3[index] = Convert.ToInt64(val);
                            break;
                    }
                }
            }
        }

        private void index_Item_Clear(int index)
        {
            var colIndex = column.Where(x => x.Index == 1).ToArray();
            for (int i = 0; i < colIndex.Length; i++)
            {
                oColumn c = colIndex[i];
                IntPtr ptr = IntPtr.Zero;
                if (dicIndex.TryGetValue(c.Id, out ptr))
                {
                    switch (c.Type)
                    {
                        case "byte":
                            byte* p1 = (byte*)ptr;
                            p1[index] = 0;
                            break;
                        case "int":
                            int* p2 = (int*)ptr;
                            p2[index] = 0;
                            break;
                        case "long":
                            long* p3 = (long*)ptr;
                            p3[index] = 0;
                            break;
                    }
                }
            }
        }

        #endregion

        #region // ITEM (RECORD): ADD, UPDATE, REMOVE, GET ...

        public long item_Find(int index)
        {
            oColumn c = column.Where(x => x.Name == index_).SingleOrDefault();
            if (c.Name != null)
            {
                IntPtr ptr = IntPtr.Zero;
                if (dicIndex.TryGetValue(c.Id, out ptr))
                { 
                    int* p2 = (int*)ptr;
                    for (int i = 0; i <= Index_Last; i++)
                        if (p2[i] == index && listKEY[i] != 0)
                            return listKEY[i];
                }
            }
            return -1;
        }

        public long[] item_Find(oWhere w)
        {
            int index = listKEY.Count;
            List<long> ls = new List<long>() { };

            oColumn c = column.Where(x => x.Name != index_ && x.Name == w.Name.ToLower()).SingleOrDefault();
            if (c.Name != null)
            {
                IntPtr ptr = IntPtr.Zero;
                if (dicIndex.TryGetValue(c.Id, out ptr))
                {
                    switch (c.Type)
                    {
                        case "byte":
                            byte v1 = Convert.ToByte(w.Value);
                            byte* p1 = (byte*)ptr;
                            for (int i = 0; i < index; i++)
                                if (p1[i] == v1 && listKEY[i] != 0)
                                    ls.Add(listKEY[i]);
                            break;
                        case "int":
                            int v2 = Convert.ToInt32(w.Value);
                            int* p2 = (int*)ptr;
                            for (int i = 0; i < index; i++)
                                if (p2[i] == v2 && listKEY[i] != 0)
                                    ls.Add(listKEY[i]);
                            break;
                        case "long":
                            long v3 = Convert.ToInt64(w.Value);
                            long* p3 = (long*)ptr;
                            for (int i = 0; i < index; i++)
                                if (p3[i] == v3 && listKEY[i] != 0)
                                    ls.Add(listKEY[i]);
                            break;
                    }
                }
            }
            return ls.ToArray();
        }

        public object item_GetByIndex(int index)
        {
            if (index >= listKEY.Count) return null;
            object o = null;
            if (dicKeyStore.TryGetValue(listKEY[index], out o))
                return o;
            return null;
        }
        public object item_GetByIndexLast()
        {
            object o = null;
            if (dicKeyStore.TryGetValue(listKEY[Index_Last], out o))
                return o;
            return null;
        }

        public object[] item_GetTop(int top)
        {
            List<object> rs = new List<object>() { };
            if (listKEY.Count > 0)
            {
                int count = 0;
                for (int k = listKEY.Count - 1; k > -1; k--)
                {
                    object o = null;
                    if (dicKeyStore.TryGetValue(listKEY[k], out o) && o != null)
                    {
                        rs.Add(o);
                        count++;
                        if (count > top) break;
                    }
                }
            }
            return rs.ToArray();
        }
        public object[] item_Get(oWhere w)
        {
            List<object> rs = new List<object>() { };
            long[] a = item_Find(w);
            if (a.Length > 0)
            {
                for (int k = 0; k < a.Length; k++)
                {
                    object o = null;
                    if (dicKeyStore.TryGetValue(a[k], out o) && o != null)
                        rs.Add(o);
                }
            }
            return rs.ToArray();
        }

        public object[] item_Get(oWhere[] ws)
        {
            List<object> rs = new List<object>() { };
            List<long> lk = new List<long>() { };
            foreach (oWhere w in ws)
            {
                long[] a = item_Find(w);
                if (a.Length > 0) lk.AddRange(a);
            }
            if (lk.Count > 0)
            {
                long[] a = lk.Distinct().ToArray();
                for (int k = 0; k < a.Length; k++)
                {
                    object o = null;
                    if (dicKeyStore.TryGetValue(a[k], out o) && o != null)
                        rs.Add(o);
                }
            }

            return rs.ToArray();
        }

        public long item_Add(object item)
        {
            long key = 0;
            Type ti = item.GetType();
            if (item != null && ti.Name == ItemTypeName)
            {
                int index_new = listKEY.IndexOf(0);

                FieldInfo mkey = ti.GetField(key_), mindex = ti.GetField(index_);
                if (mkey != null && mindex != null)
                { 
                    mindex.SetValue(item, index_new);
                    long key_new = key_Create_Random();
                    mkey.SetValue(item, key_new);
                    lock (lock_)
                    {
                        if (dicKeyStore.ContainsKey(key_new) == false)
                        {
                            listKEY[index_new] = key_new;
                            dicKeyStore.Add(key_new, item);

                            file_Append(index_new, item);
                            index_Item_Add(index_new, item);

                            if (index_new > Index_Last) Index_Last = index_new;
                            Count_Item++;
                            key = key_new;
                        }
                    }
                }
            }
            return key;
        }

        public eItemRemove item_Remove(object item)
        {
            if (item == null) return eItemRemove.NOT_FIND;
            long key = 0;
            Type ti = item.GetType();
            if (ti.Name == ItemTypeName)
            {
                FieldInfo fk = ti.GetField(key_);
                if (fk != null)
                {
                    object kix = fk.GetValue(item);
                    if (kix != null)
                        if (long.TryParse(kix.ToString(), out key) && key > 0)
                            return item_Remove(key);
                }
            }
            return eItemRemove.REMOVE_FAIL;
        }

        public eItemRemove item_Remove(long key)
        {
            int index = listKEY.IndexOf(key);
            if (index == -1) return eItemRemove.NOT_FIND;
            if (listKEY[index] == 0) return eItemRemove.NOT_FIND;

            lock (lock_)
            {
                listKEY[index] = 0;
                if (dicKeyStore.ContainsKey(key)) dicKeyStore.Remove(key);

                file_Append(index, new byte[ItemSize], Count_Item - 1);
                index_Item_Clear(index);

                Count_Item--;
                if (index == Index_Last) Index_Last--;
            }
            return eItemRemove.REMOVE_FAIL;
        }

        #endregion

        #region // MEMORY - MARSHAL: ARRAY BYTE, ...

        private object get_Item(byte[] buf)
        {
            object item = null;
            IntPtr ptrBuf = Marshal.AllocHGlobal(ItemSize);
            Marshal.Copy(buf, 0, ptrBuf, ItemSize);
            item = Marshal.PtrToStructure(ptrBuf, ItemType);
            Marshal.FreeHGlobal(ptrBuf);
            return item;
        }

        #endregion

        #region // KEY ITEM ...

        public static long key_Create_Random(bool isNone = false)
        {
            Thread.Sleep(1);
            long key = 0;
            int rid = new Random().Next(1000, 9999);
            string id = DateTime.Now.ToString("yyMMddHHmmssfff") + (isNone ? "0000" : rid.ToString());
            long.TryParse(id, out key);
            return key;
        }

        public static bool key_Check_Is_None(string key)
        {
            return key.EndsWith("0000");
        }

        #endregion
    }
}
