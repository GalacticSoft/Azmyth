using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azmyth.Procedural
{
    public class RandomNoise : Noise
    {
        Random m_random = null;
       
        public RandomNoise(int seed)
        {
            m_random = new Random(seed);
        }

        public RandomNoise(double persistence, double frequency, double amplitude, int octaves, long seed)
            : base(persistence, frequency, amplitude, octaves, seed) { }   

        public override double GetValue(double x, double y)
        {
            int result = (int)x;
            result *= 1257787 | 1;
            result += 2796203;
            result = (result >> 32) + (result << 32);
            result ^= 859433;
            result += (int)y;
            result *= 946669 | 1;
            result += (int)m_seed;
            result = (result >> 32) + (result << 32);
            result ^= 3337333;
            result *= 9369319 | 1;

            Random random = new Random(result);
            return random.NextDouble();
        }

        public override double GetValue(double x, double y, double z)
        {
            throw new NotImplementedException();
        }

        public override double GetValue(double x, double y, double z, double t)
        {
            throw new NotImplementedException();
        }
    }
}
