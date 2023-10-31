using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cadastre.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cadastre.DataItems;
using Cadastre.DataStructure.Templates;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace Cadastre.DataStructure.Tests
{
    [TestClass()]
    public class QuadTreeTests
    {
        [TestMethod()]
        public void insertTest()
        {
            int numberOfOperations = 10000000;
            int sizeOfTree = 10000000;
            int height = 100;

            double xbottom = 0;
            double ybottom = 0;
            int xupper = 0;
            int yupper = 0;
            QuadTree<Area> tree = new QuadTree<Area>(0, 0, sizeOfTree, sizeOfTree, height);
            Random rand = new Random();
            //Random rand = new Random(201,30);
            for (int i = 0; i < numberOfOperations; i++)
            {
                GPSPosition[] gps = new GPSPosition[2];
                xbottom = 0 + (sizeOfTree) * rand.NextDouble();
                ybottom = 0 + (sizeOfTree) * rand.NextDouble();
                gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                gps[1] = new GPSPosition('S', 'W', rand.NextDouble() * (sizeOfTree - xbottom) + xbottom, rand.NextDouble() * (sizeOfTree - ybottom) + ybottom);
                Area test = new Area(i, "nic", gps);
                Assert.IsTrue(tree.insert(test));
                /*
                int pocet = tree.find(new Rectangle(0, 0, 1000000, 1000000)).Count;
                if (tree.find(new Rectangle(0, 0, 1000000, 1000000)).Count != tree.Size || tree.Size % 1000 == 0 || tree.Size > 4115)
                {
                    List<Area> fik = tree.find(new Rectangle(0, 0, 1000000, 1000000));
                    int p = 5;
                }*/
            }
            /*
            List<Area> listok = tree.find(new Rectangle(0, 0, 1000000, 1000000));
            for (int i = 0; i < numberOfOperations; i++)
            {
                Area foundArea = listok.Find(item => item.Id == i);
                if(foundArea == null)
                {
                    int p = i;
                }
            }*/
            Assert.IsTrue(tree.Size == numberOfOperations);
            Assert.IsTrue(tree.find(new QuadTreeRectangle(0, 0, sizeOfTree, sizeOfTree)).Count == numberOfOperations);
        }

        [TestMethod()]
        public void removeTest()
        {
            int numberOfOperations = 1000000;
            int sizeOfTree = 10000000;
            int height = 100;
            int removeOperations = 100000;
            double xbottom = 0;
            double ybottom = 0;
            int xupper = 0;
            int yupper = 0;
            QuadTree<Area> tree = new QuadTree<Area>(0, 0, sizeOfTree, sizeOfTree, height);
            Random rand = new Random();
            List<Area> list = new List<Area>();
            for (int i = 0; i < numberOfOperations; i++)
            {
                GPSPosition[] gps = new GPSPosition[2];
                xbottom = 0 + (sizeOfTree-50) * rand.NextDouble();
                ybottom = 0 + (sizeOfTree-50) * rand.NextDouble();
                gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                //gps[1] = new GPSPosition('S', 'W', rand.NextDouble() * (sizeOfTree - xbottom) + xbottom, rand.NextDouble() * (sizeOfTree - ybottom) + ybottom);
                gps[1] = new GPSPosition('S', 'W', xbottom + 50, ybottom + 50);
                Area test = new Area(i, "nic", gps);
                list.Add(test);
                Assert.IsTrue(tree.insert(test));
            }
            Area helpArea;
            int index;
            for (int i = 0; i < removeOperations; i++)
            {
                index = rand.Next(list.Count);
                helpArea = list[index];
                list.RemoveAt(index);
                Assert.IsTrue(tree.remove(helpArea));
            }
            List<Area> items = tree.find(new QuadTreeRectangle(0, 0, sizeOfTree, sizeOfTree));
            for (int i = 0; i < list.Count; i++)
            {
                helpArea = list[i];

                Assert.IsTrue(items.Find(item => item.Id == helpArea.Id) == helpArea);
            }
        }

        [TestMethod()]
        public void findTest()
        {
            int numberOfOperations = 1000000;
            int sizeOfTree = 1000000;
            int height = 100;

            double xbottom = 0;
            double ybottom = 0;
            int xupper = 0;
            int yupper = 0;
            QuadTree<Area> tree = new QuadTree<Area>(0, 0, sizeOfTree, sizeOfTree, height);
            Random rand = new Random();
            List<Area> list = new List<Area>();
            Area helpArea;
            for (int i = 0; i < numberOfOperations * 3 / 4; i++)
            {
                GPSPosition[] gps = new GPSPosition[2];
                xbottom = 0 + (sizeOfTree / 2) * rand.NextDouble();
                ybottom = 0 + (sizeOfTree) * rand.NextDouble();
                gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                gps[1] = new GPSPosition('S', 'W', rand.NextDouble() * (sizeOfTree / 2 - xbottom) + xbottom, rand.NextDouble() * (sizeOfTree - ybottom) + ybottom);
                Area test = new Area(i, "nic", gps);
                tree.insert(test);
            }

            for (int i = 0; i < numberOfOperations / 4; i++)
            {
                GPSPosition[] gps = new GPSPosition[2];
                xbottom = (sizeOfTree / 2) + (sizeOfTree / 2) * rand.NextDouble();
                ybottom = 0 + (sizeOfTree) * rand.NextDouble();
                gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                gps[1] = new GPSPosition('S', 'W', rand.NextDouble() * (sizeOfTree - xbottom) + xbottom, rand.NextDouble() * (sizeOfTree - ybottom) + ybottom);
                Area test = new Area(i, "nic", gps);
                tree.insert(test);
                list.Add(test);
            }
            List<Area> items = tree.find(new QuadTreeRectangle(sizeOfTree / 2, 0, sizeOfTree, sizeOfTree));
            Assert.IsTrue(tree.find(new QuadTreeRectangle(sizeOfTree / 2, 0, sizeOfTree, sizeOfTree)).Count == numberOfOperations / 4);
            for (int i = 0; i < list.Count; i++)
            {
                helpArea = list[i];
                Assert.IsTrue(items.Find(item => item.Id == helpArea.Id) == helpArea);
            }
        }

        [TestMethod()]
        public void changeMaxHeightTest()
        {
            int numberOfOperations = 100000;
            int sizeOfTree = 100000;
            int height = 100;

            double xbottom = 0;
            double ybottom = 0;
            int xupper = 0;
            int yupper = 0;
            QuadTree<Area> tree = new QuadTree<Area>(0, 0, sizeOfTree, sizeOfTree, height);
            Random rand = new Random();
            List<Area> list = new List<Area>();
            for (int i = 0; i < numberOfOperations; i++)
            {
                GPSPosition[] gps = new GPSPosition[2];
                xbottom = 0 + (sizeOfTree) * rand.NextDouble();
                ybottom = 0 + (sizeOfTree) * rand.NextDouble();
                gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                gps[1] = new GPSPosition('S', 'W', rand.NextDouble() * (sizeOfTree - xbottom) + xbottom, rand.NextDouble() * (sizeOfTree - ybottom) + ybottom);
                Area test = new Area(i, "nic", gps);
                Assert.IsTrue(tree.insert(test));
                list.Add(test);
            }

            tree.changeMaxHeight(200);

            Area helpArea;
            List<Area> items = tree.find(new QuadTreeRectangle(0, 0, sizeOfTree, sizeOfTree));
            Assert.IsTrue(items.Count == list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                helpArea = list[i];
                Assert.IsTrue(items.Find(item => item.Id == helpArea.Id) == helpArea);
            }

            tree.changeMaxHeight(150);
            items = tree.find(new QuadTreeRectangle(0, 0, sizeOfTree, sizeOfTree));
            Assert.IsTrue(items.Count == list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                helpArea = list[i];
                Assert.IsTrue(items.Find(item => item.Id == helpArea.Id) == helpArea);
            }


        }

        [TestMethod()]
        public void calculateHealthTest()
        {
            int numberOfOperations = 1000000;
            int sizeOfTree = 1000000;
            int height = 100;

            double xbottom = 0;
            double ybottom = 0;
            int xupper = 0;
            int yupper = 0;
            QuadTree<Area> tree = new QuadTree<Area>(0, 0, sizeOfTree, sizeOfTree, height);
            Random rand = new Random(30);
            List<Area> list = new List<Area>();
            for (int i = 0; i < numberOfOperations; i++)
            {
                GPSPosition[] gps = new GPSPosition[2];
                xbottom = 0 + (sizeOfTree) * rand.NextDouble();
                ybottom = 0 + (sizeOfTree) * rand.NextDouble();
                gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                gps[1] = new GPSPosition('S', 'W', rand.NextDouble() * (sizeOfTree - xbottom) + xbottom, rand.NextDouble() * (sizeOfTree - ybottom) + ybottom);
                Area test = new Area(i, "nic", gps);
                Assert.IsTrue(tree.insert(test));
                list.Add(test);
            }
            double[] health = tree.calculateHealth();
            Assert.IsTrue(health[4] > 0 && health[4] < 1);
        }

        [TestMethod()]
        public void rebuildTreeTest()
        {
            int numberOfOperations = 100000;
            int sizeOfTree = 100000;
            int height = 5;

            double xbottom = 0;
            double ybottom = 0;
            int xupper = 0;
            int yupper = 0;
            QuadTree<Area> tree = new QuadTree<Area>(0, 0, sizeOfTree, sizeOfTree, height);
            Random rand = new Random();
            List<Area> list = new List<Area>();
            for (int i = 0; i < numberOfOperations; i++)
            {
                GPSPosition[] gps = new GPSPosition[2];
                xbottom = 0 + (sizeOfTree) * rand.NextDouble();
                ybottom = 0 + (sizeOfTree) * rand.NextDouble();
                gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                gps[1] = new GPSPosition('S', 'W', rand.NextDouble() * (sizeOfTree - xbottom) + xbottom, rand.NextDouble() * (sizeOfTree - ybottom) + ybottom);
                Area test = new Area(i, "nic", gps);
                Assert.IsTrue(tree.insert(test));
                list.Add(test);
            }
            double[] health = tree.calculateHealth();
            Assert.IsTrue(health[4] > 0 && health[4] < 1);

            Area helpArea;
            tree = tree.rebuildTree();
            List<Area> items = tree.find(new QuadTreeRectangle(0 - (sizeOfTree / 4), 0 - (sizeOfTree / 4), sizeOfTree + (sizeOfTree / 4), sizeOfTree + (sizeOfTree / 4)));
            Assert.IsTrue(items.Count == list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                helpArea = list[i];
                Assert.IsTrue(items.Find(item => item.Id == helpArea.Id) == helpArea);
            }

            double[] newHealth = tree.calculateHealth();
            Assert.IsTrue(health[4] > 0 && health[4] < 1);
        }

        [TestMethod()]
        public void mixTest()
        {
            Random rand = new Random(31);

            int startingSize = 100000;
            int sizeOfTree = 100000;
            int height = 30;
            int numberOfOperations = 1000000;
            double insertChance = 0.45;
            double removeChance = 0.45;
            double findChance = 0.1;
            int index;

            double xbottom = 0;
            double ybottom = 0;
            Land helpLand;
            QuadTree<Land> tree = new QuadTree<Land>(0, 0, sizeOfTree, sizeOfTree, height);
            List<Land> availableLands = new List<Land>();
            List<Land> usedLands = new List<Land>();

            for (int i = 0; i < startingSize + numberOfOperations; i++)
            {
                GPSPosition[] gps = new GPSPosition[2];
                xbottom = 0 + (sizeOfTree) * rand.NextDouble();
                ybottom = 0 + (sizeOfTree) * rand.NextDouble();
                gps[0] = new GPSPosition('N', 'E', xbottom, ybottom);
                gps[1] = new GPSPosition('S', 'W', rand.NextDouble() * (sizeOfTree - xbottom) + xbottom, rand.NextDouble() * (sizeOfTree - ybottom) + ybottom);
                Land test = new Land(i, "nic", gps);
                availableLands.Add(test);
                
            }

            for (int i = 0; i < startingSize; i++)
            {
                helpLand = availableLands[i];
                availableLands.RemoveAt(i);
                usedLands.Add(helpLand);
                Assert.IsTrue(tree.insert(helpLand));
            }

            double action;
            for(int i = 0; i < numberOfOperations; i++)
            {
                action = rand.NextDouble();
                if(action < insertChance)
                {
                    index = rand.Next(availableLands.Count);
                    helpLand = availableLands[index];
                    availableLands.RemoveAt(index);
                    usedLands.Add(helpLand);
                    Assert.IsTrue(tree.insert(helpLand));
                }
                else if(action < insertChance + removeChance)
                {
                    index = rand.Next(usedLands.Count);
                    helpLand = usedLands[index];
                    usedLands.RemoveAt(index);
                    availableLands.Add(helpLand);
                    Assert.IsTrue(tree.remove(helpLand));
                }
                else
                {
                    helpLand = usedLands[rand.Next(usedLands.Count)];
                    List<Land> lands = tree.find(new QuadTreeRectangle(helpLand.GpsLocation[0].lengthPosition - 1, 
                                                                        helpLand.GpsLocation[0].widthPosition - 1,
                                                                        helpLand.GpsLocation[1].lengthPosition + 1,
                                                                        helpLand.GpsLocation[1].widthPosition + 1));
                    Assert.IsTrue(lands.Find(item => item.Id == helpLand.Id) == helpLand);
                }
            }

        }
    }
}