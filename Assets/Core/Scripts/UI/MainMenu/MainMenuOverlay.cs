using UnityEngine;
using UnityEngine.UI;

namespace Minofall.UI
{
    public class MainMenuOverlay : UIOverlay
    {
        [SerializeField] private Button _playButton;

        public void ToggleMainMenu(bool active)
        {
            gameObject.SetActive(active);
        }

        public void SetPlayButtonInteractable(bool interactable)
        {
            _playButton.interactable = interactable;
        }
    }
}