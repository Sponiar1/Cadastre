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
