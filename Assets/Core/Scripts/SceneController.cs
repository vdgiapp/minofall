using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minofall
{
    // Singleton in Core scene
    public class SceneController : MonoBehaviour
    {
        public static SceneController Instance { get; private set; }

        // Scene names
        public class SceneName
        {
            public const string MainMenu = "MainMenu";
            public const string MainGame = "MainGame";
            public const string GameResult = "GameResult";
        }

        // Scene transition request
        public class SceneTransitionRequest
        {
            public readonly List<string> ScenesToLoad = new();
            public readonly List<string> ScenesToUnload = new();
            public string ActiveSceneName { get; private set; } = "";
            public bool ClearUnusedAssets { get; private set; } = false;
            public bool UseOverlay { get; private set; } = false;

            // Builder
            public SceneTransitionRequest Load(string sceneName, bool setActive = false)
            {
                ScenesToLoad.Add(sceneName);
                if (setActive) ActiveSceneName = sceneName;
                return this;
            }

            public SceneTransitionRequest Unload(string sceneName)
            {
                ScenesToUnload.Add(sceneName);
                return this;
            }

            public SceneTransitionRequest WithOverlay()
            {
                UseOverlay = true;
                return this;
            }

            public SceneTransitionRequest WithClearUnusedAssets()
            {
                ClearUnusedAssets = true;
                return this;
            }

            // Executor
            public void Perform()
            {
                SceneController.Instance.PerformTransition(this).Forget();
            }
        }

        [SerializeField] private LoadingOverlay _loadingOverlay;

        private bool _isBusy = false;

        private void Awake()
        {
            InstanceInit();
        }

        public SceneTransitionRequest NewTransition()
        {
            return new SceneTransitionRequest();
        }

        public async UniTaskVoid PerformTransition(SceneTransitionRequest request)
        {
            if (_isBusy)
            {
                Debug.LogWarning("SceneController is busy. Ignoring transition request.");
                return;
            }

            _isBusy = true;

            // Show overlay if needed
            if (request.UseOverlay) await _loadingOverlay.ShowAsync();

            // Unload scenes
            foreach (string sceneName in request.ScenesToUnload)
            {
                if (Utils.IsSceneLoaded(sceneName))
                {
                    await SceneManager.UnloadSceneAsync(sceneName);
                }
            }

            // Clear unused assets
            if (request.ClearUnusedAssets) await Resources.UnloadUnusedAssets();

            // Load scenes
            foreach (string sceneName in request.ScenesToLoad)
            {
                if (Utils.IsSceneLoaded(sceneName))
                {
                    // Nếu đã load thì unload trước
                    await SceneManager.UnloadSceneAsync(sceneName);
                }
                await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }

            // Set active scene
            if (!string.IsNullOrEmpty(request.ActiveSceneName))
            {
                Scene scene = SceneManager.GetSceneByName(request.ActiveSceneName);
                if (scene.IsValid()) SceneManager.SetActiveScene(scene);
                else
                {
                    Debug.LogWarning($"Active scene '{request.ActiveSceneName}' not found after load.");
                }
            }

            // Hide overlay if needed
            if (request.UseOverlay) await _loadingOverlay.HideAsync();

            _isBusy = false;
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