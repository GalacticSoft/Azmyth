using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azmyth.Assets;

namespace Azmyth.Mono
{
    public enum Planes
    {
        Prime   = 0,
        Earth   = 1,
        Air     = 2,
        Water   = 3,
        Fire    = 4,
    }

    
    public class Plane
    {
        public bool IsVisible = true;
        public Planes m_plane = Planes.Prime;
        public World world;
        public Directions direction;
        public TerrainChunk chunk;

        public Plane(Planes plane)
        {
            m_plane = plane;
        }

        public void LoadPlane(int seed, int x, int y, int radius)
        {
            world = new World(seed);

            chunk = new TerrainChunk(world, x, y, radius);
        }
    }
}
