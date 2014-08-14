using Azmyth.Assets;
using Azmyth.Math;

namespace Azmyth.Assets
{

    public class World : Asset
    {
        private PerlinNoise m_terrain;
        private PerlinNoise m_forest;
        private PerlinNoise m_rivers;
        private int m_seed;

        public World(VectorID vectorID, int seed)
        {
            AssetID = vectorID;
            m_seed = seed;

            //create noise generator
        }

        public override void AddObject(Asset obj)
        {
            base.AddObject(obj);
        }

        public override void RemoveObject(Asset obj)
        {
            base.RemoveObject(obj);
        }
       
    }
}
