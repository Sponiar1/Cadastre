﻿using Cadastre.Files;
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
            properties = new DynamicHash<Property>(3, "Properties_test.bin", 5, "PropertiesOverflow_test.bin");
            Random rand = new Random(7);
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
            Property test = new Property(0, "nicnicnicnicnic", gpss);
            test.LandsId = new List<int>() { -1, -1, -1, -1, -1, -1 };
            properties.Insert(test);
            availableProperties.Add(test);
            int id = 1;

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
                    test = new Property(id, "nicnicnicnicnic", gps);
                    properties.Insert(test);
                    id++;
                    availableProperties.Add(test);

                }
                else if (action < 1)
                {
                    int idf = rand.Next(availableProperties.Count);
                    Property dummy = new Property(idf, null, null);
                    properties.FindItem(dummy);
                }
                else
                {

                }


            }

            for(int i = 0; i < availableProperties.Count; i++)
            {

                if (properties.Find(availableProperties[i]) == null)
                {
                    label5.Text = "Missing item id:" + i;
                    return;
                }
            }
            label5.Text = "Test successful";
            string[] content = properties.FileExtract();

            textBox1.Clear();
            foreach (string line in content)
            {
                if (line != null)
                {
                    textBox1.AppendText(line + Environment.NewLine);
                }
            }
        }
    }
}