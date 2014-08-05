using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Math
{
    public struct VectorCell
    {
        public static readonly VectorCell Empty = new VectorCell(0, 0, 0);

        private Vector _cellVector;

        public Vector CellVector { get { return _cellVector; } }

        public int X 
        { 
            get 
            { 
                return _cellVector.X; 
            } 
            set 
            {
                _cellVector = new Vector(value, _cellVector.Y, _cellVector.Z);
            } 
        }

        public int Y 
        { 
            get 
            { 
                return _cellVector.Y; 
            } 
            set 
            {
                _cellVector = new Vector(_cellVector.X, value, _cellVector.Z);
            } 
        }

        public int Z
        {
            get
            {
                return _cellVector.Z;
            }
            set
            {
                _cellVector = new Vector(_cellVector.X, _cellVector.Y, value);
            }
        }

        public VectorCell(int x, int y, int z)
        {
            _cellVector = new Vector(x, y, z);
        }

        public override bool Equals(object obj)
        {
            bool equals = false;

            if (obj != null)
            {
                if (obj is VectorCell)
                {
                    equals = (((VectorCell)obj) == this);
                }
            }

            return equals;
        }

        public static bool operator==(VectorCell cell1, VectorCell cell2)
        {
            bool equals = false;

            if (cell1 != null && cell2 != null)
            {
                equals = (cell1.CellVector == cell2.CellVector);
            }

            return equals;
        }

        public static bool operator !=(VectorCell cell1, VectorCell cell2)
        {
            return !(cell1 == cell2);
        }

        public override int GetHashCode()
        {
            return _cellVector.GetHashCode();
        }
    }
}
