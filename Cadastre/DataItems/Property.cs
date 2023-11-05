using Cadastre.DataStructure.Templates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Cadastre.DataItems
{
    public class Property : Area, IComparator<Property>
    {
        public List<Land> Lands {get; set;}
        public Property(int id, string description, GPSPosition[] gpsLocation) : base(id, description, gpsLocation)
        {
            Lands = new List<Land>();
        }

        int IComparator<Property>.CompareById(Property otherItem)
        {
            return CompareById(otherItem);
        }

        override
        public string getListOfAreas()
        {
            string listOfAreas = "";
            for (int i = 0; i < Lands.Count; i++)
            {
                listOfAreas += Lands[i].Id + ", ";
            }
            return listOfAreas;
        }
    }
}
