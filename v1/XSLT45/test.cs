using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace XSLT
{
    unsafe public class Test
    {
        public static void demo1(string xml_data_input = "Input.xml", string xslt_data = "Data.xslt")
        {
            string f_xml_data_output = "xml_data_output.xml";
            string f_excel_output = "excel_output.xlsx";

            XmlDocument xmlobj = new XmlDocument();
            xmlobj.Load(xml_data_input);
            XslCompiledTransform xXslt = new XslCompiledTransform();
            xXslt.Load(xslt_data);

            StringWriter sw = new StringWriter();

            xXslt.Transform(xmlobj, null, sw);
            string xml_data_output = sw.ToString();
            sw.Close();
            XmlDocument Xdoc = new XmlDocument();
            Xdoc.LoadXml(sw.ToString());
            Xdoc.Save(f_xml_data_output);
            StreamReader sr = File.OpenText(f_xml_data_output);

            string strSheetData = sr.ReadToEnd();
            ArrayList DataNode = new ArrayList();
            ArrayList FinalXML = new ArrayList();
            XmlNodeList DN;
            DN = xmlobj.DocumentElement.GetElementsByTagName("Data");
            for (int i = 0; i < DN.Count; i++)
            {
                DataNode.Add("<ShaleDataExport><Data Flag = '" + i + "' >" + DN.Item(i).InnerXml + "</Data></ShaleDataExport>");

            }
            string ShaleDataExportXML;
            int k = 0;
            while (k < DN.Count)
            {
                ShaleDataExportXML = DataNode[k].ToString();
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(ShaleDataExportXML);
                StringWriter sw1 = new StringWriter();
                xXslt.Transform(xml, null, sw1);
                FinalXML.Add(sw1);
                sw.Close();
                k++;
            }

            using (SpreadsheetDocument doc = SpreadsheetDocument.Create(f_excel_output, 
                DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbook1 = doc.AddWorkbookPart();
                string XML;
                string WorbookXML;
                WorbookXML = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?><workbook xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"" xmlns:r=""http://schemas.openxmlformats.org/officeDocument/2006/relationships""><sheets>";
                for (int j = 0; j < DN.Count; j++)
                {
                    WorksheetPart[] sheet = new WorksheetPart[DN.Count];
                    sheet[j] = workbook1.AddNewPart<WorksheetPart>();
                    string sheetId = workbook1.GetIdOfPart(sheet[j]);
                    XML = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?><worksheet xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"" >";
                    XML += FinalXML[j].ToString() + "</worksheet>";
                    string SheetXML = XML.ToString();
                    XmlDocument SXML = new XmlDocument();
                    SXML.LoadXml(SheetXML);
                    byte[] byteArray = Encoding.ASCII.GetBytes(SXML.OuterXml);
                    MemoryStream stream = new MemoryStream(byteArray);
                    StreamReader reader = new StreamReader(stream);
                    string text = reader.ReadToEnd();
                    WorbookXML += "<sheet name=" + "\"sheet" + (j + 1).ToString() + "\" " + " sheetId=\"" + (j + 1).ToString() + "\"  r:id=\"" + sheetId.ToString() + "\" />";
                    AddPartXml(sheet[j], text); 
                }
                WorbookXML += "</sheets></workbook>";
                AddPartXml(workbook1, WorbookXML);
                doc.Close();
            }

        }

        static void AddPartXml(OpenXmlPart part, string xml)
        {
            using (Stream stream = part.GetStream())
            {
                byte[] buffer = (new UTF8Encoding()).GetBytes(xml);
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        public static void demo2()
        { 

        }


    }
}
