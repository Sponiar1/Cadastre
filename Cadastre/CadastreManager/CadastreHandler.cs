using Cadastre.DataItems;
using Cadastre.DataStructure;
using Cadastre.DataStructure.Templates;
using Cadastre.FileManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Cadastre.CadastreManager
{
    internal class CadastreHandler
    {
        QuadTree<Area> lands;
        QuadTree<Area> properties;
        CSVHandler csvhandler;
        public CadastreHandler() 
        {
            csvhandler = new CSVHandler();
        }
        public void GenerateEmptyTrees(double[] configuration)
        {
            lands = new QuadTree<Area>(configuration[0], configuration[1], configuration[2], configuration[3], (int)configuration[5]);
            properties = new QuadTree<Area>(configuration[0], configuration[1], configuration[2], configuration[3], (int)configuration[5]);
        }
        public void GenerateCadastre(double[] configuration)
        {
            CadastreGenerator generator = new CadastreGenerator();
            QuadTree<Area>[] trees = generator.generateData(configuration);
            lands = trees[0];
            properties = trees[1];
        }
        public Boolean insertItem(double[] configuration, string description, int type)
        {

            if (type == 0)
            {
                GPSPosition[] gps = new GPSPosition[2];
                gps[0] = new GPSPosition('N', 'E', configuration[0], configuration[1]);
                gps[1] = new GPSPosition('S', 'W', configuration[2], configuration[3]);
                Land land = new Land((int)configuration[4], description, gps);
                if(!lands.insert(land))
                {
                    return false;
                }
                List<Area> propertiesInArea = properties.find(new QuadTreeRectangle(configuration[0], configuration[1], configuration[2], configuration[3]));
                foreach (Property property in propertiesInArea)
                {
                    land.Properties.Add(property);
                    property.Lands.Add(land);
                }
                return true;
            }
            else
            {
                GPSPosition[] gps = new GPSPosition[2];
                gps[0] = new GPSPosition('N', 'E', configuration[0], configuration[1]);
                gps[1] = new GPSPosition('S', 'W', configuration[2], configuration[3]);
                Property property = new Property((int)configuration[4], description, gps);
                if (!properties.insert(property))
                {
                    return false;
                }
                List<Area> landsInArea = lands.find(new QuadTreeRectangle(configuration[0], configuration[1], configuration[2], configuration[3]));
                foreach (Land land in landsInArea)
                {
                    land.Properties.Add(property);
                    property.Lands.Add(land);
                }
                return true;
            }
        }
        public Boolean editItem(double[] configuration, string description, int type, Area item)
        {

            if (configuration[0] == item.GpsLocation[0].lengthPosition && configuration[1] == item.GpsLocation[0].widthPosition
                            && configuration[2] == item.GpsLocation[1].lengthPosition && configuration[3] == item.GpsLocation[1].lengthPosition)
            {
                item.Id = (int)configuration[4];
                item.Description = description;
                return true;
            }
            else
            {
                if (type == 0)
                {
                    List<Area> propertiesInArea = properties.find(new QuadTreeRectangle(item.GpsLocation[0].lengthPosition, item.GpsLocation[0].widthPosition,
                                                                                        item.GpsLocation[1].lengthPosition, item.GpsLocation[1].widthPosition));
                    foreach (Property property in propertiesInArea)
                    {
                        property.Lands.Remove((Land)item);
                    }
                    ((Land)item).Properties.Clear();
                    lands.remove(item);
                    GPSPosition[] gps = new GPSPosition[2];
                    gps[0] = new GPSPosition('N', 'E', configuration[0], configuration[1]);
                    gps[1] = new GPSPosition('S', 'W', configuration[2], configuration[3]);
                    item.GpsLocation = gps;
                    item.Id = (int)configuration[4];
                    item.Description = description;

                    if(!lands.insert(item))
                    {
                        return false;
                    }
                    propertiesInArea = properties.find(new QuadTreeRectangle(configuration[0], configuration[1], configuration[2], configuration[3]));
                    foreach (Property property in propertiesInArea)
                    {

                        ((Land)item).Properties.Add(property);
                        property.Lands.Add((Land)item);
                    }
                    return true;
                }
                else
                {
                    List<Area> landsInArea = lands.find(new QuadTreeRectangle(item.GpsLocation[0].lengthPosition, item.GpsLocation[0].widthPosition,
                                                                              item.GpsLocation[1].lengthPosition, item.GpsLocation[1].widthPosition));
                    foreach (Land land in landsInArea)
                    {
                        land.Properties.Remove((Property)item);
                    }
                    ((Property)item).Lands.Clear();
                    properties.remove(item);

                    GPSPosition[] gps = new GPSPosition[2];
                    gps[0] = new GPSPosition('N', 'E', configuration[0], configuration[1]);
                    gps[1] = new GPSPosition('S', 'W', configuration[2], configuration[3]);
                    item.GpsLocation = gps;
                    item.Id = (int)configuration[4];
                    item.Description = description;
                    if(!properties.insert(item))
                    {
                        return false;
                    }
                    landsInArea = lands.find(new QuadTreeRectangle(configuration[0], configuration[1], configuration[2], configuration[3]));
                    foreach (Land land in landsInArea)
                    {

                        ((Property)item).Lands.Add(land);
                        land.Properties.Add((Property)item);
                    }
                    return true;
                }
            }
        }
        public Boolean deleteItem(Area item, int type)
        {
            List<Area> associatedItems;
            if (type == 0)
            {
                if(!lands.remove(item))
                {
                    return false;
                }
                associatedItems = properties.find(new QuadTreeRectangle(item.GpsLocation[0].lengthPosition, item.GpsLocation[0].widthPosition,
                                                                        item.GpsLocation[1].lengthPosition, item.GpsLocation[1].widthPosition));
                foreach (Property property in associatedItems)
                {
                    property.Lands.Remove((Land)item);
                }
                return true;
            }
            else
            {
                if(!properties.remove(item))
                { 
                    return false;
                }
                associatedItems = lands.find(new QuadTreeRectangle(item.GpsLocation[0].lengthPosition, item.GpsLocation[0].widthPosition,
                                                                        item.GpsLocation[1].lengthPosition, item.GpsLocation[1].widthPosition));
                foreach (Land land in associatedItems)
                {
                    land.Properties.Remove((Property)item);
                }
                return true;
            }
        }
        public List<Area>[] findAll(double[] coordinates)
        {
            List<Area>[] results = new List<Area>[2];
            results[0] = lands.find(new QuadTreeRectangle(coordinates[0], coordinates[1], coordinates[2], coordinates[3]));
            results[1] = properties.find(new QuadTreeRectangle(coordinates[0], coordinates[1], coordinates[2], coordinates[3]));
            return results;
        }
        public List<Area> findLands(double[] coordinates)
        {
            return lands.find(new QuadTreeRectangle(coordinates[0], coordinates[1], coordinates[2], coordinates[3]));
        }
        public List<Area> findProperty(double[] coordinates)
        {
            return properties.find(new QuadTreeRectangle(coordinates[0], coordinates[1], coordinates[2], coordinates[3]));
        }
        public void saveToCSV(string landName, string propertyName)
        {
            csvhandler.SaveAreaToCSV(lands, "lands.csv");
            csvhandler.SaveAreaToCSV(properties, "properties.csv");
        }
        public void loadFromCSV(string landName, string propertyName)
        {
            QuadTree<Area>[] trees = csvhandler.LoadTreeFromCSV(landName, propertyName);
            lands = trees[0];
            properties = trees[1];
        }
    }
}
