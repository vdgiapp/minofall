using UnityEngine;

namespace Minofall.UI
{
    public class MainMenuUIController : MonoBehaviour
    {
        [Header("Overlays")]
        public MainMenuOverlay MainMenuOverlay;
        public HighScoresOverlay HighScoresOverlay;
        public TutorialsOverlay TutorialsOverlay;
        public SettingsOverlay SettingsOverlay;
    }
}