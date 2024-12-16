using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASAMonitor
{
    public partial class MoreThanOneServer : Form
    {
        public Guna2TextBox TextBox1
        {
            get { return guna2TextBox1; }
        }

        public MoreThanOneServer()
        {
            InitializeComponent();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
