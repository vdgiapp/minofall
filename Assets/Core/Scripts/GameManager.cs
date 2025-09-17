using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Minofall
{
    public class GameManager : MonoBehaviour
    {
        // References
        [SerializeField] private BoardController _boardController;
        [SerializeField] private MainGameUI _uiView;

        // Stats
        private int score = 0;
        private int level = 1;
        private int linesCleared = 0;

        private void Start()
        {
            // Connect events
            InputManager.Instance.OnMoveLeft += OnMoveLeft;
            InputManager.Instance.OnMoveRight += OnMoveRight;
            InputManager.Instance.OnRotateLeft += OnRotateLeft;
            InputManager.Instance.OnRotateRight += OnRotateRight;
            InputManager.Instance.OnSoftDrop += OnSoftDrop;
            InputManager.Instance.OnHardDrop += OnHardDrop;
            InputManager.Instance.OnHold += OnHold;

            _boardController.OnGameOver += OnGameOver;
            _boardController.OnPieceLocked += OnPieceLocked;
            _boardController.OnLinesCleared += OnLinesCleared;
            _boardController.OnNextPiecesChanged += OnNextPiecesChanged;
            _boardController.OnHoldPieceChanged += OnHoldPieceChanged;

            _uiView.OnPauseButtonClicked += OnPauseButtonClicked;
            _uiView.OnResumeButtonClicked += OnResumeButtonClicked;
            _uiView.OnSettingsButtonClicked += OnSettingsButtonClicked;
            _uiView.OnTutorialsButtonClicked += OnTutorialsButtonClicked;
            _uiView.OnQuitButtonClicked += OnQuitButtonClicked;

            // Pause game for cooldown
            _boardController.enabled = false;

            // Hide pause button
            _uiView.SetPauseButtonActive(false);

            // Set default texts
            _uiView.UpdateScoreText("0");
            _uiView.UpdateLevelText("01");
            _uiView.UpdateLinesText("0");
            // TODO: set best score

            // Disable input
            InputManager.Instance.DisableKeyboardInput();
            InputManager.Instance.DisableTouchInput();

            // Cooldown
            HandleCooldownBeforePlay().Forget();
        }

        private void OnApplicationPause(bool pause)
        {
            OnPauseButtonClicked();
        }

        private void OnDestroy()
        {
            InputManager.Instance.OnMoveLeft -= OnMoveLeft;
            InputManager.Instance.OnMoveRight -= OnMoveRight;
            InputManager.Instance.OnRotateLeft -= OnRotateLeft;
            InputManager.Instance.OnRotateRight -= OnRotateRight;
            InputManager.Instance.OnSoftDrop -= OnSoftDrop;
            InputManager.Instance.OnHardDrop -= OnHardDrop;
            InputManager.Instance.OnHold -= OnHold;

            _boardController.OnGameOver -= OnGameOver;
            _boardController.OnPieceLocked -= OnPieceLocked;
            _boardController.OnLinesCleared -= OnLinesCleared;
            _boardController.OnNextPiecesChanged -= OnNextPiecesChanged;
            _boardController.OnHoldPieceChanged -= OnHoldPieceChanged;

            _uiView.OnPauseButtonClicked -= OnPauseButtonClicked;
            _uiView.OnResumeButtonClicked -= OnResumeButtonClicked;
            _uiView.OnSettingsButtonClicked -= OnSettingsButtonClicked;
            _uiView.OnTutorialsButtonClicked -= OnTutorialsButtonClicked;
            _uiView.OnQuitButtonClicked -= OnQuitButtonClicked;
        }

        private async UniTask HandleCooldownBeforePlay()
        {
            // Show cooldown overlay and hide pause menu overlay
            _uiView.SetMenuOverlayActive(false);
            _uiView.SetCooldownOverlayActive(true);

            // Countdown 3, 2, 1
            int countdown = 3;
            while (countdown > 0)
            {
                await _uiView.AnimateCooldownNumberAsync(countdown.ToString(), 1f);
                countdown--;
            }

            // Resume game
            _boardController.enabled = true;

            // Hide cooldown overlay and show pause button
            _uiView.SetPauseButtonActive(true);
            _uiView.SetCooldownOverlayActive(false);
            InputManager.Instance.EnableKeyboardInput();
            InputManager.Instance.EnableTouchInput();
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
            _uiView.SetPauseButtonActive(false);
            _uiView.SetGameOverOverlayActive(true);
            _uiView.SetMobileButtonsActive(false);

            // Disable input
            InputManager.Instance.DisableKeyboardInput();
            InputManager.Instance.DisableTouchInput();

            // Wait for a few seconds before returning to main menu
            await UniTask.WaitForSeconds(5);
            (new SceneTransitionRequest())
                .Load(SceneController.SceneNames.MainMenu, true)
                .Unload(SceneController.SceneNames.MainGame)
                .WithOverlay()
                .WithClearUnusedAssets()
            .Perform();
        }

        private void OnPieceLocked()
        {
            InputManager.Instance.ResetInputState();
            //AudioManager.Instance.PlaySFX("select_002");
        }

        private void OnLinesCleared(int lines)
        {
            // Update stats
            linesCleared += lines;
            level = 1 + linesCleared / 10;
            score += CalculateScore(lines, level);

            // Update drop time
            _boardController.SetDropTime(TetrisGravity.GetDropTime(level));

            // Update UI
            _uiView.UpdateScoreText(score.ToString("N0"));
            _uiView.UpdateLevelText(level.ToString("D2"));
            _uiView.UpdateLinesText(linesCleared.ToString());
        }

        private void OnNextPiecesChanged(int[] nextPieces)
        {
            for (int i = 0; i < nextPieces.Length; i++)
            {
                // Update UI for each next piece
                _uiView.UpdateNextPieceSprite(i, nextPieces[i]);
                _uiView.UpdateNextPieceColor(i, Tetrominoes.GetColor(nextPieces[i]));
            }
        }

        private void OnHoldPieceChanged(int holdPiece)
        {
            if (holdPiece < 0)
            {
                // No hold piece
                _uiView.UpdateHoldPieceColor(Color.clear);
            }
            else
            {
                // Update UI for hold piece
                _uiView.UpdateHoldPieceSprite(holdPiece);
                _uiView.UpdateHoldPieceColor(Tetrominoes.GetColor(holdPiece));
            }
        }

        private void OnPauseButtonClicked()
        {
            // Pause game
            _boardController.enabled = false;

            // Show pause menu overlay
            _uiView.SetPauseButtonActive(false);
            _uiView.SetMobileButtonsActive(false);
            _uiView.SetMenuOverlayActive(true);

            // Disable input
            InputManager.Instance.DisableKeyboardInput();
            InputManager.Instance.DisableTouchInput();
        }

        private void OnResumeButtonClicked()
        {
            _uiView.SetMobileButtonsActive(true);
            HandleCooldownBeforePlay().Forget();
        }

        private void OnSettingsButtonClicked()
        {

        }

        private void OnTutorialsButtonClicked()
        {

        }

        private void OnQuitButtonClicked()
        {
            (new SceneTransitionRequest())
                .Load(SceneController.SceneNames.MainMenu, true)
                .Unload(SceneController.SceneNames.MainGame)
                .WithOverlay()
                .WithClearUnusedAssets()
            .Perform();
        }
    }
}