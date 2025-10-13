using UnityEngine;

namespace Minofall
{
    [System.Serializable]
    public class Cell
    {
        public Color color;
        public bool occupied;

        public Cell()
        {
            Initialize(Color.clear, false);
        }

        public Cell(Color color, bool occupied)
        {
            Initialize(color, occupied);
        }

        public void Initialize(Color color, bool occupied)
        {
            this.color = color;
            this.occupied = occupied;
        }
    }
}