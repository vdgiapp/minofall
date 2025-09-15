using UnityEngine;

namespace Minofall
{
    /// <summary>
    /// Biểu diễn một mảnh (piece) trong trò chơi Minofall.
    /// </summary>
    public class Piece
    {
        /// <summary>
        /// Index của tetromino (kiểu mảnh) trong danh sách Tetrominoes.
        /// </summary>
        public int tetrominoIndex;

        /// <summary>
        /// Index của góc độ xoay hiện tại của mảnh.
        /// </summary>
        public int rotationIndex;

        /// <summary>
        /// Vị trí của mảnh trên lưới trò chơi.
        /// </summary>
        public Vector2Int position;

        /// <summary>
        /// Mau sắc của mảnh.
        /// </summary>
        public Color color;

        /// <summary>
        /// Constructor mặc định khởi tạo một mảnh với các giá trị mặc định.
        /// </summary>
        public Piece()
        {
            Initialize(0, 0, new(0, 0), Tetrominoes.GetColor(0));
        }

        /// <summary>
        /// Constructor khởi tạo một mảnh với các giá trị cụ thể.
        /// </summary>
        /// <param name="tetrominoIndex">Index của tetromino</param>
        /// <param name="rotationIndex">Index của góc độ xoay</param>
        /// <param name="position">Vị trí trên lưới</param>
        /// <param name="color">Màu sắc</param>
        public Piece(int tetrominoIndex, int rotationIndex, Vector2Int position, Color color)
        {
            Initialize(tetrominoIndex, rotationIndex, position, color);
        }

        /// <summary>
        /// Đặt lại các thuộc tính của mảnh.
        /// </summary>
        /// <param name="tetrominoIndex">Index của tetromino</param>
        /// <param name="rotationIndex">Index của góc độ xoay</param>
        /// <param name="position">Vị trí trên lưới</param>
        /// <param name="color">Màu sắc</param>
        public void Initialize(int tetrominoIndex, int rotationIndex, Vector2Int position, Color color)
        {
            this.tetrominoIndex = tetrominoIndex;
            this.rotationIndex = rotationIndex;
            this.position = position;
            this.color = color;
        }
    }
}