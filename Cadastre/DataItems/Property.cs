using Cadastre.DataStructure.Templates;
using Cadastre.Files.Templates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Cadastre.DataItems
{
    public class Property : Area, IComparator<Property>, IData<Property>
    {
        public string Description { get; set; }
        public int RegisterNumber {  get; set; }
        public List<Land> Lands { get; set; }
        public List<int> LandsId { get; set; }
        public Property(int id, string description, GPSPosition[] gpsLocation) : base(id, gpsLocation)
        {
            this.Description = description;
            Lands = new List<Land>();
            LandsId = new List<int>(6) { -1,-1,-1,-1,-1,-1};
            RegisterNumber = 11;
        }
        public Property() : base(-1,null)
        {
            
        }
        int IComparator<Property>.CompareById(Property otherItem)
        {
            return CompareById(otherItem);
        }

        override
        public string GetListOfAreas()
        {
            string listOfAreas = "";
            for (int i = 0; i < Lands.Count; i++)
            {
                listOfAreas += Lands[i].Id + ", ";
            }
            return listOfAreas;
        }

        override
        public string GetListOfAreasID()
        {
            string listOfAreas = "";
            for (int i = 0; i < LandsId.Count; i++)
            {
                if (LandsId[i] != -1)
                {
                    listOfAreas += LandsId[i] + ", ";
                }
            }
            return listOfAreas;
        }
        public bool Equals(Property obj)
        {
            return base.Equals(obj);
        }

        public BitArray GetHash()
        {
            return base.GetHash();
        }

        public int GetSize()
        {
            return base.GetSize() + 15 + sizeof(int) + 6*sizeof(int);
        }

        public byte[] ToByteArray()
        {
            byte[] bytes = new byte[GetSize()];

            byte[] parent = base.ToByteArray();
            int totalLength = parent.Length;
            Array.Copy(parent, 0, bytes, 0, parent.Length);

            Description = Description.PadRight(15, ' ');
            byte[] descArray = Encoding.UTF8.GetBytes(Description);
            Array.Copy(descArray, 0, bytes, totalLength, descArray.Length);
            totalLength += descArray.Length;

            byte[] registerArray = BitConverter.GetBytes(RegisterNumber);
            Array.Copy(registerArray, 0, bytes, totalLength, registerArray.Length);
            totalLength += registerArray.Length;

            for (int i = 0; i < 6; i++)
            {
                int landid = LandsId[i];
                byte[] idArray = BitConverter.GetBytes(landid);
                Array.Copy(idArray, 0, bytes, totalLength, idArray.Length);
                totalLength += idArray.Length;
            }

            return bytes;
        }

        public void FromByteArray(byte[] byteArray)
        {
            base.FromByteArray(byteArray);
            int offset = base.GetSize();


            Description = Encoding.UTF8.GetString(byteArray, offset, 15);
            //Description = Description.TrimEnd('#');
            offset += 15;

            RegisterNumber = BitConverter.ToInt32(byteArray, offset);
            offset += sizeof(int);

            for (int i = 0; i < 6; i++)
            {
                byte[] idArray = new byte[4];
                Array.Copy(byteArray, offset, idArray, 0, idArray.Length);
                LandsId[i] = BitConverter.ToInt32(idArray, 0);
                offset += sizeof(int);
            }
        }

        public Property CreateInstance()
        {
            GPSPosition[] gps = new GPSPosition[2] { new GPSPosition(int.MaxValue, int.MaxValue, 0), new GPSPosition(int.MaxValue, int.MaxValue, 1) };
            Property dummy = new Property(-1, "", gps);
            dummy.RegisterNumber = -1;
            dummy.LandsId = new List<int>(6);
            for (int i = 0; i < 6; i++)
            {
                dummy.LandsId.Add(-1);
            }
            return dummy;
        }
        public string ExtractInfo()
        {
            string baseInfo = base.ExtractInfo() + "Description: " + Description + ", Register number: " + RegisterNumber + " Related Lands: ";
            for (int i = 0; i < 6; i++)
            {
                baseInfo += LandsId[i] + ", ";
            }
            return baseInfo;
        }
    }
}
