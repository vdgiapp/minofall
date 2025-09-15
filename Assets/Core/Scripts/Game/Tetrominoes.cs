using UnityEngine;

namespace Minofall
{
    /// <summary>
    /// Static class, chứa dữ liệu các khối tetromino, bao gồm các vị trí ô vuông, góc quay và màu sắc.
    /// </summary>
    public static class Tetrominoes
    {
        /// <summary>
        /// Chứa dữ liệu của một tetromino, bao gồm các vị trí ô vuông cho từng góc quay và màu sắc.
        /// </summary>
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

        /// <summary>
        /// <para>Dữ liệu các tetromino, bao gồm vị trí ô vuông cho từng góc quay và màu sắc.</para>
        /// <para>Lưu ý: Góc quay được định nghĩa theo chiều kim đồng hồ, bắt đầu từ 0 độ.
        /// Và gốc tọa độ (0,0) được đặt ở góc dưới bên trái của khối tetromino.</para>
        /// </summary>
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
                new Color(0f, 0f, 1f) // Blue
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
                new Color(1f, 0.5f, 0f) // Orange
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
                new Color(1f, 1f, 0f) // Yellow
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
                new Color(0f, 1f, 0f) // Green
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
                new Color(0.6f, 0f, 1f) // Purple
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
                new Color(1f, 0f, 0f) // Red
            ),
        };

        /// <summary>
        /// Độ dài mảng tetrominoes.
        /// </summary>
        public static int Length => _tetrominoes.Length;

        /// <summary>
        /// Độ dài của mảng góc quay (số góc quay).
        /// </summary>
        public static int MaxRotations => _tetrominoes[0].cells.Length;

        /// <summary>
        /// Trả về màu sắc của tetromino có index được truyền vào.
        /// </summary>
        /// <param name="tetrominoIndex">Index của tetromino</param>
        public static Color GetColor(int tetrominoIndex) => _tetrominoes[tetrominoIndex].color;

        /// <summary>
        /// Trả về mảng các vị trí ô vuông của tetromino có index và góc quay được truyền vào.
        /// </summary>
        /// <param name="idx">Index của tetromino</param>
        /// <param name="rotationIdx">Index góc xoay của tetromino</param>
        public static Vector2Int[] GetCells(int tetrominoIndex, int rotationIndex)
            => _tetrominoes[tetrominoIndex].cells[rotationIndex];
    }
}