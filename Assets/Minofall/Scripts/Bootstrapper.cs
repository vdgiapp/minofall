using Unity.Mathematics;
using UnityEngine;

namespace Minofall
{
    // Bootstrapper class
    public class Bootstrapper : MonoBehaviour
    {
        public static Bootstrapper Instance { get; private set; }

        // Load Bootstrapper prefab in Resources folder
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void BootstrapperInit()
        {
            if (Instance != null) return;
            var prefab = Resources.Load<Bootstrapper>("Prefabs/Bootstrapper");
            if (prefab == null)
            {
                Debug.LogError($"[{nameof(Bootstrapper)}] Bootstrapper prefab not found in Prefabs folder.");
                return;
            }
            // Instantiate prefab
            var i = Instantiate<Bootstrapper>(prefab);
            i.name = prefab.name;
            Instance = i;
            DontDestroyOnLoad(i.gameObject);
        }
    }
}