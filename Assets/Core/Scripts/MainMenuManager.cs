using UnityEngine;
using Minofall.UI;
using Minofall.Data;

namespace Minofall
{
    public class MainMenuManager : MonoBehaviour
    {
        // References
        [SerializeField] private MainMenuUIController _uiController;

        private void Start()
        {
            _uiController.MainMenuOverlay.SetPlayButtonInteractable(PlayerData.Instance.Progress.isAvailable);
            UpdateHighScoresTMP();

            _uiController.SettingsOverlay.SetControlTypeDropdownValue(PlayerData.Instance.Settings.controlType);
            _uiController.SettingsOverlay.SetMasterVolumeSliderValue(PlayerData.Instance.Settings.masterVolume * 100);
            _uiController.SettingsOverlay.SetMusicVolumeSliderValue(PlayerData.Instance.Settings.musicVolume * 100);
            _uiController.SettingsOverlay.SetSFXVolumeSliderValue(PlayerData.Instance.Settings.sfxVolume * 100);
        }

        public void OnPlayButtonClicked()
        {
            if (PlayerData.Instance.Progress.isAvailable)
            {
                AudioManager.Instance.PlaySFX("select_006");
                SceneController.NewTransition()
                    .Load(SceneController.SceneNames.MainGame, true)
                    .Unload(SceneController.SceneNames.MainMenu)
                    .WithOverlay()
                    .WithClearUnusedAssets()
                .Perform();
            }
            else
            {
                AudioManager.Instance.PlaySFX("question_004");
            }
        }

        public void OnNewGameButtonClicked()
        {
            AudioManager.Instance.PlaySFX("select_006");
            PlayerData.Instance.Progress.Reset();
            SceneController.NewTransition()
                .Load(SceneController.SceneNames.MainGame, true)
                .Unload(SceneController.SceneNames.MainMenu)
                .WithOverlay()
                .WithClearUnusedAssets()
            .Perform();
        }

        public void OnHighScoresButtonClicked()
        {
            _uiController.HighScoresOverlay.ToggleHighScores(true);
            _uiController.MainMenuOverlay.ToggleMainMenu(false);
            AudioManager.Instance.PlaySFX("select_006");
        }

        public void OnTutorialsButtonClicked()
        {
            _uiController.TutorialsOverlay.ToggleTutorials(true);
            _uiController.MainMenuOverlay.ToggleMainMenu(false);
            AudioManager.Instance.PlaySFX("select_006");
        }

        public void OnSettingsButtonClicked()
        {
            _uiController.SettingsOverlay.ToggleSettings(true);
            _uiController.MainMenuOverlay.ToggleMainMenu(false);
            AudioManager.Instance.PlaySFX("select_006");
        }

        public void OnAboutButtonClicked()
        {

        }

        public void OnBackToMenuButtonClicked()
        {
            _uiController.HighScoresOverlay.ToggleHighScores(false);
            _uiController.TutorialsOverlay.ToggleTutorials(false);
            _uiController.SettingsOverlay.ToggleSettings(false);
            _uiController.MainMenuOverlay.ToggleMainMenu(true);
            PlayerData.Instance.SaveAll();
            AudioManager.Instance.PlaySFX("select_006");
        }

        public void OnResetHighScoresButtonClicked()
        {
            PlayerData.Instance.HighScores.Reset();
            UpdateHighScoresTMP();
            AudioManager.Instance.PlaySFX("maximize_006");
        }

        private void UpdateHighScoresTMP()
        {
            string stringHighscores = "";
            Data.HighScores highscores = PlayerData.Instance.HighScores;
            int[] scores = highscores.scores;
            bool foundLastScores = false;
            for (int i = 0; i < scores.Length; i++)
            {
                if (!foundLastScores && scores[i] == highscores.lastScore)
                {
                    foundLastScores = true;
                    stringHighscores += $" <color=#B4E2FF><sprite name=\"star\" color=#B4E2F/>{scores[i]:N0}</color>\r\n";
                    continue;
                }
                if (i == 0)
                {
                    stringHighscores += $" <color=#FFEA7D><sprite name=\"crown\" color=#FFEA7/>{scores[i]:N0}</color>\r\n";
                    continue;
                }
                stringHighscores += $"{i + 1}. {scores[i]:N0}\r\n";

            }
            _uiController.HighScoresOverlay.UpdateHighScoresText(stringHighscores);
        }
    }
}