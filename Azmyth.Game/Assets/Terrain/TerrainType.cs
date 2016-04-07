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
        Ocean       = 1,
        Sand        = 2,
        Dirt        = 3,
        Grass       = 4,
        Stone       = 5,
        Snow        = 6,
        Lava        = 7,
        Ice         = 8,
        River       = 9,
        Road        = 10,
        Max         = 11
    }

    public struct TerrainType
    {
        public static readonly TerrainType[] Terrain =
        {
            new TerrainType(TerrainTypes.None,  "None",     false,  false,  false,  0.0f),
            new TerrainType(TerrainTypes.Ocean, "Ocean",    true,   true,   false,  0.1f),
            new TerrainType(TerrainTypes.Sand,  "Sand",     true,   false,  false,  0.7f),
            new TerrainType(TerrainTypes.Dirt,  "Dirt",     true,   false,  false,  0.9f),
            new TerrainType(TerrainTypes.Grass, "Grass",    true,   false,  false,  0.8f),
            new TerrainType(TerrainTypes.Stone, "Stone",    false,  false,  false,  0.0f),
            new TerrainType(TerrainTypes.Snow,  "Snow",     true,   false,  false,  0.5f),
            new TerrainType(TerrainTypes.Lava,  "Lava",     true,   false,  false,  0.2f),
            new TerrainType(TerrainTypes.Ice,   "Ice",      true,   false,  false,  0.6f),
            new TerrainType(TerrainTypes.River, "River",    true,   true,   true,   0.4f),
            new TerrainType(TerrainTypes.Road,  "Road",     true,   false,  false,  1.0f),
        };

        public TerrainType(TerrainTypes terrainType, string name, bool canPass, bool isWater, bool canSwim, float moveRate)
        {
            m_name = name;
            
            m_isWater = isWater;
            m_canPass = canPass;
            m_canSwim = canSwim;

            m_moveRate = moveRate;

            m_terrainType = terrainType;
        }

        private string m_name;
        
        private bool m_isWater;
        private bool m_canSwim;
        private bool m_canPass;

        private float m_moveRate;

        private TerrainTypes m_terrainType;

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
            private set
            {
                m_name = value;
            }
        }

        public bool IsWater
        {
            get
            {
                return m_isWater;
            }
            private set
            {
                m_isWater = value;
            }
        }

        public bool CanPass
        {
            get
            {
                return m_canPass;
            }
            private set
            {
                m_canPass = value;
            }
        }

        public bool CanSwim
        {
            get
            {
                return m_canSwim;
            }
            private set
            {
                m_canSwim = value;
            }
        }

        public float MoveRate
        {
            get
            {
                return m_moveRate;
            }
            private set
            {
                m_moveRate = value;
            }
        }

        public TerrainTypes Type
        {
            get
            {
                return m_terrainType;
            }
            private set
            {
                m_terrainType = value;
            }
        }
    }
}
