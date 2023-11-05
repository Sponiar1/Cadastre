using Cadastre.DataStructure.Templates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.DataItems
{
    public class Land : Area, IComparator<Land>
    {
        public List<Property> Properties { get; set; }

        public Land(int idNumber, string description, GPSPosition[] gpsLocation) : base(idNumber, description, gpsLocation)
        {
            Properties = new List<Property>();
        }

        int IComparator<Land>.CompareById(Land otherItem)
        {
            return CompareById(otherItem);
        }

        override
        public string getListOfAreas()
        {
            string listOfAreas = "";
            for(int i = 0; i < Properties.Count; i++)
            {
                listOfAreas += Properties[i].Id + ", ";
            }
            return listOfAreas;
        }
    }
}
