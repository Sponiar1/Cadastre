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
        public int RegisterNumber {  get; set; }
        public List<Land> Lands { get; set;}
        public Property(int id, string description, GPSPosition[] gpsLocation) : base(id, description, gpsLocation)
        {
            Lands = new List<Land>();
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
            return base.GetSize() + 15 + sizeof(int) + 5*sizeof(int);
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

            for (int i = 0; i < 5; i++)
            {
                byte[] idArray = BitConverter.GetBytes(Lands[i].Id);
                Array.Copy(idArray, 0, bytes, totalLength, idArray.Length);
                totalLength += idArray.Length;
            }
            return bytes;
        }

        public void FromByteArray(byte[] byteArray)
        {
            base.FromByteArray(byteArray);
            int offset = base.GetSize();

            Description = Encoding.UTF8.GetString(byteArray, offset, 10);
        }
    }
}
