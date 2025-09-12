using System;
using UnityEngine;

namespace Minofall
{
    public class BoardController : MonoBehaviour
    {
        // Constants
        public static readonly Vector2Int BOARD_SIZE = new(10, 20);
        public static readonly Vector2Int DEFAULT_SPAWN_POSITION = new(3, 17);

        // References
        [SerializeField] private CellDisplay _displayPrefab;
        [SerializeField] private Transform _container;

        // Events
        public event Action OnGameOver;
        public event Action<int> OnLinesCleared;
        public event Action<int> OnHoldPieceChanged;
        public event Action<int[]> OnNextPiecesChanged;

        // Main data / logic
        private Cell[,] _cells;
        private Piece _currentPiece = new();
        private Vector2Int _ghostPosition = new(0, 0);
        private readonly PieceGenerator _pieceGenerator = new();
        private int _holdingPiece = -1;

        private bool _hasHeldThisTurn = false;
        private int _highestOccupiedRow = 0;
        private float _dropTime = TetrisGravity.GetDropTime(1);
        private float _dropTimer = 0.0f;

        private void Awake()
        {
            InitializeBoard();

            // Initialize piece generator
            _pieceGenerator.FillBag();
            _pieceGenerator.FillQueue();
        }

        private void Start()
        {
            SpawnPieceFromQueue();
            RaiseNextPiecesChanged();
        }

        private void Update()
        {
            HandlePieceDropTimer();
        }

        private void InitializeBoard()
        {
            _cells = new Cell[BOARD_SIZE.y, BOARD_SIZE.x];
            for (int r = 0; r < BOARD_SIZE.y; r++)
            {
                for (int c = 0; c < BOARD_SIZE.x; c++)
                {
                    CellDisplay display = Instantiate(_displayPrefab, _container);
                    display.transform.position = new(c, r);
                    display.Hide();
                    _cells[r, c] = new Cell(display, Color.clear, false);
                }
            }
        }

        private void SpawnPieceFromQueue()
        {
            int idx = _pieceGenerator.GetNextPiece();
            _currentPiece.Initialize(idx, 0, DEFAULT_SPAWN_POSITION, Tetrominoes.GetColor(idx));
            _dropTimer = 0f;
            // Check for game over
            if (!IsValidPiece(_currentPiece.position, _currentPiece.rotationIndex))
            {
                OnGameOver?.Invoke();
                return;
            }
            ShowGhost();
            ShowPiece();
        }

        private void LockPiece()
        {
            Vector2Int[] tetrominoCells = Tetrominoes.GetCells(_currentPiece.tetrominoIndex, _currentPiece.rotationIndex);
            foreach (Vector2Int c in tetrominoCells)
            {
                Vector2Int lockPos = _currentPiece.position + c;
                if (IsWithinBounds(lockPos))
                {
                    Cell data = _cells[lockPos.y, lockPos.x];
                    data.occupied = true;
                    data.color = _currentPiece.color;
                    data.cellDisplay.Show(_currentPiece.color);
                }
            }
            _highestOccupiedRow = Mathf.Min(BOARD_SIZE.y, Mathf.Max(_highestOccupiedRow, _currentPiece.position.y + 4));
            _hasHeldThisTurn = false;
            ClearFullRows();
            SpawnPieceFromQueue();
            RaiseNextPiecesChanged();
        }

        private void ShowPiece() => TogglePiece(true);
        private void HidePiece() => TogglePiece(false);
        private void TogglePiece(bool show)
        {
            Vector2Int[] tetrominoCells = Tetrominoes.GetCells(_currentPiece.tetrominoIndex, _currentPiece.rotationIndex);
            foreach (Vector2Int c in tetrominoCells)
            {
                Vector2Int pos = _currentPiece.position + c;
                if (IsWithinBounds(pos))
                {
                    Cell cell = _cells[pos.y, pos.x];
                    if (show) cell.cellDisplay.Show(_currentPiece.color);
                    else cell.cellDisplay.Hide();
                }
            }
        }

        private void ShowGhost() => ToggleGhost(true);
        private void HideGhost() => ToggleGhost(false);
        private void ToggleGhost(bool show)
        {
            _ghostPosition = _currentPiece.position;
            while (IsValidPiece(_ghostPosition + Vector2Int.down, _currentPiece.rotationIndex))
            {
                _ghostPosition += Vector2Int.down;
            }
            if (_ghostPosition != _currentPiece.position)
            {
                Vector2Int[] cells = Tetrominoes.GetCells(_currentPiece.tetrominoIndex, _currentPiece.rotationIndex);
                foreach (Vector2Int offset in cells)
                {
                    Vector2Int pos = _ghostPosition + offset;
                    if (IsWithinBounds(pos))
                    {
                        Cell cell = _cells[pos.y, pos.x];
                        if (show) cell.cellDisplay.Ghost(Color.white);
                        else cell.cellDisplay.Hide();
                    }
                }
            }
        }

        private bool IsValidPiece(Vector2Int pos, int rot)
        {
            Vector2Int[] cells = Tetrominoes.GetCells(_currentPiece.tetrominoIndex, rot);
            foreach (var offset in cells)
            {
                Vector2Int checkPos = pos + offset;
                if (!IsValidPosition(checkPos)) return false;
            }
            return true;
        }

        private bool IsValidPosition(Vector2Int pos)
        {
            if (!IsWithinBounds(pos)) return false;
            if (_cells[pos.y, pos.x].occupied) return false;
            return true;
        }

        private bool IsWithinBounds(Vector2Int pos)
        {
            return pos.x >= 0 && pos.x < BOARD_SIZE.x && pos.y >= 0 && pos.y < BOARD_SIZE.y;
        }

