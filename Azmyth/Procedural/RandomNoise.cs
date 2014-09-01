using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmyth.Procedural
{
    public class RandomNoise : INoise
    {
        int m_seed = 0;
        Random m_random = null;
       
        public RandomNoise(int seed)
        {
            m_seed = seed;
            m_random = new Random(seed);
        }

        public double GetValue(double x, double y)
        {
            //int seed = (int)(x * 28657 + y) * 514229 + m_seed;

            //Random random = new Random((int)(m_seed + (long)x & ((long)x ^ (long)y)));

            int result = (int)x;
            result *= 1257787 | 1;
            result += 2796203;
            result = (result >> 32) + (result << 32);
            result ^= 859433;
            result += (int)y;
            result *= 946669 | 1;
            result += m_seed;
            result = (result >> 32) + (result << 32);
            result ^= 3337333;
            result *= 9369319 | 1;

            Random random = new Random(result);
            return random.NextDouble();

        }
    }
}
