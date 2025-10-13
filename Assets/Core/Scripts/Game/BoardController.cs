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
        public event Action OnPieceLocked;
        public event Action<int, bool> OnLinesCleared;
        public event Action<int> OnHoldPieceChanged;
        public event Action<int[]> OnNextPiecesChanged;
        public event Action OnPieceSoftDrop;
        public event Action<int> OnPieceHardDrop;

        // Main data / logic
        private Cell[,] _cells = new Cell[BOARD_SIZE.y, BOARD_SIZE.x];
        private CellDisplay[,] _cellDisplays = new CellDisplay[BOARD_SIZE.y, BOARD_SIZE.x];
        private Piece _currentPiece = new();
        private Vector2Int _ghostPosition = Vector2Int.zero;
        private PieceGenerator _pieceGenerator = new();
        private int _holdingPiece;
        private bool _hasHeldThisTurn;
        private int _highestOccupiedRow;
        private float _dropTime = TetrisGravity.GetDropTime(1);
        private float _dropTimer;

        public Cell[,] GetCells() => _cells;
        public BoardController WithCells(Cell[,] cells)
        {
            _cells = cells;
            _cellDisplays = new CellDisplay[BOARD_SIZE.y, BOARD_SIZE.x];
            for (int r = 0; r < BOARD_SIZE.y; r++)
            {
                for (int c = 0; c < BOARD_SIZE.x; c++)
                {
                    CellDisplay display = Instantiate(_displayPrefab, _container);
                    display.transform.position = new(c, r);
                    display.Hide();
                    _cellDisplays[r, c] = display;

                    Cell cell = _cells[r, c];
                    if (cell.occupied) _cellDisplays[r, c].Show(cell.color);
                    else _cellDisplays[r, c].Hide();
                }
            }
            return this;
        }

        public Piece GetPiece() => _currentPiece;
        public BoardController WithPiece(Piece piece)
        {
            _currentPiece = piece;
            return this;
        }

        public Vector2Int GetGhostPosition() => _ghostPosition;
        public BoardController WithGhostPosition(Vector2Int pos)
        {
            _ghostPosition = pos;
            return this;
        }

        public PieceGenerator GetPieceGenerator() => _pieceGenerator;
        public BoardController WithPieceGenerator(PieceGenerator pieceGenerator)
        {
            _pieceGenerator = pieceGenerator;
            return this;
        }

        public int GetHoldingPiece() => _holdingPiece;
        public BoardController WithHoldingPiece(int holdingPiece)
        {
            _holdingPiece = holdingPiece;
            return this;
        }

        public bool GetHeldThisTurn() => _hasHeldThisTurn;
        public BoardController WithHeldThisTurn(bool heldThisTurn)
        {
            _hasHeldThisTurn = heldThisTurn;
            return this;
        }

        public int GetHighestRow() => _highestOccupiedRow;
        public BoardController WithHighestRow(int highestRow)
        {
            _highestOccupiedRow = highestRow;
            return this;
        }

        public float GetDropTimer() => _dropTimer;
        public BoardController WithDropTimer(float dropTimer)
        {
            _dropTimer = dropTimer;
            return this;
        }

        public void PauseGame() => enabled = false;
        public void ResumeGame() => enabled = true;

        public void InitGame(bool isNewGame)
        {
            if (isNewGame)
            {
                _pieceGenerator.Initialize();
                SpawnPieceFromQueue();
            }
            else
            {
                ShowGhost();
                ShowPiece();
            }
            OnHoldPieceChanged?.Invoke(_holdingPiece);
            OnNextPiecesChanged?.Invoke(_pieceGenerator.PeekQueue());
        }

        private void Update()
        {
            // Handle automatic piece dropping
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
                    CellDisplay displayData = _cellDisplays[lockPos.y, lockPos.x];
                    displayData.Show(_currentPiece.color);
                }
            }
            OnPieceLocked?.Invoke();
            _highestOccupiedRow = Mathf.Min(BOARD_SIZE.y, Mathf.Max(_highestOccupiedRow, _currentPiece.position.y + 4));
            _hasHeldThisTurn = false;
            ClearFullRows();
            SpawnPieceFromQueue();
            OnNextPiecesChanged?.Invoke(_pieceGenerator.PeekQueue());
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
                    CellDisplay cellDisplay = _cellDisplays[pos.y, pos.x];
                    if (show) cellDisplay.Show(_currentPiece.color);
                    else cellDisplay.Hide();
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
                        CellDisplay cellDisplay = _cellDisplays[pos.y, pos.x];
                        if (show) cellDisplay.Ghost(Color.white);
                        else cellDisplay.Hide();
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
            bool isTSpin = DetectTSpin();
            if (linesCleared > 0) OnLinesCleared?.Invoke(linesCleared, isTSpin);
            else if (isTSpin) OnLinesCleared?.Invoke(0, isTSpin);
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
                CellDisplay cellDisplay = _cellDisplays[to, c];
                if (dst.occupied) cellDisplay.Show(dst.color);
                else cellDisplay.Hide();
            }
        }

        private void ClearRow(int row)
        {
            for (int c = 0; c < BOARD_SIZE.x; c++)
            {
                Cell cell = _cells[row, c];
                cell.occupied = false;
                cell.color = Color.clear;
                CellDisplay cellDisplay = _cellDisplays[row, c];
                cellDisplay.Hide();
            }
        }

        private bool DetectTSpin()
        {
            if (_currentPiece.tetrominoIndex != 5) return false;

            Vector2Int center = _currentPiece.position;

            Vector2Int[] corners =
            {
                new(center.x - 1, center.y - 1), // bottom-left
                new(center.x + 1, center.y - 1), // bottom-right
                new(center.x - 1, center.y + 1), // top-left
                new(center.x + 1, center.y + 1)  // top-right
            };

            int blockedCount = 0;
            foreach (var c in corners)
            {
                if (!IsWithinBounds(c) || _cells[c.y, c.x].occupied)
                {
                    blockedCount++;
                }
            }

            return blockedCount >= 3;
        }


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
            OnHoldPieceChanged?.Invoke(_holdingPiece);
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
            if (moved)
            {
                _dropTimer = 0f;
                OnPieceSoftDrop?.Invoke();
            }
            return moved;
        }

        public void HardDropPiece()
        {
            int distance = 0;
            while (MovePiece(Vector2Int.down))
            {
                distance++;
                continue;
            }
            LockPiece();
            OnPieceHardDrop?.Invoke(distance);
        }

        public void SetDropTime(float dropTime)
        {
            _dropTime = dropTime;
        }
    }
}