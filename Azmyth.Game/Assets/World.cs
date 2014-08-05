using Azmyth.Assets;

namespace Azmyth.Assets
{

    public class World : Asset
    {
        public World(VectorID vectorID)
        {
            AssetID = vectorID;

			//AddStat(new Pulse() { Vector=new VectorID(StatVector.Counter, 1), Abbreviation="AC", Name="Area Count"       });
			//AddStat(new Pulse() { Vector=new VectorID(StatVector.Counter, 2), Abbreviation="RC", Name="Room Count"       });
			//AddStat(new Pulse() { Vector=new VectorID(StatVector.Counter, 3), Abbreviation="EC", Name="Exit Count"       });

			//AddStat(new Pulse() { Vector=new VectorID(StatVector.Pulse, 1), Abbreviation="WP", Name="Weather Pulse"      });

        }

        public override void AddObject(Asset obj)
        {
            if (obj is Area)
            {
               // GetStat(StatVector.Counter, "Area Count").Modifier++;
            }

            if (obj is Room)
            {
                //GetStat(StatVector.Counter, "Room Count").Modifier++;
            }

            base.AddObject(obj);
        }

        public override void RemoveObject(Asset obj)
        {
            if (obj is Area)
            {
                //GetStat(StatVector.Counter, "Area Count").Modifier--;
            }

            if (obj is Room)
            {
                //GetStat(StatVector.Counter, "Room Count").Modifier--;
            }

            base.RemoveObject(obj);
        }
       
    }
}
