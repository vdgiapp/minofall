using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Minofall
{
    public class SceneTransitionRequest
    {
        public List<string> ScenesToLoad 
        { get; private set; } = new();

        public List<string> ScenesToUnload 
        { get; private set; } = new();

        public string ActiveSceneName 
        { get; private set; } = "";

        public bool ClearUnusedAssets 
        { get; private set; } = false;

        public bool UseOverlay 
        { get; private set; } = false;

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

        public void Perform()
        {
            SceneController.Instance.PerformTransition(this).Forget();
        }
    }
}