using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Procedural
{
    public static class ShuffleArray
    {
        public static int Seed = 1000;

        public static T[] Shuffle<T>(T[] arr)
        {
            List<KeyValuePair<int, T>> list = new List<KeyValuePair<int, T>>();
            // Add all strings from array
            // Add new random int each time
            foreach (T i in arr)
            {
                list.Add(new KeyValuePair<int, T>(new Random(Seed).Next(), i));
            }

            // Sort the list by the random number
            var sorted = from item in list
                         orderby item.Key
                         select item;

            // Allocate new string array
            T[] result = new T[arr.Length];

            // Copy values to array
            int index = 0;

            foreach (KeyValuePair<int, T> pair in sorted)
            {
                result[index] = pair.Value;
                index++;
            }
            // Return copied array
            return result;
        }

        public static T[] Shuffle2<T>(T[] arr)
        {
            Random rnd = new Random(Seed);
            T[] rando = arr.OrderBy(x => rnd.Next()).ToArray();

            return rando;
        }
    }
}
