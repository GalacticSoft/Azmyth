using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Azmyth.Procedural;
using System.Drawing;

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
        private PerlinNoise m_tempurature;
        private RandomNoise m_rocks;
        private RandomNoise m_randomForest;
        private RandomNoise m_city;

        private int m_seed = 500;
        private float m_terrainHeight = 1000;
        private float m_persistance = 1.00f;
        private float m_frequency = 0.02f; // continent size.
        private int m_octaves = 2; //Roughness

        private int m_chunkSize = 32;

        private float m_coastLine = 0.00f;
        private float m_shoreLine = 0.05f;
        private float m_treeLine = 0.40f;
        private float m_snowLine = 0.50f;
        
        private Random m_random = new Random(500);

        private RectangleF m_spawnArea = new RectangleF(0, 0, 1, 1);

        //QuadTree<TerrainTile> m_rooms = new QuadTree<TerrainTile>(new System.Drawing.Size(64, 64), 64);
        //QuadTree<City> m_cities = new QuadTree<City>(new System.Drawing.Size(64, 64), 64);

        private QuadTree<TerrainChunk> m_terrainChunks = new QuadTree<TerrainChunk>(new System.Drawing.Size(100, 100), 100);

        private MarkovNameGenerator nameGenerator;

        public float ContinentSize
        {
            get { return m_frequency;  }
            set { m_frequency = value; UpdateGenerators(); }
        }

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
            nameGenerator = new MarkovNameGenerator("Abrielle,Acalia,Adair,Adara,Adriel,Aiyana,Alaire,Alissa,Alixandra,Altair,Amara,Anatola,Anya,Arcadia,Ariadne,Arianwen,Aurelia,Aurelian,Aurelius,Auristela,Avalon,Bastian,Breena,Briallan,Brielle,Briseis,Cambria,Cara,Carys,Caspian,Cassia,Cassiel,Cassiopeia,Cassius,Chaniel,Cora,Corbin,Cyprian,Dagen,Daire,Darius,Destin,Devlin,Devlyn,Drake,Drystan,Eira,Eirian,Eliron,Elysia,Eoin,Evadne,Evanth,Fineas,Finian,Fyodor,Gaerwn,Gareth,Gavriel,Ginerva,Griffin,Guinevere,Hadriel,Hannelore,Hermione,Hesperos,Iagan,Ianthe,Ignacia,Ignatius,Iseult,Isolde,Jessalyn,Kara,Katriel,Kerensa,Korbin,Kyler,Kyra,Kyrielle,Leala,Leila,Leira,Lilith,Liora,Liriene,Liron,Lucien,Lyra,Maia,Marius, Mathieu,Maylea,Meira,Mireille,Mireya,Natania,Neirin,Nerys,Nuriel,Nyfain,Nyssa,Oisin,Oleisa,Oralie,Orinthea,Orion,Orpheus,Ozara,Peregrine,Persephone,Perseus,Petronela,Phelan,Pryderi,Pyralia,Pyralis,Qadira,Quinevere,Quintessa,Raisa,Remus,Renfrew,Rhyan,Rhydderch,Riona,Saira,Saoirse,Sarai,Sarielle,Sebastian,Seraphim,Seraphina,Serian,Sirius,Sorcha,Séverin,Tavish,Tearlach,Terra,Thalia,Thaniel,Theia,Torian,Torin,Tressa,Tristana,Ulyssia,Uriela,Urien,Vanora,Vasilis,Vespera,Xanthus,Xara,Xylia,Yadira,Yakira,Yeira,Yeriel,Yestin,Yseult,Zaira,Zaniel,Zarek,Zephyr,Zora,Zorion".Split(','), 2);
        
            UpdateGenerators();
        }

        public World(VectorID worldID) : this()
        {
            AssetID = worldID;
        }

        public World(VectorID vectorID, int seed)
        { 
            AssetID = vectorID;
            m_seed = seed;

            UpdateGenerators();
        }

        public TerrainTypes GetTerrainType(int x, int y)
        {
            //SimplexValueSplineNoise spline = new SimplexValueSplineNoise(m_seed);
            //OpenSimplexNoise spline = new OpenSimplexNoise(m_seed);
            TerrainTypes terrain = TerrainTypes.Black;
            double height = Math.Round(m_terrain.GetHeight(x, y));//spline.eval((double)x / 25, (double)y / 25);// ;
            
            if (height < 0)
            {
                terrain = TerrainTypes.Water;
            }


            if (height > m_coastLine && height <= (m_coastLine + ((m_terrainHeight - m_coastLine) * m_shoreLine)))
            {
                terrain = TerrainTypes.Dirt;
            }

            if (height > m_coastLine + ((m_terrainHeight - m_coastLine) * m_shoreLine))
            {
                terrain = TerrainTypes.Grass;
            }

            if (height > m_coastLine + ((m_terrainHeight - m_coastLine) * m_treeLine))
            {
                terrain = TerrainTypes.Stone;
            }

            if (terrain != TerrainTypes.Water)
            {
                if (Math.Abs(m_rivers.GetHeight(x, y)) < m_terrainHeight * .06)
                {
                    terrain = TerrainTypes.Dirt;
                }
                if (Math.Abs(m_rivers.GetHeight(x, y)) < m_terrainHeight * .03)
                {
                    terrain = TerrainTypes.Water;
                }
            }

            return terrain;
        }

        public TerrainTile LoadTile(int x, int y, TerrainChunk chunk = null)
        {
            TerrainTile tile = null;

            tile = new TerrainTile(chunk);

            tile.X = x;
            tile.Y = y;

            tile.Height = (float)Math.Round(m_terrain.GetHeight(x, y));
            tile.m_value = (float)m_terrain.GetValue(x, y);

            if (tile.Height <= m_coastLine)
            {
                tile.Terrain = TerrainTypes.Water;
            }

            if (tile.Height > m_coastLine && tile.Height <= (m_coastLine + ((m_terrainHeight - m_coastLine) * m_shoreLine)))
            {
                tile.Terrain = TerrainTypes.Dirt;
            }

            if (tile.Height > m_coastLine + ((m_terrainHeight - m_coastLine) * m_shoreLine))
            {
                tile.Terrain = TerrainTypes.Grass;
            }

            if (tile.Height > m_coastLine + ((m_terrainHeight - m_coastLine) * m_treeLine))
            {
                tile.Terrain = TerrainTypes.Stone;
            }

            //if (room.Height > m_coastLine + ((m_terrainHeight - m_coastLine) * m_snowLine))
            //{
            //    room.Terrain = TerrainTypes.Snow;
            //}

            ///if (room.Height < m_coastLine + ((m_terrainHeight - m_coastLine) * m_treeLine) && room.Height > m_coastLine && room.Terrain != TerrainTypes.Sand && room.Terrain != TerrainTypes.Stone)
            //{
            //     if (m_forest.GetHeight(x, y) > 0)
            //     {
            //          room.Terrain = TerrainTypes.Forest;
            //      }
            //  }

            if (tile.Height > m_coastLine)
            {
                //if (Math.Abs(m_stones.GetHeight(x, y)) >= .95f)
                //    room.Terrain = TerrainTypes.Mountain;

                if (Math.Floor(m_rocks.GetValue(x, y) * 100) < 1)
                {
                    //tile.HasRock = true;
                }
            }
 
            //if (room.Height > m_coastLine + ((m_terrainHeight - m_coastLine) * m_treeLine))
            //{
            //    if (Math.Abs(m_lava.GetHeight(x, y)) >= .99f)
            //    {
            //        room.Terrain = TerrainTypes.Lava;
            //    }
            //}

            if (tile.Terrain != TerrainTypes.Water)
            {
                //if (room.Terrain != TerrainTypes.Snow)
                //{
                    if (Math.Abs(m_rivers.GetHeight(x, y)) < m_terrainHeight * .06)
                    {
                        tile.Terrain = TerrainTypes.Dirt;
                    }
                    if (Math.Abs(m_rivers.GetHeight(x, y)) < m_terrainHeight * .03)
                    {
                        tile.Terrain = TerrainTypes.Water;
                    }
                //}
                //else
                //{
                //    if (Math.Abs(m_rivers.GetHeight(x, y)) < m_terrainHeight * .03)
                //    {
                //        room.Terrain = TerrainTypes.Ice;
                //    }
                //}
            }

            //if (tile.Terrain != TerrainTypes.Water && tile.Terrain != TerrainTypes.Water && tile.Terrain != TerrainTypes.Lava)
            //{
            //    if (m_city.GetValue(x, y) * 100 <= .02)
            //    {
            //        //CreateCity(room);
            //    }
            //}

            tile.m_temp = Math.Abs((int)Math.Round(m_tempurature.GetHeight(x, y), 0));
            tile.m_tempVal = m_tempurature.GetValue(x, y);

            return tile;
        }

        public TerrainTile GetTile(int x, int y)
        {
            TerrainTile tile = null;
            TerrainChunk chunk = null;
            RectangleF tileRect = new RectangleF(x, y, 1, 1);
            List<TerrainChunk> chunks = m_terrainChunks.Query(tileRect);

            if(chunks.Count > 0)
            {
                chunk = chunks[0];

                tile = chunk.GetTiles(tileRect)[0];
            }
            else
            {
                tile = LoadTile(x, y);
            }

            return tile;
        }

        //public void CreateCity(TerrainTile room)
        //{
        //   /// Task.Factory.StartNew(target =>
        //    //{

        //        City city = new City();
        //        city.GridX = room.GridX;
        //        city.GridY = room.GridY;
        //        city.Bounds = new System.Drawing.RectangleF(room.GridX, room.GridY, 1, 1);

        //        if (room.Terrain == TerrainTypes.Sand)
        //            city.CitySize = CitySize.Port;
        //        else
        //            city.CitySize = (CitySize)m_random.Next(1, (int)CitySize.Kingdom + 1);

        //        city.Name = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(nameGenerator.Next());
               
        //        room.Name = "The " + city.CitySize + " of " + city.Name;
        //        room.Terrain = TerrainTypes.City;
        //        room.Bounds = new System.Drawing.RectangleF(room.GridX, room.GridY, 1, 1);

        //        //m_rooms.Insert(room);
        //        //m_cities.Insert(city);
        //   // }, System.Threading.CancellationToken.None);
        //}

        public TerrainChunk LoadChunk(RectangleF chunkBounds)
        {
            TerrainChunk chunk = null;
            List<TerrainChunk> chunks = m_terrainChunks.Query(chunkBounds);

            if (chunks.Count == 0)
            {
                chunk = new TerrainChunk(chunkBounds, this);

                m_terrainChunks.Insert(chunk);
            }
            else
            {
                chunk = chunks[0];
            }

            return chunk;
        }

        public List<TerrainChunk> GetChunks(RectangleF bounds)
        {
            return m_terrainChunks.Query(bounds);
        }

        public void CreateSpawn()
        {

        }

        public long TypeCount(TerrainTypes type, RectangleF bounds)
        {
            return 0;
        }

        private void UpdateGenerators()
        {
            m_random = new Random(m_seed);
            m_rocks = new RandomNoise(m_seed);
            m_randomForest = new RandomNoise(m_seed >> 2);
            m_city = new RandomNoise((m_seed / 3) * 5);

            m_terrain = new PerlinNoise(m_persistance, m_frequency, m_terrainHeight, m_octaves, m_seed);
            m_forest = new PerlinNoise(m_persistance, m_frequency, m_terrainHeight, 4, m_seed >> 2);
            m_rivers = new PerlinNoise(m_persistance, .02, m_terrainHeight, 2, (m_seed / 5) * 3);
            m_stones = new PerlinNoise(m_persistance, .02, 1, 5, (m_seed / 3) * 7);
            m_lava = new PerlinNoise(m_persistance, m_frequency, 1, 5, (m_seed / 5) * 3);

            m_tempurature = new PerlinNoise(m_persistance, m_frequency, 40, m_octaves, (m_seed / 9) * 5);
        }
    }
}
