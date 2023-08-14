using Godot;
using System;

namespace AroundYou.Utils.Extensions
{
    public static class RandomExtensions
    {
        public static float RandomBetween(this Random random, float min, float max)
        {
            return ((float)random.NextDouble() * (max - min)) + min;
        }

        public static int RandomBetween(this Random random, int min, int max)
        {
            return random.Next(min, max);
        }

        public static bool CanDoAction(this RandomNumberGenerator random, float percentage)
        {
            percentage = (float)Math.Round(percentage, 2);
            double res = Math.Round(random.RandfRange(0, 1), 2);
            return res <= percentage;
        }
    }
}
