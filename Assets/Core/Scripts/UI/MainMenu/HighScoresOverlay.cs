using TMPro;
using UnityEngine;

namespace Minofall.UI
{
    public class HighScoresOverlay : UIOverlay
    {
        [SerializeField] private TMP_Text _highScoresText;

        public void UpdateHighScoresText(string text)
        {
            _highScoresText.text = text;
        }

        public void ToggleHighScores(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}