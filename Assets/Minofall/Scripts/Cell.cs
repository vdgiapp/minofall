using UnityEngine;

namespace Minofall
{
    public class Cell
    {
        public CellDisplay cellDisplay;
        public Color color;
        public bool occupied;

        public Cell()
        {
            Initialize(new(), Color.clear, false);
        }

        public Cell(CellDisplay cellDisplay, Color color, bool occupied)
        {
            Initialize(cellDisplay, color, occupied);
        }

        public void Initialize(CellDisplay cellDisplay, Color color, bool occupied)
        {
            this.cellDisplay = cellDisplay;
            this.color = color;
            this.occupied = occupied;
        }
    }
}