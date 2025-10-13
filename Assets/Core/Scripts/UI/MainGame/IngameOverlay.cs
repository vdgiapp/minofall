using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minofall.UI
{
    public class IngameOverlay : UIOverlay
    {
        [Header("Text and images")]
        [SerializeField] private TMP_Text _linesText;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _bestScoreText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private Image _holdImage;
        [SerializeField] private Image[] _nextImage;

        [Header("Buttons")]
        [SerializeField] private GameObject _holdButton;
        [SerializeField] private GameObject _pauseButton;
        [SerializeField] private GameObject _mobileButtons;

        public Color GetLinesTextColor() => _linesText.color;
        public Color GetScoreTextColor() => _scoreText.color;
        public Color GetBestScoreTextColor() => _bestScoreText.color;

        public void UpdateLinesText(string linesText)
        {
            _linesText.text = linesText;
        }

        public void UpdateScoreText(string scoreText)
        {
            _scoreText.text = scoreText;
        }

        public void UpdateBestScoreText(string scoreText)
        {
            _bestScoreText.text = scoreText;
        }

        public void UpdateLevelText(string levelText)
        {
            _levelText.text = levelText;
        }

        public void UpdateNextPieceSprite(int nextImageIndex, Sprite sprite)
        {
            _nextImage[nextImageIndex].sprite = sprite;
        }

        public void UpdateNextPieceColor(int nextImageIndex, Color color)
        {
            _nextImage[nextImageIndex].color = color;
        }

        public void UpdateHoldPieceSprite(Sprite sprite)
        {
            _holdImage.sprite = sprite;
        }

        public void UpdateHoldPieceColor(Color color)
        {
            _holdImage.color = color;
        }

        public void ToggleMobileButtons(bool active)
        {
            _mobileButtons.SetActive(active);
        }

        public void TogglePauseButton(bool active)
        {
            _pauseButton.SetActive(active);
        }
    }
}