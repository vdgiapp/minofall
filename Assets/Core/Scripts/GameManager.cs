
using UnityEngine;

namespace Minofall
{
    // Singleton in Bootstrapper prefab
    // No need to DontDestroyOnLoad
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            InstanceInit();
        }

        private void InstanceInit()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
    }
}