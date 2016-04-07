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

        public bool Wearable;
        public WearLocation WearLocation;
        public bool Holdable;
        public bool TwoHanded;
        public bool Ranged;
    }
}
