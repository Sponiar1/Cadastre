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
        //DynamicHash<Property> properties;
        DynamicHash<Property> properties;
        public FormFileTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //properties = new DynamicHash<Property>(2, "Properties_test.bin", 2, "PropertiesOverflow_test.bin");
            properties = new DynamicHash<Property>(2, "Properties_test.bin", 3, "PropertiesOverflow_test.bin");
            Random rand = new Random(735494); //7 100 8
            double insert = (double)numericUpDown1.Value;
            double find = (double)numericUpDown2.Value;
            double remove = (double)numericUpDown3.Value;
            int numberOfOperations = (int)numericUpDown4.Value;

            int sizeOfItem = 100;
            double xbottom;
            double ybottom;
            List<Property> availableProperties = new List<Property>();
            int id = 1;
            Property test;
            for (int i = 0; i < 1000; i++)
            {
                GPSPosition[] gpss = new GPSPosition[2];
                xbottom = 10000 * rand.NextDouble();
                ybottom = 10000 * rand.NextDouble();
                gpss[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                gpss[1] = new GPSPosition('S', 'W', xbottom + sizeOfItem, ybottom + sizeOfItem);
                test = new Property(id, "nicnicnicnicnic", gpss);
                //test.LandsId = new List<int>() { -1, -1, -1, -1, -1, -1 };
                properties.Insert(test);
                availableProperties.Add(test);
                id++;
            }
            double action; //
            for (int i = 0; i < numberOfOperations; i++)
            {
                if (i == 33877) 
                {
                    int p = 9;
                }
                action = rand.NextDouble();
                if (action < insert)
                {
                    GPSPosition[] gps = new GPSPosition[2];
                    xbottom = 10000 * rand.NextDouble();
                    ybottom = 10000 * rand.NextDouble();
                    gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                    gps[1] = new GPSPosition('S', 'W', xbottom + sizeOfItem, ybottom + sizeOfItem);
                    test = new Property(id, "nicnicnicnicnic", gps);
                    if (!properties.Insert(test))
                    {
                        label5.Text = "Duplicate, id:" + id;
                        return;
                    }
                    id++;
                    availableProperties.Add(test);

                }
                else if (action < insert + find)
                {
                    if (availableProperties.Count != 0)
                    {
                        int idf = rand.Next(availableProperties.Count);
                        Property dummy = availableProperties[idf];
                        if (properties.FindItem(dummy) == null)
                        {
                            label5.Text = "Couldnt find item id:" + dummy.Id;
                            return;
                        }
                    }
                }
                else
                {
                    if (availableProperties.Count != 0)
                    {
                        int idf = rand.Next(availableProperties.Count);
                        Property toDelete = availableProperties[idf];
                        availableProperties.RemoveAt(idf);
                        if(toDelete.Id == 41)
                        {
                            int z = 4;
                        }
                        if (properties.DeleteItem(toDelete) == null)
                        {
                            label5.Text = "Error deleting item with id:" + toDelete.Id;
                            return;
                        }
                    }
                }/*
                string[] contenta = properties.FileExtract();
                
                foreach (string line in contenta)
                {
                    if (line != null)
                    {
                        textBox1.AppendText(line + Environment.NewLine);
                    }
                }*/

            }
            
            for (int i = 0; i < availableProperties.Count; i++)
            {

                if (properties.FindItem(availableProperties[i]) == null)
                {
                    label5.Text = "Missing item id:" + availableProperties[i].Id;
                    return;
                }
            }

            label5.Text = "Test successful";
            if (checkBox1.Checked)
            {
                string[] content = properties.FileExtract();

                textBox1.Clear();
                foreach (string line in content)
                {
                    if (line != null)
                    {
                        textBox1.AppendText(line + Environment.NewLine);
                    }
                }
                //textBox1.AppendText("NewBlock: " + properties.newBlock);
            }
        }
    }
}
