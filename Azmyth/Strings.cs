using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth
{
    public static class Strings
    {
        public static string Article(string word)
        {
            char first = word[0];
            char last = word[word.Length - 1];
            string article = "a";
            string vowels = "aeiouAEIOU";
            string special = "hH";
            string plural = "sS";

            if(vowels.Contains(first) || special.Contains(first))
            {
                article = "an";
            }

            if (plural.Contains(last))
            {
                article = "some";
            }

            return article;
            
        }
    }
}
