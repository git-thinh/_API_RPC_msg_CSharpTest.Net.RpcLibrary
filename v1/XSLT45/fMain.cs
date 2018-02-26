using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace XSLT
{
    unsafe public partial class fMain : Form
    {
        private string pathRoot = System.AppDomain.CurrentDomain.BaseDirectory;
        private static CacheTag cacheTag = new CacheTag(0);

        #region // Form: Load, Close ...

        public fMain()
        {
            InitializeComponent();
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            tabMain.TabPages.RemoveAt(2);

            url_LoadFromFile();

            this.WindowState = FormWindowState.Maximized;

            lbl_XLST_Excel_File.Text = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "excel.xslt");
            lbl_XLST_HTML_File.Text = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "html.xslt");

            txt_Browser_Source.Width = this.Width / 4;

            //txt_Browser_URL.Text = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "demo.html");
            //string htm_data = File.ReadAllText(txt_Browser_URL.Text);
            //txt_Browser_Source.Text = htm_data;

            //var a = HTMLTag.Format(htm_data);
            //string s = string.Join(Environment.NewLine, a.Select(x => x.ToString()).ToArray());
            //txt_HTML_Tag_Result.Text = s;

            cbo_Browser_URL.Focus();
        }

        private void fMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //cacheTag.Clear();
        }

        #endregion

        #region // Tab XLST HTML ...

        private void b_XLST_HTML_Browser_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog1.Filter = "XLST files (*.xlst)|*.xlst|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                lbl_XLST_HTML_File.Text = path;
                txt_XLST_HTML_Text.Text = File.ReadAllText(path);
            }
        }

        #endregion

        #region // Tab XLST Excel ...

        private void b_XLST_Excel_Browser_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog1.Filter = "XLST files (*.xlst)|*.xlst|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                lbl_XLST_Excel_File.Text = path;
                txt_XLST_Excel_Text.Text = File.ReadAllText(path);
            }
        }

        #endregion

        #region // Tab Main ...   

        private void browser_Source_GET()
        {
            string urlAddress = cbo_Browser_URL.Text.Trim();
            if (urlAddress.Contains(":"))
            {
                if (File.Exists(urlAddress))
                    txt_Browser_Source.Text = File.ReadAllText(urlAddress);
            }
            else
            {
                if (!urlAddress.StartsWith("http")) urlAddress = "http://" + urlAddress;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;

                    if (response.CharacterSet == null)
                    {
                        readStream = new StreamReader(receiveStream);
                    }
                    else
                    {
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    }

                    txt_HTML_Tag_Result.Text = string.Empty;
                    txt_Browser_Source.Text = readStream.ReadToEnd();

                    response.Close();
                    readStream.Close();
                }
            }
            url_UpdateFile();
        }

        private void export_Excel_03()
        {
            string path_XLST = lbl_XLST_Excel_File.Text;
            {
                if (string.IsNullOrEmpty(txt_Browser_Source.Text.Trim()))
                {
                    MessageBox.Show("Please open source HTML or open URL website ...");
                    cbo_Browser_URL.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(path_XLST))
                {
                    MessageBox.Show("Please browser XLST Excel file");
                    tabMain.SelectedIndex = 2;
                    return;
                }
            }

            string file_excel = "output_" + DateTime.Now.ToString("yyyy-MM-dd_HH-MM-ss-fff") + ".xlsx";

            var lsTag = HTMLTag.Format(txt_Browser_Source.Text).ToList();
            if (lsTag.Count == 0)
            {
                MessageBox.Show("Can not find tag input/select in source HTML");
            }
            else
            {
                txt_HTML_Tag_Result.Text = string.Join(Environment.NewLine, lsTag.Select((x, k) => (k + 1).ToString() + ": " + x.ToString() + Environment.NewLine).ToArray());
                {
                    //cacheTag.Export_XML("data.xml"); 
                    lsTag.Clear();
                    lsTag.Insert(0, new oTag(0, "Item1", "Tag1", "Type1", "Name1", "Id1", "Value1") { Onblur = "Onblur", Onclick = "Onclick", Onfocus = "Onfocus" });
                    TagXML tx = new TagXML() { items = lsTag.ToArray() };

                    string xml_data = string.Empty, xml_Sheet_Data = string.Empty, xml_style = string.Empty;
                    {
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

                        #region // strSheetData

                        XslCompiledTransform processor = new XslCompiledTransform();
                        //processor.Load("t001.xslt");
                        processor.Load(path_XLST);

                        XmlDocument xDoc = new XmlDocument();
                        xDoc.PreserveWhitespace = true;
                        xDoc.LoadXml(xml_data);

                        //XmlTextWriter xOutput = new XmlTextWriter(f_out_xml, null);
                        //processor.Transform(xDoc, null, xOutput);
                        //xOutput.Close();

                        using (var writer = new StringWriter())
                        {
                            processor.Transform(xDoc, null, writer);
                            xml_Sheet_Data = writer.ToString();
                        }

                        if (!string.IsNullOrEmpty(xml_Sheet_Data))
                            xml_Sheet_Data = xml_Sheet_Data.Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", string.Empty).Trim();

                        #endregion

                        using (StreamReader styleXmlReader2 = new System.IO.StreamReader("style.xml"))
                        {
                            xml_style = styleXmlReader2.ReadToEnd();
                            string[] a = xml_style.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Select(x => x.Trim()).ToArray();
                            xml_style = string.Join(string.Empty, a);
                        }
                    }

                    using (SpreadsheetDocument doc = SpreadsheetDocument.Create(file_excel, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                    {
                        string XML = string.Empty;
                        //xml_Sheet_Data = xml_Sheet_Data.Split(new string[] { "</Workbook>" }, StringSplitOptions.None)[0] + "</Workbook>";

                        WorkbookPart workbook = doc.AddWorkbookPart();
                        WorksheetPart sheet = workbook.AddNewPart<WorksheetPart>();

                        

                        XML = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>" + xml_Sheet_Data;
                        using (Stream stream = workbook.GetStream())
                        {
                            byte[] buffer = (new UTF8Encoding()).GetBytes(XML);
                            stream.Write(buffer, 0, buffer.Length);
                        }

                        ////string sheetId = workbook.GetIdOfPart(sheet);
                        ////{
                        ////    //Create a blank XLSX file
                        ////    XML = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?><workbook xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"" xmlns:r=""http://schemas.openxmlformats.org/officeDocument/2006/relationships""><sheets><sheet name=""{1}"" sheetId=""1"" r:id=""{0}"" /></sheets></workbook>";
                        ////    XML = string.Format(XML, sheetId, "Sheet1");
                        ////    using (Stream stream = workbook.GetStream())
                        ////    {
                        ////        byte[] buffer = (new UTF8Encoding()).GetBytes(XML);
                        ////        stream.Write(buffer, 0, buffer.Length);
                        ////    }

                        ////    //Insert our sheetData element to the sheet1.xml
                        ////    XML = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?><worksheet xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"" >{0}</worksheet>";
                        ////    XML = string.Format(XML, xml_Sheet_Data);
                        ////    using (Stream stream = sheet.GetStream())
                        ////    {
                        ////        byte[] buffer = (new UTF8Encoding()).GetBytes(XML);
                        ////        stream.Write(buffer, 0, buffer.Length);
                        ////    }
                        ////} // Append Data XMl to Sheet


                        doc.Close();
                    }

                    FileInfo fi = new FileInfo(file_excel);
                    if (fi.Exists)
                    {
                        System.Diagnostics.Process.Start(file_excel);
                    }
                }
            }
        }

        private void export_Excel_01()
        {
            string path_XLST = lbl_XLST_Excel_File.Text;
            {
                if (string.IsNullOrEmpty(txt_Browser_Source.Text.Trim()))
                {
                    MessageBox.Show("Please open source HTML or open URL website ...");
                    cbo_Browser_URL.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(path_XLST))
                {
                    MessageBox.Show("Please browser XLST Excel file");
                    tabMain.SelectedIndex = 2;
                    return;
                }
            }

            string file_excel = "output_" + DateTime.Now.ToString("yyyy-MM-dd_HH-MM-ss-fff") + ".xlsx";

            var lsTag = HTMLTag.Format(txt_Browser_Source.Text).ToList();
            if (lsTag.Count == 0)
            {
                MessageBox.Show("Can not find tag input/select in source HTML");
            }
            else
            {
                txt_HTML_Tag_Result.Text = string.Join(Environment.NewLine, lsTag.Select((x, k) => (k + 1).ToString() + ": " + x.ToString() + Environment.NewLine).ToArray());
                {
                    //cacheTag.Export_XML("data.xml"); 
                    lsTag.Insert(0, new oTag(0, "Item", "Tag", "Type", "Name", "Id", "Value") { Onblur = "Onblur", Onclick = "Onclick", Onfocus = "Onfocus" });
                    TagXML tx = new TagXML() { items = lsTag.ToArray() };

                    string xml_data = string.Empty, strSheetData = string.Empty;

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

                    #region // strSheetData

                    XslCompiledTransform processor = new XslCompiledTransform();
                    //processor.Load("t001.xslt");
                    processor.Load(path_XLST);

                    XmlDocument xDoc = new XmlDocument();
                    xDoc.PreserveWhitespace = true;
                    xDoc.LoadXml(xml_data);

                    //XmlTextWriter xOutput = new XmlTextWriter(f_out_xml, null);
                    //processor.Transform(xDoc, null, xOutput);
                    //xOutput.Close();

                    using (var writer = new StringWriter())
                    {
                        processor.Transform(xDoc, null, writer);
                        strSheetData = writer.ToString();
                    }

                    if (!string.IsNullOrEmpty(strSheetData))
                        strSheetData = strSheetData.Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", string.Empty).Trim();

                    #endregion

                    using (SpreadsheetDocument doc = SpreadsheetDocument.Create(file_excel, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                    {
                        string XML = string.Empty;
                        WorkbookPart workbook = doc.AddWorkbookPart();
                        WorksheetPart sheet = workbook.AddNewPart<WorksheetPart>();
                        string sheetId = workbook.GetIdOfPart(sheet);
                        {
                            //Create a blank XLSX file
                            XML = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?><workbook xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"" xmlns:r=""http://schemas.openxmlformats.org/officeDocument/2006/relationships""><sheets><sheet name=""{1}"" sheetId=""1"" r:id=""{0}"" /></sheets></workbook>";
                            XML = string.Format(XML, sheetId, "Sheet1");
                            using (Stream stream = workbook.GetStream())
                            {
                                byte[] buffer = (new UTF8Encoding()).GetBytes(XML);
                                stream.Write(buffer, 0, buffer.Length);
                            }

                            //Insert our sheetData element to the sheet1.xml
                            XML = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?><worksheet xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"" >{0}</worksheet>";
                            XML = string.Format(XML, strSheetData);
                            using (Stream stream = sheet.GetStream())
                            {
                                byte[] buffer = (new UTF8Encoding()).GetBytes(XML);
                                stream.Write(buffer, 0, buffer.Length);
                            }
                        } // Append Data XMl to Sheet

                        doc.Close();
                    }

                    FileInfo fi = new FileInfo(file_excel);
                    if (fi.Exists)
                    {
                        System.Diagnostics.Process.Start(file_excel);
                    }
                }
            }
        }

        private void export_Excel_02()
        {

            if (string.IsNullOrEmpty(txt_Browser_Source.Text.Trim()))
            {
                MessageBox.Show("Please open source HTML or open URL website ...");
                cbo_Browser_URL.Focus();
                return;
            }

            string path_XLST = lbl_XLST_Excel_File.Text;
            if (string.IsNullOrEmpty(path_XLST))
            {
                MessageBox.Show("Please browser XLST Excel file");
                tabMain.SelectedIndex = 2;
                return;
            }

            string file_excel = "output_" + DateTime.Now.ToString("yyyy-MM-dd_HH-MM-ss-fff") + ".xlsx";

            { // test
                //int size = Marshal.SizeOf(typeof(oTagRaw));

                //oTagRaw o1 = (oTagRaw)new oTag(1, "A", "select", "select", "a", "a", "giá trị = Tiếng Việt");

                //oTag e1 = (oTag)o1;

                //byte[] b1 = o1.Serialize_Marshal();
                //oTagRaw o11 = b1.Deserialize_Marshal<oTagRaw>();
                //oTag e11 = (oTag)o11;

                //oTagRaw o2 = (oTagRaw)new oTag(1, "B", "input", "check", "b", "b", "value = English");
                //oTag e2 = (oTag)o2;

                //byte[] b2 = o2.Serialize_Marshal();
                //oTagRaw o22 = b2.Deserialize_Marshal<oTagRaw>();
                //oTag e22 = (oTag)o22;

                //cacheTag[0] = e1;
                //cacheTag[1] = e2;

            } // end test


            var aTag = HTMLTag.Format(txt_Browser_Source.Text);
            if (aTag.Length == 0)
            {
                MessageBox.Show("Can not find tag input/select in source HTML");
            }
            else
            {
                txt_HTML_Tag_Result.Text = string.Join(Environment.NewLine, aTag.Select(x => x.ToString()).ToArray());

                //for (int k = 0; k < aTag.Length; k++) cacheTag[k] = aTag[k];

                {
                    //cacheTag.Export_XML("data.xml"); 
                    TagXML tx = new TagXML() { items = aTag };

                    string xml_data = string.Empty, strSheetData = string.Empty;

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

                    #region // strSheetData

                    XslCompiledTransform processor = new XslCompiledTransform();
                    //processor.Load("t001.xslt");
                    processor.Load(path_XLST);

                    XmlDocument xDoc = new XmlDocument();
                    xDoc.PreserveWhitespace = true;
                    xDoc.LoadXml(xml_data);

                    //XmlTextWriter xOutput = new XmlTextWriter(f_out_xml, null);
                    //processor.Transform(xDoc, null, xOutput);
                    //xOutput.Close();

                    using (var writer = new StringWriter())
                    {
                        processor.Transform(xDoc, null, writer);
                        strSheetData = writer.ToString();
                    }

                    if (!string.IsNullOrEmpty(strSheetData))
                        strSheetData = strSheetData.Replace(@"<?xml version=""1.0"" encoding=""utf-16""?>", string.Empty).Trim();

                    #endregion

                    string XML = string.Empty;
                    DocumentFormat.OpenXml.Packaging.SpreadsheetDocument spreadSheet =
                        DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(file_excel, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook, false);


                    // Create the parts and the corresponding objects

                    //// Workbook
                    //spreadSheet.AddWorkbookPart();
                    //spreadSheet.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    //spreadSheet.WorkbookPart.Workbook.Save();

                    //// Shared string table
                    //sharedStringTablePart = spreadSheet.WorkbookPart.AddNewPart<DocumentFormat.OpenXml.Packaging.SharedStringTablePart>();
                    //sharedStringTablePart.SharedStringTable = new DocumentFormat.OpenXml.Spreadsheet.SharedStringTable();
                    //sharedStringTablePart.SharedStringTable.Save();

                    //// Sheets collection
                    //spreadSheet.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
                    //spreadSheet.WorkbookPart.Workbook.Save();

                    ////WorkbookPart workbook = spreadSheet.AddWorkbookPart();
                    ////WorksheetPart sheet = workbook.AddNewPart<WorksheetPart>();
                    ////string sheetId = workbook.GetIdOfPart(sheet);
                    ////{
                    ////    // Create a blank XLSX file
                    ////    string XML = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?><workbook xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"" xmlns:r=""http://schemas.openxmlformats.org/officeDocument/2006/relationships""><sheets><sheet name=""{1}"" sheetId=""1"" r:id=""{0}"" /></sheets></workbook>";
                    ////    XML = string.Format(XML, sheetId, "Sheet1");
                    ////    using (Stream stream = workbook.GetStream())
                    ////    {
                    ////        byte[] buffer = (new UTF8Encoding()).GetBytes(XML);
                    ////        stream.Write(buffer, 0, buffer.Length);
                    ////    }

                    ////    // Insert our sheetData element to the sheet1.xml
                    ////    XML = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?><worksheet xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"" >{0}</worksheet>";
                    ////    XML = string.Format(XML, strSheetData);
                    ////    using (Stream stream = sheet.GetStream())
                    ////    {
                    ////        byte[] buffer = (new UTF8Encoding()).GetBytes(XML);
                    ////        stream.Write(buffer, 0, buffer.Length);
                    ////    }
                    ////} // Append Data XMl to Sheet


                    //  Create the Excel file contents.  This function is used when creating an Excel file either writing 
                    //  to a file, or writing to a MemoryStream.
                    spreadSheet.AddWorkbookPart();
                    spreadSheet.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

                    //  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
                    spreadSheet.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

                    //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
                    WorkbookStylesPart workbookStylesPart = spreadSheet.WorkbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");
                    //var workbookStylesPart = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>();
                    Stylesheet stylesheet = new Stylesheet(new Fills(
                            // Index 0 - required, reserved by Excel - no pattern
                            new Fill(new PatternFill { PatternType = PatternValues.DarkGray }),
                            // Index 1 - required, reserved by Excel - fill of gray 125
                            new Fill(new PatternFill { PatternType = PatternValues.Gray125 }),
                            // Index 2 - no pattern text on gray background
                            new Fill(new PatternFill
                            {
                                PatternType = PatternValues.Solid,
                                BackgroundColor = new BackgroundColor { Indexed = 64U },
                                ForegroundColor = new ForegroundColor { Rgb = "FFD9D9D9" }
                            })
                        ));
                    workbookStylesPart.Stylesheet = stylesheet;
                    workbookStylesPart.Stylesheet.Save();
                    // create a solid red fill


                    //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
                    uint worksheetNumber = 1;
                    Sheets sheets = spreadSheet.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
                    //foreach (DataTable dt in ds.Tables)
                    //{
                    //  For each worksheet you want to create
                    string worksheetName = "Hi";

                    //  Create worksheet part, and add it to the sheets collection in workbook
                    WorksheetPart newWorksheetPart = spreadSheet.WorkbookPart.AddNewPart<WorksheetPart>();
                    Sheet sheet = new Sheet() { Id = spreadSheet.WorkbookPart.GetIdOfPart(newWorksheetPart), SheetId = worksheetNumber, Name = worksheetName };
                    sheets.Append(sheet);

                    //  Append this worksheet's data to our Workbook, using OpenXmlWriter, to prevent memory problems
                    //WriteDataTableToExcelWorksheet(dt, newWorksheetPart);

                    {
                        //string sheetId = workbook.GetIdOfPart(sheet);

                        //// Create a blank XLSX file
                        //string XML = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?><workbook xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"" xmlns:r=""http://schemas.openxmlformats.org/officeDocument/2006/relationships""><sheets><sheet name=""{1}"" sheetId=""1"" r:id=""{0}"" /></sheets></workbook>";
                        //XML = string.Format(XML, sheetId, "Sheet1");
                        //using (Stream stream = workbook.GetStream())
                        //{
                        //    byte[] buffer = (new UTF8Encoding()).GetBytes(XML);
                        //    stream.Write(buffer, 0, buffer.Length);
                        //}

                        // Insert our sheetData element to the sheet1.xml
                        XML = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?><worksheet xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"" >{0}</worksheet>";
                        XML = string.Format(XML, strSheetData);
                        using (Stream stream = newWorksheetPart.GetStream())
                        {
                            byte[] buffer = (new UTF8Encoding()).GetBytes(XML);
                            stream.Write(buffer, 0, buffer.Length);
                        }
                    } // Append Data XMl to Sheet



                    worksheetNumber++;
                    //}

                    spreadSheet.WorkbookPart.Workbook.Save();
                    ////spreadSheet.Close();




                    //////// Stylesheet
                    string xml_style = string.Empty;

                    //workbookStylesPart = spreadSheet.WorkbookPart.AddNewPart<DocumentFormat.OpenXml.Packaging.WorkbookStylesPart>();
                    //workbookStylesPart.Stylesheet = new DocumentFormat.OpenXml.Spreadsheet.Stylesheet();
                    //StreamReader styleXmlReader = new System.IO.StreamReader("style2.xml");
                    //xml_style = styleXmlReader.ReadToEnd();
                    //workbookStylesPart.Stylesheet.InnerXml = xml_style;
                    //workbookStylesPart.Stylesheet.Save();



                    ////workbookStylesPart = spreadSheet.WorkbookPart.AddNewPart<WorkbookStylesPart>();
                    ////workbookStylesPart.Stylesheet = new Stylesheet();

                    ////StreamReader styleXmlReader2 = new System.IO.StreamReader("style1.xml");
                    ////xml_style = styleXmlReader2.ReadToEnd();

                    ////using (Stream stream = workbookStylesPart.GetStream())
                    ////{
                    ////    //string[] a = xml_style.Split(new string[] { }, StringSplitOptions.None).Select(x => x.Trim()).ToArray();
                    ////    //xml_style = string.Join(string.Empty, a);
                    ////    byte[] buffer = (new UTF8Encoding()).GetBytes(xml_style);
                    ////    stream.Write(buffer, 0, buffer.Length);
                    ////}
                    ////workbookStylesPart.Stylesheet.Save();




                    spreadSheet.Close();

                    //using (StreamReader styleXmlReader = new System.IO.StreamReader("style.xml"))
                    //{
                    //    string styleXml = styleXmlReader.ReadToEnd();

                    //    doc.WorkbookPart.WorkbookStylesPart.Stylesheet.InnerXml = styleXml;
                    //    doc.WorkbookPart.WorkbookStylesPart.Stylesheet.Save();
                    //}


                    //using (SpreadsheetDocument doc = SpreadsheetDocument.Create(file_excel, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                    //////using (SpreadsheetDocument doc = CreateWorkbook(file_excel))
                    //////{

                    ////WorkbookPart workbook = doc.AddWorkbookPart();
                    ////WorksheetPart sheet = workbook.AddNewPart<WorksheetPart>();
                    ////string sheetId = workbook.GetIdOfPart(sheet);

                    ////{
                    ////    // Create a blank XLSX file
                    ////    string XML = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?><workbook xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"" xmlns:r=""http://schemas.openxmlformats.org/officeDocument/2006/relationships""><sheets><sheet name=""{1}"" sheetId=""1"" r:id=""{0}"" /></sheets></workbook>";
                    ////    XML = string.Format(XML, sheetId, "Sheet1");
                    ////    using (Stream stream = workbook.GetStream())
                    ////    {
                    ////        byte[] buffer = (new UTF8Encoding()).GetBytes(XML);
                    ////        stream.Write(buffer, 0, buffer.Length);
                    ////    }

                    ////    // Insert our sheetData element to the sheet1.xml
                    ////    XML = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?><worksheet xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"" >{0}</worksheet>";
                    ////    XML = string.Format(XML, strSheetData);
                    ////    using (Stream stream = sheet.GetStream())
                    ////    {
                    ////        byte[] buffer = (new UTF8Encoding()).GetBytes(XML);
                    ////        stream.Write(buffer, 0, buffer.Length);
                    ////    }
                    ////} // Append Data XMl to Sheet

                    ////doc.Close();
                    //////}

                    FileInfo fi = new FileInfo(file_excel);
                    if (fi.Exists)
                    {
                        System.Diagnostics.Process.Start(file_excel);
                    }
                }
            }

        }


        private void b_Export_Excel_Click(object sender, EventArgs e)
        {
            export_Excel_01();
            //export_Excel_03();
        }

        private void b_Browser_Go_Click(object sender, EventArgs e)
        {
            browser_Source_GET();
        }

        private void cbo_Browser_URL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) browser_Source_GET();
        }

        private void cbo_Browser_URL_SelectedIndexChanged(object sender, EventArgs e)
        {
            browser_Source_GET();
        }

        private void b_Browser_File_HTML_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog1.Filter = "XLST files (*.xlst)|*.xlst|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                cbo_Browser_URL.Text = path;
                browser_Source_GET();
            }
        }

        private void b_Browser_Text_Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog1 = new SaveFileDialog();
            fileDialog1.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            fileDialog1.Filter = "XLST files (*.xlst)|*.xlst|All files (*.*)|*.*";
            fileDialog1.FilterIndex = 2;
            fileDialog1.RestoreDirectory = true;
            string url = cbo_Browser_URL.Text.Trim().ToLower();

            fileDialog1.FileName = url;

            if (fileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = fileDialog1.FileName;
                cbo_Browser_URL.Text = path;
                browser_Source_GET();
            }

        }

        #endregion

        #region // URL ...

        private void url_UpdateFile()
        {
            string url = cbo_Browser_URL.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(url))
            {
                if (url.StartsWith("http://")) url = url.Substring(7);
                if (url.StartsWith("www.")) url = url.Substring(4);

                List<string> ls = cbo_Browser_URL.Items.Cast<string>().ToList();
                if (ls.IndexOf(url) == -1)
                {
                    string path = Path.Combine(pathRoot, "url.txt");
                    File.AppendAllText(path, Environment.NewLine + url);
                    cbo_Browser_URL.Items.Insert(0, url);
                }
            }
        }

        private void url_LoadFromFile()
        {
            string path = Path.Combine(pathRoot, "url.txt");
            if (File.Exists(path))
            {
                string[] a = File
                    .ReadAllLines(path)
                    .Select(x => x.Trim().ToLower())
                    .Where(x => x != string.Empty)
                    .GroupBy(x => x)
                    .Select(x => x.Key)
                    .OrderBy(x => x)
                    .ToArray();
                if (a.Length > 0)
                {
                    cbo_Browser_URL.Items.AddRange(a);
                    cbo_Browser_URL.SelectedIndex = 0;
                }
            }
        }

        #endregion

    }
}
