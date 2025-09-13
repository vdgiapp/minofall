using UnityEngine;

namespace Minofall
{
    public struct SessionData
    {
        public int score;
        public int level;
        public int linesCleared;

        public SessionData(int score, int level, int linesCleared)
        {
            this.score = score;
            this.level = level;
            this.linesCleared = linesCleared;
        }
    }
}