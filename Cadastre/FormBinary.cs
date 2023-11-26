using Cadastre.DataItems;
using Cadastre.Files;
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
    public partial class FormBinary : Form
    {
        public FormBinary()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DynamicHash<Area> hash = new DynamicHash<Area>(5, "Land.bin", 5, "OverFlowLand.bin");
            /*hash.TestCreateFile();*/
            hash.TestReadFile();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            FormFileTest formTest = new FormFileTest();
            formTest.Show();
            this.Enabled = true;
        }
    }
}
