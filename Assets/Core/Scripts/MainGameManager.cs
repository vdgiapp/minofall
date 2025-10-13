using Cysharp.Threading.Tasks;
using UnityEngine;
using Minofall.UI;
using Minofall.Data;

namespace Minofall
{
    public class MainGameManager : MonoBehaviour
    {
        // References
        [SerializeField] private BoardController _boardController;
        [SerializeField] private MainGameUIController _uiController;

        // Stats
        private int score = 0;
        private int level = 1;
        private int linesCleared = 0;

        private bool backToBack = false;

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
            _boardController.OnPieceSoftDrop += OnPieceSoftDrop;
            _boardController.OnPieceHardDrop += OnPieceHardDrop;

            _boardController.PauseGame();
            _boardController.WithCells(Utils.To2DArray(
                PlayerData.Instance.Progress.cells,
                BoardController.BOARD_SIZE.y,
                BoardController.BOARD_SIZE.x)
            );
            _boardController.WithPiece(PlayerData.Instance.Progress.currentPiece);
            _boardController.WithGhostPosition(PlayerData.Instance.Progress.ghostPosition);
            _boardController.WithPieceGenerator(new PieceGenerator(
                new(PlayerData.Instance.Progress.nextQueue),
                PlayerData.Instance.Progress.pieceBag)
            );
            _boardController.WithHoldingPiece(PlayerData.Instance.Progress.holdingPiece);
            _boardController.WithHeldThisTurn(PlayerData.Instance.Progress.heldThisTurn);
            _boardController.WithHighestRow(PlayerData.Instance.Progress.highestRow);
            _boardController.WithDropTimer(PlayerData.Instance.Progress.dropTimer);

            if (!PlayerData.Instance.Progress.isAvailable)
            {
                _boardController.InitGame(true);
                PlayerData.Instance.Progress.isAvailable = true;
            }
            else _boardController.InitGame(false);

            score = PlayerData.Instance.Progress.score;
            level = PlayerData.Instance.Progress.level;
            linesCleared = PlayerData.Instance.Progress.lines;

            UpdateUIStats();
            _uiController.IngameOverlay.TogglePauseButton(false);

            InputManager.Instance.DisableKeyboardInput();
            InputManager.Instance.DisableTouchInput();

            _uiController.PauseMenuOverlay.SettingsOverlay.SetControlTypeDropdownValue(PlayerData.Instance.Settings.controlType);
            _uiController.PauseMenuOverlay.SettingsOverlay.SetMasterVolumeSliderValue(PlayerData.Instance.Settings.masterVolume * 100);
            _uiController.PauseMenuOverlay.SettingsOverlay.SetMusicVolumeSliderValue(PlayerData.Instance.Settings.musicVolume * 100);
            _uiController.PauseMenuOverlay.SettingsOverlay.SetSFXVolumeSliderValue(PlayerData.Instance.Settings.sfxVolume * 100);

            AudioManager.Instance.PlayMusic("Korobeiniki_TetrisThemeArrangedForPiano");

            Countdown321().Forget();
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
            _boardController.OnPieceSoftDrop -= OnPieceSoftDrop;
            _boardController.OnPieceHardDrop -= OnPieceHardDrop;
        }

        private void OnMoveLeft()
        {
            _boardController.MovePiece(Vector2Int.left);
            AudioManager.Instance.PlaySFX("glass_001");
        }

        private void OnMoveRight()
        {
            _boardController.MovePiece(Vector2Int.right);
            AudioManager.Instance.PlaySFX("glass_001");
        }

        private void OnRotateLeft()
        {
            _boardController.RotatePiece(-1);
            AudioManager.Instance.PlaySFX("glass_006");
        }

        private void OnRotateRight()
        {
            _boardController.RotatePiece(1);
            AudioManager.Instance.PlaySFX("glass_006");
        }
        private void OnSoftDrop() => _boardController.SoftDropPiece();
        private void OnHardDrop() => _boardController.HardDropPiece();
        private void OnHold() => _boardController.HoldPiece();

