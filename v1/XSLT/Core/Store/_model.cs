using System;
using System.Runtime.InteropServices;

namespace Dom
{
    public enum eItemRemove
    {
        NOT_FIND = 0,
        REMOVE_OK = 1,
        REMOVE_FAIL = 2
    }

    public enum eOperator
    {
        EQUAL = 1,
        EQUAL_NOT = 2
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi), Serializable]
    public struct oWhere
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string Name;

        public byte Opera;
        public byte Order;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public string Value;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi), Serializable]
    public struct oColumn
    {
        public int Id;
        public int Offset;
        public int Size;
        public byte Index;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string Name;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string Type;

        public override string ToString()
        {
            return string.Format("{0}.{1}: \t {2} \t\t {3} \t\t Size={4} \t\t Offset={5}", Id, Index, Name, Type, Size, Offset);
        }
    }
}

