using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Minofall.UI
{
    public class LoadingOverlay : UIOverlay
    {
        [SerializeField] private float _fadeInDuration = 0.5f;
        [SerializeField] private float _fadeOutDuration = 0.5f;

        protected override void Awake()
        {
            base.Awake();
#if UNITY_EDITOR
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
#endif
            _canvasGroup.interactable = false;
        }

        public async UniTask ShowAsync()
        {
            _canvasGroup.blocksRaycasts = true;
            await _canvasGroup.DOFade(1f, _fadeInDuration)
                .SetUpdate(true)
                .AsyncWaitForCompletion();
        }

        public async UniTask HideAsync()
        {
            await _canvasGroup.DOFade(0f, _fadeOutDuration)
                .SetUpdate(true)
                .AsyncWaitForCompletion();
            _canvasGroup.blocksRaycasts = false;
        }
    }
}