using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Assets
{
    public enum CitySize
    {
        None = 0,
        Camp = 1,
        Village = 3,
        Township = 4,
        Town = 4,
        Port = 5,
        City = 6,
        Castle = 7
    }

    public class City : Area
    {
        public CitySize CitySize { get; set; }
        private int m_seed = 0;
    }
}
