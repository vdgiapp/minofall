using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Minofall.UI;
using Minofall.Data;

namespace Minofall
{
    public class SceneController : MonoBehaviour
    {
        public static SceneController Instance
        { get; private set; }

        public class SceneNames
        {
            public const string MainMenu = "MainMenu";
            public const string MainGame = "MainGame";
        }

        [SerializeField] private LoadingOverlay _loadingOverlay;

        public bool IsBusy => _isBusy;
        private bool _isBusy = false;

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

        private void Start()
        {
            LoadDataAndMainMenu().Forget();
        }

        public static SceneTransitionRequest NewTransition()
        {
            return new SceneTransitionRequest();
        }

        public async UniTask PerformTransition(SceneTransitionRequest request)
        {
            if (_isBusy)
            {
                Debug.LogWarning("SceneController is busy. Ignoring transition request.");
                return;
            }

            _isBusy = true;

            // Show overlay if needed
            if (request.UseOverlay)
            {
                await _loadingOverlay.ShowAsync();
            }

            // Unload scenes
            foreach (string sceneName in request.ScenesToUnload)
            {
                if (Utils.IsSceneLoaded(sceneName))
                {
                    await SceneManager.UnloadSceneAsync(sceneName);
                }
            }

            // Clear unused assets
            if (request.ClearUnusedAssets)
            {
                await Resources.UnloadUnusedAssets();
            }

            // Load scenes
            foreach (string sceneName in request.ScenesToLoad)
            {
                // Nếu đã load thì unload trước
                if (Utils.IsSceneLoaded(sceneName))
                {
                    await SceneManager.UnloadSceneAsync(sceneName);
                }
                await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }

            // Set active scene
            if (!string.IsNullOrEmpty(request.ActiveSceneName))
            {
                Scene scene = SceneManager.GetSceneByName(request.ActiveSceneName);
                if (scene.IsValid())
                {
                    SceneManager.SetActiveScene(scene);
                }
                else
                {
                    Debug.LogWarning($"Active scene '{request.ActiveSceneName}' not found after load.");
                }
            }

            // Hide overlay if needed
            if (request.UseOverlay)
            {
                await _loadingOverlay.HideAsync();
            }

            _isBusy = false;
        }

        private async UniTask LoadDataAndMainMenu()
        {
            await PlayerData.Instance.LoadAllAsync();
#if (!UNITY_EDITOR)
            NewTransition()
                .Load(SceneNames.MainMenu, true)
                .WithOverlay()
                .WithClearUnusedAssets()
            .Perform();
#endif
        }
    }
}