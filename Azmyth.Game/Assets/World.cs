using Azmyth.Noise;
using System;

namespace Azmyth.Assets
{
    public enum Terrain
    {
        Ocean = 0,
        Coast = 1,
        Beach = 2,
        Dirt = 3,
        Forest = 4,
        River = 5,
        Mountain = 6,
        Stone = 7,
        Snow = 8,
        Lava = 9,
    }

    public class World : Asset
    {
        private Perlin m_terrain;
        private Perlin m_forest;
        private Perlin m_rivers;
        private Perlin m_stones;
        private Perlin m_lava;

        private int m_seed = 500;
        private double m_terrainHeight = 1024;
        private double m_persistance = 1.00f;
        private double m_frequency = 0.02f; // continent size.
        private int m_octaves = 2; //Roughness


        private double m_coastLine = 0.00f;
        private double m_shoreLine = 0.05f;
        private double m_treeLine = 0.25f;
        private double m_snowLine = 0.50f;
        
        private Random m_random = new Random(500);

        public double CoastLine
        {
            get { return m_coastLine; }
            set { m_coastLine = value; }
        }

        public double TreeLine
        {
            get { return m_treeLine; }
            set { m_treeLine = value; }
        }

        public double SnowLine
        {
            get { return m_snowLine; }
            set { m_snowLine = value; }
        }

        public double ShoreLine
        {
            get { return m_shoreLine; }
            set { m_shoreLine = value; }
        }

        public double TerrainHeight
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

        public World(VectorID worldID)
        {
            AssetID = worldID;

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

            room.m_height = Math.Round(m_terrain.GetHeight(x, y));
            room.m_value = m_terrain.GetValue(x, y);

            if (room.m_height <= m_coastLine)
                room.m_terrain = Terrain.Ocean;

            if (room.m_height > m_coastLine - ((m_terrainHeight - m_coastLine)  * m_shoreLine) && room.m_height <= m_coastLine)
                room.m_terrain = Terrain.Coast;

            if (room.m_height > m_coastLine && room.m_height <= (m_coastLine + ((m_terrainHeight - m_coastLine) * m_shoreLine)))
                room.m_terrain = Terrain.Beach;

            if (room.m_height > m_coastLine + ((m_terrainHeight - m_coastLine) * m_shoreLine))
                room.m_terrain = Terrain.Dirt;

            if (room.m_height > m_coastLine + ((m_terrainHeight - m_coastLine) * m_treeLine))
                room.m_terrain = Terrain.Mountain;

            if (room.m_height > m_coastLine + ((m_terrainHeight - m_coastLine) * m_snowLine))
                room.m_terrain = Terrain.Snow;

            if (room.m_height > m_coastLine)
            {
                if (Math.Abs(m_stones.GetHeight(x, y)) > .95f)
                {
                    room.m_terrain = Terrain.Stone;
                }
            }

            if (room.m_height > m_coastLine + ((m_terrainHeight - m_coastLine) * m_treeLine))
            {
                if (Math.Abs(m_stones.GetHeight(x, y)) >= .80f)
                {
                    room.m_terrain = Terrain.Lava;
                }
            }

            return room;
        }

        public override void AddObject(Asset obj)
        {
            base.AddObject(obj);
        }

        public override void RemoveObject(Asset obj)
        {
            base.RemoveObject(obj);
        }

        private void UpdateGenerators()
        {
            m_terrain = new Perlin(m_persistance, m_frequency, m_terrainHeight, m_octaves, m_seed);
            m_forest = new Perlin(m_persistance, m_frequency, m_terrainHeight, m_octaves, m_seed);
            m_rivers = new Perlin(m_persistance, m_frequency, m_terrainHeight, m_octaves, m_seed);
            m_stones = new Perlin(m_persistance, .07, 1, 4, m_seed);
            m_lava = new Perlin(m_persistance, m_frequency, 1, 1, (m_seed / 2) * 3);
            m_random = new Random(m_seed);
        }
    }
}
