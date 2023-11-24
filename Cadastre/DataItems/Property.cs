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

        bool IData<Property>.Equals(Property obj)
        {
            return base.Equals(obj);
        }

        BitArray IData<Property>.GetHash()
        {
            throw new NotImplementedException();
        }

        int IRecord<Property>.GetSize()
        {
            throw new NotImplementedException();
        }

        byte[] IRecord<Property>.ToByteArray()
        {
            throw new NotImplementedException();
        }

        void IRecord<Property>.FromByteArray(byte[] byteArray)
        {
            throw new NotImplementedException();
        }
    }
}
