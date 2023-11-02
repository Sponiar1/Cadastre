using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.DataStructure.Templates
{
    public interface IComparator<T>
    {
        int CompareTo(QuadTreeRectangle rectangle);
        int CompareIntersections(QuadTreeRectangle rectangle);
        int CompareById(T otherItem);
        int ChangePossible();
        void FoundSmallestZone();
        void RelocatedToBiggerZone();
    }
}
