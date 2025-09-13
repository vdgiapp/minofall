using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Minofall
{
    public class GameManager : MonoBehaviour
    {
        // Singleton references
        private InputManager _inputManager => InputManager.Instance;
        private CoreManager _coreManager => CoreManager.Instance;

        // References
        [SerializeField] private BoardController _boardController;
        [SerializeField] private MainGameUIController _uiController;

        // State
        private bool _isGameOver = false;
        private bool _isPaused = false;

        // Stats
        private SessionData _sessionData = new(0, 1, 0);

        private void Awake()
        {
            ConnectEvents();
        }

        private void Start()
        {
            // Pause game for cooldown
            _isPaused = true;
            _boardController.enabled = false;

            // Hide pause button
            _uiController.SetPauseButtonActive(false);

            // Set default texts
            _uiController.SetScoreText("0");
            _uiController.SetLevelText("01");
            _uiController.SetLinesText("Lines: 0");
            // TODO: set best score

            // Disable input
            _inputManager.DisableMainGameActions();

            // Cooldown
            HandleCooldownBeforePlay().Forget();
        }

        private void OnApplicationPause(bool pause)
        {
            OnPauseButtonClick();
        }

        private void OnDestroy()
        {
            DisconnectEvents();
            _coreManager.SetLastSessionData(_sessionData);
        }

        private void OnMoveLeft() => _boardController.MovePiece(Vector2Int.left);
        private void OnMoveRight() => _boardController.MovePiece(Vector2Int.right);
        private void OnRotateLeft() => _boardController.RotatePiece(-1);
        private void OnRotateRight() => _boardController.RotatePiece(1);
        private void OnSoftDrop() => _boardController.SoftDropPiece();
        private void OnHardDrop() => _boardController.HardDropPiece();
        private void OnHold() => _boardController.HoldPiece();

        private async void OnGameOver()
        {
            // Pause game
            _isPaused = true;
            _boardController.enabled = false;

            // Show game over overlay
            _uiController.SetPauseButtonActive(false);
            _uiController.SetGameOverOverlayActive(true);

            // Disable input
            _inputManager.DisableMainGameActions();

            await UniTask.WaitForSeconds(5);
            SceneController.Instance.NewTransition()
                .Load(SceneController.SceneName.GameResult, true)
                .Unload(SceneController.SceneName.MainGame)
                .WithOverlay()
                .WithClearUnusedAssets()
                .Perform();
        }

        private void OnLinesCleared(int lines)
        {
            // Update stats
            _sessionData.linesCleared += lines;
            _sessionData.score += CalculateScore(lines, _sessionData.level);
            _sessionData.level = 1 + _sessionData.linesCleared / 10;

            // Update drop time
            _boardController.SetDropTime(TetrisGravity.GetDropTime(_sessionData.level));

            // Update UI
            _uiController.SetScoreText(Utils.NumberFormat(_sessionData.score));
            _uiController.SetLevelText(_sessionData.level.ToString("D2"));
            _uiController.SetLinesText($"Lines: {_sessionData.linesCleared}");
        }

        private void OnNextPiecesChanged(int[] nextPieces)
        {
            for (int i = 0; i < nextPieces.Length; i++)
            {
                // Update UI for each next piece
                _uiController.SetNextPieceSprite(i, nextPieces[i]);
                _uiController.SetNextPieceColor(i, Tetrominoes.GetColor(nextPieces[i]));
            }
        }

        private void OnHoldPieceChanged(int holdPiece)
        {
            if (holdPiece < 0)
            {
                // No hold piece
                _uiController.SetHoldPieceColor(Color.clear);
            }
            else
            {
                // Update UI for hold piece
                _uiController.SetHoldPieceSprite(holdPiece);
                _uiController.SetHoldPieceColor(Tetrominoes.GetColor(holdPiece));
            }
        }

        // Connected to Pause button in inspector
        public void OnPauseButtonClick()
        {
            // Pause game
            _isPaused = true;
            _boardController.enabled = false;

            // Show pause menu overlay
            _uiController.SetPauseButtonActive(false);
            _uiController.SetMenuOverlayActive(true);

            // Disable input
            _inputManager.DisableMainGameActions();
        }

        // Connected to Resume button in inspector
        public void OnResumeButtonClick()
        {
            HandleCooldownBeforePlay().Forget();
        }

        // Connected to Quit button in inspector
        public void OnQuitButtonClick()
        {
            SceneController.Instance.NewTransition()
                .Load(SceneController.SceneName.MainMenu, true)
                .Unload(SceneController.SceneName.MainGame)
                .WithOverlay()
                .WithClearUnusedAssets()
                .Perform();
        }

        private async UniTaskVoid HandleCooldownBeforePlay()
        {
            // Show cooldown overlay and hide pause menu overlay
            _uiController.SetMenuOverlayActive(false);
            _uiController.SetCooldownOverlayActive(true);

            // Countdown 3, 2, 1
            int countdown = 3;
            while (countdown > 0)
            {
                await _uiController.AnimateCooldownNumber(countdown.ToString(), 1f);
                countdown--;
            }

            // Resume game
            _isPaused = false;
            _boardController.enabled = true;

            // Hide cooldown overlay and show pause button
            _uiController.SetPauseButtonActive(true);
            _uiController.SetCooldownOverlayActive(false);
            _inputManager.EnableMainGameActions();
        }

        private void ConnectEvents()
        {
            _inputManager.OnMoveLeft += OnMoveLeft;
            _inputManager.OnMoveRight += OnMoveRight;
            _inputManager.OnRotateLeft += OnRotateLeft;
            _inputManager.OnRotateRight += OnRotateRight;
            _inputManager.OnSoftDrop += OnSoftDrop;
            _inputManager.OnHardDrop += OnHardDrop;
            _inputManager.OnHold += OnHold;

            _boardController.OnGameOver += OnGameOver;
            _boardController.OnLinesCleared += OnLinesCleared;
            _boardController.OnNextPiecesChanged += OnNextPiecesChanged;
            _boardController.OnHoldPieceChanged += OnHoldPieceChanged;
        }

        private void DisconnectEvents()
        {
            _inputManager.OnMoveLeft -= OnMoveLeft;
            _inputManager.OnMoveRight -= OnMoveRight;
            _inputManager.OnRotateLeft -= OnRotateLeft;
            _inputManager.OnRotateRight -= OnRotateRight;
            _inputManager.OnSoftDrop -= OnSoftDrop;
            _inputManager.OnHardDrop -= OnHardDrop;
            _inputManager.OnHold -= OnHold;

            _boardController.OnGameOver -= OnGameOver;
            _boardController.OnLinesCleared -= OnLinesCleared;
            _boardController.OnNextPiecesChanged -= OnNextPiecesChanged;
            _boardController.OnHoldPieceChanged -= OnHoldPieceChanged;
        }

        private int CalculateScore(int lines, int level)
        {
            int baseScore = lines switch
            {
                1 => 40,
                2 => 100,
                3 => 300,
                4 => 1200,
                _ => 0
            };
            return baseScore * (level + 1);
        }

    }
}