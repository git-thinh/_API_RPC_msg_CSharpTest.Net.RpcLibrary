using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Dom
{
    public partial class BaseForm: Form
    {
        public BaseForm()
        {
            this.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            Stream stream = ResourceHelper.GetStream("icon.ico");
            this.Icon = new Icon(stream);

            this.FormClosed += (sender, e) => {
                main.Exit();
            };
        }        
    }
}
