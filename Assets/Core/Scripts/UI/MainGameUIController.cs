using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minofall
{
    public class MainGameUIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _linesText;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _bestScoreText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private Image _holdImage;
        [SerializeField] private Image[] _nextImage;
        [SerializeField] private Button _pauseButton;

        [Tooltip("0 = I, 1 = J, 2 = L, 3 = O, 4 = S, 5 = T, 6 = Z")]
        [SerializeField] private Sprite[] _pieceSprites;

        public event Action OnPauseButtonClick;

        private void Awake()
        {
            _pauseButton.onClick.AddListener(PauseButtonClick);
        }

        private void PauseButtonClick()
        {
            OnPauseButtonClick?.Invoke();
        }

        public void SetLinesText(string txt)
        {
            _levelText.text = txt;
        }

        public void SetScoreText(string txt)
        {
            _levelText.text = txt;
        }

        public void SetBestScoreText(string txt)
        {
            _levelText.text = txt;
        }

        public void SetLevelText(string txt)
        {
            _levelText.text = txt;
        }

        public void SetNextPieceSprite(int nextImageIndex, int tetrominoIndex)
        {
            _nextImage[nextImageIndex].sprite = _pieceSprites[tetrominoIndex];
        }

        public void SetNextPieceColor(int nextImageIndex, Color color)
        {
            _nextImage[nextImageIndex].color = color;
        }

        public void SetHoldPieceSprite(int tetrominoIndex)
        {
            _holdImage.sprite = (tetrominoIndex < 0) ? null : _pieceSprites[tetrominoIndex];
        }

        public void SetHoldPieceColor(Color color)
        {
            _holdImage.color = color;
        }
    }
}