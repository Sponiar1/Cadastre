using Cadastre.DataItems;
using Cadastre.DataStructure;
using Microsoft.VisualBasic;

namespace Cadastre
{
    public partial class Form1 : Form
    {
        QuadTree<Land> lands;
        QuadTree<Property> properties;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            FormTest formTest = new FormTest();
            formTest.Show();
            this.Enabled = true;
        }

        private void buttonFindProperty_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Please enter a number:", "Input Number", "0");

            // Check if the user provided input and it's a valid number
            if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int number))
            {
                MessageBox.Show("You entered: " + number, "Number Entered");
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter a valid number.", "Error");
            }
        }

        private void buttonFindLand_Click(object sender, EventArgs e)
        {
            using (var numberInputForm = new InputCoordinatesForm())
            {
                if (numberInputForm.ShowDialog() == DialogResult.OK)
                {
                    // Get the entered numbers after the form is closed
                    double[] enteredNumbers = numberInputForm.EnteredNumbers;

                    // Use the entered numbers as needed
                    MessageBox.Show($"Entered Numbers: {string.Join(", ", enteredNumbers)}");
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {

        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {

        }

        private void buttonNew_Click(object sender, EventArgs e)
        {

        }
    }
}