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
            string article = "a";
            string vowels = "aeiouAEIOU";
            string special = "hH";

            if(vowels.Contains(first) || special.Contains(first))
            {
                article = "an";
            }

            return article;
            
        }
    }
}
