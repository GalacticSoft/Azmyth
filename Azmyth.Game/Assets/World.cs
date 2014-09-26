using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Azmyth.Procedural;

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
        private RandomNoise m_randomNoise;
        private RandomNoise m_city;

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

        QuadTree<Room> m_rooms = new QuadTree<Room>(new System.Drawing.Size(64, 64), 64);
        QuadTree<City> m_cities = new QuadTree<City>(new System.Drawing.Size(64, 64), 64);

        private MarkovNameGenerator nameGenerator;

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

        public Room GetRoom(int x, int y)
        {
            Room room = null;
            List<Room> rooms = m_rooms.Query(new System.Drawing.RectangleF(x, y, 1, 1));

            if(rooms.Count > 0)
            {
                room = rooms[0];
            }
            else
            {
                room = new Room();

                room.GridX = x;
                room.GridY = y;

                room.Height = (float)Math.Round(m_terrain.GetHeight(x, y));
                room.m_value = (float)m_terrain.GetValue(x, y);

                if (room.Height <= m_coastLine)
                {
                    room.Terrain = TerrainTypes.Ocean;
                }

                if (room.Height > m_coastLine && room.Height <= (m_coastLine + ((m_terrainHeight - m_coastLine) * m_shoreLine)))
                {
                    room.Terrain = TerrainTypes.Sand;
                }

                if (room.Height > m_coastLine + ((m_terrainHeight - m_coastLine) * m_shoreLine))
                {
                    room.Terrain = TerrainTypes.Dirt;
                }

                if (room.Height > m_coastLine + ((m_terrainHeight - m_coastLine) * m_treeLine))
                {
                    room.Terrain = TerrainTypes.Mountain;
                }

                if (room.Height > m_coastLine)
                {
                    if (Math.Abs(m_stones.GetHeight(x, y)) >= .95f)
                        room.Terrain = TerrainTypes.Stone;

                    if (Math.Floor(m_randomNoise.GetValue(x, y) * 100) < 1)
                        room.Terrain = TerrainTypes.Stone;
                }

                if (room.Height > m_coastLine + ((m_terrainHeight - m_coastLine) * m_snowLine))
                {
                    room.Terrain = TerrainTypes.Snow;
                }

                if (room.Height > m_coastLine + ((m_terrainHeight - m_coastLine) * m_treeLine))
                {
                    if (Math.Abs(m_lava.GetHeight(x, y)) >= .99f)
                    {
                        room.Terrain = TerrainTypes.Lava;
                    }
                }

                if (room.Terrain != TerrainTypes.Ocean)
                {
                    if (room.Terrain != TerrainTypes.Snow)
                    {
                        if (Math.Abs(m_rivers.GetHeight(x, y)) < m_terrainHeight * .06)
                        {
                            room.Terrain = TerrainTypes.Sand;
                        }
                        if (Math.Abs(m_rivers.GetHeight(x, y)) < m_terrainHeight * .03)
                        {
                            room.Terrain = TerrainTypes.River;
                        }
                    }
                    else
                    {
                        if (Math.Abs(m_rivers.GetHeight(x, y)) < m_terrainHeight * .03)
                        {
                            room.Terrain = TerrainTypes.Ice;
                        }
                    }
                }

                if (room.Terrain != TerrainTypes.Ocean && room.Terrain != TerrainTypes.River && room.Terrain != TerrainTypes.Lava)
                {
                    if (m_city.GetValue(x, y) * 100 <= .02)
                    {
                        
                        CreateCity(room);
                    }
                }
            }

            room.m_temp = Math.Abs((int)Math.Round(m_tempurature.GetHeight(x, y), 0));
            room.m_tempVal = m_tempurature.GetValue(x, y);
           
            return room;
        }

        public void CreateCity(Room room)
        {
           /// Task.Factory.StartNew(target =>
            //{

                City city = new City();
                city.GridX = room.GridX;
                city.GridY = room.GridY;
                city.Bounds = new System.Drawing.RectangleF(room.GridX, room.GridY, 1, 1);

                if (room.Terrain == TerrainTypes.Sand)
                    city.CitySize = CitySize.Port;
                else
                    city.CitySize = (CitySize)m_random.Next(1, (int)CitySize.Kingdom + 1);

                city.Name = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(nameGenerator.Next());
               
                room.Name = "The " + city.CitySize + " of " + city.Name;
                room.Terrain = TerrainTypes.City;
                room.Bounds = new System.Drawing.RectangleF(room.GridX, room.GridY, 1, 1);

                m_rooms.Insert(room);
                m_cities.Insert(city);
           // }, System.Threading.CancellationToken.None);
        }

        public City GetCity(int x, int y)
        {
            City city = null;
            List<City> cities = m_cities.Query(new System.Drawing.RectangleF(x, y, 1, 1));

            if (cities.Count > 0)
            {
                city = cities[0];
            }

            return city;
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
            m_random = new Random(m_seed);
            m_randomNoise = new RandomNoise(m_seed);
            m_city = new RandomNoise((m_seed / 3) * 5);

            m_terrain = new PerlinNoise(m_persistance, m_frequency, m_terrainHeight, m_octaves, m_seed);
            m_forest = new PerlinNoise(m_persistance, m_frequency, m_terrainHeight, m_octaves, m_seed);
            m_rivers = new PerlinNoise(m_persistance, .02, m_terrainHeight, 2, (m_seed / 5) * 3);
            m_stones = new PerlinNoise(m_persistance, .02, 1, 5, (m_seed / 3) * 7);
            m_lava = new PerlinNoise(m_persistance, m_frequency, 1, 5, (m_seed / 5) * 3);

            m_tempurature = new PerlinNoise(m_persistance, m_frequency, 40, m_octaves, (m_seed / 9) * 5);
        }
    }
}
