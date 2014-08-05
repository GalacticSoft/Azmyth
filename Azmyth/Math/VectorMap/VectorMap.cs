using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Math
{
    public struct VectorMap
    {
        public const int MinX = 0;
        public const int MinY = 0;
        public const int MinZ = 0;

        private int _maxX;
        private int _maxY;
        private int _maxZ;

        public int MaxX { get { return _maxX; } }
        public int MaxY { get { return _maxY; } }
        public int MaxZ { get { return _maxZ; } }

        public VectorGrid[] Grids { get { return _grids; } }

        private VectorGrid[] _grids;

        public VectorMap(int maxX, int maxY, int maxZ)
        {
            _maxX = maxX;
            _maxY = maxY;
            _maxZ = maxZ;

            _grids = new VectorGrid[_maxZ];

            for (int z = 0; z < _maxZ; z++)
            {
                _grids[z] = new VectorGrid(_maxX, _maxY, z);
            }
        }

        public VectorGrid this[int z]
        {
            get
            {
                return _grids[z];
            }
            set
            {
                VectorGrid grid = value;
                
                grid.ZIndex = z;

                _grids[z] = grid;
            }
        }

        public VectorCell this[int x, int y, int z]
        {
            get
            {
                if((z >= 0 && z < MaxZ) && (x >= 0 && x < MaxX) && (y >= 0 && y < MaxY))
                {
                    return _grids[z][x, y];
                }

                throw new IndexOutOfRangeException();
            }

            set
            {
                if((z >= 0 && z < MaxZ) && (x >= 0 && x < MaxX) && (y >= 0 && y < MaxY))
                {
                    _grids[z][x, y] = value;
                }
                

                throw new IndexOutOfRangeException();
            }
        }

        public VectorCell this[Vector vector]
        {
            get
            {
                if ((vector.Z >= 0 && vector.Z < MaxZ) && (vector.X >= 0 && vector.X < MaxX) && (vector.Y >= 0 && vector.Y < MaxY))
                {
                    return _grids[vector.Z][vector.X, vector.Y];
                }

                throw new IndexOutOfRangeException();
            }

            set
            {
                if ((vector.Z >= 0 && vector.Z < MaxZ) && (vector.X >= 0 && vector.X < MaxX) && (vector.Y >= 0 && vector.Y < MaxY))
                {
                    _grids[vector.Z][vector.X, vector.Y] = value;
                }

                throw new IndexOutOfRangeException();
            }
        }
    }
}
