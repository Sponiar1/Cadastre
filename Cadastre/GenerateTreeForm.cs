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
    public partial class GenerateTreeForm : Form
    {
        public double[] EnteredNumbers { get; private set; }
        int typeOfInput;
        public GenerateTreeForm()
        {
            InitializeComponent();
            this.Text = "Generate data";
            typeOfInput = 0;
        }
        public GenerateTreeForm(int status)
        {
            InitializeComponent();
            this.Text = "Generate tree";
            numericUpDown5.Visible = false;
            numericUpDown7.Visible = false;
            numericUpDown8.Visible = false;
            label5.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            typeOfInput = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            double number1 = (double)numericUpDown1.Value;
            double number2 = (double)numericUpDown2.Value;
            double number3 = (double)numericUpDown3.Value;
            double number4 = (double)numericUpDown4.Value;
            double number5 = (double)numericUpDown5.Value;
            double number6 = (double)numericUpDown6.Value;
            double number7 = (double)numericUpDown7.Value;
            double number8 = (double)numericUpDown8.Value;
            double number9 = (double)numericUpDown9.Value;
            EnteredNumbers = new double[] { number1, number2, number3, number4, number5, number6, number7, number8, number9 };
            if (number1 > number3)
            {
                MessageBox.Show("Bottom X is bigger than Upper X", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (number2 > number4)
            {
                MessageBox.Show("Bottom Y is bigger than Upper Y", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
