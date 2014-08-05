using Azmyth.Game;

namespace Azmyth.Assets
{
    public class Exit : Asset
    {
        public VectorID ToRoom { get; set; }
        public Directions Direction { get; set; }

        public bool IsOpen { get; set; }
        public bool IsLocked { get; set; }
        public bool IsBreakable { get; set; }

        public Exit(VectorID exitID) 
        {
            AssetID = exitID;

            //Pulse[] stats = new Pulse[]
            //{
            //    new Pulse() { Vector=new VectorID(StatVector.Point, 1), Abbreviation="VIS",  Name="Visibility" },
            //    new Pulse() { Vector=new VectorID(StatVector.Point, 2), Abbreviation="LOCK", Name="Lock Level" },
           // };

            //foreach (IStat stat in stats)
           // {
            //    AddStat(stat);
            //}
        }

        private Exit()
        {    
           
        }
    }
}
