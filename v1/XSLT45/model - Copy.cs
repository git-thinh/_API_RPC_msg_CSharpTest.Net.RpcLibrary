using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace XSLT
{
    [Serializable]
    [XmlRootAttribute("root")]
    public class TagXML
    {
        //[XmlArray("data")]
        [XmlElement(ElementName = "data")]
        public oTag[] items { set; get; }
    }

    public enum eTagStatus
    {
        OK,
        MISS_CLOSE_TAG
    }

    [Serializable]
    //[XmlType("data")]
    public class oTag
    {
        public oTag() { }

        public oTag(string _tag, eTagStatus _status, string _htm)
        {
            Status = _status;
            Tag = _tag.ToLower().Trim(); 
            Html = _htm;

            Match m = Regex.Match(_htm, @"(name(|\s+)=(|\s+)([^ ]+))", RegexOptions.IgnoreCase);
            if (m.Success)
                Name = m.Groups[1].Value.Substring(4).Trim().Substring(1).Split('>')[0].Replace(@"""", string.Empty).Trim();
            else
                Name = string.Empty;

            m = Regex.Match(_htm, @"(id(|\s+)=(|\s+)([^ ]+))", RegexOptions.IgnoreCase);
            if (m.Success)
                Id = m.Groups[1].Value.Substring(2).Trim().Substring(1).Split('>')[0].Replace(@"""", string.Empty).Trim();
            else
                Id = string.Empty;
            Item = Id.ToUpper();

            m = Regex.Match(_htm, @"(onblur(|\s+)=(|\s+)([^ ]+))", RegexOptions.IgnoreCase);
            if (m.Success)
                Onblur = m.Groups[1].Value.Substring(6).Trim().Substring(1).Trim().Substring(1).Replace(@"""", string.Empty).Trim();
            else
                Onblur = string.Empty;

            m = Regex.Match(_htm, @"(Onclick(|\s+)=(|\s+)([^ ]+))", RegexOptions.IgnoreCase);
            if (m.Success)
                Onclick = m.Groups[1].Value.Substring(7).Trim().Substring(1).Trim().Substring(1).Replace(@"""", string.Empty).Trim();
            else
                Onclick = string.Empty;

            m = Regex.Match(_htm, @"(onfocus(|\s+)=(|\s+)([^ ]+))", RegexOptions.IgnoreCase);
            if (m.Success)
                Onfocus = m.Groups[1].Value.Substring(7).Trim().Substring(1).Trim().Substring(1).Replace(@"""", string.Empty).Trim();
            else
                Onfocus = string.Empty;
            
            switch (Tag)
            {
                case "input":
                    {
                        m = Regex.Match(_htm, @"(value(|\s+)=(|\s+)([^ ]+))", RegexOptions.IgnoreCase);
                        if (m.Success)
                            Value = m.Groups[1].Value.Substring(5).Trim().Substring(1).Replace(@"""", string.Empty).Trim();
                        else
                            Value = string.Empty;

                        m = Regex.Match(_htm, @"(type(|\s+)=(|\s+)([^ ]+))", RegexOptions.IgnoreCase);
                        if (m.Success)
                            Type = m.Groups[1].Value.Substring(4).Trim().Substring(1).Replace(@"""", string.Empty).Trim();
                        else
                            Type = string.Empty;
                        break;
                    }
                case "select":
                    {
                        Type = string.Empty;

                        string tem = _htm.Replace("</select>", string.Empty);
                        int pos = tem.IndexOf(">");
                        if (pos > 0)
                            Value = tem.Substring(pos + 1).Trim();
                        else
                            Value = string.Empty;

                        break;
                    }
            }
        }

        unsafe public oTag(int _index, string _item, string _tag, string _type, string _name, string _id, string _val)
        {
            _key = 0;
            Index = _index;

            Item = _item;
            Tag = _tag;
            Type = _type;
            Name = _name;
            Id = _id;

            Value = _val;
        }

        public static explicit operator oTag(oTagRaw o)
        {
            return o.Get_Tag();
        }

        [XmlIgnore]
        public long _key { set; get; }

        [XmlIgnore]
        public int Index { set; get; }

        [XmlElement(ElementName = "item")]
        public string Item { set; get; }

        [XmlElement(ElementName = "tag")]
        public string Tag { set; get; }

        [XmlElement(ElementName = "type")]
        public string Type { set; get; }

        [XmlElement(ElementName = "name")]
        public string Name { set; get; }

        [XmlElement(ElementName = "id")]
        public string Id { set; get; }

        [XmlElement(ElementName = "onfocus")]
        public string Onfocus { set; get; }

        [XmlElement(ElementName = "onblur")]
        public string Onblur { set; get; }

        [XmlElement(ElementName = "onclick")]
        public string Onclick { set; get; }

        [XmlElement(ElementName = "value")]
        public string Value { set; get; }

        [XmlIgnore]
        public eTagStatus Status { set; get; }

        [XmlIgnore]
        public string Html { set; get; }

        public override string ToString()
        {
            return Status.ToString() + "|" + Tag + "|type=" + Type + "|id=" + Id + "|name=" + Name + "|value=" + Value + Environment.NewLine + Html;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi), Serializable]
    public unsafe struct oTagRaw
    {
        #region // Member ...

        const int char_Size_ = 50;
        const int value_Size_ = 200;

        public long _key;
        public int Index;

        public fixed byte Item[char_Size_];
        public fixed byte Tag[char_Size_];
        public fixed byte Type[char_Size_];
        public fixed byte Name[char_Size_];
        public fixed byte Id[char_Size_];

        public fixed byte ValueByte[value_Size_];

        #endregion

        #region // Contractor(); ClearAll() ...

        unsafe public static explicit operator oTagRaw(Dictionary<string, object> dicData)
        {
            oTagRaw o = default(oTagRaw);
            object val = string.Empty;
            if (dicData.TryGetValue("Value", out val))
            {
                if (val != null)
                {
                    string _val = (string)val;
                    byte[] b = _val.arrayByte_Unicode(value_Size_);
                    o.SetData(eTagMember.VALUE, b);
                }
            }
            return o;
        }

        unsafe public static explicit operator oTagRaw(oTag data)
        {
            return new oTagRaw(data);
        }

        unsafe public oTagRaw(oTag o) : this()
        {
            if (o.Item == null) o.Item = string.Empty;
            if (o.Tag == null) o.Tag = string.Empty;
            if (o.Type == null) o.Type = string.Empty;
            if (o.Name == null) o.Name = string.Empty;
            if (o.Id == null) o.Id = string.Empty;

            if (o.Value == null) o.Value = string.Empty;

            _key = o._key;
            Index = o.Index;

            if (o.Value.Length > value_Size_) o.Value = o.Value.Substring(0, value_Size_);
            byte[] b_val = o.Value.arrayByte_Unicode(value_Size_);

            if (o.Item.Length > value_Size_) o.Item = o.Item.Substring(0, value_Size_);
            if (o.Tag.Length > value_Size_) o.Tag = o.Tag.Substring(0, value_Size_);
            if (o.Type.Length > value_Size_) o.Type = o.Type.Substring(0, value_Size_);
            if (o.Name.Length > value_Size_) o.Name = o.Name.Substring(0, value_Size_);
            if (o.Id.Length > value_Size_) o.Id = o.Id.Substring(0, value_Size_);

            byte[] b_item = Encoding.ASCII.GetBytes(o.Item);
            byte[] b_tag = Encoding.ASCII.GetBytes(o.Tag);
            byte[] b_type = Encoding.ASCII.GetBytes(o.Type);
            byte[] b_name = Encoding.ASCII.GetBytes(o.Name);
            byte[] b_id = Encoding.ASCII.GetBytes(o.Id);

            fixed (byte*
                value = ValueByte,
                id = Id, item = Item, name = Name, tag = Tag, type = Type)
            {
                for (int i = 0; i < b_id.Length; i++) id[i] = b_id[i];
                for (int i = 0; i < b_item.Length; i++) item[i] = b_item[i];
                for (int i = 0; i < b_tag.Length; i++) tag[i] = b_tag[i];
                for (int i = 0; i < b_name.Length; i++) name[i] = b_name[i];
                for (int i = 0; i < b_type.Length; i++) type[i] = b_type[i];

                for (int i = 0; i < b_val.Length; i++) value[i] = b_val[i];
            }
        }

        unsafe public void ClearAll()
        {
            Index = 0;
            _key = 0;

            fixed (byte* value = ValueByte, id = Id, item = Item, name = Name, tag = Tag, type = Type)
            {
                for (int i = 0; i < char_Size_; i++) id[i] = 0;
                for (int i = 0; i < char_Size_; i++) item[i] = 0;
                for (int i = 0; i < char_Size_; i++) name[i] = 0;
                for (int i = 0; i < char_Size_; i++) tag[i] = 0;
                for (int i = 0; i < char_Size_; i++) type[i] = 0;
                for (int i = 0; i < value_Size_; i++) value[i] = 0;
            }
        }

        #endregion

        #region // Clear, SetData, GetArray, GetData ...

        public void Clear(eTagMember member)
        {
            fixed (byte* value = ValueByte, id = Id, item = Item, name = Name, tag = Tag, type = Type)
            {
                switch (member)
                {
                    case eTagMember.ID:
                        for (int i = 0; i < char_Size_; i++) id[i] = 0;
                        break;
                    case eTagMember.ITEM:
                        for (int i = 0; i < char_Size_; i++) item[i] = 0;
                        break;
                    case eTagMember.NAME:
                        for (int i = 0; i < char_Size_; i++) name[i] = 0;
                        break;
                    case eTagMember.TAG:
                        for (int i = 0; i < char_Size_; i++) tag[i] = 0;
                        break;
                    case eTagMember.TYPE:
                        for (int i = 0; i < char_Size_; i++) type[i] = 0;
                        break;
                    case eTagMember.VALUE:
                        for (int i = 0; i < value_Size_; i++) value[i] = 0;
                        break;
                }
            }
        }

        public void SetData(eTagMember member, byte[] data)
        {
            int len = data.Length;
            fixed (byte* value = ValueByte, id = Id, item = Item, name = Name, tag = Tag, type = Type)
            {
                switch (member)
                {
                    case eTagMember.ID:
                        if (len > char_Size_) len = char_Size_;
                        for (int i = 0; i < len; i++) id[i] = data[i];
                        break;
                    case eTagMember.ITEM:
                        if (len > char_Size_) len = char_Size_;
                        for (int i = 0; i < len; i++) item[i] = data[i];
                        break;
                    case eTagMember.NAME:
                        if (len > char_Size_) len = char_Size_;
                        for (int i = 0; i < len; i++) name[i] = data[i];
                        break;
                    case eTagMember.TAG:
                        if (len > char_Size_) len = char_Size_;
                        for (int i = 0; i < len; i++) tag[i] = data[i];
                        break;
                    case eTagMember.TYPE:
                        if (len > char_Size_) len = char_Size_;
                        for (int i = 0; i < len; i++) type[i] = data[i];
                        break;
                    case eTagMember.VALUE:
                        if (len > value_Size_) len = value_Size_;
                        for (int i = 0; i < len; i++) value[i] = data[i];
                        break;
                }
            }
        }

        public void SetData(eTagMember member, string data)
        {
            int len = data.Length;
            fixed (byte* value = ValueByte, id = Id, item = Item, name = Name, tag = Tag, type = Type)
            {
                switch (member)
                {
                    case eTagMember.ID:
                        break;
                    case eTagMember.ITEM:
                        break;
                    case eTagMember.NAME:
                        break;
                    case eTagMember.TAG:
                        break;
                    case eTagMember.TYPE:
                        break;
                    case eTagMember.VALUE:
                        break;
                }
            }
        }

        public string Get_Data(eTagMember member)
        {
            string data = string.Empty;
            fixed (byte* value = ValueByte, id = Id, item = Item, name = Name, tag = Tag, type = Type)
            {
                switch (member)
                {
                    case eTagMember.ID:
                        data = Marshal.PtrToStringAnsi((IntPtr)(char*)id);
                        break;
                    case eTagMember.ITEM:
                        data = Marshal.PtrToStringAnsi((IntPtr)(char*)item);
                        break;
                    case eTagMember.NAME:
                        data = Marshal.PtrToStringAnsi((IntPtr)(char*)name);
                        break;
                    case eTagMember.TAG:
                        data = Marshal.PtrToStringAnsi((IntPtr)(char*)tag);
                        break;
                    case eTagMember.TYPE:
                        data = Marshal.PtrToStringAnsi((IntPtr)(char*)type);
                        break;
                    case eTagMember.VALUE:
                        data = Marshal.PtrToStringUni((IntPtr)(char*)value);
                        break;
                }
            }
            return data;
        }

        unsafe public oTag Get_Tag()
        {
            oTag o = new oTag();
            fixed (byte* value = ValueByte, id = Id, item = Item, name = Name, tag = Tag, type = Type)
            {
                o._key = _key;
                o.Index = Index;

                o.Id = Marshal.PtrToStringAnsi((IntPtr)(char*)id);
                o.Item = Marshal.PtrToStringAnsi((IntPtr)(char*)item);
                o.Name = Marshal.PtrToStringAnsi((IntPtr)(char*)name);
                o.Tag = Marshal.PtrToStringAnsi((IntPtr)(char*)tag);
                o.Type = Marshal.PtrToStringAnsi((IntPtr)(char*)type);

                o.Value = Marshal.PtrToStringUni((IntPtr)(char*)value);
            }
            return o;
        }

        #endregion

    }

    public enum eEncoding
    {
        ASCII,
        UNICODE
    }

    public enum eTagMember
    {
        ITEM,
        TAG,
        TYPE,
        NAME,
        ID,
        VALUE
    }

    //[StructLayout(LayoutKind.Explicit, Size = 16, CharSet = CharSet.Ansi), Serializable]
    //public struct oTag
    //{
    //    [FieldOffset(0)]
    //    public long _key;

    //    [FieldOffset(8)]
    //    public int Index;

    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)] // 50
    //    [FieldOffset(12)]
    //    public string Item;

    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)] // 50
    //    [FieldOffset(62)]
    //    public string Tag;

    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)] // 50
    //    [FieldOffset(112)]
    //    public string Type;

    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)] // 50
    //    [FieldOffset(162)]
    //    public string Name;

    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)] // 50
    //    [FieldOffset(212)]
    //    public string Id;

    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)] // 200
    //    [FieldOffset(412)]
    //    public string Value;
    //}
}
