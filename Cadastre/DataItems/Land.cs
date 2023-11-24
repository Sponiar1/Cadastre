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

        bool IData<Land>.Equals(Land obj)
        {
            return base.Equals(obj);
        }

        BitArray IData<Land>.GetHash()
        {
            throw new NotImplementedException();
        }

        int IRecord<Land>.GetSize()
        {
            throw new NotImplementedException();
        }

        byte[] IRecord<Land>.ToByteArray()
        {
            throw new NotImplementedException();
        }

        void IRecord<Land>.FromByteArray(byte[] byteArray)
        {
            throw new NotImplementedException();
        }

    }
}
