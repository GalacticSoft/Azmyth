using Azmyth.Game;
using System.Collections.Generic;
using System.Drawing;

namespace Azmyth.Assets
{
    public class TerrainTile : Asset
    {
        private World m_world = null;
        private TerrainChunk m_chunk = null;
        private Dictionary<Directions, TerrainTile> m_neighbors = null;

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

        public Dictionary<Directions, TerrainTile> Neighbors
        {
            get
            {
                return m_neighbors;
            }
            protected set
            {
                m_neighbors = value;
            }
        }

        public Dictionary<Directions, TerrainTile> VonNuemanNeighbors
        {
            get
            {
                Dictionary<Directions, TerrainTile> neighbors = new Dictionary<Directions, TerrainTile>();

                for (Directions direction = Directions.North; direction < Directions.MaxCardinal; direction += 2)
                {
                    if(m_neighbors.ContainsKey(direction))
                    {
                        neighbors.Add(direction, m_neighbors[direction]);
                    }
                }

                return neighbors;
            }
        }

        public TerrainTile GetNeighbor(Directions direction)
        {
            TerrainTile neighbor = null;

            if (Neighbors.ContainsKey(direction))
            {
                neighbor = Neighbors[direction];
            }
            else 
            {
                neighbor = LoadNeighbor(direction);
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

        public void UpdateMooreNeighbors()
        {
            if (m_chunk != null)
            {
                for (Directions direction = Directions.North; direction < Directions.MaxCardinal; direction++)
                {
                    if (m_neighbors.ContainsKey(direction))
                        m_neighbors[direction] = LoadNeighbor(direction);
                    else
                        m_neighbors.Add(direction, LoadNeighbor(direction));
                }
            }
        }

        public void UpdateVonNeumanNeighbors()
        {
            for (Directions direction = Directions.North; direction < Directions.MaxCardinal; direction += 2)
            {
                if (m_neighbors.ContainsKey(direction))
                    m_neighbors[direction] = LoadNeighbor(direction);
                else
                    m_neighbors.Add(direction, LoadNeighbor(direction));
            }
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
