using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Math
{
    public struct VectorGrid
    {
        private int _maxX;
        private int _maxY;
        private int _zIndex;

        public int MaxX { get { return _maxX; } }
        public int MaxY { get { return _maxY; } }
        public int ZIndex { get { return _zIndex; } set { _zIndex = value; } }

        public VectorCell[] Cells { get { return _cells; } }

        private VectorCell[] _cells;

        public VectorGrid(int maxX, int maxY, int zIndex)
        {
            _maxX = maxX;
            _maxY = maxY;
            _zIndex = zIndex;

            _cells = new VectorCell[_maxX * _maxY];

            for (int i = 0; i < (MaxX * MaxY); i++)
            {
                _cells[i].X = i / MaxX;
                _cells[i].Y = i % MaxX;
                _cells[i].Z = zIndex;
            }
        }

        public VectorCell GetCell(int x, int y)
        {
            if ((x >= 0 && x < MaxX-1) && (y >= 0 && y < MaxY-1))
            {
                return _cells[(x * _maxY) + y];
            }

            return VectorCell.Empty;
        }

        public VectorCell this[int x, int y]
        {
            get
            {
                VectorCell cell = new VectorCell(-1, -1, -1);

                if ((x >= 0 && x < MaxX) && (y >= 0 && y < MaxY))
                {
                    cell = _cells[(x * _maxY) + y];
                }

                return cell;
            }
            set
            {
                if ((x >= 0 && x < MaxX) && (y >= 0 && y < MaxY))
                {
                    VectorCell newCell = value;
                    
                    newCell.X = x;
                    newCell.Y = y;
                    newCell.Z = ZIndex;

                    _cells[(x * _maxY) + y] = newCell;
                }
            }
        }
    }

}
