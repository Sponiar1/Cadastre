using Cadastre.CadastreManager;
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
        CadastreBinaryManager manager;
        public FormBinary()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            FormFileTest formTest = new FormFileTest();
            formTest.Show();
            this.Enabled = true;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (manager != null)
            {
                manager.Save();
                label1.Text = "Data saved to file";
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            manager = new CadastreBinaryManager();
            manager.Load();
            label1.Text = "Data loaded from files";
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            manager = new CadastreBinaryManager();
            manager.NewFile();
            label1.Text = "New database created";
        }

        private void buttonFindProperty_Click(object sender, EventArgs e)
        {

        }

        private void buttonFindLand_Click(object sender, EventArgs e)
        {

        }

        private void buttonAddpropland_Click(object sender, EventArgs e)
        {

        }

        private void buttonDeleteProperty_Click(object sender, EventArgs e)
        {

        }

        private void buttonEditLand_Click(object sender, EventArgs e)
        {

        }

        private void buttonEditProperty_Click(object sender, EventArgs e)
        {

        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {

        }
    }
}
