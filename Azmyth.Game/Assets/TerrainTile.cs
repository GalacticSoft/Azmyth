using Azmyth.Game;
using System.Collections.Generic;
using System.Drawing;

namespace Azmyth.Assets
{
    public class TerrainTile : Asset
    {
        private World m_world = null;
        private TerrainChunk m_chunk = null;
        private Dictionary<Directions, TerrainTile> m_mooreNeighbors = null;
        private Dictionary<Directions, TerrainTile> m_vonNeumanNeighbors = null;

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

        public Dictionary<Directions, TerrainTile> MooreNeighbors
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

        public Dictionary<Directions, TerrainTile> VonNuemanNeighbors
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

        public TerrainTile GetNeighbor(Directions direction)
        {
            TerrainTile neighbor = null;

            if (direction < Directions.MaxCardinal)
            {
                if (MooreNeighbors != null)
                {
                    neighbor = MooreNeighbors[direction];
                }
                else 
                {
                    neighbor = LoadNeighbor(direction);
                }
            }

            return neighbor;
        }

        public TerrainTile LoadNeighbor(Directions direction)
        {
            TerrainTile neighbor = null;

            if(m_chunk != null)
            {
                Vector directionVector = Direction.GetDirectionVector(direction);

                neighbor = m_chunk.GetTile(X + directionVector.X, Y + directionVector.Y);
            }

            return neighbor;
        }

        public Dictionary<Directions, TerrainTile> LoadMooreNeighbors()
        {
            Dictionary<Directions, TerrainTile> neighbors = new Dictionary<Directions, TerrainTile>();

            if(m_chunk != null)
            {
                for(Directions direction = Directions.North; direction < Directions.MaxCardinal; direction++)
                {
                    neighbors.Add(direction, LoadNeighbor(direction));
                }
            }

            return neighbors;
        }

        public Dictionary<Directions, TerrainTile> LoadVonNeumanNeighbors()
        {
            Dictionary<Directions, TerrainTile> neighbors = new Dictionary<Directions, TerrainTile>();
            
            if (m_chunk != null)
            {
                for (Directions direction = Directions.North; direction < Directions.MaxCardinal; direction++)
                {
                    neighbors.Add(direction, LoadNeighbor(direction));
                }
            }

            return neighbors;
        }


        public void UpdateNeighbors()
        {
            UpdateMooreNeighbors();

            m_vonNeumanNeighbors = new Dictionary<Directions, TerrainTile>();

            m_vonNeumanNeighbors.Add(Directions.North, MooreNeighbors[Directions.North]);
            m_vonNeumanNeighbors.Add(Directions.East, MooreNeighbors[Directions.East]);
            m_vonNeumanNeighbors.Add(Directions.South, MooreNeighbors[Directions.South]);
            m_vonNeumanNeighbors.Add(Directions.West, MooreNeighbors[Directions.West]);
        }

        public void UpdateMooreNeighbors()
        {
            MooreNeighbors = LoadMooreNeighbors();
        }

        public void UpdateVonNeumanNeighbors()
        {
            VonNuemanNeighbors = LoadVonNeumanNeighbors();
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
