namespace Minofall
{
    public static class TetrisGravity
    {
        private static readonly float[] DropTimes =
        {
            0.01667f, // Level 0 (not play)
            0.8f,     // Level 1
            0.7167f,  // Level 2
            0.6333f,  // Level 3
            0.55f,    // Level 4
            0.4667f,  // Level 5
            0.3833f,  // Level 6
            0.3f,     // Level 7
            0.2167f,  // Level 8
            0.1333f,  // Level 9
            0.1f,     // Level 10
            0.0833f,  // Level 11
            0.0833f,  // Level 12
            0.0833f,  // Level 13
            0.0667f,  // Level 14
            0.0667f,  // Level 15
            0.0667f,  // Level 16
            0.05f,    // Level 17
            0.05f,    // Level 18
            0.05f,    // Level 19
            0.0333f,  // Level 20
            0.0333f,  // Level 21
            0.0333f,  // Level 22
            0.0333f,  // Level 23
            0.0333f,  // Level 24
            0.0333f   // Level 25 (Max guideline speed)
        };

        public static float GetDropTime(int level)
        {
            if (level < 1) level = 1;
            if (level > 25) level = 25;
            return DropTimes[level];
        }
    }
}