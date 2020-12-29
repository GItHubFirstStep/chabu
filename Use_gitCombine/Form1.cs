using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Use_gitCombine
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            line li= new line();
            li.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            poly po = new poly();
            po.Show();
        }
    }
}
