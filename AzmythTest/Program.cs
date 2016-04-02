using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azmyth.Game;

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

            for(int x = 1; x < 8; x++)
            {


                float log = s * (float)Math.Pow(b, x / 7.0f) + t;

                Console.WriteLine("X{0}: {1} = {2}", x, 100-log, (log - prev));

                total += log - prev;

                prev = log;
            }

            Console.WriteLine(total.ToString());

            int[] count = new int[7]
            {
                0,
                0,
                0,
                0,
                0,
                0,
                0
            };

            /*Azmyth.Assets.MaterialType type1 = Azmyth.Assets.Item.Generate(1);
            Console.WriteLine("[{0}] {1} {2}% / {3}%", 1, type1.ToString(), 1, Azmyth.Assets.Material.MaterialDropChances[type1]);

            Azmyth.Assets.MaterialType type2 = Azmyth.Assets.Item.Generate(31);
            Console.WriteLine("[{0}] {1} {2}% / {3}%", 2, type2.ToString(), 31, Azmyth.Assets.Material.MaterialDropChances[type2]);

            Azmyth.Assets.MaterialType type3 = Azmyth.Assets.Item.Generate(51);
            Console.WriteLine("[{0}] {1} {2}% / {3}%", 3, type3.ToString(), 51, Azmyth.Assets.Material.MaterialDropChances[type3]);

            Azmyth.Assets.MaterialType type4 = Azmyth.Assets.Item.Generate(61);
            Console.WriteLine("[{0}] {1} {2}% / {3}%", 4, type4.ToString(), 61, Azmyth.Assets.Material.MaterialDropChances[type4]);

            Azmyth.Assets.MaterialType type5 = Azmyth.Assets.Item.Generate(81);
            Console.WriteLine("[{0}] {1} {2}% / {3}%", 5, type5.ToString(), 81, Azmyth.Assets.Material.MaterialDropChances[type5]);

            Azmyth.Assets.MaterialType type6 = Azmyth.Assets.Item.Generate(96);
            Console.WriteLine("[{0}] {1} {2}% / {3}%", 6, type6.ToString(), 96, Azmyth.Assets.Material.MaterialDropChances[type6]);
            */
            for (int i = 0; i < 1000; i++)
            {
                
                double typePercent = Azmyth.Numbers.Percent() * 100;
                double materialPercent = Azmyth.Numbers.Percent() * 100;
                Azmyth.Assets.Result res = Azmyth.Assets.Item.Generate(typePercent, materialPercent);

                count[(int)res.type]++;

                Console.WriteLine("[{0}] {1} {2}% / {3}% {4}", i, res.type.ToString(), (float)typePercent, (float)materialPercent, res.material);
            }

            Console.WriteLine("Animal {0}, Wood {1}, Stone {2}, Metal {3}, Gemstone {4}, Elemental {5}", count[1] / 10, count[2] / 10, count[3] / 10, count[4] / 10, count[5] / 10, count[6] / 10);
            
            Console.ReadLine();
        }
    }
}
