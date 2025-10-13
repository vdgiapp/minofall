
using UnityEngine;

namespace Minofall.UI
{
    public class PauseMenuOverlay : UIOverlay
    {
        public GameObject PauseOverlay;
        public SettingsOverlay SettingsOverlay;
        public GameObject TutorialsOverlay;

        public void TogglePauseMenu(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}