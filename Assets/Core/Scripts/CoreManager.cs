
using UnityEngine;

namespace Minofall
{
    // Singleton in Core scene
    public class CoreManager : MonoBehaviour
    {
        public static CoreManager Instance { get; private set; }

        private void Awake()
        {
            InstanceInit();
        }

        private void Start()
        {
            // Change to Main Menu when build
            //SceneController.Instance.NewTransition()
            //    .Load(SceneController.SceneName.MainMenu)
            //    .WithOverlay()
            //    .Perform();
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