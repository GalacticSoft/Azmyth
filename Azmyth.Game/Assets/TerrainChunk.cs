using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azmyth.Assets;
using Azmyth;
using System.Drawing;

namespace Azmyth.Assets
{
    public class TerrainChunk : Asset
    {
        private World m_world = null;
        private RectangleF m_bounds = new RectangleF(0, 0, 0, 0);
        private QuadTree<TerrainTile> m_tiles = new QuadTree<TerrainTile>(new Size(64, 64), 64);

        private TerrainChunk() { }

        public TerrainChunk(RectangleF bounds, World world)
        {
            int totalCells = (int)bounds.Width * (int)bounds.Height;

            for (int index = 0; index < totalCells; index++)
            {
                int cellX = (int)((index / bounds.Height)) + (int)bounds.X;
                int cellY = (int)((index % bounds.Height)) + (int)bounds.Y;

                TerrainTile tile = world.GetRoom(cellX, cellY);

                tile.Bounds = new RectangleF(cellX, cellY, 1, 1);

                m_tiles.Insert(tile);
            }

            m_world = world;
            m_bounds = bounds;
        }

        public List<TerrainTile> GetTiles()
        {
            return GetTiles(m_bounds);
        }

        public List<TerrainTile> GetTiles(RectangleF bounds)
        {
            return m_tiles.Query(bounds);
        }

        public override RectangleF Bounds
        {
            get 
            { 
                return m_bounds; 
            }
        }
    }
}
