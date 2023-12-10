using Cadastre.DataItems;
using Cadastre.DataStructure;
using Cadastre.DataStructure.Templates;
using Cadastre.Files.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Cadastre.FileManager
{
    public class CSVHandler
    {
        public CSVHandler() 
        { 

        }

        public void SaveAreaToCSV(QuadTree<Area> quadTree, string fileName)
        {
            string filePath = Path.Combine(Application.StartupPath, fileName);
            QuadTreeRectangle treeBorder = quadTree.GetBorders();

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                //Tree information
                writer.Write(treeBorder.BottomLeftX);
                writer.Write(";");
                writer.Write(treeBorder.BottomLeftY);
                writer.Write(";");
                writer.Write(treeBorder.UpperRightX);
                writer.Write(";");
                writer.Write(treeBorder.UpperRightY);
                writer.Write(";");
                writer.Write(quadTree.MaxHeight);
                writer.Write(";");
                writer.WriteLine();

                //Items
                List<Area> landItems = quadTree.Find(new QuadTreeRectangle(treeBorder.BottomLeftX, treeBorder.BottomLeftY, treeBorder.UpperRightX, treeBorder.UpperRightY));
                foreach (Area area in landItems)
                {
                    writer.Write(area.Id);
                    writer.Write(";");
                    writer.Write(((Land)area).Description);
                    writer.Write(";");
                    writer.Write(area.GpsLocation[0].lengthPosition);
                    writer.Write(";");
                    writer.Write(area.GpsLocation[0].widthPosition);
                    writer.Write(";");
                    writer.Write(area.GpsLocation[1].lengthPosition);
                    writer.Write(";");
                    writer.Write(area.GpsLocation[1].widthPosition);
                    writer.Write(";");
                    writer.WriteLine();
                }
            }

        }

        public QuadTree<Area>[] LoadTreeFromCSV(string fileNameLands, string fileNameProperties)
        {
            double x0;
            double y0;
            double x1;
            double y1;
            int height;
            Queue<Area> queue = new Queue<Area>();
            QuadTree<Area>[] trees = new QuadTree<Area>[2];
            string filePath = Path.Combine(Application.StartupPath, fileNameLands);
            using (StreamReader reader = new StreamReader(filePath))
            {
                var line = reader.ReadLine();
                var values = line.Split(';');
                x0 = double.Parse(values[0]);
                y0 = double.Parse(values[1]);
                x1 = double.Parse(values[2]);
                y1 = double.Parse(values[3]);
                height = int.Parse(values[4]);

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    values = line.Split(';');

                    int id = int.Parse(values[0]);
                    string description = values[1];
                    double itemX0 = double.Parse(values[2]);
                    double itemY0 = double.Parse(values[3]);
                    double itemX1 = double.Parse(values[4]);
                    double itemY1 = double.Parse(values[5]);
                    GPSPosition[] gps = new GPSPosition[2] { new GPSPosition(itemX0, itemY0, 0), new GPSPosition(itemX1, itemY1, 1)};

                    queue.Enqueue(new Land(id, description, gps));
                }
            }
            QuadTree<Area> lands = new QuadTree<Area>(x0, y0, x1, y1, height, queue);
            trees[0] = lands;

            List<Area> landsWithProperty;
            Property item;
            filePath = Path.Combine(Application.StartupPath, fileNameProperties);
            using (StreamReader reader = new StreamReader(filePath))
            {
                var line = reader.ReadLine();
                var values = line.Split(';');
                x0 = double.Parse(values[0]);
                y0 = double.Parse(values[1]);
                x1 = double.Parse(values[2]);
                y1 = double.Parse(values[3]);
                height = int.Parse(values[4]);

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    values = line.Split(';');

                    int id = int.Parse(values[0]);
                    string description = values[1];
                    double itemX0 = double.Parse(values[2]);
                    double itemY0 = double.Parse(values[3]);
                    double itemX1 = double.Parse(values[4]);
                    double itemY1 = double.Parse(values[5]);
                    GPSPosition[] gps = new GPSPosition[2] { new GPSPosition(itemX0, itemY0, 0), new GPSPosition(itemX1, itemY1, 1) };
                    item = new Property(id, description, gps);
                    landsWithProperty = lands.Find(new QuadTreeRectangle(itemX0, itemY0, itemX1, itemY1));
                    foreach (Land land in landsWithProperty)
                    {
                        land.Properties.Add((Property)item);
                        item.Lands.Add((Land)land);
                    }

                    queue.Enqueue(item);
                }
            }
            QuadTree<Area> properties = new QuadTree<Area>(x0, y0, x1, y1, height, queue);
            trees[1] = properties;
            return trees;
        }

        public void SaveMeasurementsToCSV(double[] normal, double[] reorganized)
        {
            string filePath = Path.Combine(Application.StartupPath, "Measurements.csv");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                //Tree information
                writer.Write("Insert unorganized");
                writer.Write(";");
                writer.Write(normal[0]);
                writer.Write(";");
                writer.WriteLine();
                writer.Write("Delete unorganized");
                writer.Write(";");
                writer.Write(normal[1]);
                writer.Write(";");
                writer.WriteLine();
                writer.Write("Insert organized");
                writer.Write(";");
                writer.Write(reorganized[0]);
                writer.Write(";");
                writer.WriteLine();
                writer.Write("Delete organized");
                writer.Write(";");
                writer.Write(reorganized[1]);
                writer.Write(";");
                writer.WriteLine();
            }
        }

    }
}
