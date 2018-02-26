using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Gecko;

namespace Dom
{
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

    class fDomExport : Form
    {
        private TabControl m_tabControl;
        static string[] aTag = new string[] 
        {
            "select", "input", "textarea" };

        public fDomExport()
        {
            this.MaximizeBox = false;

            // Uncomment the follow line to enable CustomPrompt's
            GeckoWebBrowser.UseCustomPrompt();

            // If you want to further customize the GeckoFx PromptService then 
            // you will need make a class that implements nsIPromptService2 and nsIPrompt interfaces and
            // set the PromptFactory.PromptServiceCreator delegate. for example:
            // PromptFactory.PromptServiceCreator = () => new MyPromptService();

            //Xpcom.Initialize(XULRunnerLocator.GetXULRunnerLocation());
            Xpcom.Initialize("bin");
            // Uncomment the follow line to enable CustomPrompt's
            // GeckoPreferences.User["browser.xul.error_pages.enabled"] = false;

            GeckoPreferences.User["gfx.font_rendering.graphite.enabled"] = true;

            this.FormClosing += (sender, e) =>
            {
                Xpcom.Shutdown();
            };


            this.Width = 800;
            this.Height = 600;

            m_tabControl = new TabControl();
            m_tabControl.Dock = DockStyle.Fill;

            AddTab();

            Controls.Add(m_tabControl);

            m_tabControl.ControlRemoved += delegate
            {
                if (m_tabControl.TabCount == 1)
                {
                    AddTab();
                }
            };

        }

        protected void ModifyElements(GeckoElement element, string tagName, Action<GeckoElement> mod)
        {
            while (element != null)
            {
                if (element.TagName == tagName)
                {
                    mod(element);
                }
                ModifyElements(element.FirstChild as GeckoHtmlElement, tagName, mod);
                element = (element.NextSibling as GeckoHtmlElement);
            }
        }

        protected void TestModifyingDom(GeckoWebBrowser browser)
        {
            GeckoElement g = browser.Document.DocumentElement;
            ModifyElements(g, "BODY", e =>
            {
                for (int i = 1; i < 4; ++i)
                {
                    var newElement = g.OwnerDocument.CreateElement(String.Format("h{0}", i));
                    newElement.TextContent = "Geckofx added this text.";
                    g.InsertBefore(newElement, e);
                }
            });
        }

        protected void DisplayElements(GeckoElement g)
        {
            while (g != null)
            {
                Console.WriteLine("tag = {0} value = {1}", g.TagName, g.TextContent);
                DisplayElements(g.FirstChild as GeckoHtmlElement);
                g = (g.NextSibling as GeckoHtmlElement);
            }
        }

        protected void TestQueryingOfDom(GeckoWebBrowser browser)
        {
            GeckoElement g = browser.Document.DocumentElement;
            DisplayElements(g);
        }

        protected void AddTab()
        {
            var tabPage = new TabPage();
            tabPage.Text = "blank";
            var browser = new GeckoWebBrowser();
            //browser.Dock = DockStyle.Fill;
            browser.Top = this.Height;
            browser.Width = this.Width;
            browser.Height = this.Height;


            //browser.DisableWmImeSetContext = true;
            tabPage.DockPadding.Top = 25;
            tabPage.Dock = DockStyle.Fill;

            // add a handler showing how to view the DOM
            //			browser.DocumentCompleted += (s, e) => 	TestQueryingOfDom(browser);

            // add a handler showing how to modify the DOM.
            //			browser.DocumentCompleted += (s, e) => TestModifyingDom(browser);


            AddToolbarAndBrowserToTab(tabPage, browser);

            m_tabControl.TabPages.Add(tabPage);
            tabPage.Show();
            m_tabControl.SelectedTab = tabPage;

            // Uncomment this to stop links from navigating.
            // browser.DomClick += StopLinksNavigating;

            // Demo use of ReadyStateChange.
            browser.ReadyStateChange += (s, e) => this.Text = browser.Document.ReadyState;
        }

