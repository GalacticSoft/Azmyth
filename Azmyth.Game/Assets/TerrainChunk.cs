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
        private Dictionary<TerrainTypes, long> m_tileCount = new Dictionary<TerrainTypes,long>();

        public World World
        {
            get 
            { 
                return m_world; 
            }
        }

        private TerrainChunk()
        {
            foreach (TerrainTypes t in Enum.GetValues(typeof(TerrainTypes)))
            {
                m_tileCount.Add(t, 0);
            }
        }

        public TerrainChunk(RectangleF bounds, World world) : this()
        {
            int totalCells = (int)bounds.Width * (int)bounds.Height;

            for (int index = 0; index < totalCells; index++)
            {
                int cellX = (int)((index / bounds.Height)) + (int)bounds.X;
                int cellY = (int)((index % bounds.Height)) + (int)bounds.Y;

                TerrainTile tile = world.LoadTile(cellX, cellY, this);

                tile.Bounds = new RectangleF(cellX, cellY, 1, 1);

                m_tileCount[tile.Terrain]++;

                m_tiles.Insert(tile);
            }

            m_world = world;
            m_bounds = bounds;
        }

        public TerrainTile GetTile(int x, int y)
        {
            TerrainTile tile = null;
            List<TerrainTile> tiles = m_tiles.Query(new RectangleF(x, y, 1, 1));

            if(tiles.Count > 0)
            {
                tile = tiles[0];
            }
            else
            {
                tile = m_world.GetTile(x, y);
            }

            return tile;
        }

        public List<TerrainTile> GetTiles()
        {
            return GetTiles(m_bounds);
        }

        public List<TerrainTile> GetTiles(RectangleF bounds)
        {
            return m_tiles.Query(bounds);
        }

        public long this[TerrainTypes type]
        {
            get
            {
                return m_tileCount[type];
            }  
        }

        public override RectangleF Bounds
        {
            get 
            { 
                return m_bounds; 
            }
        }

        public TerrainTile this[int x, int y]
        {
            get
            {
                return GetTile(x, y);
            }
        }
    }
}
