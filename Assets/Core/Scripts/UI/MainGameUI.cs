using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;

namespace Minofall
{
    public class MainGameUI : MonoBehaviour
    {
        #region [Events] Button clicked
        public event Action OnPauseButtonClicked;
        public event Action OnResumeButtonClicked;
        public event Action OnSettingsButtonClicked;
        public event Action OnTutorialsButtonClicked;
        public event Action OnQuitButtonClicked;
        #endregion

        #region [Properties] References
        [Header("Ingame Overlay References")]
        [SerializeField] private TMP_Text _linesText;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _bestScoreText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private Image _holdImage;
        [SerializeField] private Image[] _nextImage;
        [SerializeField] private Button _holdButton;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private GameObject _mobileButtons;

        [Header("Pause Menu Overlay References")]
        [SerializeField] private CanvasGroup _pauseMenuOverlay;

        [Header("Cooldown Overlay References")]
        [SerializeField] private CanvasGroup _cooldownOverlay;
        [SerializeField] private TMP_Text _cooldownText;

        [Header("Game Over Overlay References")]
        [SerializeField] private CanvasGroup _gameOverOverlay;

        [Header("Piece Icons")]
        [Tooltip("0 = I, 1 = J, 2 = L, 3 = O, 4 = S, 5 = T, 6 = Z")]
        [SerializeField] private Sprite[] _pieceSprites;
        #endregion

        #region [Methods] Update UI
        public void UpdateLinesText(string txt)
        {
            _linesText.text = txt;
        }

        public void UpdateScoreText(string txt)
        {
            _scoreText.text = txt;
        }

        public void UpdateBestScoreText(string txt)
        {
            _bestScoreText.text = txt;
        }

        public void UpdateLevelText(string txt)
        {
            _levelText.text = txt;
        }

        public void UpdateNextPieceSprite(int nextImageIndex, int tetrominoIndex)
        {
            _nextImage[nextImageIndex].sprite = _pieceSprites[tetrominoIndex];
        }

        public void UpdateNextPieceColor(int nextImageIndex, Color color)
        {
            _nextImage[nextImageIndex].color = color;
        }

        public void UpdateHoldPieceSprite(int tetrominoIndex)
        {
            _holdImage.sprite = (tetrominoIndex < 0) ? null : _pieceSprites[tetrominoIndex];
        }

        public void UpdateHoldPieceColor(Color color)
        {
            _holdImage.color = color;
        }

        public void UpdateCooldownText(string txt)
        {
            _cooldownText.text = txt;
        }
        #endregion

        #region [Methods] Set Overlay Active
        public void SetMobileButtonsActive(bool isActive)
        {
            _mobileButtons.gameObject.SetActive(isActive);
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
        #endregion

        #region [Methods] Animate
        public async UniTask AnimateCooldownNumberAsync(string txt, float totalTime = 1f)
        {
            UpdateCooldownText(txt);

            // Reset transform
            _cooldownText.transform.localScale = Vector3.one * 0.5f;
            _cooldownText.color = new Color(1, 1, 1, 0);

            float fadeInTime = totalTime * 0.15f;
            float holdTime = totalTime * 0.7f;
            float fadeOutTime = totalTime * 0.15f;

            var sequence = DOTween.Sequence();
            sequence.Join(_cooldownText.transform.DOScale(1.2f, fadeInTime).SetEase(Ease.OutBack));
            sequence.Join(_cooldownText.DOFade(1f, fadeInTime));
            sequence.AppendInterval(holdTime);
            sequence.Append(_cooldownText.DOFade(0f, fadeOutTime));

            await sequence.AsyncWaitForCompletion().AsUniTask();
        }
        #endregion

        #region [Methods] Raise event
        public void ResumeButtonClicked() => 
            OnResumeButtonClicked?.Invoke();

        public void PauseButtonClicked() =>
            OnPauseButtonClicked?.Invoke();

        public void SettingsButtonClicked() =>
            OnSettingsButtonClicked?.Invoke();

        public void TutorialsButtonClicked() =>
            OnTutorialsButtonClicked?.Invoke();

        public void QuitButtonClicked() =>
            OnQuitButtonClicked?.Invoke();
        #endregion
    }
}