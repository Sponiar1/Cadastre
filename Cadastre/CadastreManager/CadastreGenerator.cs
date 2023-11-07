using Cadastre.DataItems;
using Cadastre.DataStructure;
using Cadastre.DataStructure.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Cadastre.CadastreManager
{
    internal class CadastreGenerator
    {
        public CadastreGenerator() { }
        public QuadTree<Area>[] generateData(double[] size)
        {
            QuadTree<Area>[] trees = new QuadTree<Area>[2];
            QuadTree<Area> lands;
            QuadTree<Area> properties;
            double xbottom = 0;
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
            }
            trees[0] = lands;
            trees[1] = properties;
            return trees;
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
