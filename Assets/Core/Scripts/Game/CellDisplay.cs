using UnityEngine;

namespace Minofall
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CellDisplay : MonoBehaviour
    {
        [SerializeField] private Sprite _ghostSprite;
        [SerializeField] private Sprite _cellSprite;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Show(Color color)
        {
            _spriteRenderer.sprite = _cellSprite;
            _spriteRenderer.color = color;
        }

        public void Ghost(Color color)
        {
            _spriteRenderer.sprite = _ghostSprite;
            _spriteRenderer.color = color;
        }

        public void Hide()
        {
            _spriteRenderer.color = Color.clear;
        }
    }
}