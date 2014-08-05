using System.ComponentModel;
using Azmyth.Assets;

namespace Azmyth.Assets
{
    public class Area : Asset
    {
        [CategoryAttribute("Position")]
        public long GridX { get; set; }

        [CategoryAttribute("Position")]
        public long GridY { get; set; }

        public Area()
        {

        }

        public Area(VectorID areaID)
        {
            this.AssetID = areaID;
        }
    }
}
