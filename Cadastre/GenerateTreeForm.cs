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
        public GenerateTreeForm()
        {
            InitializeComponent();
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

            EnteredNumbers = new double[] { number1, number2, number3, number4, number5, number6, number7 };

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
