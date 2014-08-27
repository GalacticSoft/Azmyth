using System;

namespace Azmyth
{
    public class Numbers
    {
        public const double Pi = 3.14159;

        private readonly static Random _random = new Random((int)DateTime.Now.Ticks);

        public static int NumberRange(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        public static float NumberRange(short minValue, short maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        public double GetArea(double radius)
        {
            return Numbers.Pi * (radius * radius);
        }
    }
}
