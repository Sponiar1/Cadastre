using Cadastre.DataItems;
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
        QuadTree<Property> PropertyTree;
        QuadTree<Land> LandTree;

        public CadastreBinaryManager()
        {
            propertyFileName = "Properties.bin";
            propertyOverflowName = "PropertiesOverFlow.bin";
            landFileName = "Lands.bin";
            landOverflowName = "LandsOverflow.bin";
            indexProperty = "PropertyIndex.csv";
            indexLand = "LandIndex.csv";
            PropertyTree = new QuadTree<Property>(0,0,double.MaxValue, double.MaxValue,20);
            LandTree = new QuadTree<Land>(0, 0, double.MaxValue, double.MaxValue, 20);
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
            if(target == default)
            {
                return null;
            }
            for(int i = 0; i < 6; i++)
            {
                Land dummy = new Land();
                dummy.Id = target.LandsId[i];
                if(dummy.Id == -1)
                {
                    return result;
                }
                dummy = Lands.FindItem(dummy);
                if(dummy != null)
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
            for (int i = 0; i < 6; i++)
            {
                Property dummy = new Property();
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
        public bool AddItem(Area item, int type)
        {
            if(type == 0)
            {
                if(Properties.Insert((Property)item))
                {
                    LandTree.Find(new QuadTreeRectangle(item.GpsLocation[0].lengthPosition, item.GpsLocation[0].widthPosition,
                                                        item.GpsLocation[1].lengthPosition, item.GpsLocation[2].widthPosition));
                }
            }
            return false;
        }
    }
}
