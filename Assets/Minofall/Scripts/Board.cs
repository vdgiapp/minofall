using NaughtyAttributes;
using UnityEngine;

namespace Minofall
{
    public class Board : MonoBehaviour
    {
        //
        [SerializeField]
        private CellDisplay _cellDisplayPrefab;

        [SerializeField]
        private Transform _cellDisplayContainerTransform;

        [ShowNonSerializedField]
        private Vector2Int _boardSize = new(10, 20);

        [ShowNonSerializedField]
        private Vector2Int _spawnPosition = new(3, 17);
        
        [SerializeField]
        private float _dropTime = 0.8f;

        private Cell[,] _cells;
        private Piece _currentPiece = new();
        private Vector2Int _ghostPosition = new(0, 0);

        private float _pieceDropTimer = 0.0f;
        private int _topRowExclusive = 0;

        private void Awake()
        {
            _cells = new Cell[_boardSize.y, _boardSize.x];
            _currentPiece = new();
            BoardInit();
        }

        private void Start()
        {
            ConnectEvents();
            SpawnPiece();
        }

        private void OnEnable()
        {
            InputManager.Instance.EnableIngameActions();
        }

        private void Update()
        {
            PieceDropTimer();
        }

        private void OnDisable()
        {
            InputManager.Instance.DisableIngameActions();
        }

        private void OnDestroy()
        {
            DisconnectEvents();
        }

        private void ConnectEvents()
        {
            InputManager.Instance.OnMoveLeft += MovePieceLeft;
            InputManager.Instance.OnMoveRight += MovePieceRight;
            InputManager.Instance.OnSoftDrop += SoftDropPiece;
            InputManager.Instance.OnRotateLeft += RotatePieceLeft;
            InputManager.Instance.OnRotateRight += RotatePieceRight;
            InputManager.Instance.OnHardDrop += HardDropPiece;
        }

        private void DisconnectEvents()
        {
            InputManager.Instance.OnMoveLeft -= MovePieceLeft;
            InputManager.Instance.OnMoveRight -= MovePieceRight;
            InputManager.Instance.OnSoftDrop -= SoftDropPiece;
            InputManager.Instance.OnRotateLeft -= RotatePieceLeft;
            InputManager.Instance.OnRotateRight -= RotatePieceRight;
            InputManager.Instance.OnHardDrop -= HardDropPiece;
        }

        private void BoardInit()
        {
            for (int r = 0; r < _boardSize.y; r++)
            {
                for (int c = 0; c < _boardSize.x; c++)
                {
                    CellDisplay display = Instantiate(_cellDisplayPrefab, _cellDisplayContainerTransform);
                    display.transform.position = new(c, r);
                    display.Hide();
                    _cells[r, c] = new Cell(display, Color.clear, false);
                }
            }
        }

        private void SpawnPiece()
        {
            int idx = Random.Range(0, Tetrominoes.Length);
            _currentPiece.Initialize(idx, 0, _spawnPosition, Tetrominoes.GetColor(idx));
            _pieceDropTimer = 0.0f;
            if (!IsValidPiece(_currentPiece.position, _currentPiece.rotationIndex))
            {
                Debug.Log("Game Over");
                enabled = false;
                return;
            }
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
            _topRowExclusive = Mathf.Min(_boardSize.y, Mathf.Max(_topRowExclusive, _currentPiece.position.y + 4));
            ClearFullRows();
            SpawnPiece();
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
                Color ghostColor = Tetrominoes.GetColor(_currentPiece.tetrominoIndex);
                foreach (Vector2Int c in tetrominoCells)
                {
                    int pos_y = _ghostPosition.y + c.y;
                    int pos_x = _ghostPosition.x + c.x;
                    Cell data = _cells[pos_y, pos_x];
                    if (show) data.cellDisplay.Ghost(ghostColor);
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
            if (pos.x < 0 || _boardSize.x <= pos.x) return false;
            if (pos.y < 0 || _boardSize.y <= pos.y) return false;
            Cell data = _cells[pos.y, pos.x];
            if (data.occupied) return false;
            return true;
        }

        private void ClearFullRows()
        {
            int writeRow = 0;
            for (int readRow = 0; readRow < _topRowExclusive; readRow++)
            {
                if (!IsRowFull(readRow))
                {
                    if (writeRow != readRow) CopyRow(readRow, writeRow);
                    writeRow++;
                }
            }
            for (int r = writeRow; r < _topRowExclusive; r++) ClearRow(r);
            _topRowExclusive = writeRow;
        }

        private bool IsRowFull(int r)
        {
            for (int c = 0; c < _boardSize.x; c++) if (!_cells[r, c].occupied) return false;
            return true;
        }

        private void CopyRow(int from, int to)
        {
            for (int c = 0; c < _boardSize.x; c++)
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
            for (int c = 0; c < _boardSize.x; c++)
            {
                Cell data = _cells[r, c];
                data.occupied = false;
                data.color = Color.clear;
                data.cellDisplay.Hide();
            }
        }

        private void PieceDropTimer()
        {
            _pieceDropTimer += Time.deltaTime;
            if (_pieceDropTimer >= _dropTime)
            {
                _pieceDropTimer -= _dropTime;
                if (!DropPiece())
                {
                    LockPiece();
                    return;
                }
            }
        }

        // GIZMOS
        private void OnDrawGizmos()
        {
            var color = Gizmos.color;

            // Top row exclusive
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new(-1f, _topRowExclusive, 0f), new(11f, _topRowExclusive, 0f));

            // Spawn position
            var spawnPosTopLeft = _spawnPosition + Vector2.up * 4;
            var spawnPosTopRight = spawnPosTopLeft + Vector2.right * 4;
            var spawnPosBotRight = _spawnPosition + Vector2.right * 4;
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine((Vector3Int)_spawnPosition, spawnPosTopLeft);
            Gizmos.DrawLine(spawnPosTopLeft, spawnPosTopRight);
            Gizmos.DrawLine((Vector3Int)_spawnPosition, spawnPosBotRight);
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