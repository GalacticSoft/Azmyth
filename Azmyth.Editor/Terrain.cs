using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Editor
{
    public enum TerrainTypes
    {
        Coast,
        Dirt,
        Forest,
        River
    }

    public class Terrain
    {
        public Terrain(int x, int y, TerrainTypes t, double v, double h, double hr)
        {
            X = x;
            Y = y;
            TerrainType = t;
            Value = v;
            Height = h;
            HeightRounded = hr;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public TerrainTypes TerrainType { get; set; }

        public double Value { get; set; }
        public double Height { get; set; }
        public double HeightRounded { get; set; }
    }
}
