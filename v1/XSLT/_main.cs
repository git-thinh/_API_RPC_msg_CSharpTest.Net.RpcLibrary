using System;
using System.Collections.Generic;
using System.Text;
using model;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;

namespace Dom
{
    public static class main
    {
        public static void Exit() {
            int pi = Process.GetCurrentProcess().Id;
            Process p = Process.GetProcessById(pi);
            p.Kill();
        }

        [STAThread]
        static void Main()
        {
            Gecko.Xpcom.Initialize("bin");

            //Application.ApplicationExit += (sender, e) =>
            //{
                //Xpcom.Shutdown();
            //};

            ////AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            ////    {
            ////        var resourceName = string.Format(@"msg.DLL.{0}.dll",
            ////            new AssemblyName(args.Name).Name);

            ////        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            ////        {
            ////            if (stream == null) return null;

            ////            var assemblyData = new Byte[stream.Length];
            ////            stream.Read(assemblyData, 0, assemblyData.Length);

            ////            return Assembly.Load(assemblyData);
            ////        }
            ////    };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += (se, ev) =>
            {
                try
                {
                    string errorMsg = string.Format(new System.Globalization.CultureInfo("en-us", true), "There are some problems while trying to use the Caching Quick Start, please check the following error messages: \n{0}\n", ev.Exception.Message) + Environment.NewLine;
                    DialogResult result = MessageBox.Show(errorMsg, "Application Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Stop);
                    // Exits the program when the user clicks Abort.
                    if (result == DialogResult.Abort)
                    {
                        Application.Exit();
                    }
                }
                catch
                {
                    Exit();
                }
            };
            //---------------------------------------------------------
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //---------------------------------------------------------
            new Thread(new ThreadStart(wsServer.Run)).Start();
            //---------------------------------------------------------
            //Application.Run(new fSystemTray());
            //Application.Run(new fViewSource());
            //Application.Run(new fGeckoBrowser());
            Application.Run(new fMain());
        }


        #region // JS ...

        public const string JS_getTag = @"
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
        //element.setAttribute('class', tid);

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
        #endregion

    }
}
