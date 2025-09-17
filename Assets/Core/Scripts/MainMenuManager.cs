using UnityEngine;

namespace Minofall
{
    public class MainMenuManager : MonoBehaviour
    {
        // References
        [SerializeField] private MainMenuUI _uiView;

        private void Start()
        {
            _uiView.OnPlayButtonClicked += OnPlayButtonClicked;
            _uiView.OnNewGameButtonClicked += OnNewGameButtonClicked;
            _uiView.OnHighScoresButtonClicked += OnHighScoresButtonClicked;
            _uiView.OnTutorialsButtonClicked += OnTutorialsButtonClicked;
            _uiView.OnSettingsButtonClicked += OnSettingsButtonClicked;
            _uiView.OnAboutButtonClicked += OnAboutButtonClicked;
        }

        private void OnDestroy()
        {
            _uiView.OnPlayButtonClicked -= OnPlayButtonClicked;
            _uiView.OnNewGameButtonClicked -= OnNewGameButtonClicked;
            _uiView.OnHighScoresButtonClicked -= OnHighScoresButtonClicked;
            _uiView.OnTutorialsButtonClicked -= OnTutorialsButtonClicked;
            _uiView.OnSettingsButtonClicked -= OnSettingsButtonClicked;
            _uiView.OnAboutButtonClicked -= OnAboutButtonClicked;
        }

        private void OnPlayButtonClicked()
        {
            // TODO: Continue last game
            
        }

        private void OnNewGameButtonClicked()
        {
            // New game
            (new SceneTransitionRequest())
                .Load(SceneController.SceneNames.MainGame, true)
                .Unload(SceneController.SceneNames.MainMenu)
                .WithOverlay()
                .WithClearUnusedAssets()
            .Perform();
        }

        private void OnHighScoresButtonClicked()
        {

        }

        private void OnSettingsButtonClicked()
        {

        }

        private void OnAboutButtonClicked()
        {

        }

        private void OnTutorialsButtonClicked()
        {
            // TODO: Wait for save to quit game
            Application.Quit();
        }
    }
}