using Cadastre.DataStructure.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.DataItems
{
    public class Area : QuadTreeData, IComparator<Area>
    {
        protected string description;
        public GPSPosition[] GpsLocation { get; }
        public Area(int id, string description, GPSPosition[] gpsLocation) : base(id)
        {
            this.description = description;
            this.GpsLocation = gpsLocation;
        }

        public int CompareTo(QuadTreeRectangle other)
        {
            if (GpsLocation[0].IsInside(other) && GpsLocation[1].IsInside(other))
            {
                return 0;
            }
            return -1;
        }

        int IComparator<Area>.CompareById(Area otherItem)
        {
            return CompareById(otherItem);
        }
    }
}
