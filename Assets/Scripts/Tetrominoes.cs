using UnityEngine;

namespace Minofall
{
    public static class Tetrominoes
    {
        public struct TetrominoData
        {
            public Vector2Int[][] Cells; // with rotation index
            public Color Color;

            public TetrominoData(Vector2Int[][] cells, Color color)
            {
                Cells = cells;
                Color = color;
            }
        }

        // Gốc index = bottem left
        private static readonly TetrominoData[] _tetrominoes =
        {
            // I
            new(
                new Vector2Int[][]
                {
                    new Vector2Int[] { new(0,2), new(1,2), new(2,2), new(3,2) }, // 0 độ
                    new Vector2Int[] { new(2,0), new(2,1), new(2,2), new(2,3) },
                    
                },
                new Color(0f, 1f, 1f) // Cyan
            ),

            // O
            new(
                new Vector2Int[][]
                {
                    
                },
                new Color(1f, 1f, 0f) // Yellow
            ),

            // T
            new(
                new Vector2Int[][]
                {
                    
                },
                new Color(0.6f, 0f, 1f) // Purple
            ),

            // S
            new(
                new Vector2Int[][]
                {
                    
                },
                new Color(0f, 1f, 0f) // Green
            ),

            // Z
            new(
                new Vector2Int[][]
                {
                    
                },
                new Color(1f, 0f, 0f) // Red
            ),

            // J
            new(
                new Vector2Int[][]
                {
                    
                },
                new Color(0f, 0f, 1f) // Blue
            ),

            // L
            new(
                new Vector2Int[][]
                {
                    
                },
                new Color(1f, 0.5f, 0f) // Orange
            ),
        };

        public static int Count => _tetrominoes.Length;
        public static int Length => Count;

        public static Color GetColor(int idx) => _tetrominoes[idx].Color;

        public static Vector2Int[] GetCells(int idx, int rotationIdx) => _tetrominoes[idx][rotationIdx];
    }
}