        /// <summary>
        /// An example event handler for the DomClick event.
        /// Prevents a link click from navigating.
        /// </summary>
        private void StopLinksNavigating(object sender, DomEventArgs e)
        {
            if (sender == null) return;
            if (e == null) return;
            if (e.Target == null) return;

            var element = e.Target.CastToGeckoElement();

            GeckoHtmlElement clicked = element as GeckoHtmlElement;
            if (clicked == null) return;

            // prevent clicking on Links from navigation to the
            if (clicked.TagName == "A")
            {
                e.Handled = true;
                MessageBox.Show(sender as IWin32Window, String.Format("You clicked on Link {0}", clicked.GetAttribute("href")));
            }

        }


        public string GetElementAttributes(GeckoElement element)
        {
            var result = new StringBuilder();
            foreach (var a in element.Attributes)
            {
                result.Append(String.Format(" {0} = '{1}' ", a.LocalName, a.NodeValue));
            }

            return result.ToString();
        }


        protected void AddToolbarAndBrowserToTab(TabPage tabPage, GeckoWebBrowser browser)
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

            Button excel = new Button();
            excel.Text = "-> Excel";
            excel.Left = open.Left + open.Width;

            Button scrollDown = new Button { Visible = false, Text = "Down", Left = closeWithDisposeTab.Left + 250 };
            Button scrollUp = new Button { Visible = false, Text = "Up", Left = closeWithDisposeTab.Left + 330 };

            //scrollDown.Click += (s, e) => { browser.Window.ScrollByPages(1); };
            //scrollUp.Click += (s, e) => { browser.Window.ScrollByPages(-1); };


            TextBox txtTag = new TextBox() { Top = urlbox.Height + 5, Left = 0, Width = this.Width - 27, Height = this.Height - (urlbox.Height*2),
                Dock = DockStyle.Fill,
                Multiline = true, WordWrap = true, ScrollBars = ScrollBars.Both };

