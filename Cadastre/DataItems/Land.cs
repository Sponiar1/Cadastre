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
        ArrayList areas;

        public Land(int idNumber, string description, GPSPosition[] gpsLocation) : base(idNumber, description, gpsLocation)
        {
            areas = new ArrayList();
        }

        int IComparator<Land>.CompareById(Land otherItem)
        {
            return CompareById(otherItem);
        }
    }
}
