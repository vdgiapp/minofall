#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace Minofall.Editor
{
    /// <summary>
    /// <para>Class này chỉ hoạt động trong Unity Editor.</para>
    /// <para>Class này khi bắt đầu sẽ load scene tên "Core", và sau đó load scene đã lưu từ EditorPrefs.</para>
    /// </summary>
    public class EditorCoreLoader : MonoBehaviour
    {
        void Start() => LoadCoreScene().Forget();
        async UniTask LoadCoreScene()
        {
            // Lấy scene đã lưu từ EditorPrefs
            string targetScene = UnityEditor.EditorPrefs.GetString("bootstrapTargetScene", "");
            UnityEditor.EditorPrefs.DeleteKey("bootstrapTargetScene");

            // Load Core scene
            await SceneManager.LoadSceneAsync("Core", LoadSceneMode.Additive);
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
        }
    }
}
#endif