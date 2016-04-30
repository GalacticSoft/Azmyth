using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Assets
{
    public struct ItemType
    {
        public ItemClass ItemClass;

        public string Names;

        public WearLocation WearLocation;
        public bool Ranged;

        public MaterialClass [] Materials;
    }
}
