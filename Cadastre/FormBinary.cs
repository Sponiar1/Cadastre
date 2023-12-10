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
            string userInput = Microsoft.VisualBasic.Interaction.InputBox("Enter Property ID:", "Enter ID", "");
            List<Area> result = manager.GetProperty(int.Parse(userInput));
            if(result == null)
            {
                MessageBox.Show("Property with ID " + userInput + " does not exists", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();
                InitializeTables();
                foreach (var area in result)
                {
                    dataGridView1.Rows.Add("property",area.Id, ((Land)area).Description, area.GetCoordinates(), area.GetListOfAreasID());
                }
            }
        }

        private void buttonFindLand_Click(object sender, EventArgs e)
        {
            string userInput = Microsoft.VisualBasic.Interaction.InputBox("Enter land ID:", "Enter ID", "");
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
                foreach (var area in result)
                {
                    dataGridView1.Rows.Add("Land", area.Id, ((Land)area).Description, area.GetCoordinates(), area.GetListOfAreasID());
                }
            }
        }

        private void buttonAddpropland_Click(object sender, EventArgs e)
        {
            using (var numberInputForm = new InsertForm())
            {
                if (numberInputForm.ShowDialog() == DialogResult.OK)
                {
                    double[] enteredNumbers = numberInputForm.EnteredNumbers;
                    string description = numberInputForm.Description;
                    int type = numberInputForm.TypeOfItem;
                    if (manager.AddItem(enteredNumbers, description, type))
                    {
                        MessageBox.Show("Item was inserted into register", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Item is too big", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void buttonDeleteProperty_Click(object sender, EventArgs e)
        {
            string userInput = Microsoft.VisualBasic.Interaction.InputBox("Enter Property ID to delete:", "Enter ID", "");
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
            Land result = manager.FindLand(int.Parse(userInput));
            int id = result.Id;
            string description = result.Description;
            double x0 = result.GpsLocation[0].lengthPosition;
            double y0 = result.GpsLocation[0].widthPosition;
            double x1 = result.GpsLocation[1].lengthPosition;
            double y1 = result.GpsLocation[1].widthPosition;

            using (var numberInputForm = new InsertForm(id, description, x0, y0, x1, y1))
            {
                if (numberInputForm.ShowDialog() == DialogResult.OK)
                {
                    double[] configuration = numberInputForm.EnteredNumbers;
                    GPSPosition[] gps = new GPSPosition[2];
                    gps[0] = new GPSPosition('N', 'E', configuration[0], configuration[1]);
                    gps[1] = new GPSPosition('S', 'W', configuration[2], configuration[3]);
                    description = numberInputForm.Description;
                    Land newLand = new Land((int)configuration[4], description, gps);
                    if (manager.EditLandFull(result, newLand))
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

            using (var numberInputForm = new InsertForm(id, description, x0, y0, x1, y1))
            {
                if (numberInputForm.ShowDialog() == DialogResult.OK)
                {
                    double[] configuration = numberInputForm.EnteredNumbers;
                    GPSPosition[] gps = new GPSPosition[2];
                    gps[0] = new GPSPosition('N', 'E', configuration[0], configuration[1]);
                    gps[1] = new GPSPosition('S', 'W', configuration[2], configuration[3]);
                    description = numberInputForm.Description;
                    Property newLand = new Property((int)configuration[4], description, gps);
                    if (manager.EditPropertyFull(result, newLand))
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

        }
        private void InitializeTables()
        {
            dataGridView1.Columns.Add("Type", "Type");
            dataGridView1.Columns.Add("Id", "ID");
            dataGridView1.Columns.Add("Description", "Description");
            dataGridView1.Columns.Add("Location", "Location");
            dataGridView1.Columns.Add("List", "List of related area");
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
