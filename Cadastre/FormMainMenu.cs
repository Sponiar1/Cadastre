using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cadastre
{
    public partial class FormMainMenu : Form
    {
        public FormMainMenu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            Form1 formTest = new Form1();
            formTest.Show();
            this.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            FormBinary formTest = new FormBinary();
            formTest.Show();
            this.Enabled = true;
        }
    }
}
