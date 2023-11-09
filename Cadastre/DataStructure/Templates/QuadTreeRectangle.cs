using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.DataStructure.Templates
{
    public class QuadTreeRectangle
    {
        public double BottomLeftX { get; }
        public double BottomLeftY { get; }
        public double UpperRightX { get; }
        public double UpperRightY { get; }

        public QuadTreeRectangle(double bottomLeftX, double bottomLeftY, double upperRightX, double upperRightY)
        {
            BottomLeftX = bottomLeftX;
            BottomLeftY = bottomLeftY;
            UpperRightX = upperRightX;
            UpperRightY = upperRightY;
        }

        public Boolean Blending(QuadTreeRectangle other)
        {
            if (UpperRightX < other.BottomLeftX || other.UpperRightX < BottomLeftX)
            {
                return false;
            }

            if (BottomLeftY > other.UpperRightY || other.BottomLeftY > UpperRightY)
            {
                return false;
            }

            return true;
        }
    }
}
