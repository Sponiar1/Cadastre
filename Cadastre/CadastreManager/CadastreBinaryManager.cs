﻿using Cadastre.DataItems;
using Cadastre.DataStructure;
using Cadastre.DataStructure.Templates;
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
            PropertyTree = new QuadTree<Area>(0, 0, double.MaxValue, double.MaxValue, 20);
            LandTree = new QuadTree<Area>(0, 0, double.MaxValue, double.MaxValue, 20);
        }

        public void NewFile()
        {
            Properties = new DynamicHash<Property>(2, propertyFileName, 8, propertyOverflowName);
            Lands = new DynamicHash<Land>(2, landFileName, 8, landOverflowName);
        }
        public void Save()
        {
            Properties.SaveIndex(indexProperty);
            Lands.SaveIndex(indexLand);
        }
        public void Load()
        {
            Properties = new DynamicHash<Property>(propertyFileName, propertyOverflowName, indexProperty);
            Properties.LoadIndex(propertyFileName);
            Properties.LoadIndex(landFileName);

            Lands = new DynamicHash<Land>(landFileName, landOverflowName, indexLand);
            Lands.LoadIndex(propertyFileName);
            Lands.LoadIndex(landFileName);
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
            for (int i = 0; i < 6; i++)
            {
                Land dummy = new Land();
                dummy = dummy.CreateInstance();
                dummy.Id = target.LandsId[i];
                if (dummy.Id == -1)
                {
                    return result;
                }
                dummy = Lands.FindItem(dummy);
                if (dummy != null)
                {
                    result.Add(dummy);
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
                if (dummy.Id == -1)
                {
                    return result;
                }
                dummy = Properties.FindItem(dummy);
                if (dummy != null)
                {
                    result.Add(dummy);
                }
            }
            return result;
        }
        public bool AddItem(double[] configuration, string description, int type)
        {
            bool relatedGood = true;
            GPSPosition[] gps = new GPSPosition[2];
            gps[0] = new GPSPosition('N', 'E', configuration[0], configuration[1]);
            gps[1] = new GPSPosition('S', 'W', configuration[2], configuration[3]);
            //Land
            if (type == 0)
            {
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
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                if (Lands.Insert((Land)item))
                {
                    if (true)
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
                                    Properties.UpdateItem(related);
                                    break;
                                }
                            }
                        }
                    }
                    Area newItem = new Area((int)configuration[4], gps);
                    LandTree.Insert(newItem);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //Property
                Property item = new Property((int)configuration[4], description, gps);
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
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                if (Properties.Insert((Property)item))
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
                                Lands.UpdateItem(related);
                                break;
                            }
                        }
                    }
                    //pridat do quadstromu
                    Area newItem = new Area((int)configuration[4], gps);
                    PropertyTree.Insert(newItem);
                    return true;
                }
                else
                {
                    return false;
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
    }
}