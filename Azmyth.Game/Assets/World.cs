using Azmyth.Procedural;
using System;

namespace Azmyth.Assets
{


    [Serializable]
    public class World : Asset
    {
        private PerlinNoise m_terrain;
        private PerlinNoise m_forest;
        private PerlinNoise m_rivers;
        private PerlinNoise m_stones;
        private PerlinNoise m_lava;
        private RandomNoise m_randomNoise;

        private int m_seed = 500;
        private float m_terrainHeight = 1024;
        private float m_persistance = 1.00f;
        private float m_frequency = 0.02f; // continent size.
        private int m_octaves = 2; //Roughness


        private float m_coastLine = 0.00f;
        private float m_shoreLine = 0.05f;
        private float m_treeLine = 0.40f;
        private float m_snowLine = 0.50f;
        
        private Random m_random = new Random(500);

        QuadTree<Room> m_rooms = new QuadTree<Room>(new System.Drawing.Size(16, 16), 1);

        public float CoastLine
        {
            get { return m_coastLine; }
            set { m_coastLine = value; }
        }

        public float TreeLine
        {
            get { return m_treeLine; }
            set { m_treeLine = value; }
        }

        public float SnowLine
        {
            get { return m_snowLine; }
            set { m_snowLine = value; }
        }

        public float ShoreLine
        {
            get { return m_shoreLine; }
            set { m_shoreLine = value; }
        }

        public float TerrainHeight
        {
            get 
            { 
                return m_terrainHeight; 
            }
            set 
            { 
                m_terrainHeight = value; 
                UpdateGenerators(); 
            }
        }

        public int Seed
        {
            get 
            { 
                return m_seed; 
            }
            set 
            { 
                m_seed = value; 
                UpdateGenerators(); 
            }
        }

        public World()
        {
            UpdateGenerators();
        }

        public World(VectorID worldID)
        {
            AssetID = worldID;

            m_rooms.Insert(new Room() { GridX = 0, GridY = 0, Bounds = new System.Drawing.RectangleF(0, 0, 1, 1), Height = 0, m_terrain = TerrainTypes.Black });

            m_rooms.Insert(new Room() { GridX = 1, GridY = 1, Bounds = new System.Drawing.RectangleF(1, 1, 1, 1), Height = 0, m_terrain = TerrainTypes.Black });

            m_rooms.Insert(new Room() { GridX = -1, GridY = -1, Bounds = new System.Drawing.RectangleF(-1, -1, 1, 1), Height = 0, m_terrain = TerrainTypes.Black });

            m_rooms.Insert(new Room() { GridX = 1, GridY = -1, Bounds = new System.Drawing.RectangleF(1, -1, 1, 1), Height = 0, m_terrain = TerrainTypes.Black });

            m_rooms.Insert(new Room() { GridX = -1, GridY = 1, Bounds = new System.Drawing.RectangleF(-1, 1, 1, 1), Height = 0, m_terrain = TerrainTypes.Black });


            UpdateGenerators();
        }


        public World(VectorID vectorID, int seed)
        {
            AssetID = vectorID;
            m_seed = seed;

            UpdateGenerators();
        }

        public Room GetRoom(int x, int y)
        {
            Room room = new Room();

            room.GridX = x;
            room.GridY = y;

            room.Height = (float)Math.Round(m_terrain.GetHeight(x, y));
            room.m_value = (float)m_terrain.GetValue(x, y);

            if (room.Height <= m_coastLine)
                room.m_terrain = TerrainTypes.Ocean;

            if (room.Height > m_coastLine && room.Height <= (m_coastLine + ((m_terrainHeight - m_coastLine) * m_shoreLine)))
                room.m_terrain = TerrainTypes.Sand;

            if (room.Height > m_coastLine + ((m_terrainHeight - m_coastLine) * m_shoreLine))
                room.m_terrain = TerrainTypes.Dirt;

            if (room.Height > m_coastLine + ((m_terrainHeight - m_coastLine) * m_treeLine))
                room.m_terrain = TerrainTypes.Mountain;

            if (room.Height > m_coastLine + ((m_terrainHeight - m_coastLine) * m_snowLine))
                room.m_terrain = TerrainTypes.Snow;

            if (room.Height > m_coastLine)
            {
                if (Math.Abs(m_stones.GetHeight(x, y)) >= .95f)
                    room.m_terrain = TerrainTypes.Stone;

                if (Math.Floor(m_randomNoise.GetValue(x, y) * 100) < 1)
                    room.m_terrain = TerrainTypes.Stone;
            }

            if (room.Height > m_coastLine + ((m_terrainHeight - m_coastLine) * m_treeLine))
            {
                if (Math.Abs(m_lava.GetHeight(x, y)) >= .99f)
                {
                    room.m_terrain = TerrainTypes.Lava;
                }
            }

            if (room.m_terrain != TerrainTypes.Ocean)
            {
                if (room.m_terrain != TerrainTypes.Snow)
                {
                    if (Math.Abs(m_rivers.GetHeight(x, y)) < m_terrainHeight * .06)
                    {
                        room.m_terrain = TerrainTypes.Sand;
                    }
                    if (Math.Abs(m_rivers.GetHeight(x, y)) < m_terrainHeight * .03)
                    {
                        room.m_terrain = TerrainTypes.River;
                    }
                }
                else
                {
                    if (Math.Abs(m_rivers.GetHeight(x, y)) < m_terrainHeight * .03)
                    {
                        room.m_terrain = TerrainTypes.Ice;
                    }
                }

            }

            foreach (Room r in m_rooms.Query(new System.Drawing.RectangleF(x, y, 1, 1)))
            {
                room = r;
            }

            return room;
        }

        public override void AddAsset(Asset asset)
        {
            base.AddAsset(asset);
        }

        public override void RemoveAsset(Asset asset)
        {
            base.RemoveAsset(asset);
        }

        private void UpdateGenerators()
        {
            m_terrain = new PerlinNoise(m_persistance, m_frequency, m_terrainHeight, m_octaves, m_seed);
            m_forest = new PerlinNoise(m_persistance, m_frequency, m_terrainHeight, m_octaves, m_seed);
            m_rivers = new PerlinNoise(m_persistance, .02, m_terrainHeight, 2, (m_seed / 5) * 3);
            m_stones = new PerlinNoise(m_persistance, .02, 1, 5, (m_seed / 3) * 7);
            m_lava = new PerlinNoise(m_persistance, m_frequency, 1, 5, (m_seed / 5) * 3);
            m_random = new Random(m_seed);
            m_randomNoise = new RandomNoise(m_seed);
        }
    }
}
