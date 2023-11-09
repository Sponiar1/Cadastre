﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.DataStructure.Templates
{
    public class QuadTreeData : IComparator<QuadTreeData>
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

        public double GetSize()
        {
            return 0;
        }
    }
}
