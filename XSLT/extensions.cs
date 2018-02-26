using System;
using System.Runtime.InteropServices;
using System.Text;

//namespace System.Runtime.CompilerServices
//{
//    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly)]
//    public sealed class ExtensionAttribute : Attribute { }
//}

namespace System
{
    public static class Extensions
    {
        unsafe public static byte[] arrayByte_Unicode(this string s, int size_array_bytes)
        {
            // using System.Runtime.InteropServices
            if (s == null) return null;
            // We know that String is a sequence of UTF-16 codeunits and such codeunits are 2 bytes
            var byteCount = s.Length * 2;
            if (byteCount < size_array_bytes) byteCount = size_array_bytes;
            var bytes = new byte[byteCount];
            fixed (void* pRaw = s)
            {
                Marshal.Copy((IntPtr)pRaw, bytes, 0, byteCount);
            }
            return bytes;
        }
        


        #region // Serialize - Deserialize Marshal

        // This code requires unsafe switch but should be fast ...

        // [StructLayout(LayoutKind.Sequential, Pack = 1)]
        // public struct datastruct
        // {
        //     [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)] // Max length of string
        //     public string str;
        //     public float x;
        //     public float y;
        // }

        // datastruct info = new datastruct();
        // info.str= "hej";
        // info.x = 2;
        // info.y = 4;
        // byte[] bytearray = Serialize<datastruct>(info);

        // byte[] data = bytearray; // from above code
        // object d = Deserialize<datastruct>(data);
        // datastruct ds = (datastruct)d;

        public static Byte[] Serialize_Marshal<T>(this T msg) where T : struct
        {
            int objsize = Marshal.SizeOf(typeof(T));
            Byte[] ret = new Byte[objsize];
            IntPtr buff = Marshal.AllocHGlobal(objsize);
            Marshal.StructureToPtr(msg, buff, true);
            Marshal.Copy(buff, ret, 0, objsize);
            Marshal.FreeHGlobal(buff);
            return ret;
        }

        public static T Deserialize_Marshal<T>(this Byte[] data) where T : struct
        {
            int objsize = Marshal.SizeOf(typeof(T));
            IntPtr buff = Marshal.AllocHGlobal(objsize);
            Marshal.Copy(data, 0, buff, objsize);
            T retStruct = (T)Marshal.PtrToStructure(buff, typeof(T));
            Marshal.FreeHGlobal(buff);
            return retStruct;
        }

        // [StructLayout(LayoutKind.Sequential)]
        // public struct Demo
        // {
        //     public double X;
        //     public double Y;
        // }

        // private static void Main()
        // {
        //     Demo[] array = new Demo[2];
        //     array[0] = new Demo { X = 5.6, Y = 6.6 };
        //     array[1] = new Demo { X = 7.6, Y = 8.6 };

        //     byte[] bytes = ToByteArray(array);
        //     Demo[] array2 = FromByteArray<Demo>(bytes);
        // }

        public static byte[] Serialize_Marshal<T>(this T[] source) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(source, GCHandleType.Pinned);
            try
            {
                IntPtr pointer = handle.AddrOfPinnedObject();
                byte[] destination = new byte[source.Length * Marshal.SizeOf(typeof(T))];
                Marshal.Copy(pointer, destination, 0, destination.Length);
                return destination;
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
            }
        }

        public static T[] DeserializeArray_Marshal<T>(this byte[] source) where T : struct
        {
            T[] destination = new T[source.Length / Marshal.SizeOf(typeof(T))];
            GCHandle handle = GCHandle.Alloc(destination, GCHandleType.Pinned);
            try
            {
                IntPtr pointer = handle.AddrOfPinnedObject();
                Marshal.Copy(source, 0, pointer, source.Length);
                return destination;
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
            }
        }

        public static byte[] SerializeArray_Marshal<T>(this T[] colors)
        {
            if (colors == null || colors.Length == 0)
                return null;

            int lengthOfColor32 = Marshal.SizeOf(typeof(T));
            int length = lengthOfColor32 * colors.Length;
            byte[] bytes = new byte[length];

            GCHandle handle = default(GCHandle);
            try
            {
                handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
                IntPtr ptr = handle.AddrOfPinnedObject();
                Marshal.Copy(ptr, bytes, 0, length);
            }
            finally
            {
                if (handle != default(GCHandle))
                    handle.Free();
            }

            return bytes;
        }

        #endregion


    }
}
