using UnityEngine;

namespace Minofall
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Cell : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Show(Color color)
        {
            gameObject.SetActive(true);
            _spriteRenderer.color = color;
        }

        public void Ghost(Color color)
        {
            color.a = 0.3f;
            Show(color);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}