            browser.DocumentCompleted += (sender, e) =>
            {
                GeckoWebBrowser w = (GeckoWebBrowser)sender;

                //var timer = new System.Timers.Timer(3000);
                //timer.Elapsed += (sender, ev) =>
                //{
                //    timer.Stop();
                tabPage.Text = "Dom tree completed ...";
                //GeckoElement g = w.Document.DocumentElement;
                //DisplayElements(g, txtTag);

                string htm = w.Document.Body.InnerHtml;


                string JSresult = "";
                string JStext = @"
// element-xpath
// -----------------
// Copyright(c) 2013 Bermi Ferrer <bermi@bermilabs.com>
// MIT Licensed

(function (root) {
  'use strict';

  var
    // Commons JS environment flag
    is_commons_js = typeof module !== 'undefined' && module.exports,

    // Save the previous value of the `getElementXpath` variable.
    previousgetElementXpath = root.getElementXpath,

    emptyFunction = function () {},

    // Create a safe reference to the getElementXpath object for use below.
    getElementXpath = function (el, callback) {
      var err, result, nodes, i, n, segments, siblings, id_count;
      try {
        if (callback && (typeof callback !== 'function')) {
          throw new Error('Invalid callback supplied');
        }

        // We need to get all the tags on the document,
        nodes = getElementXpath.getNodes(el);

        segments = [];
        while (el && el.nodeType === 1) {
          if (el.hasAttribute('id')) {
            segments = addIdSegments(el, nodes, segments);
            if (typeof segments === 'string') {
              result = segments;
              break;
            }
          } else if (el.hasAttribute('class')) {
            segments = addClassSegment(el, nodes, segments);
          } else {
            segments = addSiblingSegment(el, nodes, segments);
          }
          el = el.parentNode;
        }

        if (!result && segments.length) {
          result = '/' + segments.join('/');
        }

      } catch (err) {
        // On sync mode the error will be included on the callback
        // remove if only async is supported
        if (!callback) {
          throw err;
        }
      }

      // Async mode, remove condition if only async is supported
      if (callback) {
        callback(err, result);
      } else {
        return result;
      }
    };


  // Run getElementXpath in *noConflict* mode, returning the `getElementXpath`
  // variable to its previous owner. Returns a reference to
  // the getElementXpath object.
  getElementXpath.noConflict = function () {
    root.getElementXpath = previousgetElementXpath;
    return getElementXpath;
  };

  // Returns a list of nodes on the document
  // you might want to memoizee or throttle this method
  // if your DOM will not change between getElementXpath calls
  getElementXpath.getNodes = function (el) {
    return document.getElementsByTagName('*');
  };

  // Adds ID segments to the segments array
  // if the current element has an ID it will return a string
  // with the element path
  function addIdSegments(el, nodes, segments) {
    var id_count = 0,
      n = 0;
    while (n < nodes.length) {
      if (nodes[n].hasAttribute('id') && nodes[n].id === el.id) {
        id_count = id_count + 1;
      }
      if (id_count > 1) {
        break;
      }
      n = n + 1;
    }
    if (id_count === 1) {
      // The target element has an ID, that's the last node we need
      if (false && segments.length === 0) {
        segments.unshift('//[@id=\'' + el.getAttribute('id') + '\']');
      } else {
        segments.unshift('id(\'' + el.getAttribute('id') + '\')');
      }
      return segments.join('/');
    } else {
      segments.unshift(el.localName.toLowerCase() + '[@id=\'' + el.getAttribute('id') + '\']');
    }
    return segments;
  }

  // Gets the element positions among it's siblings
  function addSiblingSegment(el, nodes, segments) {
    var i = 1,
      siblings = el.previousSibling;
    while (siblings) {
      if (siblings.localName === el.localName) {
        i = i + 1;
      }
      siblings = siblings.previousSibling;
    }
    segments.unshift(el.localName.toLowerCase() + '[' + i + ']');
    return segments;
  }

  function addClassSegment(el, nodes, segments) {
    segments.unshift(el.localName.toLowerCase() + '[contains(concat(\' \', @class, \' \'),\' ' + el.getAttribute('class') + ' \')]');
    return segments;
  }

  if (is_commons_js) {
    module.exports = getElementXpath;
  } else {
    // Set getElementXpath on the browser window or as a member of the wrapping closure
    root.getElementXpath = getElementXpath;
  }

// Establish the root object, `window` in the browser or the closure that wraps the code
}(this));




 





function getElementAttributes(element) 
{
    var AttributesAssocArray = {};

    var tag = element.tagName;
    if(tag != null) tag = tag.toString().toUpperCase().trim();

    if(tag == 'SELECT' || tag == 'INPUT' || tag == 'BUTTON' || tag == 'TEXTAREA')
    {
        var tid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            var r = Math.random()*16|0, v = c == 'x' ? r : (r&0x3|0x8);
            return v.toString(16);
        });
        element.setAttribute('class', tid);

        AttributesAssocArray['tid'] = tid;
        AttributesAssocArray['tag'] = tag;

        var id = element.getAttribute('id');
        if(id == null) id = '';
        AttributesAssocArray['id'] = id;

        var name = element.getAttribute('name');
        if(name == null) name = '';
        AttributesAssocArray['name'] = name;

        var css = element.getAttribute('class');
        if(css == null) css = '';
        AttributesAssocArray['css'] = css;

        var xpath = '';
        //if(id != '') xpath = '//' + tag + '[@id=""' + id + '""]';
        //else if(css != '') xpath = '//' + tag + '[@class=""' + css + '""]';
        //else xpath = '';//getElementXpath(element); 
        //xpath = '//' + tag + '[@tid=""' + tid + '""]';

        AttributesAssocArray['xpath'] = xpath;

        var val = '';
        switch(tag)
        {
            case 'SELECT': break;
            case 'TEXTAREA': case 'BUTTON': val = element.innerHTML; break;
            case 'INPUT': val = element.getAttribute('value'); break;
            default: break;
        }
        if(val == null) val = '';
        val = '';
        AttributesAssocArray['value'] = val;

        //for (var index = 0; index < element.attributes.length; ++index) 
        //{ 
        //    AttributesAssocArray[element.attributes[index].name] = element.attributes[index].value; 
        //};

        return JSON.stringify(AttributesAssocArray);
    }
    return null;
} 

var ls = new Array();
function getTag(ns)
{
    for (var i=0, max=ns.length; i < max; i++) {
        var si = getElementAttributes(ns[i]);
        if(si != null) ls.push(si);
        var ac = ns[i].children;
        if(ac.length > 0) getTag(ac);
    }    
}

function getData(){
    return ls.join(',');
}

var all = document.getElementsByTagName('*');
getTag(all);

getData();

";

                using (AutoJSContext context = new AutoJSContext(w.JSContext))
                {
                    string result;
                    context.EvaluateScript("3 + 2;", out result);

                    context.EvaluateScript(JStext, out JSresult);
                }

                txtTag.Text = JSresult;