        private void ClearFullRows()
        {
            int linesCleared = 0;
            int writeRow = 0;
            for (int readRow = 0; readRow < _highestOccupiedRow; readRow++)
            {
                if (!IsRowFull(readRow))
                {
                    if (writeRow != readRow) CopyRow(readRow, writeRow);
                    writeRow++;
                }
                else linesCleared++;
            }
            for (int r = writeRow; r < _highestOccupiedRow; r++) ClearRow(r);
            _highestOccupiedRow = writeRow;
            if (linesCleared > 0) OnLinesCleared?.Invoke(linesCleared);
        }

        private bool IsRowFull(int row)
        {
            for (int c = 0; c < BOARD_SIZE.x; c++)
            {
                if (!_cells[row, c].occupied) return false;
            }
            return true;
        }

        private void CopyRow(int from, int to)
        {
            for (int c = 0; c < BOARD_SIZE.x; c++)
            {
                Cell src = _cells[from, c];
                Cell dst = _cells[to, c];
                dst.occupied = src.occupied;
                dst.color = src.color;
                if (dst.occupied) dst.cellDisplay.Show(dst.color);
                else dst.cellDisplay.Hide();
            }
        }

        private void ClearRow(int row)
        {
            for (int c = 0; c < BOARD_SIZE.x; c++)
            {
                Cell cell = _cells[row, c];
                cell.occupied = false;
                cell.color = Color.clear;
                cell.cellDisplay.Hide();
            }
        }

        private void HandlePieceDropTimer()
        {
            _dropTimer += Time.deltaTime;
            if (_dropTimer >= _dropTime)
            {
                _dropTimer -= _dropTime;
                if (!MovePiece(Vector2Int.down))
                {
                    LockPiece();
                }
            }
        }

        private void RaiseNextPiecesChanged() => OnNextPiecesChanged?.Invoke(_pieceGenerator.PeekQueue());
        private void RaiseHoldPieceChanged() => OnHoldPieceChanged?.Invoke(_holdingPiece);

        public bool HoldPiece()
        {
            if (_hasHeldThisTurn) return false;
            HidePiece();
            HideGhost();

            if (_holdingPiece == -1)
            {
                _holdingPiece = _currentPiece.tetrominoIndex;
                SpawnPieceFromQueue();
            }
            else
            {
                // Swap between hold and current
                int temp = _currentPiece.tetrominoIndex;
                _currentPiece.Initialize(_holdingPiece, 0, DEFAULT_SPAWN_POSITION, Tetrominoes.GetColor(_holdingPiece));
                _holdingPiece = temp;
                if (!IsValidPiece(_currentPiece.position, _currentPiece.rotationIndex))
                {
                    OnGameOver?.Invoke();
                    return false;
                }
                ShowGhost();
                ShowPiece();
            }
            _hasHeldThisTurn = true;
            RaiseHoldPieceChanged();
            return true;
        }

        public bool MovePiece(Vector2Int direction)
        {
            Vector2Int pos = _currentPiece.position + direction;
            if (!IsValidPiece(pos, _currentPiece.rotationIndex)) return false;
            HidePiece();
            HideGhost();
            _currentPiece.position = pos;
            ShowGhost();
            ShowPiece();
            return true;
        }

        public bool RotatePiece(int rotateDirection)
        {
            int rot = Utils.WrapValue(_currentPiece.rotationIndex + rotateDirection, 0, Tetrominoes.MaxRotations);
            if (!IsValidPiece(_currentPiece.position, rot)) return false;
            HidePiece();
            HideGhost();
            _currentPiece.rotationIndex = rot;
            ShowGhost();
            ShowPiece();
            return true;
        }

        public bool SoftDropPiece()
        {
            bool moved = MovePiece(Vector2Int.down);
            if (moved) _dropTimer = 0f;
            return moved;
        }

        public void HardDropPiece()
        {
            while (MovePiece(Vector2Int.down)) continue;
            LockPiece();
        }

        public void SetDropTime(float dropTime)
        {
            _dropTime = dropTime;
        }

        // GIZMOS
        private void OnDrawGizmos()
        {
            var color = Gizmos.color;

            // Top row exclusive
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new(-1f, _highestOccupiedRow, 0f), new(11f, _highestOccupiedRow, 0f));

            // Spawn position
            var spawnPosTopLeft = DEFAULT_SPAWN_POSITION + Vector2.up * 4;
            var spawnPosTopRight = spawnPosTopLeft + Vector2.right * 4;
            var spawnPosBotRight = DEFAULT_SPAWN_POSITION + Vector2.right * 4;
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine((Vector3Int)DEFAULT_SPAWN_POSITION, spawnPosTopLeft);
            Gizmos.DrawLine(spawnPosTopLeft, spawnPosTopRight);
            Gizmos.DrawLine((Vector3Int)DEFAULT_SPAWN_POSITION, spawnPosBotRight);
            Gizmos.DrawLine(spawnPosBotRight, spawnPosTopRight);

            // Piece position
            var piecePosTopLeft = _currentPiece.position + Vector2.up * 4;
            var piecePosTopRight = piecePosTopLeft + Vector2.right * 4;
            var piecePosBotRight = _currentPiece.position + Vector2.right * 4;
            Gizmos.color = Color.white;
            Gizmos.DrawLine((Vector3Int)_currentPiece.position, piecePosTopLeft);
            Gizmos.DrawLine(piecePosTopLeft, piecePosTopRight);
            Gizmos.DrawLine((Vector3Int)_currentPiece.position, piecePosBotRight);
            Gizmos.DrawLine(piecePosBotRight, piecePosTopRight);

            Gizmos.color = color;
        }
    }
}