using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minofall.Editor
{
    public class BootstrapRuntime : MonoBehaviour
    {
        private void Start()
        {
#if UNITY_EDITOR
            CoreStart().Forget();
#endif
        }


#if UNITY_EDITOR
        private async UniTaskVoid CoreStart()
        {
            string targetScene = UnityEditor.EditorPrefs.GetString("Bootstrap_TargetScene", "");
            UnityEditor.EditorPrefs.DeleteKey("Bootstrap_TargetScene");

            // Load Core scene trước
            await SceneManager.LoadSceneAsync("Core", LoadSceneMode.Additive);

            if (!string.IsNullOrEmpty(targetScene))
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(targetScene);
                await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

                Scene scene = SceneManager.GetSceneByName(sceneName);
                if (scene.IsValid())
                    SceneManager.SetActiveScene(scene);
            }
        }
#endif
    }
}