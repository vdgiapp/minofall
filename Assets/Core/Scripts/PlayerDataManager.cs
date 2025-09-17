using UnityEngine;

namespace Minofall
{
    public class PlayerDataManager : MonoBehaviour
    {
        public static PlayerDataManager Instance
        { get; private set; }

        private void Awake()
        {
            // Singleton init
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
    }
}