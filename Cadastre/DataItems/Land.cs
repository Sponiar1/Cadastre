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
    public class Land : Area, IComparator<Land>, IData<Land>
    {
        public List<Property> Properties { get; set; }
        public List<int> PropertiesId { get; set; }

        public Land(int idNumber, string description, GPSPosition[] gpsLocation) : base(idNumber, description, gpsLocation)
        {
            Properties = new List<Property>();
        }

        int IComparator<Land>.CompareById(Land otherItem)
        {
            return CompareById(otherItem);
        }

        override
        public string GetListOfAreas()
        {
            string listOfAreas = "";
            for(int i = 0; i < Properties.Count; i++)
            {
                listOfAreas += Properties[i].Id + ", ";
            }
            return listOfAreas;
        }

        public bool Equals(Land obj)
        {
            return base.Equals(obj);
        }

        public BitArray GetHash()
        {
            return base.GetHash();
        }

        public int GetSize()
        {
            return base.GetSize() + 11 + 5 * sizeof(int);
        }

        public byte[] ToByteArray()
        {
            byte[] bytes = new byte[base.GetSize()];

            byte[] parent = base.ToByteArray();
            int totalLength = parent.Length;
            Array.Copy(parent, 0, bytes, 0, parent.Length);

            byte[] descArray = Encoding.UTF8.GetBytes(Description);
            Array.Copy(descArray, 0, bytes, totalLength, descArray.Length);
            totalLength += descArray.Length;

            for(int i = 0; i < 5; i++)
            {
                byte[] idArray = BitConverter.GetBytes(Properties[i].Id);
                Array.Copy(idArray, 0, bytes, totalLength, idArray.Length);
                totalLength += idArray.Length;
            }
            return bytes;
        }

        public void FromByteArray(byte[] byteArray)
        {
            base.FromByteArray(byteArray);
            int offset = base.GetSize();

            Description = Encoding.UTF8.GetString(byteArray, offset, 11);
            offset += 11;

            for (int i = 0; i < 5; i++)
            {
                PropertiesId[i] = BitConverter.ToInt32(byteArray, offset);
                offset += sizeof(int);
            }
        }

        public Land DummyClass()
        {
            GPSPosition[] gps = new GPSPosition[2] { new GPSPosition(int.MaxValue, int.MaxValue, 0), new GPSPosition(int.MaxValue, int.MaxValue, 1) };
            Land dummy = new Land(-1, "", gps);
            PropertiesId = new List<int>(5);
            for(int i = 0; i < 5; i++)
            {
                dummy.PropertiesId[i] = -1;
            }
            return dummy;
        }
    }
}
