using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azmyth.Assets;
using Azmyth;
using System.Drawing;

namespace Azmyth.Assets
{
    public class TerrainChunk : Asset, IDisposable
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

        /// <summary>
        /// Loads tiles from world that are contained within chunkBounds
        /// </summary>
        /// <param name="world"></param>
        /// <param name="chunkBounds"></param>
        public TerrainChunk(World world, RectangleF chunkBounds) : this()
        {
            //Calculate total tiles
            int totalTiles = (int)chunkBounds.Width * (int)chunkBounds.Height;

            //Loop through each tile
            for (int index = 0; index < totalTiles; index++)
            {
                //Convert index value into x and y coordinates.
                int cellX = (int)(index / chunkBounds.Height) + (int)chunkBounds.X;
                int cellY = (int)(index % chunkBounds.Height) + (int)chunkBounds.Y;

                //Load tile.
                TerrainTile tile = world.LoadTile(cellX, cellY, this);

                m_tileCount[tile.Terrain]++;

                //Insert new tile into chunk QuadTree
                m_tiles.Insert(tile);
            }

            //Assign local variables
            m_world = world;
            m_bounds = chunkBounds;
        }

        /// <summary>
        /// Loads tiles from world that are contained within chunkBounds
        /// </summary>
        /// <param name="world"></param>
        /// <param name="chunkBounds"></param>
        public TerrainChunk(World world, int offsetX, int offsetY, int radius)
            : this()
        {
            
            int x, y;

            for (y = -radius; y <= radius; y++)
                for (x = -radius; x <= radius; x++)
                    if ((x * x) + (y * y) <= (radius * radius))
                    {
                        //chunkBounds = new System.Drawing.RectangleF(x, y, 1, 1);

                        TerrainTile tile = world.LoadTile(x + offsetX, y + offsetY, this);
                           
                       m_tileCount[tile.Terrain]++;

                       //Insert new tile into chunk QuadTree
                       m_tiles.Insert(tile);
                    }
           
            //Assign local variables
            m_world = world;
            m_bounds = new RectangleF(offsetX, offsetY, radius * 2, radius * 2);
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

        public void Dispose()
        {
            List<QuadTree<TerrainTile>.QuadNode> nodes = m_tiles.GetAllNodes();

            foreach (QuadTree<TerrainTile>.QuadNode node in nodes)
            {
                if (node.Objects.Count > 0)
                {
                    m_tiles.Remove(node.Objects[0]);
                }
            }

            m_tiles = null;
        }
    }
}
