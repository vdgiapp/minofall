using UnityEngine;

namespace Minofall
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CellDisplay : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Show(Color color)
        {
            _spriteRenderer.color = color;
        }

        public void Ghost(Color color)
        {
            color.a = 0.3f;
            Show(color);
        }

        public void Hide()
        {
            _spriteRenderer.color = Color.clear;
        }
    }
}