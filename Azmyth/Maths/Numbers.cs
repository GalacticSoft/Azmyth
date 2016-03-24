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
    }
}
