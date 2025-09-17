
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Minofall.Editor
{
    [InitializeOnLoad]
    sealed class EditorBootstrap
    {
        public static readonly string SCENE_NAME = "EditorBootstrap";
        public static readonly string VALUE_NAME = "bootstrapTargetScene";

        static EditorBootstrap() => EditorApplication.playModeStateChanged += OnPlayerModeChanged;
        static void OnPlayerModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                // Lưu scene đang mở vào EditorPrefs
                string activeScene = EditorSceneManager.GetActiveScene().path;
                EditorPrefs.SetString(VALUE_NAME, activeScene);
                
                // Set scene bắt đầu play luôn là BootstrapEditor
                string[] guids = AssetDatabase.FindAssets($"t:SceneAsset {SCENE_NAME}");
                string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
                SceneAsset bootstrapScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                if (bootstrapScene != null)
                {
                    EditorSceneManager.playModeStartScene = bootstrapScene;
                }
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