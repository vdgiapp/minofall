using UnityEngine;

namespace Minofall
{
    /// <summary>
    /// <para>Yêu cầu một <see cref="SpriteRenderer"/> để hiển thị hình ảnh.</para>
    /// Biểu diễn hình ảnh của một ô trong lưới.
    /// </summary>
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

        /// <summary>
        /// Hiển thị ô với sprite Cell và màu sắc cụ thể.
        /// </summary>
        /// <param name="color">Màu sắc</param>
        public void Show(Color color)
        {
            _spriteRenderer.sprite = _cellSprite;
            _spriteRenderer.color = color;
        }

        /// <summary>
        /// Hiển thị ô với sprite Ghost và màu sắc cụ thể.
        /// </summary>
        /// <param name="color">Màu sắc</param>
        public void Ghost(Color color)
        {
            _spriteRenderer.sprite = _ghostSprite;
            _spriteRenderer.color = color;
        }

        /// <summary>
        /// Ẩn ô đi.
        /// </summary>
        public void Hide()
        {
            _spriteRenderer.color = Color.clear;
        }
    }
}