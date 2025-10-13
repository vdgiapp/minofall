
using UnityEngine;

namespace Minofall.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIOverlay : MonoBehaviour
    {
        protected CanvasGroup _canvasGroup;

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void ShowOverlay()
        {
            _canvasGroup.alpha = 1.0f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public void HideOverlay()
        {
            _canvasGroup.alpha = 0.0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}