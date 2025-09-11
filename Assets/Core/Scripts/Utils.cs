using UnityEngine;

namespace Minofall
{
    public static class Utils
    {
        public static float WrapValue(float val, float min, float max)
        {
            float range = max - min;
            if (val < min) val += range * Mathf.Ceil((min - val) / range);
            else if (val > max) val -= range * Mathf.Ceil((val - max) / range);
            return val;
        }

        public static int WrapValue(int input, int min, int max)
        {
            if (input < min) return max - (min - input) % (max - min);
            else return min + (input - min) % (max - min);
        }

        public static string NumberFormat(long number) => number.ToString("N0");
    }
}