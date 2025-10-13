using UnityEngine;

namespace Minofall.UI
{
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