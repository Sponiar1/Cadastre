using Cadastre.DataItems;
using Cadastre.DataStructure;
using Cadastre.DataStructure.Templates;
using Cadastre.FileManager;
using Cadastre.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.CadastreManager
{
    internal class CadastreBinaryManager
    {
        DynamicHash<Property> Properties;
        DynamicHash<Land> Lands;
        string propertyFileName;
        string landFileName;
        string propertyOverflowName;
        string landOverflowName;
        string indexProperty;
        string indexLand;
        string propertyTreeFile;
        string landTreeFile;
        string indexPropertiesPropertiesFile;
        string indexLandPropertiesFile;
        int maxIndexLand;
        int maxIndexProperty;
        string managerProperties;
        QuadTree<Area> PropertyTree;
        QuadTree<Area> LandTree;

        public CadastreBinaryManager()
        {
            propertyFileName = "Properties.bin";
            propertyOverflowName = "PropertiesOverFlow.bin";
            landFileName = "Lands.bin";
            landOverflowName = "LandsOverflow.bin";
            indexProperty = "PropertyIndex.csv";
            indexLand = "LandIndex.csv";
            propertyTreeFile = "PropertyTree.csv";
            landTreeFile = "LandTree.csv";
            indexPropertiesPropertiesFile = "PropertyProperty.txt";
            indexLandPropertiesFile = "LandProperty.txt";
            PropertyTree = new QuadTree<Area>(0, 0, double.MaxValue, double.MaxValue, 20);
            LandTree = new QuadTree<Area>(0, 0, double.MaxValue, double.MaxValue, 20);
            managerProperties = "ManagerID.txt";
        }

        public void NewFile(int blockFactor, int blockFactorOverflow)
        {
            Properties = new DynamicHash<Property>(blockFactor, propertyFileName, blockFactorOverflow, propertyOverflowName);
            Lands = new DynamicHash<Land>(blockFactor, landFileName, blockFactorOverflow, landOverflowName);
            maxIndexLand = 1;
            maxIndexProperty = 1;
        }
        public void Save()
        {
            CSVHandler csvHandler = new CSVHandler();
            Properties.SaveIndex(indexProperty, indexPropertiesPropertiesFile);
            Lands.SaveIndex(indexLand, indexLandPropertiesFile);
            csvHandler.SaveMinimalAreaToCSV(PropertyTree, propertyTreeFile);
            csvHandler.SaveMinimalAreaToCSV(LandTree, landTreeFile);

            string filePath = Path.Combine(Application.StartupPath, managerProperties);
            using (StreamWriter textWriter = new StreamWriter(filePath))
            {
                textWriter.WriteLine(maxIndexLand);
                textWriter.WriteLine(maxIndexProperty);
            }
        }
        public void Load()
        {
            Properties = new DynamicHash<Property>(propertyFileName, propertyOverflowName, indexProperty, indexPropertiesPropertiesFile);
            Lands = new DynamicHash<Land>(landFileName, landOverflowName, indexLand, indexLandPropertiesFile);
            CSVHandler csvHandler = new CSVHandler();
            LandTree = csvHandler.LoadMinimalAreaFromCSV(landTreeFile);
            PropertyTree = csvHandler.LoadMinimalAreaFromCSV(propertyTreeFile);
            string filePath = Path.Combine(Application.StartupPath, managerProperties);
            using (StreamReader reader = new StreamReader(filePath))
            {
                maxIndexLand = int.Parse(reader.ReadLine());
                maxIndexProperty = int.Parse(reader.ReadLine());
            }
        }
        public List<Area> GetProperty(int id)
        {
            List<Area> result = new List<Area>();
            Property target = new Property();
            target.Id = id;
            target = Properties.FindItem(target);
            if (target == default)
            {
                return null;
            }
            result.Add(target);
            for (int i = 0; i < 6; i++)
            {
                Land dummy = new Land();
                dummy = dummy.CreateInstance();
                dummy.Id = target.LandsId[i];
                if (dummy.Id != -1)
                {
                    dummy = Lands.FindItem(dummy);
                    if (dummy != null)
                    {
                        result.Add(dummy);
                    }
                }
            }
            return result;
        }
        public List<Area> GetLand(int id)
        {
            List<Area> result = new List<Area>();
            Land target = new Land();
            target.Id = id;
            target = Lands.FindItem(target);
            if (target == default)
            {
                return null;
            }
            result.Add(target);
            for (int i = 0; i < 5; i++)
            {
                Property dummy = new Property();
                dummy = dummy.CreateInstance();
                dummy.Id = target.PropertiesId[i];
                if (dummy.Id != -1)
                {
                    dummy = Properties.FindItem(dummy);
                    if (dummy != null)
                    {
                        result.Add(dummy);
                    }
                }
            }
            return result;
        }
        public int AddItem(double[] configuration, string description, int type)
        {
            bool relatedGood = true;
            GPSPosition[] gps = new GPSPosition[2];
            gps[0] = new GPSPosition('N', 'E', configuration[0], configuration[1]);
            gps[1] = new GPSPosition('S', 'W', configuration[2], configuration[3]);
            //Land
            if (type == 0)
            {
                configuration[4] = maxIndexLand;
                Land item = new Land((int)configuration[4], description, gps);
                List<Area> relatedAreas;
                relatedAreas = PropertyTree.Find(new QuadTreeRectangle(item.GpsLocation[0].lengthPosition, item.GpsLocation[0].widthPosition,
                                                        item.GpsLocation[1].lengthPosition, item.GpsLocation[1].widthPosition));
                Property dummy = new Property();
                Property related;
                if (relatedAreas.Count < 6)
                {
                    for (int i = 0; i < relatedAreas.Count; i++)
                    {
                        dummy.Id = relatedAreas[i].Id;
                        related = Properties.FindItem(dummy);
                        for (int j = 0; j < related.LandsId.Count; j++)
                        {
                            if (related.LandsId[j] == -1)
                            {
                                break;
                            }
                            if (j == related.LandsId.Count - 1)
                            {
                                relatedGood = false;
                            }
                        }

                    }
                    if (!relatedGood)
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
                if (Lands.TryInsert(item))
                {
                    for (int i = 0; i < relatedAreas.Count; i++)
                    {
                        dummy.Id = relatedAreas[i].Id;
                        related = Properties.FindItem(dummy);
                        for (int j = 0; j < related.LandsId.Count; j++)
                        {
                            if (related.LandsId[j] == -1)
                            {
                                related.LandsId[j] = item.Id;
                                item.PropertiesId[item.PropertiesId.IndexOf(item.PropertiesId.Min())] = related.Id;
                                Properties.UpdateItem(related);
                                break;
                            }
                        }
                    }
                    Lands.Insert(item);
                    Area newItem = new Area((int)configuration[4], gps);
                    LandTree.Insert(newItem);
                    maxIndexLand++;
                    return item.Id;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                //Property
                configuration[4] = maxIndexProperty;
                Property item = new Property((int)configuration[4], description, gps);
                item.RegisterNumber = (int)configuration[5];
                List<Area> relatedAreas;
                relatedAreas = LandTree.Find(new QuadTreeRectangle(item.GpsLocation[0].lengthPosition, item.GpsLocation[0].widthPosition,
                                                                    item.GpsLocation[1].lengthPosition, item.GpsLocation[1].widthPosition));
                Land dummy = new Land();
                Land related;
                if (relatedAreas.Count < 6)
                {
                    for (int i = 0; i < relatedAreas.Count; i++)
                    {
                        dummy.Id = relatedAreas[i].Id;
                        related = Lands.FindItem(dummy);
                        for (int j = 0; j < related.PropertiesId.Count; j++)
                        {
                            if (related.PropertiesId[j] == -1)
                            {
                                break;
                            }
                            if (j == related.PropertiesId.Count - 1)
                            {
                                relatedGood = false;
                            }
                        }

                    }
                    if (!relatedGood)
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
                if (Properties.TryInsert(item))
                {
                    for (int i = 0; i < relatedAreas.Count; i++)
                    {
                        dummy.Id = relatedAreas[i].Id;
                        related = Lands.FindItem(dummy);
                        for (int j = 0; j < related.PropertiesId.Count; j++)
                        {
                            if (related.PropertiesId[j] == -1)
                            {
                                related.PropertiesId[j] = item.Id;
                                item.LandsId[item.LandsId.IndexOf(item.LandsId.Min())] = related.Id;
                                Lands.UpdateItem(related);
                                break;
                            }
                        }
                    }
                    Properties.Insert(item);
                    Area newItem = new Area((int)configuration[4], gps);
                    PropertyTree.Insert(newItem);
                    maxIndexProperty++;
                    return item.Id;
                }
                else
                {
                    return -1;
                }
            }
        }
        public bool DeleteProperty(int id)
        {
            Property deletedItem = new Property();
            deletedItem.Id = id;
            deletedItem = Properties.DeleteItem(deletedItem);
            Land dummy = new Land();
            if (deletedItem != null)
            {
                for (int i = 0; i < deletedItem.LandsId.Count; i++)
                {
                    dummy.Id = deletedItem.LandsId[i];
                    if (dummy.Id != -1)
                    {
                        dummy = Lands.FindItem(dummy);
                        for (int j = 0; j < dummy.PropertiesId.Count; j++)
                        {
                            if (dummy.PropertiesId[j] == deletedItem.Id)
                            {
                                dummy.PropertiesId[j] = -1;
                                Lands.UpdateItem(dummy);
                                break;
                            }
                        }
                    }
                }
                PropertyTree.Remove(deletedItem);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeleteLand(int id)
        {
            Land deletedItem = new Land();
            deletedItem.Id = id;
            deletedItem = Lands.DeleteItem(deletedItem);
            Property dummy = new Property();
            if (deletedItem != null)
            {
                for (int i = 0; i < deletedItem.PropertiesId.Count; i++)
                {
                    dummy.Id = deletedItem.PropertiesId[i];
                    if (dummy.Id != -1)
                    {
                        dummy = Properties.FindItem(dummy);
                        for (int j = 0; j < dummy.LandsId.Count; j++)
                        {
                            if (dummy.LandsId[j] == deletedItem.Id)
                            {
                                dummy.LandsId[j] = -1;
                                Properties.UpdateItem(dummy);
                                break;
                            }
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public Property FindProperty(int id)
        {
            Property dummy = new Property();
            dummy.Id = id;
            return Properties.FindItem(dummy);
        }
        public Land FindLand(int id)
        {
            Land dummy = new Land();
            dummy.Id = id;
            return Lands.FindItem(dummy);
        }
        public bool EditPropertyFull(Property itemBefore, Property itemAfter)
        {
            Land dummy = new Land();
            bool relatedGood = true;
            List<Area> relatedAreas;
            relatedAreas = LandTree.Find(new QuadTreeRectangle(itemAfter.GpsLocation[0].lengthPosition, itemAfter.GpsLocation[0].widthPosition,
                                                    itemAfter.GpsLocation[1].lengthPosition, itemAfter.GpsLocation[1].widthPosition));
            Land related;
            //prehľadávam či môžem zmeniť referencie
            if (relatedAreas.Count < 6)
            {
                for (int i = 0; i < relatedAreas.Count; i++)
                {
                    dummy.Id = relatedAreas[i].Id;
                    related = Lands.FindItem(dummy);
                    for (int j = 0; j < related.PropertiesId.Count; j++)
                    {
                        if (related.PropertiesId[j] == -1)
                        {
                            break;
                        }
                        if (j == related.PropertiesId.Count - 1)
                        {
                            relatedGood = false;
                        }
                    }

                }
                if (!relatedGood)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            //clearujem stare referencie
            for (int i = 0; i < itemBefore.LandsId.Count; i++)
            {
                if (itemBefore.LandsId[i] != -1)
                {
                    dummy.Id = itemBefore.LandsId[i];
                    dummy = Lands.FindItem(dummy);
                    for (int j = 0; j < dummy.PropertiesId.Count; j++)
                    {
                        if (dummy.PropertiesId[j] == itemBefore.Id)
                        {
                            dummy.PropertiesId[j] = -1;
                            Lands.UpdateItem(dummy);
                            break;
                        }
                    }
                }
            }
            PropertyTree.Remove(itemBefore);
            Area area = new Area(itemAfter.Id, itemAfter.GpsLocation);
            PropertyTree.Insert(area);
            //davam nove
            for (int i = 0; i < relatedAreas.Count; i++)
            {
                dummy.Id = relatedAreas[i].Id;
                related = Lands.FindItem(dummy);
                for (int j = 0; j < related.PropertiesId.Count; j++)
                {
                    if (related.PropertiesId[j] == -1)
                    {
                        related.PropertiesId[j] = itemAfter.Id;
                        itemAfter.LandsId[itemAfter.LandsId.IndexOf(itemAfter.LandsId.Min())] = related.Id;
                        Lands.UpdateItem(related);
                        break;
                    }
                }
            }
            return EditProperty(itemAfter);
        }
        public bool EditLandFull(Land itemBefore, Land itemAfter)
        {
            Property dummy = new Property();
            bool relatedGood = true;
            List<Area> relatedAreas;
            relatedAreas = PropertyTree.Find(new QuadTreeRectangle(itemAfter.GpsLocation[0].lengthPosition, itemAfter.GpsLocation[0].widthPosition,
                                                    itemAfter.GpsLocation[1].lengthPosition, itemAfter.GpsLocation[1].widthPosition));
            Property related;
            //prehľadávam či môžem zmeniť referencie
            if (relatedAreas.Count < 5)
            {
                for (int i = 0; i < relatedAreas.Count; i++)
                {
                    dummy.Id = relatedAreas[i].Id;
                    related = Properties.FindItem(dummy);
                    for (int j = 0; j < related.LandsId.Count; j++)
                    {
                        if (related.LandsId[j] == -1)
                        {
                            break;
                        }
                        if (j == related.LandsId.Count - 1)
                        {
                            relatedGood = false;
                        }
                    }

                }
                if (!relatedGood)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            //clearujem stare referencie
            for (int i = 0; i < itemBefore.PropertiesId.Count; i++)
            {
                if (itemBefore.PropertiesId[i] != -1)
                {
                    dummy.Id = itemBefore.PropertiesId[i];
                    dummy = Properties.FindItem(dummy);
                    for (int j = 0; j < dummy.LandsId.Count; j++)
                    {
                        if (dummy.LandsId[j] == itemBefore.Id)
                        {
                            dummy.LandsId[j] = -1;
                            Properties.UpdateItem(dummy);
                            break;
                        }
                    }
                }
            }
            LandTree.Remove(itemBefore);
            Area area = new Area(itemAfter.Id, itemAfter.GpsLocation);
            LandTree.Insert(area);
            //davam nove
            for (int i = 0; i < relatedAreas.Count; i++)
            {
                dummy.Id = relatedAreas[i].Id;
                related = Properties.FindItem(dummy);
                for (int j = 0; j < related.LandsId.Count; j++)
                {
                    if (related.LandsId[j] == -1)
                    {
                        related.LandsId[j] = itemAfter.Id;
                        itemAfter.PropertiesId[itemAfter.PropertiesId.IndexOf(itemAfter.PropertiesId.Min())] = related.Id;
                        Properties.UpdateItem(related);
                        break;
                    }
                }
            }
            return EditLand(itemAfter);
        }
        public bool EditProperty(Property item)
        {
            return Properties.UpdateItem(item);
        }
        public bool EditLand(Land item)
        {
            return Lands.UpdateItem(item);
        }
        public void GenerateData(double[] configuration)
        {
            CadastreGenerator generator = new CadastreGenerator();
            generator.GenerateBinaryData(configuration, Lands, Properties, LandTree, PropertyTree, this);
        }
        public string[] FileExtract(int type)
        {
            if (type == 0)
            {
                return Lands.FileExtract();
            }
            else
            {
                return Properties.FileExtract();
            }
        }

    }
}
