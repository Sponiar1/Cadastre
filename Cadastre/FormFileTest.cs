using Cadastre.Files;
using Cadastre.DataItems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cadastre.DataStructure.Templates;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Reflection;

namespace Cadastre
{
    public partial class FormFileTest : Form
    {
        DynamicHash<Property> properties;
        public FormFileTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            properties = new DynamicHash<Property>(5, "Properties_test.bin", 5, "PropertiesOverflow_test.bin");
            Random rand = new Random();
            double insert = (double)numericUpDown1.Value;
            double find = (double)numericUpDown2.Value;
            double remove = (double)numericUpDown3.Value;
            int numberOfOperations = (int)numericUpDown4.Value;

            int sizeOfItem = 100;
            double xbottom;
            double ybottom;
            List<Property> availableProperties = new List<Property>();

            GPSPosition[] gpss = new GPSPosition[2];
            xbottom = 10000 * rand.NextDouble();
            ybottom = 10000 * rand.NextDouble();
            gpss[0] = new GPSPosition('N', 'E', xbottom, ybottom);
            gpss[1] = new GPSPosition('S', 'W', xbottom + sizeOfItem, ybottom + sizeOfItem);
            Property test = new Property(0, "nic", gpss);
            properties.Insert(test);
            availableProperties.Add(test);

            double action;
            for (int i = 0; i < numberOfOperations; i++)
            {
                action = rand.NextDouble();
                if (action < insert)
                {
                    GPSPosition[] gps = new GPSPosition[2];
                    xbottom = 10000 * rand.NextDouble();
                    ybottom = 10000 * rand.NextDouble();
                    gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                    gps[1] = new GPSPosition('S', 'W', xbottom + sizeOfItem, ybottom + sizeOfItem);
                    test = new Property(i+1, "nic", gps);
                    properties.Insert(test);
                    availableProperties.Add(test);
                }
                else if (action < insert + find)
                {
                    int id = rand.Next(availableProperties.Count);
                    Property dummy = new Property(id, null, null);
                    properties.FindItem(dummy);
                }
                else
                {

                }


            }

            string[] content = properties.FileExtract();

            textBox1.Clear();
            foreach (string line in content)
            {
                textBox1.AppendText(line + Environment.NewLine);
            }
        }
    }
}
