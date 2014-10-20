using Azmyth.Game;
using System.Collections.Generic;
using System.Drawing;

namespace Azmyth.Assets
{
    
    public class TerrainTile : Asset
    {
        private World m_world = null;
        private TerrainChunk m_chunk = null;
        private Dictionary<Vector, TerrainTile> m_mooreNeighbors = null;
        private Dictionary<Vector, TerrainTile> m_vonNeumanNeighbors = null;

        private TerrainTypes m_terrain;

        private float m_height = 0;
        public float m_value = 0;
        public double m_temp = 0;
        public double m_tempVal = 0;

        private TerrainTile() { }

        public TerrainTile(VectorID tileID) 
            : this(tileID, null) { }

        public TerrainTile(TerrainChunk chunk) 
            : this(new VectorID(0, 0), chunk) { }

        public TerrainTile(VectorID tileID, TerrainChunk chunk) 
            : base(tileID)
        {
            m_chunk = chunk;

            if (chunk != null)
            {
                m_world = m_chunk.World;
            }
        }

        public int X 
        { 
            get; 
            set; 
        }
        public int Y 
        { 
            get; 
            set; 
        }

        public float Height
        {
            get 
            { 
                return m_height; 
            }
            set 
            { 
                m_height = value; 
            }
        }

        public TerrainTypes Terrain
        {
            get 
            { 
                return m_terrain; 
            }
            set 
            { 
                m_terrain = value; 
            }
        }

        public Dictionary<Vector, TerrainTile> MooreNeighbors
        {
            get
            {
                return m_mooreNeighbors;
            }
            protected set
            {
                m_mooreNeighbors = value;
            }
        }
        public Dictionary<Vector, TerrainTile> VonNuemanNeighbors
        {
            get
            {
                return m_vonNeumanNeighbors;
            }
            protected set
            {
                m_vonNeumanNeighbors = value;
            }
        }

        public Dictionary<Vector, TerrainTile> GetMooreNeighbors()
        {
            Dictionary<Vector, TerrainTile> neighbors = new Dictionary<Vector, TerrainTile>();

            if(m_chunk != null)
            {
                for(int i = 0; i < (int)Directions.MaxCardinal; i++)
                {
                    Vector direction = Direction.GetDirectionVector((Directions)i);

                    neighbors.Add(direction, m_chunk.GetTile(X + direction.X, Y + direction.Y));
                }
            }

            return neighbors;
        }

        public Dictionary<Vector, TerrainTile> GetVonNeumanNeighbors()
        {
            Dictionary<Vector, TerrainTile> neighbors = new Dictionary<Vector, TerrainTile>();
            
            if (m_chunk != null)
            {
                for (int i = 0; i < (int)Directions.MaxCardinal; i += 2)
                {
                    Vector direction = Direction.GetDirectionVector((Directions)i);

                    neighbors.Add(direction, m_chunk.GetTile(X + direction.X, Y + direction.Y));
                }
            }

            return neighbors;
        }

        public Dictionary<Vector, TerrainTile> LoadMooreNeighbors()
        {
            MooreNeighbors = GetMooreNeighbors();

            return MooreNeighbors;
        }

        public Dictionary<Vector, TerrainTile> LoadVonNeumanNeighbors()
        {
            VonNuemanNeighbors = GetVonNeumanNeighbors();

            return VonNuemanNeighbors;
        }

        //private Exit[] _exits = new Exit[(long)Directions.Max];
        //public void AddExit(Exit exit, Directions direction)
        //{
        //    exit.Direction = direction;

        //    if(_exits[(long)direction] != null)
        //    {
        //        RemoveAsset(_exits[(long)direction]);
        //    }

        //    _exits[(long)direction] = exit;

        //    if (!_assets.ContainsKey(exit.AssetID))
        //    {
        //        AddAsset(exit);
        //    }
        //}

        //public void RemoveExit(Directions direction)
        //{
        //    if(_exits[(long)direction] != null)
        //    {
        //        RemoveAsset(_exits[(long)direction]);
        //    }

        //    _exits[(long)direction] = null;
        //}

        //public Exit this[Directions direction]
        //{
        //    get
        //    {
        //        return base[(long)EntityVector.Exit, (long)direction] as Exit;
        //    }
        //}
    }
}