                string ssss = w.Document.Body.InnerHtml;

                if (JSresult.Contains("{") && JSresult.Contains("}")) {
                    try
                    {
                        oTag[] at = JsonConvert.DeserializeObject<oTag[]>("[" + JSresult + "]");
                        foreach (oTag t in at) {
                            var el = w.Document.GetElementsByClassName(t.css);
                            if (el.Count > 0) {

                            }
                        }

                    }
                    catch(Exception ex) {

                    }

                }

                //////foreach (string ti in aTag)
                //////{
                //////    var ts = w.Document.GetElementsByName(ti);
                //////    if (ts.Count > 0)
                //////    {
                //////        foreach (var ele in ts) {
                //////            string s = GetElementAttributes(ele);
                //////            txtTag.Text = s + Environment.NewLine + Environment.NewLine + txtTag.Text;
                //////        }
                //////    }
                //////}
                //displayTags(cs.Cast<GeckoElement>().ToArray(), txtTag);


                //};
                //timer.AutoReset = true;
                //timer.Enabled = true;
                //timer.Start();
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

            newTab.Click += delegate { AddTab(); };

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
                //filePicker.Init(browser.Window.DomWindow, new nsAString("hello"), nsIFilePickerConsts.modeOpen);
                //filePicker.AppendFilter(new nsAString("png"), new nsAString("*.png"));
                filePicker.AppendFilter(new nsAString("html"), new nsAString("*.html"));
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
            tabPage.Controls.Add(excel);
            tabPage.Controls.Add(browser);
            tabPage.Controls.Add(scrollDown);
            tabPage.Controls.Add(scrollUp);
            tabPage.Controls.Add(txtTag);
        }

        private void displayTags(GeckoElement[] nodes, TextBox txt)
        {
            foreach (GeckoElement g in nodes)
            {
                string tag = g.TagName;
                //if (!string.IsNullOrEmpty(tag) && lsOut.IndexOf(tag) == -1)
                //{
                string name = g.GetAttribute("name");
                if (name == null) name = string.Empty;

                string text = g.TextContent;
                if (text.Length > 50) text = text.Substring(0, 50) + "...";

                string css = "";// g.ClassName == null ? string.Empty : g.ClassName;
                string id = "";
                string s = string.Format(
                    "TAG = {0} ID = {1} NAME = {2} CLASS = {3} VALUE = {4}",
                    tag,
                    id,
                    name,
                    css,
                    text);

                txt.Text = s + Environment.NewLine + txt.Text;
            }
        }


        static List<string> lsOut = new List<string>() { "HTML", "HEAD", "STYLE", "SCRIPT", "BODY", "SCRIPT" };
        protected void DisplayElements(GeckoElement g, TextBox txt)
        {
            while (g != null)
            {
                string tag = g.TagName;
                //if (!string.IsNullOrEmpty(tag) && lsOut.IndexOf(tag) == -1)
                //{
                    string name = g.GetAttribute("name");
                    if (name == null) name = string.Empty;

                    string text = g.TextContent;
                    if (text.Length > 50) text = text.Substring(0, 50) + "...";

                    string css = "";// g.ClassName == null ? string.Empty : g.ClassName;
                    string id = "";
                    string s = string.Format(
                        "TAG = {0} ID = {1} NAME = {2} CLASS = {3} VALUE = {4}",
                        tag,
                        id,
                        name,
                        css,
                        text);

                    txt.Text = s + Environment.NewLine + txt.Text;
                //}

                DisplayElements(g.FirstChild as GeckoElement, txt);
                g = (g.NextSibling as GeckoElement);
            }
        }

    }

    /*
    class fDomExport : BaseForm
    {
        private TabControl m_tabControl;
        private string uri = "http://google.com.vn";

        public fDomExport()
        {
            //uri = "http://khoeplus24h.vn";
            uri = "http://kienthuc.net.vn/";

            Xpcom.Initialize(@"bin");
            this.FormClosing += (sender, e) =>
            {
                Xpcom.Shutdown();
            };
            GeckoPreferences.User["security.warn_viewing_mixed"] = false;
            GeckoPreferences.User["plugin.state.flash"] = 0;

            this.Width = 800;
            this.Height = 600;

            m_tabControl = new TabControl();
            m_tabControl.Dock = DockStyle.Fill;

            AddTab();

            Controls.Add(m_tabControl);

            m_tabControl.ControlRemoved += delegate
            {
                if (m_tabControl.TabCount == 0)
                {
                    AddTab();
                }
            };

        }

        protected void AddTab()
        {
            var tabPage = new TabPage() { Text = "blank", Dock = DockStyle.Bottom };
            var browser = new GeckoWebBrowser();
            browser.Dock = DockStyle.Fill;

            AddToolbarAndBrowserToTab(tabPage, browser);

            m_tabControl.TabPages.Add(tabPage);
            tabPage.Show();
            m_tabControl.SelectedTab = tabPage;
        }


        protected void AddToolbarAndBrowserToTab(TabPage tabPage, GeckoWebBrowser browser)
        {
            int w = this.Width, wi = 70;
            Button closeTab = new Button() { Text = "Close", Width = wi - 2, Left = w - wi, TextAlign = System.Drawing.ContentAlignment.MiddleLeft, ForeColor = System.Drawing.Color.White, FlatStyle = FlatStyle.Flat, BackColor = System.Drawing.Color.Gray };
            Button newTab = new Button() { Text = "NewTab", Width = wi - 2, Left = w - (wi * 2), ForeColor = System.Drawing.Color.White, FlatStyle = FlatStyle.Flat, BackColor = System.Drawing.Color.Gray };
            Button go = new Button() { Text = "Go", Width = wi - 2, Left = w - (wi * 3), ForeColor = System.Drawing.Color.White, FlatStyle = FlatStyle.Flat, BackColor = System.Drawing.Color.Gray };

            TextBox urlbox = new TextBox() { Text = uri, Width = w - (wi * 3), Top = 1 };

            TextBox txtTag = new TextBox() { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical, WordWrap = true };

            go.Click += delegate
            {
                // use javascript to warn if url box is empty.
                if (string.IsNullOrEmpty(urlbox.Text.Trim()))
                    browser.Navigate("javascript:alert('hey try typing a url!');");

                browser.Navigate(urlbox.Text);
                tabPage.Text = urlbox.Text;
                txtTag.Text = "";
            };

            newTab.Click += delegate { AddTab(); };

            closeTab.Click += delegate
            {
                m_tabControl.Controls.Remove(tabPage);
                browser.Dispose();
            };

            browser.DocumentCompleted += (s, e) =>
            {
                //var timer = new System.Timers.Timer(3000);
                //timer.Elapsed += (sender, ev) =>
                //{
                //    timer.Stop();
                    tabPage.Text = "Dom tree completed ...";
                    GeckoElement g = browser.Document.DocumentElement;
                    DisplayElements(g, txtTag);
                //};
                //timer.AutoReset = true;
                //timer.Enabled = true;
                //timer.Start();
            };

            Panel box = new Panel() { Dock = DockStyle.Top, Height = urlbox.Height + 2, BackColor = System.Drawing.Color.WhiteSmoke };
            box.Controls.Add(urlbox);
            box.Controls.Add(go);
            box.Controls.Add(newTab);
            box.Controls.Add(closeTab);

            tabPage.Controls.Add(box);
            tabPage.Controls.Add(browser);
            tabPage.Controls.Add(txtTag);

            txtTag.BringToFront();
        }


        static List<string> lsOut = new List<string>() { "HTML", "HEAD", "STYLE", "SCRIPT", "BODY", "SCRIPT" };
        protected void DisplayElements(GeckoElement g, TextBox txt)
        {
            while (g != null)
            {
                string tag = g.TagName;
                if (!string.IsNullOrEmpty(tag) && lsOut.IndexOf(tag) == -1)
                {
                    string name = g.GetAttribute("name");
                    if (name == null) name = string.Empty;

                    string text = g.TextContent;
                    if (text.Length > 50) text = text.Substring(0, 50) + "...";

                    string css = "";// g.ClassName == null ? string.Empty : g.ClassName;
                    string id = "";
                    string s = string.Format(
                        "TAG = {0} ID = {1} NAME = {2} CLASS = {3} VALUE = {4}",
                        tag,
                        id,
                        name,
                        css,
                        text);

                    txt.Text = s + Environment.NewLine + txt.Text;
                }

                DisplayElements(g.FirstChild as GeckoElement, txt);
                g = (g.NextSibling as GeckoElement);
            }
        }
    }
    */
}
