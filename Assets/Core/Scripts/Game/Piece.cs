using UnityEngine;

namespace Minofall
{
    [System.Serializable]
    public class Piece
    {
        public int tetrominoIndex;
        public int rotationIndex;
        public Vector2Int position;
        public Color color;

        public Piece()
        {
            Initialize(0, 0, BoardController.DEFAULT_SPAWN_POSITION, Tetrominoes.GetColor(0));
        }

        public Piece(int tetrominoIndex, int rotationIndex, Vector2Int position, Color color)
        {
            Initialize(tetrominoIndex, rotationIndex, position, color);
        }

        public void Initialize(int tetrominoIndex, int rotationIndex, Vector2Int position, Color color)
        {
            this.tetrominoIndex = tetrominoIndex;
            this.rotationIndex = rotationIndex;
            this.position = position;
            this.color = color;
        }
    }
}