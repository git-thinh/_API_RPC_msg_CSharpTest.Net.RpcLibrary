using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Xsl;
using System.Xml;

namespace XSLT
{
    unsafe public class CacheTag
    {
        private static oTagRaw* pTag;
        private static Dictionary<long, int> dicKeyIndex;
        private static readonly object lock_KeyIndex = new object();

        public int Capacity { set; get; }
        public int Count { set; get; }

        unsafe public CacheTag(int _capacity = 1000)
        {
            Capacity = _capacity;
            Count = 0;

            pTag = (oTagRaw*)ArrayPointer.NewAndInit<oTagRaw>(_capacity);
            dicKeyIndex = new Dictionary<long, int>() { };
        }

        unsafe public oTag this[int index]
        {
            get
            {
                oTag o = null;
                if (index != 0 && index < Count) o = (oTag)pTag[index];
                return o;
            }

            set
            {
                if (index >= 0 && index <= Count)
                    pTag[index] = (oTagRaw)value;
                else
                {
                    // resize array ....
                    pTag = (oTagRaw*)ArrayPointer.Resize<oTagRaw>(pTag, 1);
                    pTag[index] = (oTagRaw)value;
                    lock (lock_KeyIndex)
                    {
                        if (dicKeyIndex.ContainsKey(value._key))
                            dicKeyIndex[value._key] = index;
                        else
                            dicKeyIndex.Add(value._key, index);
                    }
                }
                Count++;
                if (Count > Capacity) Capacity = Count;
            }
        }

        unsafe public void Remove(long key)
        {
            if (dicKeyIndex.ContainsKey(key))
            {
                int index = 0;
                lock (lock_KeyIndex)
                {
                    if (dicKeyIndex.TryGetValue(key, out index))
                    {
                        dicKeyIndex.Remove(key);
                        if (index < Count)
                        {
                            pTag[index].ClearAll();
                            if (Count > 0)
                                Count--;
                        }
                    }
                }
            }
        }


        unsafe public string Export_XLST(string path_file_xslt)
        {
            List<oTag> ls = new List<oTag>() { };
            for (int k = 0; k < Count; k++)
            {
                oTag o = (oTag)pTag[k];
                ls.Add(o);
            }

            TagXML tx = new TagXML() { items = ls.ToArray() };

            string xml_data = string.Empty, xlst_data = string.Empty;
            
            #region // xml_data

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriter writer = XmlWriter.Create(ms, settings);

                XmlSerializerNamespaces names = new XmlSerializerNamespaces();
                names.Add("", "");
                XmlSerializer cs = new XmlSerializer(tx.GetType());
                cs.Serialize(writer, tx, names);
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);
                xml_data = sr.ReadToEnd();
            }

            #endregion

            #region // xlst_data

            XslCompiledTransform processor = new XslCompiledTransform();
            //processor.Load("t001.xslt");
            processor.Load(path_file_xslt);
            
            XmlDocument xDoc = new XmlDocument();
            xDoc.PreserveWhitespace = true;
            xDoc.LoadXml(xml_data);

            //XmlTextWriter xOutput = new XmlTextWriter(f_out_xml, null);
            //processor.Transform(xDoc, null, xOutput);
            //xOutput.Close();
            
            using (var writer = new StringWriter())
            {
                processor.Transform(xDoc, null, writer);
                xlst_data = writer.ToString();
            }

            if (!string.IsNullOrEmpty(xlst_data))
                xlst_data = xlst_data.Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", string.Empty).Trim();

            #endregion

            return xlst_data;
        }

        unsafe public void Export_XML(string file_path)
        {
            List<oTag> ls = new List<oTag>() { };
            for (int k = 0; k < Count; k++)
            {
                oTag o = (oTag)pTag[k];
                ls.Add(o);
            }

            TagXML tx = new TagXML() { items = ls.ToArray() };

            //var xns = new XmlSerializerNamespaces();
            //xns.Add(string.Empty, string.Empty);
            //XmlSerializer xml = new XmlSerializer(tx.GetType());
            //TextWriter writer = new StreamWriter(file_path);
            //xml.Serialize(writer, tx, xns);

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            MemoryStream ms = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(ms, settings);

            XmlSerializerNamespaces names = new XmlSerializerNamespaces();
            names.Add("", "");
            XmlSerializer cs = new XmlSerializer(tx.GetType());
            cs.Serialize(writer, tx, names);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(ms);
            var xml = sr.ReadToEnd();
        }

        unsafe public oTag GetByKey(long key)
        {
            oTag o = null;
            lock (lock_KeyIndex)
            {
                if (dicKeyIndex.ContainsKey(key))
                {
                    int index = 0;
                    if (dicKeyIndex.TryGetValue(key, out index))
                    {
                        if (index < Count)
                            o = (oTag)pTag[index];
                    }
                }
            }
            return o;
        }

        unsafe public oTag[] Search()
        {
            List<oTag> ls = new List<oTag>() { };
            for (int k = 0; k < Count; k++)
            {
                oTag o = (oTag)pTag[k];
                ls.Add(o);
            }
            return ls.ToArray();
        }

        public void Clear()
        {
            ArrayPointer.Free(pTag);
        }

        ~CacheTag()
        {
            ArrayPointer.Free(pTag);
        }
    }
}
