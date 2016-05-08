using System;

namespace Azmyth
{
    public static class Numbers
    {


        public const double Pi = 3.14159;

        private readonly static Random m_random = new Random((int)DateTime.Now.Ticks);

        public static double Percent()
        {
            return m_random.NextDouble();
        }

        public static int NumberRange(int minValue, int maxValue)
        {
            return m_random.Next(minValue, maxValue);
        }

        public static float NumberRange(short minValue, short maxValue)
        {
            return m_random.Next(minValue, maxValue);
        }

        public static double GetArea(double radius)
        {
            return Numbers.Pi * (radius * radius);
        }

        public static float ConvertCoordinate(float coordinate, float scale)
        {
            float converted = 0.0f;

            if (coordinate < 0.0f)
            {
                converted = (float)Math.Floor((double)(coordinate / scale));
            }
            else
            {
                converted = (float)Math.Floor((double)((coordinate) / scale));
            }

            return converted;
        }

        // Linear Interpolate
        public static double lerp(double a, double b, double x)
        {
            return a + x * (b - a);
        }

        // Source: http://riven8192.blogspot.com/2010/08/calculate-perlinnoise-twice-as-fast.html
        public static double grad(int hash, double x, double y, double z)
        {
            switch (hash & 0xF)
            {
                case 0x0: return x + y;
                case 0x1: return -x + y;
                case 0x2: return x - y;
                case 0x3: return -x - y;
                case 0x4: return x + z;
                case 0x5: return -x + z;
                case 0x6: return x - z;
                case 0x7: return -x - z;
                case 0x8: return y + z;
                case 0x9: return -y + z;
                case 0xA: return y - z;
                case 0xB: return -y - z;
                case 0xC: return y + x;
                case 0xD: return -y + z;
                case 0xE: return y - x;
                case 0xF: return -y - z;
                default: return 0; // never happens
            }
        }


        public static double fade(double t)
        {
            // Fade function as defined by Ken Perlin.  This eases coordinate values
            // so that they will ease towards integral values.  This ends up smoothing
            // the final output.
            return t * t * t * (t * (t * 6 - 15) + 10);         // 6t^5 - 15t^4 + 10t^3
        }
    }
}
