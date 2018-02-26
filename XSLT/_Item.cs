using System.Runtime.InteropServices;
namespace model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct item1
    {                                //   offset size  index
        public long _key;            // 0    0     8     0
        public byte _status;         // 1    8     1     1
        public byte _searchlevel;    // 2    9     1     1
        public int _index;           // 3    10    4     0
        public long _urlid;          // 4    14    8     1
        public byte _xpathlevel;     // 5    22    1     1

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string _item;         // 6    23    50    0
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string _tag;          // 7    73    50    1
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string _type;         // 8    123   50    1
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string _name;         // 9    173   50    1
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string _id;           // 10   223   50    1
        //273
    }
    //#//
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct item2
    {
        public long _key;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)] // 200
        public string _value;
    }
}
