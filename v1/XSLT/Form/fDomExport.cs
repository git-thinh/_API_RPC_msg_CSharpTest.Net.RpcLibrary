using System;
using System.Windows.Forms;
using Gecko;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace Dom
{
    class fMain : Form
    {
        private RichTextBox viewTag;
        private TextBox txtURL;
        private RichTextBox viewXSLT;
        private RichTextBox viewXML;
        private RichTextBox viewHTML;
        private GeckoWebBrowser browser;
        private TabControl m_tabControl;
        private string[] aXSLT = new string[] { };
        private string[] aXML = new string[] { };

        public fMain()
        {
            init_Controls();

            ////addTab();
            ////Controls.Add(m_tabControl);
            ////m_tabControl.ControlRemoved += delegate
            ////{
            ////    if (m_tabControl.TabCount == 1)
            ////    {
            ////        addTab();
            ////    }
            ////};
        }

        public void init_Controls()
        {
            TabControl.CheckForIllegalCrossThreadCalls = false;
            GeckoWebBrowser.CheckForIllegalCrossThreadCalls = false;
            RichTextBox.CheckForIllegalCrossThreadCalls = false;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //this.WindowState = FormWindowState.Maximized;
            this.MaximizeBox = false;
            this.Width = 999;
            this.Height = 600;

            m_tabControl = new TabControl();
            m_tabControl.Dock = DockStyle.Fill;

            txtURL = new TextBox() { Top = 0, Width = 200, BorderStyle = BorderStyle.FixedSingle };

            Button nav = new Button() { Text = "Go", Left = txtURL.Width };
            Button newTab = new Button() { Text = "NewTab", Left = nav.Left + nav.Width };
            Button stop = new Button { Text = "Stop", Left = newTab.Left + newTab.Width };
            Button closeTab = new Button() { Text = "GC.Collect", Left = stop.Left + stop.Width };
            Button closeWithDisposeTab = new Button() { Text = "Close", Left = closeTab.Left + closeTab.Width };

            Button btnHTML = new Button() { Text = "FileOpen", Left = closeWithDisposeTab.Left + closeWithDisposeTab.Width };
            btnHTML.Click += button_Click_HTML_Open;

            Button btnXSLT = new Button { Text = "XSLT", Left = closeWithDisposeTab.Left + 250 };
            btnXSLT.Click += button_Click_XSLT_Open;

            Button btnXMLData = new Button { Text = "XML", Left = closeWithDisposeTab.Left + 330 };
            btnXMLData.Click += button_Click_XML_Open;

            Button btnExcel = new Button { Text = "-> Excel", Left = closeWithDisposeTab.Left + 400 };
            btnExcel.Click += button_Click_Excel;

            Panel box = new Panel() { Dock = DockStyle.Top, Height = 25 };
            box.Controls.Add(txtURL);
            box.Controls.Add(nav);
            box.Controls.Add(newTab);
            box.Controls.Add(stop);
            box.Controls.Add(closeTab);
            box.Controls.Add(closeWithDisposeTab);
            box.Controls.Add(btnHTML);

            box.Controls.Add(btnXSLT);
            box.Controls.Add(btnXMLData);
            box.Controls.Add(btnExcel);

            this.Controls.Add(box);
            this.Controls.Add(m_tabControl);
            m_tabControl.BringToFront();

            var tabBrowser = new TabPage() { Text = "Browser" };
            browser = new GeckoWebBrowser();
            browser.Dock = DockStyle.Fill;
            ////browser.JavascriptError += (sender, error) =>
            ////{
            ////    string text = "window.alert = function(){};";
            ////    using (AutoJSContext context = new AutoJSContext(browser.Window.JSContext))
            ////    {
            ////        string result;
            ////        context.EvaluateScript(text, (nsISupports)browser.Window.DomWindow, out result);
            ////    }
            ////};
            browser.ReadyStateChange += (s, e) =>
            {
                if (browser.Document.ReadyState == "interactive")
                {
                    exportTag();
                }
            };
            //browser.DocumentCompleted += (sender, e) => exportTag();
            tabBrowser.Controls.Add(browser);

            var tabExcel = new TabPage() { Text = "Excel" };
            var tabHTML = new TabPage() { Text = "HTML" };
            var tabXSLT = new TabPage() { Text = "XSLT" };
            var tabXML = new TabPage() { Text = "XML" };

            viewHTML = new RichTextBox() { Dock = DockStyle.Fill, ScrollBars = RichTextBoxScrollBars.Both, WordWrap = true };
            viewXML = new RichTextBox() { Dock = DockStyle.Fill, ScrollBars = RichTextBoxScrollBars.Both, WordWrap = true };
            viewXSLT = new RichTextBox() { Dock = DockStyle.Fill, ScrollBars = RichTextBoxScrollBars.Both, WordWrap = true };
            //richTextBox.Rtf = CodeSyntaxHighlighter.Process(s);
            tabHTML.Controls.Add(viewHTML);
            tabXML.Controls.Add(viewXML);
            tabXSLT.Controls.Add(viewXSLT);
            m_tabControl.TabPages.AddRange(new TabPage[] { tabXSLT, tabXML });

            m_tabControl.TabPages.Add(tabHTML);
            m_tabControl.TabPages.Add(tabBrowser);
            tabBrowser.Show();
            m_tabControl.SelectedTab = tabBrowser;

            viewTag = new RichTextBox() { Dock = DockStyle.Fill };
            tabExcel.Controls.Add(viewTag);
            m_tabControl.TabPages.Add(tabExcel);
        }

        void showTag(string tag)
        {

        }

        private void exportTag()
        {
            string jsonTag = "";
            using (AutoJSContext context = new AutoJSContext(browser.JSContext))
                context.EvaluateScript(main.JS_getTag, out jsonTag);
            viewTag.Text = jsonTag;

            //new Thread(() =>
            //{
            //    if (jsonTag.Contains("{") && jsonTag.Contains("}"))
            //    {
            //        try
            //        {
            //            //oTag[] at = JsonConvert.DeserializeObject<oTag[]>("[" + jsonTag + "]");
            //            //foreach (oTag t in at)
            //            //{
            //            //}
            //            //viewTag.Text = JsonConvert.SerializeObject(at, Formatting.Indented);
            //            //m_tabControl.SelectedIndex = m_tabControl.TabCount - 1;
            //        }
            //        catch (Exception ex)
            //        {
            //        }
            //    }
            //}).Start();
        }

        protected void addTab()
        {
            var tabPage = new TabPage();
            tabPage.Text = "blank";
            var browser = new GeckoWebBrowser();
            browser.Dock = DockStyle.Fill;
            tabPage.DockPadding.Top = 25;
            tabPage.Dock = DockStyle.Fill;

            browser.DocumentCompleted += (sender, e) => exportTag();

            addToolbarAndBrowserToTab(tabPage, browser);

            m_tabControl.TabPages.Add(tabPage);
            tabPage.Show();
            m_tabControl.SelectedTab = tabPage;

            // Demo use of ReadyStateChange.
            browser.ReadyStateChange += (s, e) => this.Text = browser.Document.ReadyState;
        }

        protected void addToolbarAndBrowserToTab(TabPage tabPage, GeckoWebBrowser browser)
        {
            TextBox urlbox = new TextBox();
            urlbox.Top = 0;
            urlbox.Width = 200;

            Button nav = new Button();
            nav.Text = "Go";
            nav.Left = urlbox.Width;

            Button newTab = new Button();
            newTab.Text = "NewTab";
            newTab.Left = nav.Left + nav.Width;

            Button stop = new Button
            {
                Text = "Stop",
                Left = newTab.Left + newTab.Width
            };

            Button closeTab = new Button();
            closeTab.Text = "GC.Collect";
            closeTab.Left = stop.Left + stop.Width;

            Button closeWithDisposeTab = new Button();
            closeWithDisposeTab.Text = "Close";
            closeWithDisposeTab.Left = closeTab.Left + closeTab.Width;

            Button open = new Button();
            open.Text = "FileOpen";
            open.Left = closeWithDisposeTab.Left + closeWithDisposeTab.Width;

            Button btnXSLT = new Button { Text = "XSLT", Left = closeWithDisposeTab.Left + 250 };
            Button btnXMLData = new Button { Text = "XML", Left = closeWithDisposeTab.Left + 330 };

            Button btnExcel = new Button { Text = "-> Excel", Left = closeWithDisposeTab.Left + 400 };
            btnExcel.Click += button_Click_Excel;

            btnXSLT.Click += (s, e) =>
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                openFile.Filter = "XLST files (*.xsl)|*.xsl";
                openFile.FilterIndex = 2;
                openFile.RestoreDirectory = true;
                openFile.Multiselect = true;

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    aXSLT = openFile.FileNames;
                }
            };
            btnXMLData.Click += (s, e) =>
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                openFile.Filter = "XML files (*.xml)|*.xml";
                openFile.FilterIndex = 2;
                openFile.RestoreDirectory = true;
                openFile.Multiselect = true;

                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    aXML = openFile.FileNames;
                }
            };

            nav.Click += delegate
            {
                // use javascript to warn if url box is empty.
                if (string.IsNullOrEmpty(urlbox.Text.Trim()))
                    browser.Navigate("javascript:alert('hey try typing a url!');");

                try
                {
                    browser.Navigate(urlbox.Text);
                }
                catch { }
                tabPage.Text = urlbox.Text;
            };

            newTab.Click += delegate { addTab(); };

            stop.Click += delegate { browser.Stop(); };

            closeTab.Click += delegate
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            };

            closeWithDisposeTab.Click += delegate
            {
                m_tabControl.Controls.Remove(tabPage);
                tabPage.Dispose();
            };

            open.Click += (s, a) =>
            {
                nsIFilePicker filePicker = Xpcom.CreateInstance<nsIFilePicker>("@mozilla.org/filepicker;1");
                filePicker.Init(browser.Window.DomWindow, new nsAString("hello"), nsIFilePickerConsts.modeOpen);
                filePicker.AppendFilter(new nsAString("html"), new nsAString("*.html"));
                filePicker.AppendFilter(new nsAString("htm"), new nsAString("*.htm"));
                if (nsIFilePickerConsts.returnOK == filePicker.Show())
                {
                    using (nsACString str = new nsACString())
                    {
                        filePicker.GetFileAttribute().GetNativePathAttribute(str);
                        browser.Navigate(str.ToString());
                    }
                }
            };

            tabPage.Controls.Add(urlbox);
            tabPage.Controls.Add(nav);
            tabPage.Controls.Add(newTab);
            tabPage.Controls.Add(stop);
            tabPage.Controls.Add(closeTab);
            tabPage.Controls.Add(closeWithDisposeTab);
            tabPage.Controls.Add(open);
            tabPage.Controls.Add(browser);
            tabPage.Controls.Add(btnXSLT);
            tabPage.Controls.Add(btnXMLData);
            tabPage.Controls.Add(btnExcel);
        }


        private void button_Click_XSLT_Open(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            openFile.Filter = "XLST files (*.xsl)|*.xsl";
            openFile.FilterIndex = 2;
            openFile.RestoreDirectory = true;
            //openFile.Multiselect = true;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                aXSLT = openFile.FileNames;
                string text = File.ReadAllText(aXSLT[0]);
                viewXSLT.Rtf = CodeSyntaxHighlighter.Process(text);
                m_tabControl.SelectedIndex = 0;
            }
        }

        private void button_Click_XML_Open(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            openFile.Filter = "XML files (*.xml)|*.xml";
            openFile.FilterIndex = 2;
            openFile.RestoreDirectory = true;
            //openFile.Multiselect = true;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                aXML = openFile.FileNames;
                string text = File.ReadAllText(aXML[0]);
                viewXML.Rtf = CodeSyntaxHighlighter.Process(text);
                m_tabControl.SelectedIndex = 1;
            }
        }

        private void button_Click_HTML_Open(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            openFile.Filter = "HTML files (*.html)|*.html";
            openFile.FilterIndex = 2;
            openFile.RestoreDirectory = true;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string file = openFile.FileName;
                string htm = File.ReadAllText(file);
                browser.LoadHtml(htm);
            }
        }

        private void button_Click_Excel(object sender, EventArgs e)
        {

        }
    }

    //==============================================================================================

    public class oTag
    {
        public string tid { set; get; }
        public string tag { set; get; }
        public string id { set; get; }
        public string name { set; get; }
        public string css { set; get; }
        public string xpath { set; get; }
        public string value { set; get; }
    }

}

