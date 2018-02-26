namespace XSLT
{
    partial class fMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.b_Export_Excel = new System.Windows.Forms.Button();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.pageMain = new System.Windows.Forms.TabPage();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.txt_HTML_Tag_Result = new System.Windows.Forms.TextBox();
            this.txt_Browser_Source = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cbo_Browser_URL = new System.Windows.Forms.ComboBox();
            this.b_Browser_File_HTML = new System.Windows.Forms.Button();
            this.b_Browser_Go = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pageXLST_HTML = new System.Windows.Forms.TabPage();
            this.txt_XLST_HTML_Text = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_XLST_HTML_File = new System.Windows.Forms.Label();
            this.b_XLST_HTML_Browser = new System.Windows.Forms.Button();
            this.pageXLST_Excel = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_XLST_Excel_File = new System.Windows.Forms.Label();
            this.b_XLST_Excel_Browser = new System.Windows.Forms.Button();
            this.txt_XLST_Excel_Text = new System.Windows.Forms.TextBox();
            this.b_Browser_Text_Save = new System.Windows.Forms.Button();
            this.tabMain.SuspendLayout();
            this.pageMain.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pageXLST_HTML.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pageXLST_Excel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // b_Export_Excel
            // 
            this.b_Export_Excel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.b_Export_Excel.Location = new System.Drawing.Point(782, 7);
            this.b_Export_Excel.Name = "b_Export_Excel";
            this.b_Export_Excel.Size = new System.Drawing.Size(85, 23);
            this.b_Export_Excel.TabIndex = 0;
            this.b_Export_Excel.Text = "Export Excel";
            this.b_Export_Excel.UseVisualStyleBackColor = true;
            this.b_Export_Excel.Click += new System.EventHandler(this.b_Export_Excel_Click);
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.pageMain);
            this.tabMain.Controls.Add(this.pageXLST_Excel);
            this.tabMain.Controls.Add(this.pageXLST_HTML);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Margin = new System.Windows.Forms.Padding(0);
            this.tabMain.Name = "tabMain";
            this.tabMain.Padding = new System.Drawing.Point(0, 0);
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(883, 498);
            this.tabMain.TabIndex = 1;
            // 
            // pageMain
            // 
            this.pageMain.Controls.Add(this.splitter1);
            this.pageMain.Controls.Add(this.txt_HTML_Tag_Result);
            this.pageMain.Controls.Add(this.txt_Browser_Source);
            this.pageMain.Controls.Add(this.panel3);
            this.pageMain.Location = new System.Drawing.Point(4, 22);
            this.pageMain.Margin = new System.Windows.Forms.Padding(0);
            this.pageMain.Name = "pageMain";
            this.pageMain.Size = new System.Drawing.Size(875, 472);
            this.pageMain.TabIndex = 0;
            this.pageMain.Text = "Main";
            this.pageMain.UseVisualStyleBackColor = true;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.Color.DarkGray;
            this.splitter1.Location = new System.Drawing.Point(446, 38);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 434);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // txt_HTML_Tag_Result
            // 
            this.txt_HTML_Tag_Result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_HTML_Tag_Result.Font = new System.Drawing.Font("Consolas", 15F, System.Drawing.FontStyle.Bold);
            this.txt_HTML_Tag_Result.Location = new System.Drawing.Point(446, 38);
            this.txt_HTML_Tag_Result.Multiline = true;
            this.txt_HTML_Tag_Result.Name = "txt_HTML_Tag_Result";
            this.txt_HTML_Tag_Result.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_HTML_Tag_Result.Size = new System.Drawing.Size(429, 434);
            this.txt_HTML_Tag_Result.TabIndex = 3;
            this.txt_HTML_Tag_Result.WordWrap = false;
            // 
            // txt_Browser_Source
            // 
            this.txt_Browser_Source.Dock = System.Windows.Forms.DockStyle.Left;
            this.txt_Browser_Source.Font = new System.Drawing.Font("Consolas", 15F);
            this.txt_Browser_Source.Location = new System.Drawing.Point(0, 38);
            this.txt_Browser_Source.Multiline = true;
            this.txt_Browser_Source.Name = "txt_Browser_Source";
            this.txt_Browser_Source.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_Browser_Source.Size = new System.Drawing.Size(446, 434);
            this.txt_Browser_Source.TabIndex = 2;
            this.txt_Browser_Source.WordWrap = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.DarkGray;
            this.panel3.Controls.Add(this.b_Browser_Text_Save);
            this.panel3.Controls.Add(this.cbo_Browser_URL);
            this.panel3.Controls.Add(this.b_Browser_File_HTML);
            this.panel3.Controls.Add(this.b_Browser_Go);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.b_Export_Excel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(875, 38);
            this.panel3.TabIndex = 1;
            // 
            // cbo_Browser_URL
            // 
            this.cbo_Browser_URL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbo_Browser_URL.BackColor = System.Drawing.Color.PeachPuff;
            this.cbo_Browser_URL.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cbo_Browser_URL.FormattingEnabled = true;
            this.cbo_Browser_URL.Location = new System.Drawing.Point(40, 7);
            this.cbo_Browser_URL.Name = "cbo_Browser_URL";
            this.cbo_Browser_URL.Size = new System.Drawing.Size(461, 21);
            this.cbo_Browser_URL.TabIndex = 5;
            this.cbo_Browser_URL.SelectedIndexChanged += new System.EventHandler(this.cbo_Browser_URL_SelectedIndexChanged);
            this.cbo_Browser_URL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbo_Browser_URL_KeyDown);
            // 
            // b_Browser_File_HTML
            // 
            this.b_Browser_File_HTML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.b_Browser_File_HTML.Location = new System.Drawing.Point(562, 7);
            this.b_Browser_File_HTML.Name = "b_Browser_File_HTML";
            this.b_Browser_File_HTML.Size = new System.Drawing.Size(117, 23);
            this.b_Browser_File_HTML.TabIndex = 4;
            this.b_Browser_File_HTML.Text = "Browser File HTML";
            this.b_Browser_File_HTML.UseVisualStyleBackColor = true;
            this.b_Browser_File_HTML.Click += new System.EventHandler(this.b_Browser_File_HTML_Click);
            // 
            // b_Browser_Go
            // 
            this.b_Browser_Go.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.b_Browser_Go.Location = new System.Drawing.Point(507, 7);
            this.b_Browser_Go.Name = "b_Browser_Go";
            this.b_Browser_Go.Size = new System.Drawing.Size(49, 23);
            this.b_Browser_Go.TabIndex = 3;
            this.b_Browser_Go.Text = "GO";
            this.b_Browser_Go.UseVisualStyleBackColor = true;
            this.b_Browser_Go.Click += new System.EventHandler(this.b_Browser_Go_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "URL";
            // 
            // pageXLST_HTML
            // 
            this.pageXLST_HTML.Controls.Add(this.txt_XLST_HTML_Text);
            this.pageXLST_HTML.Controls.Add(this.panel1);
            this.pageXLST_HTML.Location = new System.Drawing.Point(4, 22);
            this.pageXLST_HTML.Margin = new System.Windows.Forms.Padding(0);
            this.pageXLST_HTML.Name = "pageXLST_HTML";
            this.pageXLST_HTML.Size = new System.Drawing.Size(875, 472);
            this.pageXLST_HTML.TabIndex = 2;
            this.pageXLST_HTML.Text = "XLST HTML";
            this.pageXLST_HTML.UseVisualStyleBackColor = true;
            // 
            // txt_XLST_HTML_Text
            // 
            this.txt_XLST_HTML_Text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_XLST_HTML_Text.Location = new System.Drawing.Point(0, 43);
            this.txt_XLST_HTML_Text.Multiline = true;
            this.txt_XLST_HTML_Text.Name = "txt_XLST_HTML_Text";
            this.txt_XLST_HTML_Text.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_XLST_HTML_Text.Size = new System.Drawing.Size(875, 429);
            this.txt_XLST_HTML_Text.TabIndex = 2;
            this.txt_XLST_HTML_Text.WordWrap = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkGray;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lbl_XLST_HTML_File);
            this.panel1.Controls.Add(this.b_XLST_HTML_Browser);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(875, 43);
            this.panel1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Path XLST HTML";
            // 
            // lbl_XLST_HTML_File
            // 
            this.lbl_XLST_HTML_File.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_XLST_HTML_File.BackColor = System.Drawing.Color.PeachPuff;
            this.lbl_XLST_HTML_File.Location = new System.Drawing.Point(106, 7);
            this.lbl_XLST_HTML_File.Name = "lbl_XLST_HTML_File";
            this.lbl_XLST_HTML_File.Size = new System.Drawing.Size(683, 23);
            this.lbl_XLST_HTML_File.TabIndex = 1;
            // 
            // b_XLST_HTML_Browser
            // 
            this.b_XLST_HTML_Browser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.b_XLST_HTML_Browser.Location = new System.Drawing.Point(795, 7);
            this.b_XLST_HTML_Browser.Name = "b_XLST_HTML_Browser";
            this.b_XLST_HTML_Browser.Size = new System.Drawing.Size(75, 23);
            this.b_XLST_HTML_Browser.TabIndex = 0;
            this.b_XLST_HTML_Browser.Text = "Browser";
            this.b_XLST_HTML_Browser.UseVisualStyleBackColor = true;
            this.b_XLST_HTML_Browser.Click += new System.EventHandler(this.b_XLST_HTML_Browser_Click);
            // 
            // pageXLST_Excel
            // 
            this.pageXLST_Excel.Controls.Add(this.panel2);
            this.pageXLST_Excel.Controls.Add(this.txt_XLST_Excel_Text);
            this.pageXLST_Excel.Location = new System.Drawing.Point(4, 22);
            this.pageXLST_Excel.Margin = new System.Windows.Forms.Padding(0);
            this.pageXLST_Excel.Name = "pageXLST_Excel";
            this.pageXLST_Excel.Size = new System.Drawing.Size(875, 472);
            this.pageXLST_Excel.TabIndex = 1;
            this.pageXLST_Excel.Text = "XLST Excel";
            this.pageXLST_Excel.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DarkGray;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lbl_XLST_Excel_File);
            this.panel2.Controls.Add(this.b_XLST_Excel_Browser);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(875, 43);
            this.panel2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Path XLST Excel";
            // 
            // lbl_XLST_Excel_File
            // 
            this.lbl_XLST_Excel_File.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_XLST_Excel_File.BackColor = System.Drawing.Color.PeachPuff;
            this.lbl_XLST_Excel_File.Location = new System.Drawing.Point(106, 7);
            this.lbl_XLST_Excel_File.Name = "lbl_XLST_Excel_File";
            this.lbl_XLST_Excel_File.Size = new System.Drawing.Size(683, 23);
            this.lbl_XLST_Excel_File.TabIndex = 1;
            this.lbl_XLST_Excel_File.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // b_XLST_Excel_Browser
            // 
            this.b_XLST_Excel_Browser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.b_XLST_Excel_Browser.Location = new System.Drawing.Point(795, 7);
            this.b_XLST_Excel_Browser.Name = "b_XLST_Excel_Browser";
            this.b_XLST_Excel_Browser.Size = new System.Drawing.Size(75, 23);
            this.b_XLST_Excel_Browser.TabIndex = 0;
            this.b_XLST_Excel_Browser.Text = "Browser";
            this.b_XLST_Excel_Browser.UseVisualStyleBackColor = true;
            this.b_XLST_Excel_Browser.Click += new System.EventHandler(this.b_XLST_Excel_Browser_Click);
            // 
            // txt_XLST_Excel_Text
            // 
            this.txt_XLST_Excel_Text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_XLST_Excel_Text.Location = new System.Drawing.Point(0, 0);
            this.txt_XLST_Excel_Text.Multiline = true;
            this.txt_XLST_Excel_Text.Name = "txt_XLST_Excel_Text";
            this.txt_XLST_Excel_Text.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_XLST_Excel_Text.Size = new System.Drawing.Size(875, 472);
            this.txt_XLST_Excel_Text.TabIndex = 4;
            this.txt_XLST_Excel_Text.WordWrap = false;
            // 
            // b_Browser_Text_Save
            // 
            this.b_Browser_Text_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.b_Browser_Text_Save.Location = new System.Drawing.Point(685, 7);
            this.b_Browser_Text_Save.Name = "b_Browser_Text_Save";
            this.b_Browser_Text_Save.Size = new System.Drawing.Size(91, 23);
            this.b_Browser_Text_Save.TabIndex = 6;
            this.b_Browser_Text_Save.Text = "Save HTML";
            this.b_Browser_Text_Save.UseVisualStyleBackColor = true;
            this.b_Browser_Text_Save.Click += new System.EventHandler(this.b_Browser_Text_Save_Click);
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 498);
            this.Controls.Add(this.tabMain);
            this.Name = "fMain";
            this.Text = "fMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fMain_FormClosing);
            this.Load += new System.EventHandler(this.fMain_Load);
            this.tabMain.ResumeLayout(false);
            this.pageMain.ResumeLayout(false);
            this.pageMain.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.pageXLST_HTML.ResumeLayout(false);
            this.pageXLST_HTML.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pageXLST_Excel.ResumeLayout(false);
            this.pageXLST_Excel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button b_Export_Excel;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage pageMain;
        private System.Windows.Forms.TabPage pageXLST_HTML;
        private System.Windows.Forms.TabPage pageXLST_Excel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_XLST_HTML_File;
        private System.Windows.Forms.Button b_XLST_HTML_Browser;
        private System.Windows.Forms.TextBox txt_XLST_HTML_Text;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_XLST_Excel_File;
        private System.Windows.Forms.Button b_XLST_Excel_Browser;
        private System.Windows.Forms.TextBox txt_XLST_Excel_Text;
        private System.Windows.Forms.TextBox txt_Browser_Source;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button b_Browser_File_HTML;
        private System.Windows.Forms.Button b_Browser_Go;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TextBox txt_HTML_Tag_Result;
        private System.Windows.Forms.ComboBox cbo_Browser_URL;
        private System.Windows.Forms.Button b_Browser_Text_Save;
    }
}