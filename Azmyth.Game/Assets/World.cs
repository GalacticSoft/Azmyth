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
        #region Properties

        // Noise Interface for the terrain.
        private INoise m_terrain;

        // The noise generation algorithm to use for base terrain.
        private NoiseTypes m_terrainNoise = NoiseTypes.Perlin;

        // The noise generation algorithm to use for rivers.
        // Noise Interface for the terrain.
        private INoise m_rivers;

        // The noise generation algorithm to use for rivers.
        private NoiseTypes m_riverNoise = NoiseTypes.Perlin;

        private INoise m_tempurature;

        // Random Seed
        private long m_seed = new Random((int)DateTime.Now.Ticks).Next(1000, 9999);

        // Terrain Height is the approximate maximum value the terrain can be.
        private float m_terrainHeight = 1000;

        // Frequency controls the size of Continents by 
        // increasing or decreasing the wavelength of the noise.
        private float m_continentSize = 0.02f;

        // Octaves Increase Roughness by adding noise values together
        private int m_octaves = 2; 

        private float m_persistance = 1.00f;

        // Coast starts at height 0
        private float m_coastLine = 0.00f;

        // Shore line is 5% higher than coast line
        private float m_shoreLine = 0.05f;

        // Tree line is 40% higher than coast line
        private float m_treeLine = 0.40f;

        // Snow line is 50% higher than coast line
        private float m_snowLine = 0.50f;
        
        private Random m_random = new Random(92465468);

        private QuadTree<TerrainChunk> m_terrainChunks = new QuadTree<TerrainChunk>(new System.Drawing.Size(100, 100), 100);

        private MarkovNameGenerator nameGenerator;

        #endregion

        #region Methods

        public float ContinentSize
        {
            get 
            { 
                return m_continentSize;  
            }
            set 
            { 
                m_continentSize = value; 
                UpdateNoise(); 
            }
        }

        public float CoastLine
        {
            get 
            { 
                return m_coastLine; 
            }
            set 
            { 
                m_coastLine = value; 
            }
        }

        public float TreeLine
        {
            get 
            { 
                return m_treeLine; 
            }
            set 
            { 
                m_treeLine = value; 
            }
        }

        public float SnowLine
        {
            get 
            { 
                return m_snowLine; 
            }
            set 
            { 
                m_snowLine = value; 
            }
        }

        public float ShoreLine
        {
            get 
            { 
                return m_shoreLine; 
            }
            set 
            { 
                m_shoreLine = value; 
            }
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
                UpdateNoise(); 
            }
        }

        public long Seed
        {
            get 
            { 
                return m_seed; 
            }
            set 
            { 
                m_seed = value; 
                UpdateNoise(); 
            }
        }

        public NoiseTypes TerrainNoise
        {
            get 
            { 
                return m_terrainNoise; 
            }
            set 
            { 
                m_terrainNoise = value;
                UpdateNoise();
            }
        }

        public NoiseTypes RiverNoise
        {
            get 
            { 
                return m_riverNoise; 
            }
            set 
            { 
                m_riverNoise = value;
                UpdateNoise();
            }
        }

        #endregion

        #region Constructors

        public World(VectorID worldID)
            : this(worldID, new Random((int)DateTime.Now.Ticks).Next(1000, 9999)) { }

        public World(long seed)
            : this(new VectorID(0, 0), seed) { }

        public World(VectorID vectorID, long seed)
        {
            nameGenerator =
                new MarkovNameGenerator("Abrielle,Acalia,Adair,Adara,Adriel,Aiyana,Alaire,Alissa,Alixandra,Altair,Amara,Anatola,Anya,Arcadia,Ariadne,Arianwen,Aurelia,Aurelian,Aurelius,Auristela,Avalon,Bastian,Breena,Briallan,Brielle,Briseis,Cambria,Cara,Carys,Caspian,Cassia,Cassiel,Cassiopeia,Cassius,Chaniel,Cora,Corbin,Cyprian,Dagen,Daire,Darius,Destin,Devlin,Devlyn,Drake,Drystan,Eira,Eirian,Eliron,Elysia,Eoin,Evadne,Evanth,Fineas,Finian,Fyodor,Gaerwn,Gareth,Gavriel,Ginerva,Griffin,Guinevere,Hadriel,Hannelore,Hermione,Hesperos,Iagan,Ianthe,Ignacia,Ignatius,Iseult,Isolde,Jessalyn,Kara,Katriel,Kerensa,Korbin,Kyler,Kyra,Kyrielle,Leala,Leila,Leira,Lilith,Liora,Liriene,Liron,Lucien,Lyra,Maia,Marius, Mathieu,Maylea,Meira,Mireille,Mireya,Natania,Neirin,Nerys,Nuriel,Nyfain,Nyssa,Oisin,Oleisa,Oralie,Orinthea,Orion,Orpheus,Ozara,Peregrine,Persephone,Perseus,Petronela,Phelan,Pryderi,Pyralia,Pyralis,Qadira,Quinevere,Quintessa,Raisa,Remus,Renfrew,Rhyan,Rhydderch,Riona,Saira,Saoirse,Sarai,Sarielle,Sebastian,Seraphim,Seraphina,Serian,Sirius,Sorcha,Séverin,Tavish,Tearlach,Terra,Thalia,Thaniel,Theia,Torian,Torin,Tressa,Tristana,Ulyssia,Uriela,Urien,Vanora,Vasilis,Vespera,Xanthus,Xara,Xylia,Yadira,Yakira,Yeira,Yeriel,Yestin,Yseult,Zaira,Zaniel,Zarek,Zephyr,Zora,Zorion".Split(','), 2);

            AssetID = vectorID;
            m_seed = seed;

            UpdateNoise();
        }

        #endregion

        #region Tile Functions

        public TerrainTypes GetTerrainType(int x, int y)
        {
            TerrainTypes terrain = TerrainTypes.None;
            double height = Math.Round(m_terrain.GetHeight(x, y));

            if (height <= m_coastLine)
            {
                terrain = TerrainTypes.Ocean;
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

            if (terrain != TerrainTypes.Ocean)
            {
                if (Math.Abs(m_rivers.GetHeight(x, y)) < m_terrainHeight * .06)
                {
                    terrain = TerrainTypes.Dirt;
                }
                if (Math.Abs(m_rivers.GetHeight(x, y)) < m_terrainHeight * .03)
                {
                    terrain = TerrainTypes.Ocean;
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

            tile.Height =  (float)Math.Round(m_terrain.GetHeight(x, y));
            tile.m_value = (float)m_terrain.GetValue(x, y);

            tile.Bounds = new RectangleF(x, y, 1, 1);

            tile.Terrain = GetTerrainType(x, y);

            tile.m_temp = Math.Abs(Math.Round(m_tempurature.GetHeight(x, y), 0));
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

        #endregion

        #region Chunk Functions

        public TerrainChunk LoadChunk(RectangleF chunkBounds)
        {
            TerrainChunk chunk = null;
            List<TerrainChunk> chunks = m_terrainChunks.Query(chunkBounds);

            if (chunks.Count == 0)
            {
                chunk = new TerrainChunk(this, chunkBounds);

                m_terrainChunks.Insert(chunk);
            }
            else
            {
                chunk = chunks[0];
            }

            return chunk;
        }

        public void UnloadChunk(RectangleF chunkBounds)
        {
            List<TerrainChunk> chunks = m_terrainChunks.Query(chunkBounds);

            if (chunks.Count > 0)
            {
                m_terrainChunks.Remove(chunks[0]);
            }
        }

        public int GetNodeCount()
        {
            return m_terrainChunks.GetQuadNodeCount();
        }

        public int GetChunkCount()
        {
            return m_terrainChunks.GetQuadObjectCount();
        }

        public bool IsChunkLoaded(RectangleF chunkBounds)
        {
            return (m_terrainChunks.Query(chunkBounds).Count > 0);
        }

        public List<TerrainChunk> GetChunks(RectangleF chunkBounds)
        {
            return m_terrainChunks.Query(chunkBounds);
        }

        public List<TerrainChunk> GetChunks()
        {
            List<TerrainChunk> chunks = new List<TerrainChunk>();
            List<QuadTree<TerrainChunk>.QuadNode> nodes = m_terrainChunks.GetAllNodes();

            foreach(QuadTree<TerrainChunk>.QuadNode node in nodes)
            {
                if (node.Objects.Count > 0) 
                {
                    chunks.Add(node.Objects[0]);
                }
                
            }

            return chunks;
        }

        #endregion

        private void UpdateNoise()
        {
            m_random = new Random((int)m_seed);

            switch(m_terrainNoise)
            { 
                case NoiseTypes.Perlin:
                    m_terrain = new PerlinNoise(m_persistance, m_continentSize, m_terrainHeight, m_octaves, m_seed);
                    break;
                case NoiseTypes.Simplex:
                    m_terrain = new SimplexOpenNoise(m_persistance, m_continentSize, m_terrainHeight, m_octaves, m_seed);
                    break;
            }

            switch (m_riverNoise)
            {
                case NoiseTypes.Perlin:
                    m_rivers = new PerlinNoise(m_persistance, .02, m_terrainHeight, 2, new Random((int)m_seed).Next(1000, 9999));
                    break;
                case NoiseTypes.Simplex:
                    m_rivers = new SimplexOpenNoise(m_persistance, .02, m_terrainHeight, 2, new Random((int)m_seed).Next(1000, 9999));
                    break;
            }

            m_tempurature = new PerlinNoise(m_persistance, m_continentSize, 40, m_octaves, (m_seed / 9) * 5);
        }

        public void UpdateChunks(int x, int y, int chunkSize)
        {
            System.Drawing.RectangleF chunkBounds;
            List<Vector> newChunks = new List<Vector>();
            
            // Calculate Coordinates
            int chunkX = (int)Numbers.ConvertCoordinate(x, chunkSize);
            int chunkY = (int)Numbers.ConvertCoordinate(y, chunkSize);

            // Calculate Moore neighborhood.
            for(int offsetY = chunkY - 1; offsetY <= chunkY + 1; offsetY++)
            {
                for(int offsetX = chunkX - 1; offsetX <= chunkX + 1; offsetX++)
                {
                    newChunks.Add(new Vector(offsetX, offsetY, 0));
                }
            }

            // Get all the loaded Chunks.
            List<TerrainChunk> AllChunks = GetChunks();

            // Cull Chunks outside of neighbor range.
            foreach (TerrainChunk chunk in AllChunks)
            {
                if (!newChunks.Contains(new Vector((int)chunk.Bounds.X, (int)chunk.Bounds.Y, 0)))
                {
                    chunkBounds = new System.Drawing.RectangleF(chunk.Bounds.X, chunk.Bounds.Y, chunkSize, chunkSize);

                    UnloadChunk(chunkBounds);
                }
            }

            // Load Neighbor Chunks
            foreach (Vector chunk in newChunks)
            {
                chunkBounds = new System.Drawing.RectangleF(chunk.X * chunkSize, chunk.Y * chunkSize, chunkSize, chunkSize);

                LoadChunk(chunkBounds);
            }
        }
    }
}
