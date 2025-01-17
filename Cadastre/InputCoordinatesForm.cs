﻿using System;
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
    public partial class InputCoordinatesForm : Form
    {
        public double[] EnteredNumbers { get; private set; }
        public InputCoordinatesForm()
        {
            InitializeComponent();
            this.Text = "Input Coordinates";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double number1 = (double)numericUpDown1.Value;
            double number2 = (double)numericUpDown2.Value;
            double number3 = (double)numericUpDown3.Value;
            double number4 = (double)numericUpDown4.Value;

            EnteredNumbers = new double[] { number1, number2, number3, number4 };
            if(number1 > number3)
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
