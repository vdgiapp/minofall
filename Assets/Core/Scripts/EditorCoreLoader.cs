
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace Minofall.Editor
{
    public class EditorCoreLoader : MonoBehaviour
    {
        public static readonly string CORE_SCENE_NAME = "Core";

        void Start() => OnStartAsync().Forget();

        async UniTask OnStartAsync()
        {
            // Lấy scene đã lưu từ EditorPrefs
            string targetScene = EditorPrefs.GetString(EditorBootstrap.VALUE_NAME, "");
            EditorPrefs.DeleteKey(EditorBootstrap.VALUE_NAME);

            // Load Core scene
            await SceneManager.LoadSceneAsync(CORE_SCENE_NAME, LoadSceneMode.Additive);
            if (!string.IsNullOrEmpty(targetScene))
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(targetScene);
                await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                Scene scene = SceneManager.GetSceneByName(sceneName);
                if (scene.IsValid())
                {
                    SceneManager.SetActiveScene(scene);
                }
            }
            Debug.Log($"{nameof(EditorCoreLoader)}: Loaded {CORE_SCENE_NAME} scene.");

            // Unload EditorBootstrap scene
            await SceneManager.UnloadSceneAsync(EditorBootstrap.SCENE_NAME, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
            Debug.Log($"{nameof(EditorCoreLoader)}: Unloaded {EditorBootstrap.SCENE_NAME} scene.");

            // Unload unused assets
            await Resources.UnloadUnusedAssets();
            Debug.Log($"{nameof(EditorCoreLoader)}: Unloaded unused assets.");
        }
    }
}
#endif