using Cadastre.DataItems;
using Cadastre.DataStructure;
using Cadastre.DataStructure.Templates;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

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
            using (var numberInputForm = new InputCoordinatesForm())
            {
                if (numberInputForm.ShowDialog() == DialogResult.OK)
                {
                    double[] enteredNumbers = numberInputForm.EnteredNumbers;

                    List<Property> results = properties.find(new QuadTreeRectangle(enteredNumbers[0], enteredNumbers[1], enteredNumbers[2], enteredNumbers[3]));
                    dataGridView1.Rows.Clear();
                    dataGridView1.Columns.Clear();
                    InitializeTables(1);
                    if (results.Count != 0)
                    {
                        foreach (var property in results)
                        {
                            dataGridView1.Rows.Add(property.Id, property.Description, property.getCoordinatesInReadable(), property.getListOfAreas());
                        }
                    }
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

                    List<Land> results = lands.find(new QuadTreeRectangle(enteredNumbers[0], enteredNumbers[1], enteredNumbers[2], enteredNumbers[3]));
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

                    List<Land> resultsLands = lands.find(new QuadTreeRectangle(enteredNumbers[0], enteredNumbers[1], enteredNumbers[2], enteredNumbers[3]));
                    List<Property> resultsProperties = properties.find(new QuadTreeRectangle(enteredNumbers[0], enteredNumbers[1], enteredNumbers[2], enteredNumbers[3]));
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
                    }
                    if (resultsProperties.Count != 0)
                    {
                        foreach (var property in resultsProperties)
                        {
                            dataGridView1.Rows.Add("Property", property.Id, property.Description, property.getCoordinatesInReadable(), property.getListOfAreas());
                        }
                    }
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

                    if(type == 0)
                    {
                        GPSPosition[] gps = new GPSPosition[2];
                        gps[0] = new GPSPosition('N', 'E', enteredNumbers[0], enteredNumbers[1]);
                        gps[1] = new GPSPosition('S', 'W', enteredNumbers[2], enteredNumbers[3]);
                        Land land = new Land((int)enteredNumbers[4], description, gps);
                        lands.insert(land);
                        List<Property> propertiesInArea = properties.find(new QuadTreeRectangle(enteredNumbers[0], enteredNumbers[1], enteredNumbers[2], enteredNumbers[3]));
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
                        List<Land> landsInArea = lands.find(new QuadTreeRectangle(enteredNumbers[0], enteredNumbers[1], enteredNumbers[2], enteredNumbers[3]));
                        foreach (Land land in landsInArea)
                        {
                            land.Properties.Add(property);
                            property.Lands.Add(land);
                        }
                    }
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
                    dataGridView1.Columns.Add("List", "List of items");
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


                    double xbottom = 0;
                    double ybottom = 0;
                    lands = new QuadTree<Land>(size[0], size[1], size[2], size[3], (int)size[5]);
                    properties = new QuadTree<Property>(size[0], size[1], size[2], size[3], (int)size[5]);
                    Random rand = new Random(50);

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
                        xbottom = (size[2] - size[6] / 2 - size[0]) * rand.NextDouble() + size[0];
                        ybottom = (size[3] - size[6] / 2 - size[1]) * rand.NextDouble() + size[1];
                        gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                        gps[1] = new GPSPosition('S', 'W', xbottom + size[6] / 2, ybottom + size[6] / 2);
                        Property property = new Property(i, generateString(), gps);
                        properties.insert(property);
                        List<Land> landsInArea = lands.find(new QuadTreeRectangle(xbottom, ybottom, xbottom + (size[6] / 2), ybottom + (size[6] / 2)));
                        foreach (Land land in landsInArea)
                        {
                            land.Properties.Add(property);
                            property.Lands.Add(land);
                        }
                    }
                }
            }
        }

        private string generateString()
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
        }

    }
}