using Azmyth.Game;

namespace Azmyth.Assets
{
    public class Room : Asset
    {

        private TerrainTypes m_terrain;
        private float m_height;
        public float m_value;
        public double m_temp;
        public double m_tempVal;
        public bool HasRock { get; set; }
        public int GridX { get; set; }
        public int GridY { get; set; }

        private Exit[] _exits = new Exit[(long)Directions.Max];

        public float Height
        {
            get { return m_height; }
            set { m_height = value;  }
        }

        public TerrainTypes Terrain
        {
            get { return m_terrain; }
            set { m_terrain = value; }
        }

        public Room()
        {

        }

        public Room(VectorID roomID)
        {
            HasRock = false;
            AssetID = roomID;
        }

        public void AddExit(Exit exit, Directions direction)
        {
            exit.Direction = direction;

            if(_exits[(long)direction] != null)
            {
                RemoveAsset(_exits[(long)direction]);
            }

            _exits[(long)direction] = exit;

            if (!_assets.ContainsKey(exit.AssetID))
            {
                AddAsset(exit);
            }
        }

        public void RemoveExit(Directions direction)
        {
            if(_exits[(long)direction] != null)
            {
                RemoveAsset(_exits[(long)direction]);
            }

            _exits[(long)direction] = null;
        }

        public Exit this[Directions direction]
        {
            get
            {
                return base[(long)EntityVector.Exit, (long)direction] as Exit;
            }
        }
    }
}
