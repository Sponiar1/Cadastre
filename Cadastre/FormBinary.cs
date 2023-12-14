using Cadastre.CadastreManager;
using Cadastre.DataItems;
using Cadastre.DataStructure.Templates;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Cadastre
{
    public partial class FormBinary : Form
    {
        CadastreBinaryManager manager;
        double precision = 0.0001;
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
            string userInput = Microsoft.VisualBasic.Interaction.InputBox("Enter block factor:", "Enter factor", "");
            if (userInput == "")
            {
                return;
            }
            int blockFactor = int.Parse(userInput);
            userInput = Microsoft.VisualBasic.Interaction.InputBox("Enter block factor overflow:", "Enter factor", "");
            if (userInput == "")
            {
                return;
            }
            manager = new CadastreBinaryManager();
            manager.NewFile(blockFactor, int.Parse(userInput));
            label1.Text = "New database created";
        }

        private void buttonFindProperty_Click(object sender, EventArgs e)
        {
            string userInput = Microsoft.VisualBasic.Interaction.InputBox("Enter Property ID:", "Enter ID", "");
            if (userInput == "")
            {
                return;
            }
            List<Area> result = manager.GetProperty(int.Parse(userInput));
            if (result == null)
            {
                MessageBox.Show("Property with ID " + userInput + " does not exists", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();
                InitializeTables();
                dataGridView1.Rows.Add("Property", result[0].Id, ((Property)result[0]).RegisterNumber, ((Property)result[0]).Description, result[0].GetCoordinates(), result[0].GetListOfAreasID());
                for (int i = 1; i < result.Count; i++)
                {
                    dataGridView1.Rows.Add("Land", result[i].Id, "", ((Land)result[i]).Description, result[i].GetCoordinates(), result[i].GetListOfAreasID());
                }
            }
        }

        private void buttonFindLand_Click(object sender, EventArgs e)
        {
            string userInput = Microsoft.VisualBasic.Interaction.InputBox("Enter land ID:", "Enter ID", "");
            if (userInput == "")
            {
                return;
            }
            List<Area> result = manager.GetLand(int.Parse(userInput));
            if (result == null)
            {
                MessageBox.Show("Land with ID " + userInput + " does not exists", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();
                InitializeTables();
                dataGridView1.Rows.Add("Land", result[0].Id, "", ((Land)result[0]).Description, result[0].GetCoordinates(), result[0].GetListOfAreasID());
                for (int i = 1; i < result.Count; i++)
                {
                    dataGridView1.Rows.Add("Property", result[i].Id, ((Property)result[i]).RegisterNumber, ((Property)result[i]).Description, result[i].GetCoordinates(), result[i].GetListOfAreasID());
                }
            }
        }

        private void buttonAddpropland_Click(object sender, EventArgs e)
        {
            using (var numberInputForm = new InsertForm(1))
            {
                if (numberInputForm.ShowDialog() == DialogResult.OK)
                {
                    double[] enteredNumbers = numberInputForm.EnteredNumbers;
                    string description = numberInputForm.Description;
                    int type = numberInputForm.TypeOfItem;
                    int result = manager.AddItem(enteredNumbers, description, type);
                    if (result != -1)
                    {
                        MessageBox.Show("Item was inserted into register with ID" + result, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Item is too big and cannot cover related areas", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void buttonDeleteProperty_Click(object sender, EventArgs e)
        {
            string userInput = Microsoft.VisualBasic.Interaction.InputBox("Enter Property ID to delete:", "Enter ID", "");
            if (userInput == "")
            {
                return;
            }
            bool result = manager.DeleteProperty(int.Parse(userInput));
            if (result)
            {
                MessageBox.Show("Property with ID " + userInput + " was deleted", "Delete successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Property with ID " + userInput + " does not exists", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonDeleteLand_Click(object sender, EventArgs e)
        {
            string userInput = Microsoft.VisualBasic.Interaction.InputBox("Enter Property ID to delete:", "Enter ID", "");
            if (userInput == "")
            {
                return;
            }
            bool result = manager.DeleteLand(int.Parse(userInput));
            if (result)
            {
                MessageBox.Show("Land with ID " + userInput + " was deleted", "Delete successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Land with ID " + userInput + " does not exists", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonEditLand_Click(object sender, EventArgs e)
        {
            string userInput = Microsoft.VisualBasic.Interaction.InputBox("Enter land ID you wish to edit:", "Enter ID", "");
            if (userInput == "")
            {
                return;
            }
            Land result = manager.FindLand(int.Parse(userInput));
            int id = result.Id;
            string description = result.Description;
            double x0 = result.GpsLocation[0].lengthPosition;
            double y0 = result.GpsLocation[0].widthPosition;
            double x1 = result.GpsLocation[1].lengthPosition;
            double y1 = result.GpsLocation[1].widthPosition;
            bool success = false;
            using (var numberInputForm = new InsertForm(id, description, x0, y0, x1, y1,0,0))
            {
                if (numberInputForm.ShowDialog() == DialogResult.OK)
                {
                    double[] configuration = numberInputForm.EnteredNumbers;
                    description = numberInputForm.Description;
                    if (Math.Abs(x0 - configuration[0]) < precision && Math.Abs(y0 - configuration[1]) < precision
                        && Math.Abs(x1 - configuration[2]) < precision && Math.Abs(y1 - configuration[3]) < precision)
                    {
                        result.Description = description;
                        success = manager.EditLand(result);
                    }
                    else
                    {
                        GPSPosition[] gps = new GPSPosition[2];
                        gps[0] = new GPSPosition('N', 'E', configuration[0], configuration[1]);
                        gps[1] = new GPSPosition('S', 'W', configuration[2], configuration[3]);
                        Land newLand = new Land(result.Id, description, gps);
                        success = manager.EditLandFull(result, newLand);
                    }
                    if (success)
                    {
                        MessageBox.Show("Item was succesfully edited", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Item could not be edited, related properties are full", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void buttonEditProperty_Click(object sender, EventArgs e)
        {
            string userInput = Microsoft.VisualBasic.Interaction.InputBox("Enter property ID you wish to edit:", "Enter ID", "");
            Property result = manager.FindProperty(int.Parse(userInput));
            int id = result.Id;
            string description = result.Description;
            double x0 = result.GpsLocation[0].lengthPosition;
            double y0 = result.GpsLocation[0].widthPosition;
            double x1 = result.GpsLocation[1].lengthPosition;
            double y1 = result.GpsLocation[1].widthPosition;
            bool success = false;
            using (var numberInputForm = new InsertForm(id, description, x0, y0, x1, y1, 1, result.RegisterNumber))
            {
                if (numberInputForm.ShowDialog() == DialogResult.OK)
                {
                    double[] configuration = numberInputForm.EnteredNumbers;
                    description = numberInputForm.Description;
                    double si = Math.Abs(x0 - configuration[0]);
                    double pi = double.Epsilon;
                    if (si > pi)
                    {
                        double sip = si-pi;
                    }
                    if (Math.Abs(x0 - configuration[0]) < precision && Math.Abs(y0 - configuration[1]) < precision
                        && Math.Abs(x1 - configuration[2]) < precision && Math.Abs(y1 - configuration[3]) < precision)
                    {
                        result.Description = description;
                        result.RegisterNumber = (int)configuration[5];
                        success = manager.EditProperty(result);
                    }
                    else
                    {
                        GPSPosition[] gps = new GPSPosition[2];
                        gps[0] = new GPSPosition('N', 'E', configuration[0], configuration[1]);
                        gps[1] = new GPSPosition('S', 'W', configuration[2], configuration[3]);
                        description = numberInputForm.Description;
                        Property newLand = new Property((int)configuration[4], description, gps);
                        success = manager.EditPropertyFull(result, newLand);
                    }
                    if (success)
                    {
                        MessageBox.Show("Item was succesfully edited", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Item could not be edited, related lands are full", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            using (var numberInputForm = new GenerateTreeForm())
            {
                if (numberInputForm.ShowDialog() == DialogResult.OK)
                {
                    double[] size = numberInputForm.EnteredNumbers;
                    manager.GenerateData(size);
                }
            }
        }
        private void InitializeTables()
        {
            dataGridView1.Columns.Add("Type", "Type");
            dataGridView1.Columns.Add("Id", "ID");
            dataGridView1.Columns.Add("Register", "Register");
            dataGridView1.Columns.Add("Description", "Description");
            dataGridView1.Columns.Add("Location", "Location");
            dataGridView1.Columns.Add("List", "List of related area");
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void buttonFileExtract_Click(object sender, EventArgs e)
        {
            string[] content = manager.FileExtract(0);
            textBox1.Clear();
            foreach (string line in content)
            {
                if (line != null)
                {
                    textBox1.AppendText(line + Environment.NewLine);
                }
            }

            content = manager.FileExtract(1);
            textBox2.Clear();
            foreach (string line in content)
            {
                if (line != null)
                {
                    textBox2.AppendText(line + Environment.NewLine);
                }
            }
        }
    }
}

