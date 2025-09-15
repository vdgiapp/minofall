using UnityEngine;

namespace Minofall
{
    public class MainMenuUIController : MonoBehaviour
    {
        public void OnPlayButtonPressed()
        {
            SceneController.NewTransition()
                .Load(SceneController.SceneNames.MainGame, true)
                .Unload(SceneController.SceneNames.MainMenu)
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