﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmyth.Assets
{
    public enum TerrainTypes
    {
        Ocean       = 0,
        Sand        = 1,
        Dirt        = 2,
        Forest      = 3,
        River       = 4,
        Mountain    = 5,
        Stone       = 6,
        Snow        = 7,
        Lava        = 8,
        Ice         = 9,
        Black       = 10,
        City        = 11,
    }

    public class Terrains
    {
        private TerrainTypes m_terrainType = TerrainTypes.Ocean;

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