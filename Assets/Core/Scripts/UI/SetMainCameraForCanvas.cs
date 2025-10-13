using UnityEngine;

namespace Minofall.UI
{
    [RequireComponent(typeof(Canvas))]
    public class SetMainCameraForCanvas : MonoBehaviour
    {
        private Canvas _canvas;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.worldCamera = Camera.main;
        }

        private void Start()
        {
            // Just in case the main camera is not set at Awake
            if (_canvas.worldCamera == null)
            {
                _canvas.worldCamera = Camera.main;
            }
        }
    }
}