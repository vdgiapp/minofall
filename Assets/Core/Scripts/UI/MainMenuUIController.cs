using UnityEngine;

namespace Minofall
{
    public class MainMenuUIController : MonoBehaviour
    {
        public void OnPlayButtonPressed()
        {
            SceneController.Instance.NewTransition()
                .Load(SceneController.SceneName.MainGame, true)
                .Unload(SceneController.SceneName.MainMenu)
                .WithOverlay()
                .WithClearUnusedAssets()
                .Perform();
        }

        public void OnHighScoresButtonPressed()
        {
            
        }

        public void OnSettingsButtonPressed()
        {

        }

        public void OnAboutButtonPressed()
        {

        }

        public void OnQuitButtonPressed()
        {
            Application.Quit();
        }
    }
}