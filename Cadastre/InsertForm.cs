using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cadastre
{
    public partial class InsertForm : Form
    {
        public double[] EnteredNumbers { get; private set; }
        public int TypeOfItem { get; private set; }
        public string Description { get; private set; }

        public InsertForm()
        {
            InitializeComponent();
            comboBox1.Items.Add("Land");
            comboBox1.Items.Add("Property");

            this.Text = "New Item";
        }

        public InsertForm(int id, string description, double x0, double y0, double x1, double y1, int type) : this()
        {
            comboBox1.Visible = false;
            numericUpDown1.Value = (decimal)x0;
            numericUpDown2.Value = (decimal)y0;
            numericUpDown3.Value = (decimal)x1;
            numericUpDown4.Value = (decimal)y1;
            numericUpDown5.Value = (decimal)id;
            textBox1.Text = description;
            switch (type)
            {
                case 0:
                    TypeOfItem = 0;
                    break;
                case 1:
                    TypeOfItem = 1;
                    break;
            }

            this.Text = "Edit item";
        }

        public InsertForm(int id, string description, double x0, double y0, double x1, double y1) : this()
        {
            comboBox1.Visible = false;
            numericUpDown1.Value = (decimal)x0;
            numericUpDown2.Value = (decimal)y0;
            numericUpDown3.Value = (decimal)x1;
            numericUpDown4.Value = (decimal)y1;
            numericUpDown5.Value = (decimal)id;
            textBox1.Text = description;

            this.Text = "Edit item";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double number1 = (double)numericUpDown1.Value;
            double number2 = (double)numericUpDown2.Value;
            double number3 = (double)numericUpDown3.Value;
            double number4 = (double)numericUpDown4.Value;
            double number5 = (double)numericUpDown5.Value;
            Description = (string)textBox1.Text;
            EnteredNumbers = new double[] { number1, number2, number3, number4, number5 };

            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    TypeOfItem = 0;
                    break;
                case 1:
                    TypeOfItem = 1;
                    break;
            }
            DialogResult = DialogResult.OK;
            Close();

        }
    }
}
