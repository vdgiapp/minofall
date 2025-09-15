using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Minofall
{
    /// <summary>
    /// Kiểm soát luồng chính của trò chơi, bao gồm quản lý trạng thái trò chơi,
    /// cập nhật giao diện người dùng và xử lý sự kiện trò chơi.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        // References
        [SerializeField] private BoardController _boardController;
        [SerializeField] private MainGameUIController _uiController;

        // Stats
        private int score = 0;
        private int level = 1;
        private int linesCleared = 0;

        private void Start()
        {
            // Connect events
            ConnectEvents();

            // Pause game for cooldown
            _boardController.enabled = false;

            // Hide pause button
            _uiController.SetPauseButtonActive(false);

            // Set default texts
            _uiController.SetScoreText("0");
            _uiController.SetLevelText("01");
            _uiController.SetLinesText("Lines: 0");
            // TODO: set best score

            // Disable input
            InputManager.Instance.DisableMainGameActions();

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
            _boardController.enabled = false;

            // Show game over overlay
            _uiController.SetPauseButtonActive(false);
            _uiController.SetGameOverOverlayActive(true);

            // Disable input
            InputManager.Instance.DisableMainGameActions();

            // Wait for a few seconds before returning to main menu
            await UniTask.WaitForSeconds(5);
            SceneController.NewTransition()
                .Load(SceneController.SceneNames.MainMenu, true)
                .Unload(SceneController.SceneNames.MainGame)
                .WithOverlay()
                .WithClearUnusedAssets()
                .Perform();
        }

        private void OnLinesCleared(int lines)
        {
            // Update stats
            linesCleared += lines;
            score += CalculateScore(lines, level);
            level = 1 + linesCleared / 10;

            // Update drop time
            _boardController.SetDropTime(TetrisGravity.GetDropTime(level));

            // Update UI
            _uiController.SetScoreText(Utils.NumberFormat(score));
            _uiController.SetLevelText(level.ToString("D2"));
            _uiController.SetLinesText($"Lines: {linesCleared}");
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

        /// <summary>
        /// Gọi khi nút Pause được nhấp.
        /// Được kết nối với sự kiện onClick của PauseButton trong Inspector
        /// </summary>
        public void OnPauseButtonClick()
        {
            // Pause game
            _boardController.enabled = false;

            // Show pause menu overlay
            _uiController.SetPauseButtonActive(false);
            _uiController.SetMenuOverlayActive(true);

            // Disable input
            InputManager.Instance.DisableMainGameActions();
        }

        /// <summary>
        /// Gọi khi nút Resume được nhấp.
        /// Được kết nối với sự kiện onClick của ResumeButton trong Inspector
        /// </summary>
        public void OnResumeButtonClick()
        {
            HandleCooldownBeforePlay().Forget();
        }

        /// <summary>
        /// Gọi khi nút Quit được nhấp.
        /// Được kết nối với sự kiện onClick của QuitButton trong Inspector
        /// </summary>
        public void OnQuitButtonClick()
        {
            SceneController.NewTransition()
                .Load(SceneController.SceneNames.MainMenu, true)
                .Unload(SceneController.SceneNames.MainGame)
                .WithOverlay()
                .WithClearUnusedAssets()
                .Perform();
        }

        private async UniTask HandleCooldownBeforePlay()
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
            _boardController.enabled = true;

            // Hide cooldown overlay and show pause button
            _uiController.SetPauseButtonActive(true);
            _uiController.SetCooldownOverlayActive(false);
            InputManager.Instance.EnableMainGameActions();
        }

        private void ConnectEvents()
        {
            InputManager.Instance.OnMoveLeft += OnMoveLeft;
            InputManager.Instance.OnMoveRight += OnMoveRight;
            InputManager.Instance.OnRotateLeft += OnRotateLeft;
            InputManager.Instance.OnRotateRight += OnRotateRight;
            InputManager.Instance.OnSoftDrop += OnSoftDrop;
            InputManager.Instance.OnHardDrop += OnHardDrop;
            InputManager.Instance.OnHold += OnHold;

            _boardController.OnGameOver += OnGameOver;
            _boardController.OnLinesCleared += OnLinesCleared;
            _boardController.OnNextPiecesChanged += OnNextPiecesChanged;
            _boardController.OnHoldPieceChanged += OnHoldPieceChanged;
        }

        private void DisconnectEvents()
        {
            InputManager.Instance.OnMoveLeft -= OnMoveLeft;
            InputManager.Instance.OnMoveRight -= OnMoveRight;
            InputManager.Instance.OnRotateLeft -= OnRotateLeft;
            InputManager.Instance.OnRotateRight -= OnRotateRight;
            InputManager.Instance.OnSoftDrop -= OnSoftDrop;
            InputManager.Instance.OnHardDrop -= OnHardDrop;
            InputManager.Instance.OnHold -= OnHold;

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