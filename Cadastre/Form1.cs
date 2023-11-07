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
                    /*
                    List<Area> results = lands.find(new QuadTreeRectangle(enteredNumbers[0], enteredNumbers[1], enteredNumbers[2], enteredNumbers[3]));
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();
                    InitializeTables(2);
                    if (results.Count != 0)
                    {
                        foreach (var land in results)
                        {
                            dataGridView1.Rows.Add(land.Id, land.Description, land.getCoordinatesInReadable(), land.getListOfAreas());
                        }
                    }
                    lastSearchItem = results;*/
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
                    /*
                    List<Area> resultsLands = lands.find(new QuadTreeRectangle(enteredNumbers[0], enteredNumbers[1], enteredNumbers[2], enteredNumbers[3]));
                    List<Area> resultsProperties = properties.find(new QuadTreeRectangle(enteredNumbers[0], enteredNumbers[1], enteredNumbers[2], enteredNumbers[3]));
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();
                    dataGridView1.Columns.Add("Type", "Type");
                    InitializeTables(3);
                    if (resultsLands.Count != 0)
                    {
                        foreach (var land in resultsLands)
                        {
                            dataGridView1.Rows.Add("Land", land.Id, land.Description, land.getCoordinatesInReadable(), land.getListOfAreas());
                        }
                    }*/
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
                    if(cadastre.insertItem(enteredNumbers, description, type))
                    {
                        MessageBox.Show("Item was inserted into register", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Item is too big", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    /*
                    if (type == 0)
                    {
                        GPSPosition[] gps = new GPSPosition[2];
                        gps[0] = new GPSPosition('N', 'E', enteredNumbers[0], enteredNumbers[1]);
                        gps[1] = new GPSPosition('S', 'W', enteredNumbers[2], enteredNumbers[3]);
                        Land land = new Land((int)enteredNumbers[4], description, gps);
                        lands.insert(land);
                        List<Area> propertiesInArea = properties.find(new QuadTreeRectangle(enteredNumbers[0], enteredNumbers[1], enteredNumbers[2], enteredNumbers[3]));
                        foreach (Property property in propertiesInArea)
                        {
                            land.Properties.Add(property);
                            property.Lands.Add(land);
                        }
                    }
                    else
                    {
                        GPSPosition[] gps = new GPSPosition[2];
                        gps[0] = new GPSPosition('N', 'E', enteredNumbers[0], enteredNumbers[1]);
                        gps[1] = new GPSPosition('S', 'W', enteredNumbers[2], enteredNumbers[3]);
                        Property property = new Property((int)enteredNumbers[4], description, gps);
                        properties.insert(property);
                        List<Area> landsInArea = lands.find(new QuadTreeRectangle(enteredNumbers[0], enteredNumbers[1], enteredNumbers[2], enteredNumbers[3]));
                        foreach (Land land in landsInArea)
                        {
                            land.Properties.Add(property);
                            property.Lands.Add(land);
                        }
                    }*/
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
                string pattern = @"\d+";

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
                        if(cadastre.editItem(enteredNumbers, description, lastSearch, item))
                        {
                            MessageBox.Show("Item was succesfully edited", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Item is too big", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        /*
                        if (enteredNumbers[0] == item.GpsLocation[0].lengthPosition && enteredNumbers[1] == item.GpsLocation[0].widthPosition
                            && enteredNumbers[2] == item.GpsLocation[1].lengthPosition && enteredNumbers[3] == item.GpsLocation[1].lengthPosition)
                        {
                            item.Id = id;
                            item.Description = description;
                        }
                        else
                        {
                            if (lastSearch == 0)
                            {
                                List<Area> propertiesInArea = properties.find(new QuadTreeRectangle(x0, y0, x1, y1));
                                foreach (Property property in propertiesInArea)
                                {
                                    property.Lands.Remove((Land)item);
                                }
                                ((Land)item).Properties.Clear();
                                lands.remove(item);
                                GPSPosition[] gps = new GPSPosition[2];
                                gps[0] = new GPSPosition('N', 'E', enteredNumbers[0], enteredNumbers[1]);
                                gps[1] = new GPSPosition('S', 'W', enteredNumbers[2], enteredNumbers[3]);
                                item.GpsLocation = gps;
                                item.Id = (int)enteredNumbers[4];
                                item.Description = description;

                                lands.insert(item);
                                propertiesInArea = properties.find(new QuadTreeRectangle(enteredNumbers[0], enteredNumbers[1], enteredNumbers[2], enteredNumbers[3]));
                                foreach (Property property in propertiesInArea)
                                {

                                    ((Land)item).Properties.Add(property);
                                    property.Lands.Add((Land)item);
                                }
                            }
                            else
                            {
                                List<Area> landsInArea = lands.find(new QuadTreeRectangle(x0, y0, x1, y1));
                                foreach (Land land in landsInArea)
                                {

                                    land.Properties.Remove((Property)item);
                                }
                                ((Property)item).Lands.Clear();
                                properties.remove(item);

                                GPSPosition[] gps = new GPSPosition[2];
                                gps[0] = new GPSPosition('N', 'E', enteredNumbers[0], enteredNumbers[1]);
                                gps[1] = new GPSPosition('S', 'W', enteredNumbers[2], enteredNumbers[3]);
                                item.GpsLocation = gps;
                                item.Id = (int)enteredNumbers[4];
                                item.Description = description;

                                properties.insert(item);
                                landsInArea = lands.find(new QuadTreeRectangle(enteredNumbers[0], enteredNumbers[1], enteredNumbers[2], enteredNumbers[3]));
                                foreach (Land land in landsInArea)
                                {

                                    ((Property)item).Lands.Add(land);
                                    land.Properties.Add((Property)item);
                                }
                            }
                        }*/

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
                List<Area> associatedItems;
                int id = (int)selectedRow.Cells[0].Value;
                item = lastSearchItem.Find(item => item.Id == id);
                DialogResult result = MessageBox.Show("Are you sure you want to delete selected item?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if(cadastre.deleteItem(item, lastSearch))
                    {
                        MessageBox.Show("Item was successfully deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView1.Rows.Remove(selectedRow);
                    }
                    else
                    {
                        MessageBox.Show("Item was not found", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    /*
                    if (lastSearch == 0)
                    {
                        lands.remove(item);
                        associatedItems = properties.find(new QuadTreeRectangle(item.GpsLocation[0].lengthPosition, item.GpsLocation[0].widthPosition,
                                                                                item.GpsLocation[1].lengthPosition, item.GpsLocation[1].widthPosition));
                        foreach (Property property in associatedItems)
                        {
                            property.Lands.Remove((Land)item);
                        }
                        dataGridView1.Rows.Remove(selectedRow);
                    }
                    else if (lastSearch == 1)
                    {
                        properties.remove(item);
                        associatedItems = lands.find(new QuadTreeRectangle(item.GpsLocation[0].lengthPosition, item.GpsLocation[0].widthPosition,
                                                                                item.GpsLocation[1].lengthPosition, item.GpsLocation[1].widthPosition));
                        foreach (Land land in associatedItems)
                        {
                            land.Properties.Remove((Property)item);
                        }
                        dataGridView1.Rows.Remove(selectedRow);
                    }*/
                }

            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            /*handler.SaveAreaToCSV(lands, "lands.csv");
            handler.SaveAreaToCSV(properties, "properties.csv");*/
            cadastre.saveToCSV("lands.csv", "properties.csv");
        }
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            /*QuadTree<Area>[] trees=  handler.LoadTreeFromCSV("lands.csv", "properties.csv");
            lands = trees[0];
            properties = trees[1];*/
            cadastre.loadFromCSV("lands.csv", "properties.csv");
        }
        private void buttonNew_Click(object sender, EventArgs e)
        {
            using (var numberInputForm = new GenerateTreeForm(1))
            {
                if (numberInputForm.ShowDialog() == DialogResult.OK)
                {
                    double[] size = numberInputForm.EnteredNumbers; 
                    
                    cadastre.GenerateEmptyTrees(size);
                    /*lands = new QuadTree<Area>(size[0], size[1], size[2], size[3], (int)size[5]);
                    properties = new QuadTree<Area>(size[0], size[1], size[2], size[3], (int) size[5]);*/
                    
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
                    /*double xbottom = 0;
                    double ybottom = 0;
                    lands = new QuadTree<Area>(size[0], size[1], size[2], size[3], (int)size[5]);
                    properties = new QuadTree<Area>(size[0], size[1], size[2], size[3], (int)size[5]);
                    Random rand = new Random();

                    for (int i = 0; i < (int)size[4]; i++)
                    {
                        GPSPosition[] gps = new GPSPosition[2];
                        xbottom = (size[2] - size[6] - size[0]) * rand.NextDouble() + size[0];
                        ybottom = (size[3] - size[6] - size[1]) * rand.NextDouble() + size[1];
                        gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                        gps[1] = new GPSPosition('S', 'W', xbottom + size[6], ybottom + size[6]);
                        Land land = new Land(i, generateString(), gps);
                        lands.insert(land);

                    }

                    for (int i = 0; i < (int)size[4] * 10; i++)
                    {
                        GPSPosition[] gps = new GPSPosition[2];
                        xbottom = (size[2] - size[7] - size[0]) * rand.NextDouble() + size[0];
                        ybottom = (size[3] - size[7] - size[1]) * rand.NextDouble() + size[1];
                        gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                        gps[1] = new GPSPosition('S', 'W', xbottom + size[7], ybottom + size[7]);
                        Property property = new Property(i, generateString(), gps);
                        properties.insert(property);
                        List<Area> landsInArea = lands.find(new QuadTreeRectangle(xbottom, ybottom, xbottom + (size[7]), ybottom + (size[7])));
                        foreach (Land land in landsInArea)
                        {
                            land.Properties.Add(property);
                            property.Lands.Add(land);
                        }
                    }*/
                }
            }
        }
        /*private string generateString()
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            Random random = new Random();

            int length = random.Next(6, 20);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(characters.Length);
                sb.Append(characters[index]);
            }

            string randomString = sb.ToString();
            return randomString;
        }*/

    }
}