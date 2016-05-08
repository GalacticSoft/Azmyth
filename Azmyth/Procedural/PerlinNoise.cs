using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Procedural
{
    public class PerlinNoise : Noise
    {
        public static int repeat { get; set; }

        private static readonly int[] permutation = { 151,160,137,91,90,15,                 // Hash lookup table as defined by Ken Perlin.  This is a randomly
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,    // arranged array of all numbers from 0-255 inclusive.
            190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
            129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
            49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
        };

        private static readonly int[] p;                                                    // Doubled permutation to avoid overflow

        static PerlinNoise()
        {
            repeat = 0;

            p = new int[512];


            for (int x = 0; x < 512; x++)
            {
                p[x] = permutation[x % 256];
            }
        }

        public PerlinNoise(double persistence, double frequency, double amplitude, int octaves, long seed)
            : base(persistence, frequency, amplitude, octaves, seed) 
        {
           
        }      

        public override double GetValue(double x, double y)
        {
            return GetValue(x, y, 1);
        }

        double Interpolate(double x, double y, double a) 
        {
            double negA = 1.0 - a;

            double negASqr = negA * negA;
        
            double fac1 = 3.0 * (negASqr) - 2.0 * (negASqr * negA);
      
            double aSqr = a * a;
        
            double fac2 = 3.0 * aSqr - 2.0 * (aSqr * a);

            return x * fac1 + y * fac2; //add the weighted factors
        }

        double Noise(int x, int y)
        {
            int n = x + y * 57;
        
            n = (n << 13) ^ n;
        
            int t = (n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff;
        
            return 1.0 - ((double)t) * 0.931322574615478515625e-9;/// 1073741824.0);
        }

        public override double GetValue(double x, double y, double z)
        {
            double total = 0;
            double frequency = 1;
            double amplitude = 1;
            double maxValue = 0;  // Used for normalizing result to 0.0 - 1.0
            for (int i = 0; i < m_octaves; i++)
            {
                total += perlin(x * frequency, y * frequency, z * frequency) * amplitude;

                maxValue += amplitude;

                amplitude *= m_persistence;
                frequency *= 2;
            }

            return total / maxValue;
        }

        public override double GetValue(double x, double y, double z, double t)
        {
            throw new NotImplementedException();
        }

        private int inc(int num)
        {
            num++;
            if (repeat > 0) num %= repeat;

            return num;
        }

        private double perlin(double x, double y, double z)
        {
            if (repeat > 0)
            {                                    // If we have any repeat on, change the coordinates to their "local" repetitions
                x = x % repeat;
                y = y % repeat;
                z = z % repeat;
            }

            int xi = (int)x & 255;                              // Calculate the "unit cube" that the point asked will be located in
            int yi = (int)y & 255;                              // The left bound is ( |_x_|,|_y_|,|_z_| ) and the right bound is that
            int zi = (int)z & 255;                              // plus 1.  Next we calculate the location (from 0.0 to 1.0) in that cube.
            double xf = x - (int)x;
            double yf = y - (int)y;
            double zf = z - (int)z;

            int aaa, aba, aab, abb, baa, bba, bab, bbb;
            aaa = p[p[p[xi] + yi] + zi];
            aba = p[p[p[xi] + inc(yi)] + zi];
            aab = p[p[p[xi] + yi] + inc(zi)];
            abb = p[p[p[xi] + inc(yi)] + inc(zi)];
            baa = p[p[p[inc(xi)] + yi] + zi];
            bba = p[p[p[inc(xi)] + inc(yi)] + zi];
            bab = p[p[p[inc(xi)] + yi] + inc(zi)];
            bbb = p[p[p[inc(xi)] + inc(yi)] + inc(zi)];

            double u = Numbers.fade(xf);
            double v = Numbers.fade(yf);
            double w = Numbers.fade(zf);

            double x1, x2, y1, y2;
            x1 = Numbers.lerp(
                Numbers.grad(aaa, xf, yf, zf),           // The gradient function calculates the dot product between a pseudorandom
                Numbers.grad(baa, xf - 1, yf, zf),             // gradient vector and the vector from the input coordinate to the 8
                        u);                                     // surrounding points in its unit cube.
            x2 = Numbers.lerp(
                Numbers.grad(aba, xf, yf - 1, zf),           // This is all then lerped together as a sort of weighted average based on the faded (u,v,w)
                Numbers.grad(bba, xf - 1, yf - 1, zf),             // values we made earlier.
                          u);
            y1 = Numbers.lerp(x1, x2, v);

            x1 = Numbers.lerp(
                Numbers.grad(aab, xf, yf, zf - 1),
                Numbers.grad(bab, xf - 1, yf, zf - 1),
                        u);
            x2 = Numbers.lerp(
                Numbers.grad(abb, xf, yf - 1, zf - 1),
                Numbers.grad(bbb, xf - 1, yf - 1, zf - 1),
                          u);
            y2 = Numbers.lerp(x1, x2, v);

            return (Numbers.lerp(y1, y2, w) + 1) / 2;                      // For convenience we bind the result to 0 - 1 (theoretical min/max before is [-1, 1])
        }

    }
}
