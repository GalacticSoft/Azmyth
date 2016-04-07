using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Assets
{
    

    public class Material
    {
        public MaterialClass MaterialType { get; set; }
        public MaterialType MaterialID { get; set; }
        public string Name { get; set; }

        public float PriceMod { get; set; }
        public float DamageMod { get; set; }
        public float WeightMod { get; set; }
        public float DropMod { get; set; }
    }
}
