
using UnityEngine;

namespace Minofall
{
    // Singleton in Core scene
    public class CoreManager : MonoBehaviour
    {
        public static CoreManager Instance { get; private set; }
        
        public SessionData lastSessionData { get; private set; } = new(0, 1, 0);

        private void Awake()
        {
            InstanceInit();
        }

        private void Start()
        {
#if (!UNITY_EDITOR)
            SceneController.Instance.NewTransition()
                .Load(SceneController.SceneName.MainMenu, true)
                .Unload("BootstrapEditor")
                .WithOverlay()
                .Perform();
#endif
        }

        private void OnDestroy()
        {
            
        }

        public void SetLastSessionData(SessionData data) => lastSessionData = data;

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