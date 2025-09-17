using UnityEngine;

namespace Minofall
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