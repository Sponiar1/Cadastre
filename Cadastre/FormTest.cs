using Cadastre.DataItems;
using Cadastre.DataStructure.Templates;
using Cadastre.DataStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cadastre
{
    public partial class FormTest : Form
    {
        public FormTest()
        {
            InitializeComponent();
            comboBox1.Items.Add("Insert");
            comboBox1.Items.Add("Delete");
            comboBox1.Items.Add("Find");
            comboBox1.Items.Add("Mix");
            comboBox1.Items.Add("Change Height");
            comboBox1.Items.Add("Check Health");
            comboBox1.Items.Add("Reorganize");
            labelInsert.Visible = false;
            numericUpDownInsert.Visible = false;
            labelRemove.Visible = false;
            numericUpDownDelete.Visible = false;
            labelFind.Visible = false;
            numericUpDownFind.Visible = false;
            labelNewHeight.Visible = false;
            numericUpDownNewHeight.Visible = false;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    InsertTest();
                    break;
                case 1:
                    RemoveTest();
                    break;
                case 2:
                    FindTest();
                    break;
                case 3:
                    if (numericUpDownDelete.Value + numericUpDownFind.Value + numericUpDownInsert.Value != 1)
                    {
                        labelResult.Text = "Sum of chances has to be 1";
                    }
                    else
                    {
                        MixTest();
                    }
                    break;
                case 4:
                    ChangeHeightTest();
                    break;
                case 5:
                    CheckHealthTest();
                    break;
                case 6:
                    ReorganizeTest();
                    break;
            }
        }

        private void InsertTest()
        {
            int initialSize = (int)numericUpDownStart.Value;
            int sizeOfTree = (int)numericUpDownSize.Value;
            int height = (int)numericUpDownHeight.Value;
            int numberOfOperations = (int)numericUpDownOperations.Value;
            int sizeOfItem = (int)numericUpDownItemSize.Value;
            if (sizeOfItem > sizeOfTree)
            {
                sizeOfItem = sizeOfTree;
            }

            double xbottom = 0;
            double ybottom = 0;
            int xupper = 0;
            int yupper = 0;
            QuadTree<Area> tree = new QuadTree<Area>(0, 0, sizeOfTree, sizeOfTree, height);
            Random rand = new Random();
            List<Area> items = new List<Area>();
            //Random rand = new Random(201,30);
            if (sizeOfItem != 0)
            {
                for (int i = 0; i < initialSize + numberOfOperations; i++)
                {
                    GPSPosition[] gps = new GPSPosition[2];
                    xbottom = (sizeOfTree - sizeOfItem) * (rand.NextDouble() * (1 - double.Epsilon));
                    ybottom = (sizeOfTree - sizeOfItem) * (rand.NextDouble() * (1 - double.Epsilon));
                    gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                    gps[1] = new GPSPosition('S', 'W', xbottom + sizeOfItem, ybottom + sizeOfItem);
                    Area test = new Area(i, "nic", gps);
                    tree.insert(test);
                    items.Add(test);
                }
            }
            else
            {
                for (int i = 0; i < initialSize + numberOfOperations; i++)
                {
                    GPSPosition[] gps = new GPSPosition[2];
                    xbottom = (sizeOfTree) * (rand.NextDouble() * (1 - double.Epsilon));
                    ybottom = (sizeOfTree) * (rand.NextDouble() * (1 - double.Epsilon));
                    gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                    gps[1] = new GPSPosition('S', 'W', rand.NextDouble() * (sizeOfTree - xbottom) + xbottom, rand.NextDouble() * (sizeOfTree - ybottom) + ybottom);
                    Area test = new Area(i, "nic", gps);
                    tree.insert(test);
                    items.Add(test);
                }
            }
            List<Area> treeItems = tree.find(new QuadTreeRectangle(0, 0, sizeOfTree, sizeOfTree));
            if (checkBoxItems.Checked)
            {
                for (int i = 0; i < numberOfOperations + sizeOfTree; i++)
                {
                    Area foundArea = treeItems.Find(item => item.Id == i);
                    if (foundArea == null)
                    {
                        labelResult.Text = "Tree is missing items";
                    }
                }
                labelResult.Text = "Test successful";
            }
            else
            {
                if (treeItems.Count == items.Count)
                {
                    labelResult.Text = "Test successful";
                }
                else if (treeItems.Count > items.Count)
                {
                    labelResult.Text = "Tree contains duplicates";
                }
                else
                {
                    labelResult.Text = "Tree is missing items";
                }
            }


        }

        private void RemoveTest()
        {
            int initialSize = (int)numericUpDownStart.Value;
            int sizeOfTree = (int)numericUpDownSize.Value;
            int height = (int)numericUpDownHeight.Value;
            int numberOfOperations = (int)numericUpDownOperations.Value;
            int sizeOfItem = (int)numericUpDownItemSize.Value;
            if (sizeOfItem > sizeOfTree)
            {
                sizeOfItem = sizeOfTree;
            }

            double xbottom = 0;
            double ybottom = 0;
            int xupper = 0;
            int yupper = 0;
            QuadTree<Area> tree = new QuadTree<Area>(0, 0, sizeOfTree, sizeOfTree, height);
            Random rand = new Random();
            List<Area> items = new List<Area>();
            if (sizeOfItem != 0)
            {
                for (int i = 0; i < initialSize; i++)
                {
                    GPSPosition[] gps = new GPSPosition[2];
                    xbottom = (sizeOfTree - sizeOfItem) * (rand.NextDouble() * (1 - double.Epsilon));
                    ybottom = (sizeOfTree - sizeOfItem) * (rand.NextDouble() * (1 - double.Epsilon));
                    gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                    gps[1] = new GPSPosition('S', 'W', xbottom + sizeOfItem, ybottom + sizeOfItem);
                    Area test = new Area(i, "nic", gps);
                    tree.insert(test);
                    items.Add(test);
                }
            }
            else
            {
                for (int i = 0; i < initialSize; i++)
                {
                    GPSPosition[] gps = new GPSPosition[2];
                    xbottom = (sizeOfTree) * (rand.NextDouble() * (1 - double.Epsilon));
                    ybottom = (sizeOfTree) * (rand.NextDouble() * (1 - double.Epsilon));
                    gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                    gps[1] = new GPSPosition('S', 'W', rand.NextDouble() * (sizeOfTree - xbottom) + xbottom, rand.NextDouble() * (sizeOfTree - ybottom) + ybottom);
                    Area test = new Area(i, "nic", gps);
                    tree.insert(test);
                    items.Add(test);
                }
            }

            Area helpArea;
            int index;
            for (int i = 0; i < numberOfOperations; i++)
            {
                index = rand.Next(items.Count);
                helpArea = items[index];
                items.RemoveAt(index);
                tree.remove(helpArea);
            }

            List<Area> treeItems = tree.find(new QuadTreeRectangle(0, 0, sizeOfTree, sizeOfTree));
            if (checkBoxItems.Checked)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    helpArea = items[i];
                    if (items.Find(item => item.Id == helpArea.Id) != helpArea)
                    {
                        labelResult.Text = "Tree is missing items";
                    }
                }
                labelResult.Text = "Test successful";
            }
            else
            {
                if (treeItems.Count == items.Count)
                {
                    labelResult.Text = "Test successful";
                }
                else if (treeItems.Count > items.Count)
                {
                    labelResult.Text = "Tree contains duplicates";
                }
                else
                {
                    labelResult.Text = "Tree is missing items";
                }
            }
        }

        private void FindTest()
        {
            int initialSize = (int)numericUpDownStart.Value;
            int sizeOfTree = (int)numericUpDownSize.Value;
            int height = (int)numericUpDownHeight.Value;
            int numberOfOperations = (int)numericUpDownOperations.Value;
            int sizeOfItem = (int)numericUpDownItemSize.Value;
            if (sizeOfItem > sizeOfTree)
            {
                sizeOfItem = sizeOfTree;
            }

            double xbottom = 0;
            double ybottom = 0;
            int xupper = 0;
            int yupper = 0;
            QuadTree<Area> tree = new QuadTree<Area>(0, 0, sizeOfTree, sizeOfTree, height);

            Random rand = new Random(30);
            List<Area> items = new List<Area>();
            List<Area> itemsToFind = new List<Area>();
            if (sizeOfItem != 0)
            {
                for (int i = 0; i < initialSize; i++)
                {
                    GPSPosition[] gps = new GPSPosition[2];
                    xbottom = (sizeOfTree - sizeOfItem) * (rand.NextDouble() * (1 - double.Epsilon));
                    ybottom = (sizeOfTree - sizeOfItem) * (rand.NextDouble() * (1 - double.Epsilon));
                    gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                    gps[1] = new GPSPosition('S', 'W', xbottom + sizeOfItem, ybottom + sizeOfItem);
                    Area test = new Area(i, "nic", gps);
                    tree.insert(test);
                    items.Add(test);
                }
            }
            else
            {
                for (int i = 0; i < initialSize; i++)
                {
                    GPSPosition[] gps = new GPSPosition[2];
                    xbottom = (sizeOfTree) * (rand.NextDouble() * (1 - double.Epsilon));
                    ybottom = (sizeOfTree) * (rand.NextDouble() * (1 - double.Epsilon));
                    gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                    gps[1] = new GPSPosition('S', 'W', rand.NextDouble() * (sizeOfTree - xbottom) + xbottom, rand.NextDouble() * (sizeOfTree - ybottom) + ybottom);
                    Area test = new Area(i, "nic", gps);
                    tree.insert(test);
                    items.Add(test);
                }
            }
            List<Area> treeItems = tree.find(new QuadTreeRectangle(0, 0, sizeOfTree, sizeOfTree));
            Area helpArea;
            if (checkBoxItems.Checked)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    helpArea = items[i];
                    if (items.Find(item => item.Id == helpArea.Id) != helpArea)
                    {
                        labelResult.Text = "Tree is missing items";
                    }
                }
                labelResult.Text = "Test successful";
            }
            else
            {
                if (treeItems.Count == items.Count)
                {
                    labelResult.Text = "Test successful";
                }
                else if (treeItems.Count > items.Count)
                {
                    labelResult.Text = "Tree contains duplicates";
                }
                else
                {
                    labelResult.Text = "Tree is missing items";
                }
            }
        }

        private void MixTest()
        {
            int initialSize = (int)numericUpDownStart.Value;
            int sizeOfTree = (int)numericUpDownSize.Value;
            int height = (int)numericUpDownHeight.Value;
            int numberOfOperations = (int)numericUpDownOperations.Value;
            int sizeOfItem = (int)numericUpDownItemSize.Value;
            if (sizeOfItem > sizeOfTree)
            {
                sizeOfItem = sizeOfTree;
            }
            double insertChance = (double)numericUpDownInsert.Value;
            double removeChance = (double)numericUpDownDelete.Value;
            double findChance = (double)numericUpDownFind.Value;
            int index;

            double xbottom = 0;
            double ybottom = 0;
            QuadTree<Area> tree = new QuadTree<Area>(0, 0, sizeOfTree, sizeOfTree, height);
            Random rand = new Random();
            List<Area> availableLands = new List<Area>();
            List<Area> usedLands = new List<Area>();
            Area helpArea;



            if (sizeOfItem != 0)
            {
                for (int i = 0; i < initialSize + numberOfOperations; i++)
                {
                    GPSPosition[] gps = new GPSPosition[2];
                    xbottom = (sizeOfTree - sizeOfItem) * (rand.NextDouble() * (1 - double.Epsilon));
                    ybottom = (sizeOfTree - sizeOfItem) * (rand.NextDouble() * (1 - double.Epsilon));
                    gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                    gps[1] = new GPSPosition('S', 'W', xbottom + sizeOfItem, ybottom + sizeOfItem);
                    Area test = new Area(i, "nic", gps);
                    availableLands.Add(test);

                }
            }
            else
            {
                for (int i = 0; i < initialSize + numberOfOperations; i++)
                {
                    GPSPosition[] gps = new GPSPosition[2];
                    xbottom = 0 + (sizeOfTree) * rand.NextDouble();
                    ybottom = 0 + (sizeOfTree) * rand.NextDouble();
                    gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                    gps[1] = new GPSPosition('S', 'W', rand.NextDouble() * (sizeOfTree - xbottom) + xbottom, rand.NextDouble() * (sizeOfTree - ybottom) + ybottom);
                    Land test = new Land(i, "nic", gps);
                    availableLands.Add(test);

                }
            }

            for (int i = 0; i < initialSize; i++)
            {
                helpArea = availableLands[0];
                availableLands.RemoveAt(0);
                usedLands.Add(helpArea);
                tree.insert(helpArea);
            }

            double action;
            for (int i = 0; i < numberOfOperations; i++)
            {
                action = rand.NextDouble();
                if (action < insertChance)
                {
                    index = rand.Next(availableLands.Count);
                    helpArea = availableLands[index];
                    availableLands.RemoveAt(index);
                    usedLands.Add(helpArea);
                    tree.insert(helpArea);
                }
                else if (action < insertChance + removeChance)
                {
                    index = rand.Next(usedLands.Count);
                    helpArea = usedLands[index];
                    usedLands.RemoveAt(index);
                    availableLands.Add(helpArea);
                    if (!tree.remove(helpArea))
                    {
                        labelResult.Text = "Item for delete not found";
                        return;
                    }
                }
                else
                {
                    helpArea = usedLands[rand.Next(usedLands.Count)];
                    List<Area> areas = tree.find(new QuadTreeRectangle(helpArea.GpsLocation[0].lengthPosition - 1,
                                                                        helpArea.GpsLocation[0].widthPosition - 1,
                                                                        helpArea.GpsLocation[1].lengthPosition + 1,
                                                                        helpArea.GpsLocation[1].widthPosition + 1));

                    if (areas.Find(item => item.Id == helpArea.Id) != helpArea)
                    {
                        labelResult.Text = "Item Not Found";
                        return;
                    }
                }


            }

            List<Area> treeItems = tree.find(new QuadTreeRectangle(0, 0, sizeOfTree, sizeOfTree));
            if (checkBoxItems.Checked)
            {
                for (int i = 0; i < usedLands.Count; i++)
                {
                    helpArea = usedLands[i];
                    if (usedLands.Find(item => item.Id == helpArea.Id) != helpArea)
                    {
                        labelResult.Text = "Tree is missing items";
                    }
                }
                labelResult.Text = "Test successful";
            }
            else
            {
                if (treeItems.Count == usedLands.Count)
                {
                    labelResult.Text = "Test successful";
                }
                else if (treeItems.Count > usedLands.Count)
                {
                    labelResult.Text = "Tree contains duplicates";
                }
                else
                {
                    labelResult.Text = "Tree is missing items";
                }
            }
        }

        private void ChangeHeightTest()
        {
            int initialSize = (int)numericUpDownStart.Value;
            int sizeOfTree = (int)numericUpDownSize.Value;
            int height = (int)numericUpDownHeight.Value;
            int numberOfOperations = (int)numericUpDownOperations.Value;
            int sizeOfItem = (int)numericUpDownItemSize.Value;
            int newHeight = (int)numericUpDownNewHeight.Value;
            if (sizeOfItem > sizeOfTree)
            {
                sizeOfItem = 0;
            }

            double xbottom = 0;
            double ybottom = 0;
            int xupper = 0;
            int yupper = 0;
            QuadTree<Area> tree = new QuadTree<Area>(0, 0, sizeOfTree, sizeOfTree, height);
            Random rand = new Random();
            List<Area> items = new List<Area>();
            if (sizeOfItem != 0)
            {
                for (int i = 0; i < initialSize; i++)
                {
                    GPSPosition[] gps = new GPSPosition[2];
                    xbottom = (sizeOfTree - sizeOfItem) * (rand.NextDouble() * (1 - double.Epsilon));
                    ybottom = (sizeOfTree - sizeOfItem) * (rand.NextDouble() * (1 - double.Epsilon));
                    gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                    gps[1] = new GPSPosition('S', 'W', xbottom + sizeOfItem, ybottom + sizeOfItem);
                    Area test = new Area(i, "nic", gps);
                    tree.insert(test);
                    items.Add(test);
                }
            }
            else
            {
                for (int i = 0; i < initialSize; i++)
                {
                    GPSPosition[] gps = new GPSPosition[2];
                    xbottom = (sizeOfTree) * (rand.NextDouble() * (1 - double.Epsilon));
                    ybottom = (sizeOfTree) * (rand.NextDouble() * (1 - double.Epsilon));
                    gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                    gps[1] = new GPSPosition('S', 'W', rand.NextDouble() * (sizeOfTree - xbottom) + xbottom, rand.NextDouble() * (sizeOfTree - ybottom) + ybottom);
                    Area test = new Area(i, "nic", gps);
                    tree.insert(test);
                    items.Add(test);
                }
            }
            tree.changeMaxHeight(newHeight);
            List<Area> treeItems = tree.find(new QuadTreeRectangle(0, 0, sizeOfTree, sizeOfTree));

            if (checkBoxItems.Checked)
            {
                for (int i = 0; i < sizeOfTree; i++)
                {
                    Area foundArea = treeItems.Find(item => item.Id == i);
                    if (foundArea == null)
                    {
                        labelResult.Text = "Tree is missing items";
                    }
                }
                labelResult.Text = "Test successful";
            }
            else
            {
                if (treeItems.Count == items.Count)
                {
                    labelResult.Text = "Test successful";
                }
                else if (treeItems.Count > items.Count)
                {
                    labelResult.Text = "Tree contains duplicates";
                }
                else
                {
                    labelResult.Text = "Tree is missing items";
                }
            }
        }

        private void CheckHealthTest()
        {
            int initialSize = (int)numericUpDownStart.Value;
            int sizeOfTree = (int)numericUpDownSize.Value;
            int height = (int)numericUpDownHeight.Value;
            int numberOfOperations = (int)numericUpDownOperations.Value;
            int sizeOfItem = (int)numericUpDownItemSize.Value;
            if (sizeOfItem > sizeOfTree)
            {
                sizeOfItem = 0;
            }

            double xbottom = 0;
            double ybottom = 0;
            int xupper = 0;
            int yupper = 0;
            QuadTree<Area> tree = new QuadTree<Area>(0, 0, sizeOfTree, sizeOfTree, height);
            Random rand = new Random();
            List<Area> items = new List<Area>();
            //Random rand = new Random(201,30);
            if (sizeOfItem != 0)
            {
                for (int i = 0; i < initialSize; i++)
                {
                    GPSPosition[] gps = new GPSPosition[2];
                    xbottom = (sizeOfTree - sizeOfItem) * (rand.NextDouble() * (1 - double.Epsilon));
                    ybottom = (sizeOfTree - sizeOfItem) * (rand.NextDouble() * (1 - double.Epsilon));
                    gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                    gps[1] = new GPSPosition('S', 'W', xbottom + sizeOfItem, ybottom + sizeOfItem);
                    Area test = new Area(i, "nic", gps);
                    tree.insert(test);
                    items.Add(test);
                }
            }
            else
            {
                for (int i = 0; i < initialSize; i++)
                {
                    GPSPosition[] gps = new GPSPosition[2];
                    xbottom = (sizeOfTree) * (rand.NextDouble() * (1 - double.Epsilon));
                    ybottom = (sizeOfTree) * (rand.NextDouble() * (1 - double.Epsilon));
                    gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                    gps[1] = new GPSPosition('S', 'W', rand.NextDouble() * (sizeOfTree - xbottom) + xbottom, rand.NextDouble() * (sizeOfTree - ybottom) + ybottom);
                    Area test = new Area(i, "nic", gps);
                    tree.insert(test);
                    items.Add(test);
                }
            }
            double oldHealth = tree.calculateHealth()[4];
            oldHealth = Math.Round(oldHealth, 3);
            List<Area> treeItems = tree.find(new QuadTreeRectangle(0 - (sizeOfTree / 4), 0 - (sizeOfTree / 4), sizeOfTree + (sizeOfTree / 4), sizeOfTree + (sizeOfTree / 4)));

            if (checkBoxItems.Checked)
            {
                for (int i = 0; i < sizeOfTree; i++)
                {
                    Area foundArea = treeItems.Find(item => item.Id == i);
                    if (foundArea == null)
                    {
                        labelResult.Text = "Tree is missing items";
                    }
                }
                labelResult.Text = "Test successful (Old health = " + oldHealth;
            }
            else
            {
                if (treeItems.Count == items.Count)
                {
                    labelResult.Text = "Test successful (Old health = " + oldHealth;
                }
                else if (treeItems.Count > items.Count)
                {
                    labelResult.Text = "Tree contains duplicates";
                }
                else
                {
                    labelResult.Text = "Tree is missing items";
                }
            }
        }

        private void ReorganizeTest()
        {
            int initialSize = (int)numericUpDownStart.Value;
            int sizeOfTree = (int)numericUpDownSize.Value;
            int height = (int)numericUpDownHeight.Value;
            int numberOfOperations = (int)numericUpDownOperations.Value;
            int sizeOfItem = (int)numericUpDownItemSize.Value;
            if (sizeOfItem > sizeOfTree)
            {
                sizeOfItem = 0;
            }

            double xbottom = 0;
            double ybottom = 0;
            int xupper = 0;
            int yupper = 0;
            QuadTree<Area> tree = new QuadTree<Area>(0, 0, sizeOfTree, sizeOfTree, height);
            Random rand = new Random();
            List<Area> items = new List<Area>();
            //Random rand = new Random(201,30);
            if (sizeOfItem != 0)
            {
                for (int i = 0; i < initialSize / 2; i++)
                {
                    GPSPosition[] gps = new GPSPosition[2];
                    xbottom = (sizeOfTree - sizeOfItem) * (rand.NextDouble() * (1 - double.Epsilon));
                    ybottom = (sizeOfTree - sizeOfItem) * (rand.NextDouble() * (1 - double.Epsilon));
                    gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                    gps[1] = new GPSPosition('S', 'W', xbottom + sizeOfItem, ybottom + sizeOfItem);
                    Area test = new Area(i, "nic", gps);
                    tree.insert(test);
                    items.Add(test);
                }

                for (int i = 0; i < initialSize / 2; i++)
                {
                    GPSPosition[] gps = new GPSPosition[2];
                    xbottom = ((sizeOfTree - sizeOfItem) - (sizeOfTree / 2)) * (rand.NextDouble() * (1 - double.Epsilon)) + (sizeOfTree / 2);
                    ybottom = ((sizeOfTree - sizeOfItem) - (sizeOfTree / 2)) * (rand.NextDouble() * (1 - double.Epsilon)) + (sizeOfTree / 2);
                    gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                    gps[1] = new GPSPosition('S', 'W', xbottom + sizeOfItem, ybottom + sizeOfItem);
                    Area test = new Area(i, "nic", gps);
                    tree.insert(test);
                    items.Add(test);
                }
            }
            else
            {
                for (int i = 0; i < initialSize / 2; i++)
                {
                    GPSPosition[] gps = new GPSPosition[2];
                    xbottom = (sizeOfTree) * (rand.NextDouble() * (1 - double.Epsilon));
                    ybottom = (sizeOfTree) * (rand.NextDouble() * (1 - double.Epsilon));
                    gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                    gps[1] = new GPSPosition('S', 'W', rand.NextDouble() * (sizeOfTree - xbottom) + xbottom, rand.NextDouble() * (sizeOfTree - ybottom) + ybottom);
                    Area test = new Area(i, "nic", gps);
                    tree.insert(test);
                    items.Add(test);
                }

                for (int i = 0; i < initialSize / 2; i++)
                {
                    GPSPosition[] gps = new GPSPosition[2];
                    xbottom = (sizeOfTree / 2) + (sizeOfTree / 2) * rand.NextDouble();
                    ybottom = (sizeOfTree / 2) + (sizeOfTree / 2) * rand.NextDouble();
                    gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                    gps[1] = new GPSPosition('S', 'W', rand.NextDouble() * (sizeOfTree - xbottom) + xbottom, rand.NextDouble() * (sizeOfTree - ybottom) + ybottom);
                    Area test = new Area(i, "nic", gps);
                    tree.insert(test);
                    items.Add(test);
                }
            }
            double oldHealth = tree.calculateHealth()[4];
            tree = tree.rebuildTree();
            double newHealth = tree.calculateHealth()[4];
            oldHealth = Math.Round(oldHealth, 3);
            newHealth = Math.Round(newHealth, 3);
            List<Area> treeItems = tree.find(new QuadTreeRectangle(0 - (sizeOfTree / 4), 0 - (sizeOfTree / 4), sizeOfTree + (sizeOfTree / 4), sizeOfTree + (sizeOfTree / 4)));

            if (checkBoxItems.Checked)
            {
                for (int i = 0; i < sizeOfTree; i++)
                {
                    Area foundArea = treeItems.Find(item => item.Id == i);
                    if (foundArea == null)
                    {
                        labelResult.Text = "Tree is missing items";
                    }
                }
                labelResult.Text = "Test successful (Old health/New health = " + oldHealth + " / " + newHealth;
            }
            else
            {
                if (treeItems.Count == items.Count)
                {
                    labelResult.Text = "Test successful (Old health/New health = " + oldHealth + " / " + newHealth;
                }
                else if (treeItems.Count > items.Count)
                {
                    labelResult.Text = "Tree contains duplicates";
                }
                else
                {
                    labelResult.Text = "Tree is missing items";
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = comboBox1.SelectedIndex;

            switch (selectedIndex)
            {
                case 0:
                    showChances(0);
                    showNewHeight(0);
                    break;
                case 1:
                    showChances(0);
                    showNewHeight(0);
                    break;
                case 2:
                    showChances(0);
                    showNewHeight(0);
                    break;
                case 3:
                    showNewHeight(0);
                    showChances(1);
                    break;
                case 4:
                    showChances(0);
                    showNewHeight(1);
                    break;
                case 5:
                    showChances(0);
                    showNewHeight(0);
                    break;
                case 6:
                    showChances(0);
                    showNewHeight(0);
                    break;
            }
        }

        private void showChances(int status)
        {
            if (labelInsert.Visible == true && status == 0)
            {
                labelInsert.Visible = false;
                numericUpDownInsert.Visible = false;
                labelRemove.Visible = false;
                numericUpDownDelete.Visible = false;
                labelFind.Visible = false;
                numericUpDownFind.Visible = false;
            }
            else if (labelInsert.Visible == false && status == 1)
            {
                labelInsert.Visible = true;
                numericUpDownInsert.Visible = true;
                labelRemove.Visible = true;
                numericUpDownDelete.Visible = true;
                labelFind.Visible = true;
                numericUpDownFind.Visible = true;
            }
        }

        private void showNewHeight(int status)
        {
            if (labelNewHeight.Visible == true && status == 0)
            {
                labelNewHeight.Visible = false;
                numericUpDownNewHeight.Visible = false;
            }
            else if (labelNewHeight.Visible == false && status == 1)
            {
                labelNewHeight.Visible = true;
                numericUpDownNewHeight.Visible = true;
            }
        }
    }
}
