using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace XSLT
{
    public class HTMLTag
    {
        public static oTag[] Format(string htm)
        {
            string s = htm;

            Regex rRemScript = new Regex(@"<script[^>]*>[\s\S]*?</script>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            s = rRemScript.Replace(s, string.Empty);

            s = Regex.Replace(s, "<!--.*?-->", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            s = Regex.Replace(s, @"<style>(.|\n)*?</style>", string.Empty);
            s = Regex.Replace(s, @"<xml>(.|\n)*?</xml>", string.Empty); // remove all <xml></xml> tags and anything inbetween.  
            //s = Regex.Replace(s, @"<(.|\n)*?>", string.Empty); // remove any tags but not there content "<p>bob<span> johnson</span></p>" becomes "bob johnson"

            //s = Regex.Replace(s, @"<img\s[^>]*>(?:\s*?</img>)?", "", RegexOptions.IgnoreCase);

            s = Regex.Replace(s, @"<meta\s[^>]*>(?:\s*?>)?", string.Empty, RegexOptions.IgnoreCase);
            //s = Regex.Replace(s, @"\s+", " ");
            //=======================================================================================

            List<oTag> ls = new List<oTag>() { };

            Regex r;
            MatchCollection ms;

            { // tag input
                r = new Regex(@"<(|\s+)input\s[^>]*>(?:\s*?>)?", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
                ms = r.Matches(s);
                foreach (Match m in ms)
                {
                    string item = m.ToString();
                    ls.Add(new oTag("input", eTagStatus.OK, item));
                    s = s.Replace(item, string.Empty);
                }
            } // tag input

            { // tag select
                s = Regex.Replace(s, @"<(|\s+)/(|\s+)select(|\s+)>", "</select>", RegexOptions.IgnoreCase);
                r = new Regex(@"<(|\s+)select[^>]*>[\s\S]*?<(|\s+)/(|\s+)select(|\s+)>", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
                ms = r.Matches(s);
                foreach (Match m in ms)
                {
                    string item = m.ToString().Trim();
                    string[] a = item.ToLower().Split(new string[] { "<select", "< select" }, StringSplitOptions.None);
                    switch (a.Length)
                    {
                        case 0:
                        case 1:
                            break;
                        case 2:
                            ls.Add(new oTag("select", eTagStatus.OK, item));
                            s = s.Replace(item, string.Empty);
                            break;
                        default:
                            s = s.Replace(item, string.Empty);
                            {
                                string tem = Regex.Replace(item, @"<(\s+)select", "<select", RegexOptions.IgnoreCase);

                                { // get last block <select></select> 
                                    char[] c = tem.ToCharArray().Reverse().ToArray();
                                    string met = new string(c);
                                    if (met.StartsWith(">tceles/<"))
                                    {
                                        int pos = met.IndexOf("tceles<");
                                        while (pos > 0)
                                        {
                                            string t0 = met.Substring(0, pos + 7);
                                            char[] c0 = t0.ToCharArray().Reverse().ToArray();
                                            string s0 = new string(c0);

                                            tem = tem.Replace(s0, string.Empty).Trim();
                                            met = met.Replace(t0, string.Empty).Trim();
                                            ls.Add(new oTag("select", eTagStatus.OK, s0));

                                            if (met.StartsWith(">tceles/<"))
                                                pos = met.IndexOf("tceles<");
                                            else
                                                pos = -1;
                                        }
                                    }
                                }

                                { // get tag <select> missing </select>
                                    var ri = new Regex(@"<(|\s+)select\s[^>]*>(?:\s*?>)?", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
                                    var msi = ri.Matches(tem);
                                    foreach (Match mi in msi)
                                    {
                                        string s0 = mi.ToString();
                                        ls.Add(new oTag("select", eTagStatus.MISS_CLOSE_TAG, s0));
                                    }
                                }
                            }
                            break;
                    }
                }
            } // tag select

            return ls.ToArray();
        }
        
    }
}
