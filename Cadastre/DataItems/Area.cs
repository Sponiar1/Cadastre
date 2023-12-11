using Cadastre.DataStructure.Templates;
using Cadastre.Files.Templates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.DataItems
{
    public class Area : QuadTreeData, IComparator<Area>, IData<Area>
    {
        public GPSPosition[] GpsLocation { get; set; }
        public Area(int id, GPSPosition[] gpsLocation) : base(id)
        {
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

        public string GetCoordinates()
        {
            double x0 = Math.Round(GpsLocation[0].lengthPosition, 2);
            double y0 = Math.Round(GpsLocation[0].widthPosition, 2);
            double x1 = Math.Round(GpsLocation[1].lengthPosition, 2);
            double y1 = Math.Round(GpsLocation[1].widthPosition, 2);
            return "[" + x0 + " , " + y0 + "] [" + x1 + " , " + y1 + "]";
        }

        virtual public string GetListOfAreas()
        {
            return "";
        }
        virtual public string GetListOfAreasID()
        {
            return "";
        }
        public double GetSizeArea()
        {
            return GpsLocation[1].lengthPosition - GpsLocation[0].lengthPosition;
        }

        public bool Equals(Area obj)
        {
            return base.Equals(obj);
        }

        public BitArray GetHash()
        {//7919, 3, 191, 1123
            int hashCode = Id % 191;
            BitArray hash = new BitArray(BitConverter.GetBytes(hashCode));
            BitArray reversedHash = new BitArray(hash.Count);
            for (int i = 0; i < hash.Count; i++)
            {
                reversedHash[i] = hash[hash.Count - i - 1];
            }
            return reversedHash;
        }

        public int GetSize()
        {
            return 4 * sizeof(double) + sizeof(int);
        }

        public byte[] ToByteArray()
        {
            byte[] bytes = new byte[GetSize()];
            int totalLength;

            byte[] idArray = BitConverter.GetBytes(Id);
            Array.Copy(idArray, 0, bytes, 0, idArray.Length);
            totalLength = idArray.Length;

            byte[] bottomXArray = BitConverter.GetBytes(GpsLocation[0].lengthPosition);
            Array.Copy(bottomXArray, 0, bytes, totalLength, bottomXArray.Length);
            totalLength += bottomXArray.Length;

            byte[] bottomYArray = BitConverter.GetBytes(GpsLocation[0].widthPosition);
            Array.Copy(bottomYArray, 0, bytes, totalLength, bottomYArray.Length);
            totalLength += bottomYArray.Length;

            byte[] UpperXArray = BitConverter.GetBytes(GpsLocation[1].lengthPosition);
            Array.Copy(UpperXArray, 0, bytes, totalLength, UpperXArray.Length);
            totalLength += UpperXArray.Length;

            byte[] UpperYArray = BitConverter.GetBytes(GpsLocation[1].widthPosition);
            Array.Copy(UpperYArray, 0, bytes, totalLength, UpperYArray.Length);

            return bytes;
        }

        public void FromByteArray(byte[] byteArray)
        {
            int offset = 0;
            Id = BitConverter.ToInt32(byteArray, offset);
            offset += sizeof(int);

            GpsLocation[0].lengthPosition = BitConverter.ToDouble(byteArray, offset);
            offset += sizeof(double);

            GpsLocation[0].widthPosition = BitConverter.ToDouble(byteArray, offset);
            offset += sizeof(double);

            GpsLocation[1].lengthPosition = BitConverter.ToDouble(byteArray, offset);
            offset += sizeof(double);

            GpsLocation[1].widthPosition = BitConverter.ToDouble(byteArray, offset);

        }

        public Area CreateInstance()
        {
            GPSPosition[] gps = new GPSPosition[2] { new GPSPosition(int.MaxValue, int.MaxValue, 0), new GPSPosition(int.MaxValue, int.MaxValue, 1) };
            return new Area(-1, gps);
        }

        public string ExtractInfo()
        {
            return "Id: " + Id + ", GPS: [" + GpsLocation[0].lengthPosition + ", " + GpsLocation[0].widthPosition + "][" + GpsLocation[1].lengthPosition + ", " + GpsLocation[1].widthPosition + "]";
        }
    }
}
