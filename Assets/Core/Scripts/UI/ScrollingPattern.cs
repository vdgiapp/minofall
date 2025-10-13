using UnityEngine;
using UnityEngine.UI;

namespace Minofall.UI
{
    [RequireComponent(typeof(RawImage))]
    public class ScrollingPattern : MonoBehaviour
    {
        [SerializeField] private float _xSpeed = 0.0f;
        [SerializeField] private float _ySpeed = 0.1f;

        private RawImage _rawImage;

        private void Awake()
        {
            _rawImage = GetComponent<RawImage>();
        }

        private void Update()
        {
            _rawImage.uvRect = new Rect(
                _rawImage.uvRect.x + _xSpeed * Time.deltaTime,
                _rawImage.uvRect.y + _ySpeed * Time.deltaTime,
                _rawImage.uvRect.width,
                _rawImage.uvRect.height
            );
        }
    }
}