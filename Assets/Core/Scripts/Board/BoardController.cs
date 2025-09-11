using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Minofall
{
    public class BoardController : MonoBehaviour
    {
        public static readonly Vector2Int BOARD_SIZE = new(10, 20);
        public static readonly Vector2Int DEFAULT_SPAWN_POSITION = new(3, 17);

        [SerializeField] private CellDisplay _cellDisplayPrefab;
        [SerializeField] private Transform _cellDisplayContainerTransform;
        [SerializeField] private MainGameUIController _uiController;

        private int _level = 0;
        private int _linesCleared = 0;
        private int _score = 0;
        private int _bestScore = 0; // Get from PlayerPrefs

        // Main logic
        private Cell[,] _cells;
        private Piece _currentPiece = new();
        private Vector2Int _ghostPosition = new(0, 0);
        private PieceGenerator _pieceGenerator = new();
        private int _holdingPiece = -1;

        // Highest occupied row (for clearing rows)
        private int _highestOccupiedRow = 0;

        // Timer
        private float _dropTime = 0.8f;
        private float _dropTimer = 0.0f;

        private InputManager _inputManager => InputManager.Instance;

        private void Awake()
        {
            _cells = new Cell[BOARD_SIZE.y, BOARD_SIZE.x];
            _currentPiece = new();
            BoardInit();
            _pieceGenerator.FillBag();
            _pieceGenerator.FillQueue();
        }

        private void Start()
        {
            ConnectEvents();
            SpawnPieceFromQueue();
        }

        private void OnEnable()
        {
            _inputManager.EnableKeyboardActions();
        }

        private void Update()
        {
            PieceDropTimer();
        }

        private void OnDisable()
        {
            _inputManager.DisableKeyboardActions();
        }

        private void OnDestroy()
        {
            DisconnectEvents();
        }

        private void ConnectEvents()
        {
            _inputManager.OnMoveLeft += MovePieceLeft;
            _inputManager.OnMoveRight += MovePieceRight;
            _inputManager.OnSoftDrop += SoftDropPiece;
            _inputManager.OnRotateLeft += RotatePieceLeft;
            _inputManager.OnRotateRight += RotatePieceRight;
            _inputManager.OnHardDrop += HardDropPiece;
        }

        private void DisconnectEvents()
        {
            _inputManager.OnMoveLeft -= MovePieceLeft;
            _inputManager.OnMoveRight -= MovePieceRight;
            _inputManager.OnSoftDrop -= SoftDropPiece;
            _inputManager.OnRotateLeft -= RotatePieceLeft;
            _inputManager.OnRotateRight -= RotatePieceRight;
            _inputManager.OnHardDrop -= HardDropPiece;
        }

        private void BoardInit()
        {
            for (int r = 0; r < BOARD_SIZE.y; r++)
            {
                for (int c = 0; c < BOARD_SIZE.x; c++)
                {
                    CellDisplay display = Instantiate(_cellDisplayPrefab, _cellDisplayContainerTransform);
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
            _dropTimer = 0.0f;

            // Game Over check
            if (!IsValidPiece(_currentPiece.position, _currentPiece.rotationIndex))
            {
                Debug.Log("Game Over");
                enabled = false;
                return;
            }
            UpdateNextPieceSprites();
            ShowGhost();
            ShowPiece();
        }

        private void MovePieceLeft() => MovePiece(Vector2Int.left);
        private void MovePieceRight() => MovePiece(Vector2Int.right);
        private void MovePiece(Vector2Int direction)
        {
            Vector2Int pos = _currentPiece.position + direction;
            if (!IsValidPiece(pos, _currentPiece.rotationIndex)) return;
            HidePiece();
            HideGhost();
            _currentPiece.position = pos;
            ShowGhost();
            ShowPiece();
        }

        private void RotatePieceLeft() => RotatePiece(-1);
        private void RotatePieceRight() => RotatePiece(1);
        private void RotatePiece(int rotateDirection)
        {
            int rot = Utils.WrapValue(_currentPiece.rotationIndex + rotateDirection, 0, Tetrominoes.MaxRotations);
            if (!IsValidPiece(_currentPiece.position, rot)) return;
            HidePiece();
            HideGhost();
            _currentPiece.rotationIndex = rot;
            ShowGhost();
            ShowPiece();
        }

        private bool DropPiece()
        {
            Vector2Int pos = _currentPiece.position + Vector2Int.down;
            if (!IsValidPiece(pos, _currentPiece.rotationIndex)) return false;
            HidePiece();
            HideGhost();
            _currentPiece.position = pos;
            ShowGhost();
            ShowPiece();
            return true;
        }

        private void SoftDropPiece()
        {
            DropPiece();
            _dropTimer = 0.0f;
        }

        private void HardDropPiece()
        {
            while (DropPiece()) continue;
            LockPiece();
        }

        private void LockPiece()
        {
            Vector2Int[] tetrominoCells = Tetrominoes.GetCells(_currentPiece.tetrominoIndex, _currentPiece.rotationIndex);
            foreach (Vector2Int c in tetrominoCells)
            {
                Vector2Int lockPos = _currentPiece.position + c;
                Cell data = _cells[lockPos.y, lockPos.x];
                data.occupied = true;
                data.color = _currentPiece.color;
                data.cellDisplay.Show(_currentPiece.color);
            }
            _highestOccupiedRow = Mathf.Min(BOARD_SIZE.y, Mathf.Max(_highestOccupiedRow, _currentPiece.position.y + 4));
            ClearFullRows();
            SpawnPieceFromQueue();
        }

        private void ShowPiece() => TogglePiece(true);
        private void HidePiece() => TogglePiece(false);
        private void TogglePiece(bool show)
        {
            Vector2Int[] tetrominoCells = Tetrominoes.GetCells(_currentPiece.tetrominoIndex, _currentPiece.rotationIndex);
            foreach (Vector2Int c in tetrominoCells)
            {
                int pos_y = _currentPiece.position.y + c.y;
                int pos_x = _currentPiece.position.x + c.x;
                Cell data = _cells[pos_y, pos_x];
                if (show) data.cellDisplay.Show(_currentPiece.color);
                else data.cellDisplay.Hide();
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
                Vector2Int[] tetrominoCells = Tetrominoes.GetCells(_currentPiece.tetrominoIndex, _currentPiece.rotationIndex);
                //Color ghostColor = Tetrominoes.GetColor(_currentPiece.tetrominoIndex);
                foreach (Vector2Int c in tetrominoCells)
                {
                    int pos_y = _ghostPosition.y + c.y;
                    int pos_x = _ghostPosition.x + c.x;
                    Cell data = _cells[pos_y, pos_x];
                    if (show) data.cellDisplay.Ghost(Color.white);//ghostColor);
                    else data.cellDisplay.Hide();
                }
            }
        }

        private bool IsValidPiece(Vector2Int pos, int rot)
        {
            Vector2Int[] tetrominoCells = Tetrominoes.GetCells(_currentPiece.tetrominoIndex, rot);
            foreach (Vector2Int c in tetrominoCells) if (!IsValidPosition(pos + c)) return false;
            return true;
        }

        private bool IsValidPosition(Vector2Int pos)
        {
            if (pos.x < 0 || BOARD_SIZE.x <= pos.x) return false;
            if (pos.y < 0 || BOARD_SIZE.y <= pos.y) return false;
            Cell data = _cells[pos.y, pos.x];
            if (data.occupied) return false;
            return true;
        }

        private void ClearFullRows()
        {
            int writeRow = 0;
            for (int readRow = 0; readRow < _highestOccupiedRow; readRow++)
            {
                if (!IsRowFull(readRow))
                {
                    if (writeRow != readRow) CopyRow(readRow, writeRow);
                    writeRow++;
                }
            }
            for (int r = writeRow; r < _highestOccupiedRow; r++) ClearRow(r);
            _highestOccupiedRow = writeRow;
        }

        private bool IsRowFull(int r)
        {
            for (int c = 0; c < BOARD_SIZE.x; c++) if (!_cells[r, c].occupied) return false;
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

        private void ClearRow(int r)
        {
            for (int c = 0; c < BOARD_SIZE.x; c++)
            {
                Cell data = _cells[r, c];
                data.occupied = false;
                data.color = Color.clear;
                data.cellDisplay.Hide();
            }
        }

        private void PieceDropTimer()
        {
            _dropTimer += Time.deltaTime;
            if (_dropTimer >= _dropTime)
            {
                _dropTimer -= _dropTime;
                if (!DropPiece())
                {
                    LockPiece();
                    return;
                }
            }
        }

        private void UpdateNextPieceSprites()
        {
            for (int i = 0; i < PieceGenerator.PREVIEW_SIZE; i++)
            {
                _uiController.SetNextPieceSprite(i, _pieceGenerator.PeekQueue()[i]);
                _uiController.SetNextPieceColor(i, Tetrominoes.GetColor(_pieceGenerator.PeekQueue()[i]));
            }
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