using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minofall
{
    /// <summary>
    /// Là một singleton, sử dụng để xử lý chuyển cảnh trong game,
    /// bao gồm load/unload scene, hiển thị overlay loading, v.v.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        public static SceneController Instance
        { get; private set; }

        /// <summary>
        /// Class chứa tên các scene trong game để tránh lỗi đánh máy và magic strings.
        /// </summary>
        public class SceneNames
        {
            public const string MainMenu = "MainMenu";
            public const string MainGame = "MainGame";
        }

        [SerializeField] private LoadingOverlay _loadingOverlay;

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
            // TODO: Wait for player data loaded
#if (!UNITY_EDITOR)
            // First scene load
            NewTransition()
                .Load(SceneNames.MainMenu, true)
                .Unload("EditorBootstrap")
                .WithOverlay()
            .Perform();
#endif
        }

        /// <summary>
        /// Tạo một yêu cầu chuyển cảnh mới.
        /// </summary>
        public static SceneTransitionRequest NewTransition()
        {
            return new SceneTransitionRequest();
        }

        /// <summary>
        /// Thực hiện chuyển cảnh asynchronously dựa trên yêu cầu đã cho.
        /// </summary>
        /// <param name="request">Cấu hình yêu cầu chuyển đổi</param>
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
                await _loadingOverlay.ShowAsync();

            // Unload scenes
            foreach (string sceneName in request.ScenesToUnload)
            {
                if (Utils.IsSceneLoaded(sceneName)) 
                    await SceneManager.UnloadSceneAsync(sceneName);
            }

            // Clear unused assets
            if (request.ClearUnusedAssets)
                await Resources.UnloadUnusedAssets();

            // Load scenes
            foreach (string sceneName in request.ScenesToLoad)
            {
                // Nếu đã load thì unload trước
                if (Utils.IsSceneLoaded(sceneName))
                    await SceneManager.UnloadSceneAsync(sceneName);
                await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }

            // Set active scene
            if (!string.IsNullOrEmpty(request.ActiveSceneName))
            {
                Scene scene = SceneManager.GetSceneByName(request.ActiveSceneName);
                if (scene.IsValid())
                    SceneManager.SetActiveScene(scene);
                else
                    Debug.LogWarning($"Active scene '{request.ActiveSceneName}' not found after load.");
            }

            // Hide overlay if needed
            if (request.UseOverlay)
                await _loadingOverlay.HideAsync();

            _isBusy = false;
        }
    }
}