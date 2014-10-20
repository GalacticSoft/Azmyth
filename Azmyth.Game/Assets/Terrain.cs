using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmyth.Assets
{
    public enum TerrainTypes
    {
        None        = 0,
        Water       = 1,
        Sand        = 2,
        Dirt        = 3,
        Grass       = 4,
        Stone       = 5,
        Snow        = 6,
        Lava        = 7,
        Ice         = 8,
        Black       = 9,
        City        = 10,
        River       = 11,
    }

    public class Terrain
    {
        private TerrainTypes m_terrainType = TerrainTypes.Water;

        private bool m_isPassable = true;
        private bool m_isWater = true;
        private bool m_canSwim = true;

        private string m_name = "";

        public string Name
        {
            get
            {
                if(m_name == "")
                {
                    m_name = m_terrainType.ToString();
                }

                return m_name;
            }
            set
            {
                m_name = value;
            }
        }
        
    }
}
