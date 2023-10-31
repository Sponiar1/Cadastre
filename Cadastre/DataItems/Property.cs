using Cadastre.DataStructure.Templates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.DataItems
{
    public class Property : Area, IComparator<Property>
    {
        ArrayList lands;
        public Property(int id, string description, GPSPosition[] gpsLocation) : base(id, description, gpsLocation)
        {
            lands = new ArrayList();
        }

        int IComparator<Property>.CompareById(Property otherItem)
        {
            return CompareById(otherItem);
        }
    }
}
