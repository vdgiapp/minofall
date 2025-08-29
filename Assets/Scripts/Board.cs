using UnityEngine;
using URandom = UnityEngine.Random;

namespace Minofall
{
    public class Board : MonoBehaviour
    {
        public static readonly Vector2Int SIZE = new(10, 20);
        public static readonly Vector2Int SPAWN_POSITION = new(3, 17);
        public const float DROP_TIME = 0.8f;

        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private Transform _cellContainerTransform;

        private readonly CellData[,] _cellData = new CellData[SIZE.y, SIZE.x];
        private PieceData _currentPiece = new();

        private float _pieceDropTimer = 0.0f;
        private int _topRowExclusive = 0;

        private void Awake()
        {
            for (int r = 0; r < SIZE.y; r++)
            {
                for (int c = 0; c < SIZE.x; c++)
                {
                    Cell cell = Instantiate(_cellPrefab, _cellContainerTransform);
                    cell.transform.position = new(c, r, 0.0f);
                    cell.Hide();
                    CellData data = new()
                    {
                        Cell = cell,
                        Color = Color.clear,
                        Occupied = false
                    };
                    _cellData[r, c] = data;
                }
            }
            SpawnPiece();
        }

        private void Update()
        {
            PieceDropTimer();

            // TODO: Just need to connect event in Awake (remove later)
            if (Input.GetKeyDown(KeyCode.A))
            {
                MovePiece(Vector2Int.left);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                MovePiece(Vector2Int.right);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                // TODO: soft drop
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                RotatePiece(true); // right
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                RotatePiece(false); // left
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                // TODO: hard drop
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                // TODO: hold
            }
        }

        private void SpawnPiece()
        {
            int t_idx = URandom.Range(0, Tetrominoes.Count);
            _currentPiece = new()
            {
                TetrominoIndex = t_idx,
                Position = SPAWN_POSITION,
                RotationIndex = 0,
                Color = Tetrominoes.GetColor(t_idx)
            };
            _pieceDropTimer = 0.0f;
            ShowPiece();
        }

        private void MovePiece(Vector2Int direction)
        {
            Vector2Int pos = _currentPiece.Position + direction;
            if (!IsValidPiece(pos, _currentPiece.RotationIndex)) return;
            HidePiece();
            _currentPiece.Position = pos;
            ShowPiece();
        }

        private void RotatePiece(bool right)
        {
            int rot = (_currentPiece.RotationIndex + (right ? 1 : -1) + Tetrominoes.MaxSize) % Tetrominoes.MaxSize;
            if (!IsValidPiece(_currentPiece.Position, rot)) return;
            HidePiece();
            _currentPiece.RotationIndex = rot;
            ShowPiece();
        }

        private bool DropPiece()
        {
            Vector2Int pos = _currentPiece.Position + Vector2Int.down;
            if (!IsValidPiece(pos, _currentPiece.RotationIndex)) return false;
            HidePiece();
            _currentPiece.Position = pos;
            ShowPiece();
            return true;
        }

        private void PlacePiece()
        {
            Vector2Int[] tetrominoCells = Tetrominoes.GetCells(_currentPiece.TetrominoIndex, _currentPiece.RotationIndex);
            foreach (Vector2Int c in tetrominoCells)
            {
                Vector2Int lockPos = _currentPiece.Position + c;
                CellData data = _cellData[lockPos.y, lockPos.x];
                data.Occupied = true;
                data.Color = _currentPiece.Color;
                data.Cell.Show(_currentPiece.Color);
            }
            _topRowExclusive = Mathf.Min(SIZE.y, Mathf.Max(_topRowExclusive, _currentPiece.Position.y + Tetrominoes.MaxSize));
            ClearFullRows();
            SpawnPiece();
        }

        private void ShowPiece() => TogglePiece(true);
        private void HidePiece() => TogglePiece(false);
        private void TogglePiece(bool show)
        {
            Vector2Int[] tetrominoCells = Tetrominoes.GetCells(_currentPiece.TetrominoIndex, _currentPiece.RotationIndex);
            foreach (Vector2Int c in tetrominoCells)
            {
                int pos_y = _currentPiece.Position.y + c.y;
                int pos_x = _currentPiece.Position.x + c.x;
                CellData data = _cellData[pos_y, pos_x];
                if (show) data.Cell.Show(_currentPiece.Color);
                else data.Cell.Hide();
            }
        }

        private bool IsValidPiece(Vector2Int pos, int rot)
        {
            Vector2Int[] tetrominoCells = Tetrominoes.GetCells(_currentPiece.TetrominoIndex, rot);
            foreach (Vector2Int c in tetrominoCells) if (!IsValidPosition(pos + c)) return false;
            return true;
        }

        private bool IsValidPosition(Vector2Int pos)
        {
            if (pos.x < 0 || SIZE.x <= pos.x) return false;
            if (pos.y < 0 || SIZE.y <= pos.y) return false;
            CellData data = _cellData[pos.y, pos.x];
            if (data.Occupied) return false;
            return true;
        }

        private void ClearFullRows()
        {
            int writeRow = 0; // row mới sẽ ghi vào
            for (int readRow = 0; readRow < _topRowExclusive; readRow++)
            {
                if (!IsRowFull(readRow))
                {
                    if (writeRow != readRow)
                    {
                        CopyRow(readRow, writeRow);
                    }
                    writeRow++;
                }
            }

            // clear các row còn lại
            for (int r = writeRow; r < _topRowExclusive; r++)
            {
                ClearRow(r);
            }

            _topRowExclusive = writeRow;
        }

        private bool IsRowFull(int r)
        {
            for (int c = 0; c < SIZE.x; c++)
            {
                if (!_cellData[r, c].Occupied) return false;
            }
            return true;
        }

        private void CopyRow(int from, int to)
        {
            for (int c = 0; c < SIZE.x; c++)
            {
                CellData src = _cellData[from, c];
                CellData dst = _cellData[to, c];

                dst.Occupied = src.Occupied;
                dst.Color = src.Color;
                if (dst.Occupied) dst.Cell.Show(dst.Color);
                else dst.Cell.Hide();
            }
        }

        private void ClearRow(int r)
        {
            for (int c = 0; c < SIZE.x; c++)
            {
                CellData data = _cellData[r, c];
                data.Occupied = false;
                data.Color = Color.clear;
                data.Cell.Hide();
            }
        }

        private void PieceDropTimer()
        {
            _pieceDropTimer += Time.deltaTime;
            if (_pieceDropTimer >= DROP_TIME)
            {
                _pieceDropTimer -= DROP_TIME;
                if (!DropPiece())
                {
                    PlacePiece();
                    return;
                }
            }
        }

        // GIZMOS
        private void OnDrawGizmos()
        {
            var color = Gizmos.color;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new(-1f, _topRowExclusive, 0f), new(11f, _topRowExclusive, 0f));
            Gizmos.color = color;
        }
    }
}