        private async void OnGameOver()
        {
            _boardController.PauseGame();

            _uiController.IngameOverlay.TogglePauseButton(false);
            _uiController.GameOverOverlay.ToggleGameOver(true);
            _uiController.IngameOverlay.ToggleMobileButtons(false);

            InputManager.Instance.DisableKeyboardInput();
            InputManager.Instance.DisableTouchInput();

            PlayerData.Instance.Progress.isAvailable = false;
            PlayerData.Instance.HighScores.AddScore(score);
            PlayerData.Instance.HighScores.lastScore = score;

            PlayerData.Instance.SaveAll();

            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlaySFX("jingles_PIZZI01");

            await UniTask.WaitForSeconds(5);
            SceneController.NewTransition()
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

        private void OnPieceSoftDrop()
        {
            score++;
            UpdateUIStats();
        }

        private void OnPieceHardDrop(int distance)
        {
            score += (2 * distance);
            UpdateUIStats();
            AudioManager.Instance.PlaySFX("glass_002");
        }

        private void OnLinesCleared(int lines, bool isTSpin)
        {
            linesCleared += lines;
            level = 1 + linesCleared / 10;
            score += CalculateScore(lines, isTSpin, level);
            _boardController.SetDropTime(TetrisGravity.GetDropTime(level));
            UpdateUIStats();
            AudioManager.Instance.PlaySFX($"confirmation_00{Random.Range(1, 4)}");
        }

        private void OnNextPiecesChanged(int[] nextPieces)
        {
            for (int i = 0; i < nextPieces.Length; i++)
            {
                _uiController.UpdateNextPiece(i, nextPieces[i], Tetrominoes.GetColor(nextPieces[i]));
            }
        }

        private void OnHoldPieceChanged(int holdPiece)
        {
            if (holdPiece < 0) _uiController.UpdateHoldPiece(-1, Color.clear);
            else _uiController.UpdateHoldPiece(holdPiece, Tetrominoes.GetColor(holdPiece));
        }

        public void OnPauseButtonClicked()
        {
            _boardController.PauseGame();

            _uiController.IngameOverlay.TogglePauseButton(false);
            _uiController.IngameOverlay.ToggleMobileButtons(false);
            _uiController.PauseMenuOverlay.TogglePauseMenu(true);

            InputManager.Instance.DisableKeyboardInput();
            InputManager.Instance.DisableTouchInput();
        }

        public void OnResumeButtonClicked()
        {
            if (PlayerData.Instance.Settings.controlType == 1)
            {
                _uiController.IngameOverlay.ToggleMobileButtons(true);
            }
            Countdown321().Forget();
            AudioManager.Instance.PlaySFX("select_006");
        }

        public void OnSettingsButtonClicked()
        {
            _uiController.PauseMenuOverlay.PauseOverlay.SetActive(false);
            _uiController.PauseMenuOverlay.TutorialsOverlay.SetActive(false);
            _uiController.PauseMenuOverlay.SettingsOverlay.SetActive(true);
            AudioManager.Instance.PlaySFX("select_006");
        }

        public void OnTutorialsButtonClicked()
        {
            _uiController.PauseMenuOverlay.PauseOverlay.SetActive(false);
            _uiController.PauseMenuOverlay.SettingsOverlay.SetActive(false);
            _uiController.PauseMenuOverlay.TutorialsOverlay.SetActive(true);
            AudioManager.Instance.PlaySFX("select_006");
        }

        public void OnBackToPauseMenuClicked()
        {
            _uiController.PauseMenuOverlay.SettingsOverlay.SetActive(false);
            _uiController.PauseMenuOverlay.TutorialsOverlay.SetActive(false);
            _uiController.PauseMenuOverlay.PauseOverlay.SetActive(true);
            PlayerData.Instance.SaveAll();
            AudioManager.Instance.PlaySFX("select_006");
        }

        public void OnQuitButtonClicked()
        {
            // Save progress data
            PlayerData.Instance.Progress.cells = Utils.To1DArray(_boardController.GetCells());
            PlayerData.Instance.Progress.currentPiece = _boardController.GetPiece();
            PlayerData.Instance.Progress.ghostPosition = _boardController.GetGhostPosition();
            PlayerData.Instance.Progress.nextQueue = new(_boardController.GetPieceGenerator().PeekQueue());
            PlayerData.Instance.Progress.pieceBag = new(_boardController.GetPieceGenerator().PeekBag());
            PlayerData.Instance.Progress.holdingPiece = _boardController.GetHoldingPiece();
            PlayerData.Instance.Progress.heldThisTurn = _boardController.GetHeldThisTurn();
            PlayerData.Instance.Progress.highestRow = _boardController.GetHighestRow();
            PlayerData.Instance.Progress.dropTimer = _boardController.GetDropTimer();
            PlayerData.Instance.Progress.score = score;
            PlayerData.Instance.Progress.level = level;
            PlayerData.Instance.Progress.lines = linesCleared;
            PlayerData.Instance.SaveAll();

            AudioManager.Instance.PlaySFX("select_006");
            // Quit to main menu
            SceneController.NewTransition()
                .Load(SceneController.SceneNames.MainMenu, true)
                .Unload(SceneController.SceneNames.MainGame)
                .WithOverlay()
                .WithClearUnusedAssets()
            .Perform();
        }

        private async UniTask Countdown321()
        {
            _uiController.PauseMenuOverlay.TogglePauseMenu(false);
            _uiController.CountdownOverlay.ToggleCooldown(true);

            int countdown = 3;
            while (countdown > 0)
            {
                await _uiController.CountdownAndWaitAsync(countdown - 1, 1000);
                countdown--;
            }

            _boardController.ResumeGame();

            _uiController.IngameOverlay.TogglePauseButton(true);
            _uiController.CountdownOverlay.ToggleCooldown(false);
            if (PlayerData.Instance.Settings.controlType == 1)
            {
                _uiController.IngameOverlay.ToggleMobileButtons(true);
            }

            InputManager.Instance.EnableKeyboardInput();
            InputManager.Instance.EnableTouchInput();
        }

        private int CalculateScore(int lines, bool isTSpin, int level)
        {
            int baseScore;
            if (isTSpin)
            {
                baseScore = lines switch
                {
                    0 => 400,   // T-Spin (không ăn line) 
                    1 => 800,   // T-Spin Single
                    2 => 1200,  // T-Spin Double
                    3 => 1600,  // T-Spin Triple
                    _ => 0
                };
            }
            else
            {
                baseScore = lines switch
                {
                    1 => 100,   // Single
                    2 => 300,   // Double
                    3 => 500,   // Triple
                    4 => 800,   // Tetris
                    _ => 0
                };
            }

            // Back-to-Back bonus (50%)
            if ((isTSpin && lines > 0) || lines == 4)
            {
                if (backToBack && baseScore > 0)
                {
                    baseScore += baseScore / 2;
                }
                backToBack = true;
            }
            else
            {
                backToBack = false;
            }

            return baseScore * level;
        }

        private void UpdateUIStats()
        {
            _uiController.UpdateScore(score);
            _uiController.UpdateLevel(level);
            _uiController.UpdateLines(linesCleared);
            _uiController.UpdateBestScore(Mathf.Max(PlayerData.Instance.HighScores.GetBestScore(), score));
        }
    }
}