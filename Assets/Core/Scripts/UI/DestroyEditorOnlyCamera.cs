using UnityEngine;

namespace Minofall
{
    public class DestroyEditorOnlyCamera : MonoBehaviour
    {
#if UNITY_EDITOR
        private void Awake()
        {
            if (Application.isPlaying)
            {
                Destroy(gameObject);
            }
        }
#endif
    }
}