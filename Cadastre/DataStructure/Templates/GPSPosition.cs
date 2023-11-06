using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.DataStructure.Templates
{
    public class GPSPosition
    {
        public char widthDirection { get; }
        public char lengthDirection { get; }
        public double widthPosition { get; }
        public double lengthPosition { get; }

        public GPSPosition(char width, char length, double lengthPosition, double widthPosition)
        {
            this.widthDirection = width;
            this.lengthDirection = length;
            this.widthPosition = widthPosition;
            this.lengthPosition = lengthPosition;
        }
        public GPSPosition(double lengthPosition, double widthPosition, int type)
        {
            if(type == 0)
            {
                this.widthDirection = 'N';
                this.lengthDirection = 'W';
            }
            else if (type == 1) 
            {
                this.widthDirection = 'S';
                this.lengthDirection = 'E';
            }
            this.widthPosition = widthPosition;
            this.lengthPosition = lengthPosition;
        }
        public bool IsInside(QuadTreeRectangle other)
        {
            if (widthPosition >= other.BottomLeftY && widthPosition <= other.UpperRightY &&
                lengthPosition >= other.BottomLeftX && lengthPosition <= other.UpperRightX)
            {
                return true;
            }
            return false;
        }

    }
}
