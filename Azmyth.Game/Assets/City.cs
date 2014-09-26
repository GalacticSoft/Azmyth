using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azmyth.Procedural;

namespace Azmyth.Assets
{
    public enum CitySize
    {
        None = 0,
        Village = 1,
        Town = 2,
        City = 3,
        Kingdom = 4,
        Port = 5,
    }

    public class City : Area
    {
        public CitySize CitySize { get; set; }
        private int m_seed = 0;
        
    }
}
