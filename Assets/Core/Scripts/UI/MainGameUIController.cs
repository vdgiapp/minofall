using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NaughtyAttributes;

namespace Minofall
{
    public class MainGameUIController : MonoBehaviour
    {
        // References
        [SerializeField] private CanvasGroup _ingameOverlay;
        [SerializeField] private TMP_Text _linesText;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _bestScoreText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private Image _holdImage;
        [SerializeField] private Image[] _nextImage;
        [SerializeField] private Button _holdButton;
        [SerializeField] private Button _pauseButton;
        [HorizontalLine]

        [SerializeField] private CanvasGroup _pauseMenuOverlay;
        [HorizontalLine]

        [SerializeField] private CanvasGroup _cooldownOverlay;
        [SerializeField] private TMP_Text _cooldownText;
        [HorizontalLine]

        [SerializeField] private CanvasGroup _gameOverOverlay;
        [HorizontalLine]

        // Data
        [Tooltip("0 = I, 1 = J, 2 = L, 3 = O, 4 = S, 5 = T, 6 = Z")]
        [SerializeField] private Sprite[] _pieceSprites;

        public void SetLinesText(string txt)
        {
            _linesText.text = txt;
        }

        public void SetScoreText(string txt)
        {
            _scoreText.text = txt;
        }

        public void SetBestScoreText(string txt)
        {
            _bestScoreText.text = txt;
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

        public void SetPauseButtonActive(bool isActive)
        {
            _pauseButton.gameObject.SetActive(isActive);
        }

        public void SetMenuOverlayActive(bool isActive)
        {
            _pauseMenuOverlay.gameObject.SetActive(isActive);
        }

        public void SetGameOverOverlayActive(bool isActive)
        {
            _gameOverOverlay.gameObject.SetActive(isActive);
        }

        public void SetCooldownOverlayActive(bool isActive)
        {
            _cooldownOverlay.gameObject.SetActive(isActive);
        }

        public void SetCooldownText(string txt)
        {
            _cooldownText.text = txt;
        }

        public async UniTask AnimateCooldownNumber(string txt, float totalTime = 1f)
        {
            SetCooldownText(txt);

            // Reset transform
            _cooldownText.transform.localScale = Vector3.one * 0.5f;
            _cooldownText.color = new Color(1, 1, 1, 0);

            float fadeInTime = totalTime * 0.15f;
            float holdTime = totalTime * 0.7f;
            float fadeOutTime = totalTime * 0.15f;

            var seq = DOTween.Sequence();
            seq.Join(_cooldownText.transform.DOScale(1.2f, fadeInTime).SetEase(Ease.OutBack));
            seq.Join(_cooldownText.DOFade(1f, fadeInTime));
            seq.AppendInterval(holdTime);
            seq.Append(_cooldownText.DOFade(0f, fadeOutTime));

            await seq.AsyncWaitForCompletion();
        }
    }
}