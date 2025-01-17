﻿using Cadastre.Files.Templates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.DataStructure.Templates
{
    public class QuadTreeData : IComparator<QuadTreeData>, IData<QuadTreeData>
    {
        public int Id { get; set; }
        public int IsInMinimalZone { get; set; } //0=yes, 1=no/dont know

        public QuadTreeData(int id)
        {
            IsInMinimalZone = 1;
            Id = id;
        }
        public int CompareTo(QuadTreeRectangle other)
        {
            return 0;
        }
        public int CompareIntersections(QuadTreeRectangle other)
        {
            return 0;
        }
        public int ChangePossible()
        {
            return IsInMinimalZone;
        }

        public void FoundSmallestZone()
        {
            IsInMinimalZone = 0;
        }

        public void RelocatedToBiggerZone()
        {
            IsInMinimalZone = 1;
        }

        public int CompareById(QuadTreeData otherItem)
        {
            if (Id == otherItem.Id)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        public double GetSizeArea()
        {
            return 0;
        }

        public bool Equals(QuadTreeData obj)
        {
            return obj.Id == Id;
        }

        public BitArray GetHash()
        {
            throw new NotImplementedException();
        }

        public int GetSize()
        {
            return sizeof(int);
        }

        public byte[] ToByteArray()
        {
            throw new NotImplementedException();
        }

        public void FromByteArray(byte[] byteArray)
        {
            throw new NotImplementedException();
        }

        public QuadTreeData CreateInstance()
        {
            return new QuadTreeData(-1);
        }

        public string ExtractInfo()
        {
            return Id.ToString();
        }
    }
}
