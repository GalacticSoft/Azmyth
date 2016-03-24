using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azmyth.Maths
{
    public static class Dice
    {
        public static int Roll(int size)
        {
            return Roll(size, 1);
        }

        public static int Roll(int size, int dice)
        {
            int roll = 0;

            for (int die = 0; die < dice; die++)
            {
                roll += Numbers.NumberRange(1, size);
            }

            return roll;
        }

        public static int[] GetRolls(int size, int dice)
        {
            int[] roll = new int[dice];

            for (int die = 0; die < dice; die++)
            {
                roll[die] = Numbers.NumberRange(1, size);
            }

            return roll;
        }
    }
}
