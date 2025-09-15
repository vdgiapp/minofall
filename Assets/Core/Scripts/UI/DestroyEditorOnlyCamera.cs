using UnityEngine;

namespace Minofall
{
    /// <summary>
    /// Destroy game object của camera chỉ dùng trong editor khi chạy game.
    /// </summary>
    public class DestroyEditorOnlyCamera : MonoBehaviour
    {
        private void Awake()
        {
            if (Application.isPlaying)
            {
                if (gameObject) Destroy(gameObject);
            }
        }
    }
}