using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Dom
{
    public class fViewSource: Form
    {
        public fViewSource() {
            RichTextBox rtbView = new RichTextBox();
            rtbView.Dock = DockStyle.Fill;
            //rtbView.Font = new Font("Consolas", 19.0F);
            rtbView.Font = new Font("Arial", 19.0F);
            this.Controls.Add(rtbView);

            //demo4(rtbView, "demo.html");
            demo3(rtbView, "demo.html");
            //demo3(rtbView, "demo.htm");
            //demo1(rtbView, "demo.html");
            //demo2(rtbInput);

            this.WindowState = FormWindowState.Maximized;
        }

        void demo4(RichTextBox richTextBox, string file)
        {
            string s = File.ReadAllText(file);
            CodeSyntaxHighlighter.AddColouredText(richTextBox, s);
        }

        void demo3(RichTextBox richTextBox, string file)
        {
            string s = File.ReadAllText(file);
            string sr = CodeSyntaxHighlighter.Process(s);
            richTextBox.Rtf = sr;
        }

        void demo1(RichTextBox richTextBox, string file)
        {
            string s = File.ReadAllText(file); ;
            richTextBox.Text = s;

            var syntaxHighlighter = new SyntaxHighlighter(richTextBox);

            // multi-line comments
            syntaxHighlighter.AddPattern(new PatternDefinition(new Regex(@"/\*(.|[\r\n])*?\*/", RegexOptions.Multiline | RegexOptions.Compiled)), new SyntaxStyle(Color.DarkSeaGreen, false, true));
            // singlie-line comments
            syntaxHighlighter.AddPattern(new PatternDefinition(new Regex(@"//.*?$", RegexOptions.Multiline | RegexOptions.Compiled)), new SyntaxStyle(Color.Green, false, true));
            // numbers
            syntaxHighlighter.AddPattern(new PatternDefinition(@"\d+\.\d+|\d+"), new SyntaxStyle(Color.Purple));
            // double quote strings
            syntaxHighlighter.AddPattern(new PatternDefinition(@"\""([^""]|\""\"")+\"""), new SyntaxStyle(Color.Red));
            // single quote strings
            syntaxHighlighter.AddPattern(new PatternDefinition(@"\'([^']|\'\')+\'"), new SyntaxStyle(Color.Salmon));
            // keywords1
            syntaxHighlighter.AddPattern(new PatternDefinition("for", "foreach", "int", "var"), new SyntaxStyle(Color.Blue));
            // keywords2
            syntaxHighlighter.AddPattern(new CaseInsensitivePatternDefinition("public", "partial", "class", "void"), new SyntaxStyle(Color.Navy, true, false));
            // operators
            syntaxHighlighter.AddPattern(new PatternDefinition("+", "-", ">", "<", "&", "|"), new SyntaxStyle(Color.Brown));
        }


        void demo2(RichTextBox richTextBox, string file)
        {
            string s = File.ReadAllText(file); ;
            richTextBox.Text = s;

            var syntaxHighlighter = new SyntaxHighlighter(richTextBox);

            // multi-line comments
            syntaxHighlighter.AddPattern(new PatternDefinition(new Regex(@"/\*(.|[\r\n])*?\*/", RegexOptions.Multiline | RegexOptions.Compiled)), new SyntaxStyle(Color.DarkSeaGreen, false, true));
            // singlie-line comments
            syntaxHighlighter.AddPattern(new PatternDefinition(new Regex(@"//.*?$", RegexOptions.Multiline | RegexOptions.Compiled)), new SyntaxStyle(Color.Green, false, true));
            // numbers
            syntaxHighlighter.AddPattern(new PatternDefinition(@"\d+\.\d+|\d+"), new SyntaxStyle(Color.Purple));
            // double quote strings
            syntaxHighlighter.AddPattern(new PatternDefinition(@"\""([^""]|\""\"")+\"""), new SyntaxStyle(Color.Red));
            // single quote strings
            syntaxHighlighter.AddPattern(new PatternDefinition(@"\'([^']|\'\')+\'"), new SyntaxStyle(Color.Salmon));
            // keywords1
            syntaxHighlighter.AddPattern(new PatternDefinition("for", "foreach", "int", "var"), new SyntaxStyle(Color.Blue));
            // keywords2
            syntaxHighlighter.AddPattern(new CaseInsensitivePatternDefinition("public", "partial", "class", "void"), new SyntaxStyle(Color.Navy, true, false));
            // operators
            syntaxHighlighter.AddPattern(new PatternDefinition("+", "-", ">", "<", "&", "|"), new SyntaxStyle(Color.Brown));
        }



    }
}
