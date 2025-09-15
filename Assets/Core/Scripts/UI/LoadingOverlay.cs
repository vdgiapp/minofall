using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Minofall
{
    /// <summary>
    /// <para>Yêu cầu một CanvasGroup trên cùng một GameObject.</para>
    /// Dùng để hiển thị một overlay loading với hiệu ứng mờ dần.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class LoadingOverlay : MonoBehaviour
    {
        [SerializeField] private float _fadeInDuration = 0.5f;
        [SerializeField] private float _fadeOutDuration = 0.5f;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
#if UNITY_EDITOR
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
#endif
        }

        public async UniTask ShowAsync()
        {
            _canvasGroup.blocksRaycasts = true;
            await _canvasGroup.DOFade(1f, _fadeInDuration)
                .SetUpdate(true) // ignore timescale
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