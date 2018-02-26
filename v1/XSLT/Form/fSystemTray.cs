using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Dom
{
    public partial class fSystemTray : Form
    {
        public fSystemTray()
        {
            InitializeComponent();
            Stream stream = ResourceHelper.GetStream("icon.ico");
            notifyIcon1.Icon = new Icon(stream);
        }

        private void f_systemTray_Load(object sender, System.EventArgs e)
        {
            this.Width = 1;
            this.Height = 1;
            this.Location = new System.Drawing.Point(-90, -90);
            //this.notifyIcon1.Text = app.sys_name;
            //this.Text = app.sys_name;

            //app.show_Notification("Hi, " + app.username, 3000);

            //_httpServer.init();
            //app.f_http_port(_httpServer.getPortServerHTTP());
        }

        private void f_icon_Click()
        {
            //if (mApp.v_FormCurrent == null)
            //    new f_login().ShowDialog();
            //else
            //    mApp.show_Main();

            
        }

        private void notifyIcon1_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            f_icon_Click();
        }
    }
}