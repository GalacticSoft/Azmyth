using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azmyth.Math;

namespace Azmyth.Assets
{
    public class Life : Area
    {
        public LifeCell [,] LifeCells;

        public Life(VectorID assetID)
        {
            this.AssetID = assetID; 
        }
    }
}
