using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azmyth.Game;
using Azmyth.Assets;

namespace AzmythTest
{
    class Program
    {
        static void Main(string[] args)
        {
            float prev = 0;
            float total = 0;
            float b = 10.0f;
            float s = 100.0f / (b - 1);
            float t = -100.0f / (b - 1);

            for(int x = 1; x <= 7; x++)
            {
                float log = s * (float)Math.Pow(b, x / 7.0f) + t;

                Console.WriteLine("X{0}: {1} = {2}", x, 100-log, (log - prev));

                total += log - prev;

                prev = log;
            }

            Console.WriteLine(total.ToString());

            int[] count = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
            int[] qualityCount = new int[5] { 0, 0, 0, 0, 0 };
            int[] rarityCount = new int[7] { 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 1000; i++)
            {
                Azmyth.Assets.Item item = Azmyth.Assets.Item.Generate();

                count[(int)item.MaterialClass]++;
                qualityCount[(int)item.Quality]++;
                rarityCount[(int)item.Rarity]++;

                Console.WriteLine("[{0,-11}] [{1,-11}] [{2,-11}] [{3, -11}] {4}", item.Rarity, item.Quality, item.MaterialClass, item.MaterialType, item.Name);
            }
            
            Console.WriteLine("Common {0}, Uncommon {1}, Rare  {2}, Legendary {3}, Epic     {4}, Mythic    {5}", rarityCount[1] / 10, rarityCount[2] / 10, rarityCount[3] / 10, rarityCount[4] / 10, rarityCount[5] / 10, rarityCount[6] / 10);
            
            Console.WriteLine("Poor   {0}, Average  {1}, Good  {2}, Excellent {3}, Superior {4}", qualityCount[0] / 10, qualityCount[0] / 10, qualityCount[2] / 10, qualityCount[3] / 10, qualityCount[4] / 10);
            
            Console.WriteLine("Animal {0}, Wood     {1}, Stone {2}, Metal     {3}, Gemstone {4}, Elemental {5}", count[1] / 10, count[2] / 10, count[3] / 10, count[4] / 10, count[5] / 10, count[6] / 10);
            
            Console.ReadLine();
        }
    }
}
