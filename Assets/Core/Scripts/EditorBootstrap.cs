#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Minofall.Editor
{
    /// <summary>
    /// <para>Class này chỉ hoạt động trong Unity Editor.</para>
    /// <para>Class này đảm bảo rằng khi vào Play Mode, scene có tên "EditorBootstrap" sẽ được load trước.
    /// Trong scene "EditorBootstrap" gồm 1 object có class <see cref="EditorCoreLoader"/> để load scene "Core".</para>
    /// </summary>
    [InitializeOnLoad]
    sealed class EditorBootstrap
    {
        static EditorBootstrap() => EditorApplication.playModeStateChanged += OnPlayerModeChanged;
        static void OnPlayerModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                // Lưu scene đang mở vào EditorPrefs
                string activeScene = EditorSceneManager.GetActiveScene().path;
                EditorPrefs.SetString("bootstrapTargetScene", activeScene);

                // Set scene bắt đầu play luôn là BootstrapEditor
                string sceneName = "EditorBootstrap";
                string[] guids = AssetDatabase.FindAssets($"t:SceneAsset {sceneName}");
                string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
                SceneAsset bootstrapScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                if (bootstrapScene != null) EditorSceneManager.playModeStartScene = bootstrapScene;
            }
            else if (state == PlayModeStateChange.EnteredEditMode)
            {
                // Reset lại để không ảnh hưởng lần sau
                EditorSceneManager.playModeStartScene = null;
            }
        }
    }
}
#endif