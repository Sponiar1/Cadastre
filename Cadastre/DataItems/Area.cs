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
        public string Description { get; set; }
        public GPSPosition[] GpsLocation { get; set; }
        public Area(int id, string description, GPSPosition[] gpsLocation) : base(id)
        {
            this.Description = description;
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

        public int CompareIntersections(QuadTreeRectangle other)
        {
            if (GpsLocation[1].lengthPosition < other.BottomLeftX || other.UpperRightX < GpsLocation[0].lengthPosition)
            {
                return -1;
            }

            if (GpsLocation[0].widthPosition > other.UpperRightY || other.BottomLeftY > GpsLocation[1].widthPosition)
            {
                return -1;
            }

            return 0;
        }
        int IComparator<Area>.CompareById(Area otherItem)
        {
            return CompareById(otherItem);
        }

        public string getCoordinatesInReadable()
        {
            double x0 = Math.Round(GpsLocation[0].lengthPosition, 2);
            double y0 = Math.Round(GpsLocation[0].widthPosition, 2);
            double x1 = Math.Round(GpsLocation[1].lengthPosition, 2);
            double y1 = Math.Round(GpsLocation[1].widthPosition, 2);
            return "[" + x0 + " , " + y0 + "] [" + x1 + " , " + y1 + "]";
        }

        virtual public string getListOfAreas()
        {
            return "";
        }

        public double getSize()
        {
            return GpsLocation[1].lengthPosition - GpsLocation[0].lengthPosition;
        }
    }
}
