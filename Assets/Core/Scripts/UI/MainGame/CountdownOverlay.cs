using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Minofall.UI
{
    public class CountdownOverlay : UIOverlay
    {
        [SerializeField] private Image _countdownImage;

        public void SetCountdownSprite(Sprite sprite)
        {
            _countdownImage.sprite = sprite;
        }

        public void SetCountdownColor(Color color)
        {
            if (color == null)
            {
                _countdownImage.color = Color.clear;
                return;
            }
            _countdownImage.color = color;
        }

        public void ToggleCooldown(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}