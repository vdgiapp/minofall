using System.Linq;
using UnityEngine;

namespace Minofall
{
    public static class Tetrominoes
    {
        public struct TetrominoData
        {
            public Vector2Int[][] cells; // include rotation index cells
            public Color color;

            public TetrominoData(Vector2Int[][] cells, Color color)
            {
                this.cells = cells;
                this.color = color;
            }
        }

        // Gốc index = bottem left
        private static readonly TetrominoData[] _tetrominoes =
        {
            // I
            new(
                new Vector2Int[][]
                {
                    new Vector2Int[] { new(0,2), new(1,2), new(2,2), new(3,2) },    // 0
                    new Vector2Int[] { new(2,0), new(2,1), new(2,2), new(2,3) },    // 90
                    new Vector2Int[] { new(0,1), new(1,1), new(2,1), new(3,1) },    // 180
                    new Vector2Int[] { new(1,0), new(1,1), new(1,2), new(1,3) }     // 270
                },
                new Color(0f, 1f, 1f) // Cyan
            ),

            // J
            new(
                new Vector2Int[][]
                {
                    new Vector2Int[] { new(0,1), new(1,1), new(2,1), new(0,2) },
                    new Vector2Int[] { new(1,0), new(1,1), new(1,2), new(2,2) },
                    new Vector2Int[] { new(2,0), new(0,1), new(1,1), new(2,1) },
                    new Vector2Int[] { new(0,0), new(1,0), new(1,1), new(1,2) }
                },
                new Color(1f, 1f, 0f) // Yellow
            ),

            // L
            new(
                new Vector2Int[][]
                {
                    new Vector2Int[] { new(0,1), new(1,1), new(2,1), new(2,2) },
                    new Vector2Int[] { new(1,0), new(2,0), new(1,1), new(1,2) },
                    new Vector2Int[] { new(0,0), new(0,1), new(1,1), new(2,1) },
                    new Vector2Int[] { new(1,0), new(1,1), new(0,2), new(1,2) }
                },
                new Color(0.6f, 0f, 1f) // Purple
            ),

            // O
            new(
                new Vector2Int[][]
                {
                    new Vector2Int[] { new(1,1), new(2,1), new(1,2), new(2,2) },
                    new Vector2Int[] { new(1,1), new(2,1), new(1,2), new(2,2) },
                    new Vector2Int[] { new(1,1), new(2,1), new(1,2), new(2,2) },
                    new Vector2Int[] { new(1,1), new(2,1), new(1,2), new(2,2) },
                },
                new Color(0f, 1f, 0f) // Green
            ),

            // S
            new(
                new Vector2Int[][]
                {
                    new Vector2Int[] { new(0,1), new(1,1), new(1,2), new(2,2) },
                    new Vector2Int[] { new(2,0), new(1,1), new(2,1), new(1,2) },
                    new Vector2Int[] { new(0,0), new(1,0), new(1,1), new(2,1) },
                    new Vector2Int[] { new(1,0), new(0,1), new(1,1), new(0,2) },
                },
                new Color(1f, 0f, 0f) // Red
            ),

            // T
            new(
                new Vector2Int[][]
                {
                    new Vector2Int[] { new(0,1), new(1,1), new(2,1), new(1,2) },
                    new Vector2Int[] { new(1,0), new(1,1), new(2,1), new(1,2) },
                    new Vector2Int[] { new(1,0), new(0,1), new(1,1), new(2,1) },
                    new Vector2Int[] { new(1,0), new(0,1), new(1,1), new(1,2) },
                },
                new Color(0f, 0f, 1f) // Blue
            ),

            // Z
            new(
                new Vector2Int[][]
                {
                    new Vector2Int[] { new(1,1), new(2,1), new(0,2), new(1,2) },
                    new Vector2Int[] { new(1,0), new(1,1), new(2,1), new(2,2) },
                    new Vector2Int[] { new(1,0), new(2,0), new(0,1), new(1,1) },
                    new Vector2Int[] { new(0,0), new(0,1), new(1,1), new(1,2) },
                },
                new Color(1f, 0.5f, 0f) // Orange
            ),
        };

        public static int Length => _tetrominoes.Length;
        public static int MaxRotations => _tetrominoes[0].cells.Length;
        public static Color GetColor(int idx) => _tetrominoes[idx].color;
        public static Vector2Int[] GetCells(int idx, int rotationIdx) => _tetrominoes[idx].cells[rotationIdx];
    }
}