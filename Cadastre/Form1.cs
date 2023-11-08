using Cadastre.CadastreManager;
using Cadastre.DataItems;
using Cadastre.DataStructure;
using Cadastre.DataStructure.Templates;
using Cadastre.FileManager;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Cadastre
{
    public partial class Form1 : Form
    {
        int lastSearch;
        List<Area> lastSearchItem;
        CSVHandler handler;
        CadastreHandler cadastre;
        public Form1()
        {
            InitializeComponent();
            handler = new CSVHandler();
            cadastre = new CadastreHandler();
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
            using (var numberInputForm = new InputCoordinatesForm())
            {
                if (numberInputForm.ShowDialog() == DialogResult.OK)
                {
                    double[] enteredNumbers = numberInputForm.EnteredNumbers;

                    lastSearchItem = cadastre.findProperty(enteredNumbers);
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();
                    InitializeTables(1);
                    if (lastSearchItem.Count != 0)
                    {
                        foreach (var property in lastSearchItem)
                        {
                            dataGridView1.Rows.Add(property.Id, property.Description, property.getCoordinatesInReadable(), property.getListOfAreas());
                        }
                    }
                    lastSearch = 1;
                }
            }
        }
        private void buttonFindLand_Click(object sender, EventArgs e)
        {
            using (var numberInputForm = new InputCoordinatesForm())
            {
                if (numberInputForm.ShowDialog() == DialogResult.OK)
                {
                    double[] enteredNumbers = numberInputForm.EnteredNumbers;
                    lastSearchItem = cadastre.findLands(enteredNumbers);
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();
                    InitializeTables(2);
                    if (lastSearchItem.Count != 0)
                    {
                        foreach (var land in lastSearchItem)
                        {
                            dataGridView1.Rows.Add(land.Id, land.Description, land.getCoordinatesInReadable(), land.getListOfAreas());
                        }
                    }
                    lastSearch = 0;
                }
            }
        }
        private void buttonFindAll_Click(object sender, EventArgs e)
        {
            using (var numberInputForm = new InputCoordinatesForm())
            {
                if (numberInputForm.ShowDialog() == DialogResult.OK)
                {
                    double[] enteredNumbers = numberInputForm.EnteredNumbers;
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();
                    dataGridView1.Columns.Add("Type", "Type");
                    InitializeTables(3);
                    List<Area>[] result = cadastre.findAll(enteredNumbers);
                    if (result[0].Count != 0)
                    {
                        foreach (var area in result[0])
                        {
                            dataGridView1.Rows.Add("Land", area.Id, area.Description, area.getCoordinatesInReadable(), area.getListOfAreas());
                        }
                    }
                    if (result[1].Count != 0)
                    {
                        foreach (var area in result[1])
                        {
                            dataGridView1.Rows.Add("Property", area.Id, area.Description, area.getCoordinatesInReadable(), area.getListOfAreas());
                        }
                    }
                    lastSearchItem = result[0].Concat(result[1]).ToList();
                    lastSearch = 2;
                }
            }
        }
        private void buttonInsert_Click(object sender, EventArgs e)
        {
            using (var numberInputForm = new InsertForm())
            {
                if (numberInputForm.ShowDialog() == DialogResult.OK)
                {
                    double[] enteredNumbers = numberInputForm.EnteredNumbers;
                    string description = numberInputForm.Description;
                    int type = numberInputForm.TypeOfItem;
                    if (cadastre.insertItem(enteredNumbers, description, type))
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
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && lastSearch != 2)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                int id = (int)selectedRow.Cells[0].Value;
                string description = selectedRow.Cells[1].Value.ToString();

                Area item = lastSearchItem.Find(item => item.Id == id);

                double x0 = item.GpsLocation[0].lengthPosition;
                double y0 = item.GpsLocation[0].widthPosition;
                double x1 = item.GpsLocation[1].lengthPosition;
                double y1 = item.GpsLocation[1].widthPosition;

                using (var numberInputForm = new InsertForm(id, description, x0, y0, x1, y1, lastSearch))
                {
                    if (numberInputForm.ShowDialog() == DialogResult.OK)
                    {
                        double[] enteredNumbers = numberInputForm.EnteredNumbers;
                        description = numberInputForm.Description;
                        if (cadastre.editItem(enteredNumbers, description, lastSearch, item))
                        {
                            MessageBox.Show("Item was succesfully edited", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Item is too big", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && lastSearch != 2)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                Area item;
                int id = (int)selectedRow.Cells[0].Value;
                item = lastSearchItem.Find(item => item.Id == id);
                DialogResult result = MessageBox.Show("Are you sure you want to delete selected item?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (cadastre.deleteItem(item, lastSearch))
                    {
                        MessageBox.Show("Item was successfully deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView1.Rows.Remove(selectedRow);
                    }
                    else
                    {
                        MessageBox.Show("Item was not found", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            cadastre.saveToCSV("lands.csv", "properties.csv");
        }
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            cadastre.loadFromCSV("lands.csv", "properties.csv");
            changeArea();
        }
        private void buttonNew_Click(object sender, EventArgs e)
        {
            using (var numberInputForm = new GenerateTreeForm(1))
            {
                if (numberInputForm.ShowDialog() == DialogResult.OK)
                {
                    double[] size = numberInputForm.EnteredNumbers;
                    cadastre.GenerateEmptyTrees(size);
                    changeArea();
                }
            }
        }
        private void InitializeTables(int operation)
        {
            dataGridView1.Columns.Add("Id", "ID");
            dataGridView1.Columns.Add("Description", "Description");
            dataGridView1.Columns.Add("Location", "Location");
            switch (operation)
            {
                case 1:
                    dataGridView1.Columns.Add("List", "List of lands");
                    break;
                case 2:
                    dataGridView1.Columns.Add("List", "List of properties");
                    break;
                case 3:
                    dataGridView1.Columns.Add("List", "List of items in same coordinates");
                    break;
            }
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            using (var numberInputForm = new GenerateTreeForm())
            {
                if (numberInputForm.ShowDialog() == DialogResult.OK)
                {
                    double[] size = numberInputForm.EnteredNumbers;
                    cadastre.GenerateCadastre(size);
                    changeArea();
                }
            }
        }
        private void changeArea()
        {
            double[] size = cadastre.getSize();
            labelArea.Text = "Area[" + size[0] + " , " + size[1] + "] [" + size[2] + " , " + size[3] + "]";
        }
    }
}