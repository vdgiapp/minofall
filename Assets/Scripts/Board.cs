using UnityEngine;

namespace Minofall
{
    public class Board : MonoBehaviour
    {
        public static readonly Vector2Int SIZE = new(10, 20);
        public static readonly Vector2Int DEFAULT_SPAWN_POSITION = new(3, 17);

        [SerializeField]
        private Cell _cellPrefab;

        [SerializeField]
        private Transform _cellContainerTransform;

        private readonly Cell[,] _cells = new Cell[SIZE.y, SIZE.x];
        private readonly int[,] _cellData = new int[SIZE.y, SIZE.x];
        private int _currentTetrominoIndex;
        private int _currentPieceRotationIndex;
        private Vector2Int _currentPiecePosition;

        private void Awake()
        {
            for (int r = 0; r < SIZE.y; r++)
            {
                for (int c = 0; c < SIZE.x; c++)
                {
                    _cells[r, c] = Instantiate(_cellPrefab, _cellContainerTransform);
                    _cells[r, c].transform.position = new(c, r, 0.0f);
                    _cells[r, c].Hide();
                }
            }
            SpawnPiece();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                MoveCurrentPiece(Vector2Int.left);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                MoveCurrentPiece(Vector2Int.right);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                MoveCurrentPiece(Vector2Int.down);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                RotateCurrentPiece();
            }
        }

        private void SpawnPiece()
        {
            _currentTetrominoIndex = Random.Range(0, Tetrominoes.Count);
            _currentPiecePosition = DEFAULT_SPAWN_POSITION;
            _currentPieceRotationIndex = 0;
            ShowCurrentPiece();
        }

        private void MoveCurrentPiece(Vector2Int direction)
        {
            Vector2Int pos = _currentPiecePosition + direction;
            if (IsValidPiece(pos, _currentPieceRotationIndex) == false) return;
            HideCurrentPiece();
            _currentPiecePosition = pos;
            ShowCurrentPiece();
        }

        private void RotateCurrentPiece()
        {
            int rot = (_currentPieceRotationIndex + 1) % 4;
            if (IsValidPiece(_currentPiecePosition, rot) == false) return;
            HideCurrentPiece();
            _currentPieceRotationIndex = rot;
            ShowCurrentPiece();
        }

        private void ShowCurrentPiece()
        {
            Vector2Int[] tetrominoCells = Tetrominoes.GetCells(_currentTetrominoIndex, _currentPieceRotationIndex);
            Color color = Tetrominoes.GetColor(_currentTetrominoIndex);
            foreach (var c in tetrominoCells)
            {
                _cells[_currentPiecePosition.y + c.y, _currentPiecePosition.x + c.x].Show(color);
            }
        }

        private void HideCurrentPiece()
        {
            Vector2Int[] tetrominoCells = Tetrominoes.GetCells(_currentTetrominoIndex, _currentPieceRotationIndex);
            foreach (var c in tetrominoCells)
            {
                _cells[_currentPiecePosition.y + c.y, _currentPiecePosition.x + c.x].Hide();
            }
        }

        private bool IsValidPiece(Vector2Int pos, int rot)
        {
            Vector2Int[] tetrominoCells = Tetrominoes.GetCells(_currentTetrominoIndex, _currentPieceRotationIndex);
            foreach (var c in tetrominoCells)
            {
                if (IsValidPosition(pos + c) == false)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsValidPosition(Vector2Int pos)
        {
            if (pos.x < 0 || SIZE.x <= pos.x) return false;
            if (pos.y < 0 || SIZE.y <= pos.y) return false;
            if (_cellData[pos.y, pos.x] > 0) return false;
            return true;
        }
    }
}