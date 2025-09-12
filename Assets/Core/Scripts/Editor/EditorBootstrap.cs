#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Minofall.Editor
{
    // This script ensures that when the user hits Play in the Unity Editor,
    // they are redirected to a Bootstrap scene that sets up the game environment.
    // After the game session ends, it returns the user to their original scene.
    // Place this script in an 'Editor' folder within your Unity project.

    [InitializeOnLoad]
    public static class EditorBootstrap
    {
        static EditorBootstrap()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                // Lưu scene mà user đang mở
                string activeScene = EditorSceneManager.GetActiveScene().path;
                EditorPrefs.SetString("Bootstrap_TargetScene", activeScene);

                // Set scene bắt đầu play luôn là BootstrapEditor
                SceneAsset bootstrapScene = AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/Core/Resources/Scenes/Protected/BootstrapEditor.unity");
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
#endif
}