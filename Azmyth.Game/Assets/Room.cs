using Azmyth.Game;

namespace Azmyth.Assets
{
    public class Room : Asset
    {
        public long GridX { get; set; }
        public long GridY { get; set; }

        private Exit[] _exits = new Exit[(long)Directions.Max];

        public Room()
        {

        }

        public Room(VectorID roomID)
        {
            AssetID = roomID;
        }

        public void AddExit(Exit exit, Directions direction)
        {
            exit.Direction = direction;

            if(_exits[(long)direction] != null)
            {
                RemoveObject(_exits[(long)direction]);
            }

            _exits[(long)direction] = exit;

            if (!_assets.ContainsKey(exit.AssetID))
            {
                AddObject(exit);
            }
        }

        public void RemoveExit(Directions direction)
        {
            if(_exits[(long)direction] != null)
            {
                RemoveObject(_exits[(long)direction]);
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
