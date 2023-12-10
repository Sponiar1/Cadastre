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
        public string Description { get; set; }
        public List<Property> Properties { get; set; }
        public List<int> PropertiesId { get; set; }

        public Land(int idNumber, string description, GPSPosition[] gpsLocation) : base(idNumber, gpsLocation)
        {
            Description = description;
            Properties = new List<Property>();
            PropertiesId = new List<int>(5) { -1, -1, -1, -1, -1 };
        }
        public Land() : base(-1, null)
        {
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

        override
        public string GetListOfAreasID()
        {
            string listOfAreas = "";
            for (int i = 0; i < PropertiesId.Count; i++)
            {
                if (PropertiesId[i] != -1)
                {
                    listOfAreas += PropertiesId[i] + ", ";
                }
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
            byte[] bytes = new byte[GetSize()];

            byte[] parent = base.ToByteArray();
            int totalLength = parent.Length;
            Array.Copy(parent, 0, bytes, 0, parent.Length);

            Description = Description.PadRight(11, ' ');
            byte[] descArray = Encoding.UTF8.GetBytes(Description);
            Array.Copy(descArray, 0, bytes, totalLength, descArray.Length);
            totalLength += descArray.Length;

            for(int i = 0; i < 5; i++)
            {
                byte[] idArray = BitConverter.GetBytes(PropertiesId[i]);
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
                byte[] idArray = new byte[4];
                Array.Copy(byteArray, offset, idArray, 0, idArray.Length);
                PropertiesId[i] = BitConverter.ToInt32(byteArray, offset);
                offset += sizeof(int);
            }
        }

        public Land CreateInstance()
        {
            GPSPosition[] gps = new GPSPosition[2] { new GPSPosition(int.MaxValue, int.MaxValue, 0), new GPSPosition(int.MaxValue, int.MaxValue, 1) };
            Land dummy = new Land(-1, "", gps);
            dummy.PropertiesId = new List<int>(5);
            for(int i = 0; i < 5; i++)
            {
                dummy.PropertiesId.Add(-1);
            }
            return dummy;
        }

        public string ExtractInfo()
        {
            string baseInfo = base.ExtractInfo() + "Description: " + Description + ", Related Properties: ";
            for(int i = 0; i < 5; i++)
            {
                baseInfo += PropertiesId + ", ";
            }
            return baseInfo;
        }
    }
}
