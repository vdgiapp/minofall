using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Minofall
{
    /// <summary>
    /// Class yêu cầu chuyển đổi cảnh, sử dụng mẫu Builder để cấu hình các tham số chuyển đổi.
    /// </summary>
    public class SceneTransitionRequest
    {
        /// <summary>
        /// Danh sách tên scene sẽ được tải.
        /// </summary>
        public List<string> ScenesToLoad 
        { get; private set; } = new();

        /// <summary>
        /// Danh sách tên scene sẽ được gỡ bỏ.
        /// </summary>
        public List<string> ScenesToUnload 
        { get; private set; } = new();

        /// <summary>
        /// Tên của scene sẽ được đặt làm scene hoạt động sau khi chuyển đổi hoàn tất.
        /// </summary>
        public string ActiveSceneName 
        { get; private set; } = "";

        /// <summary>
        /// Cờ cho biết có nên giải phóng tài nguyên không sử dụng sau khi chuyển đổi cảnh hay không.
        /// </summary>
        public bool ClearUnusedAssets 
        { get; private set; } = false;

        /// <summary>
        /// Cờ cho biết có nên sử dụng overlay trong quá trình chuyển đổi cảnh hay không.
        /// </summary>
        public bool UseOverlay 
        { get; private set; } = false;

        /// <summary>
        /// Builder method để thêm một scene vào danh sách tải và tùy chọn đặt nó làm scene hoạt động.
        /// </summary>
        /// <param name="sceneName">Tên scene sẽ được tải</param>
        /// <param name="setActive">Có đặt scene này làm active scene không?</param>
        public SceneTransitionRequest Load(string sceneName, bool setActive = false)
        {
            ScenesToLoad.Add(sceneName);
            if (setActive) ActiveSceneName = sceneName;
            return this;
        }

        /// <summary>
        /// Builder method để thêm một scene vào danh sách gỡ bỏ.
        /// </summary>
        /// <param name="sceneName">Tên scene sẽ được gỡ</param>
        public SceneTransitionRequest Unload(string sceneName)
        {
            ScenesToUnload.Add(sceneName);
            return this;
        }

        /// <summary>
        /// Builder method để bật sử dụng overlay trong quá trình chuyển đổi cảnh.
        /// </summary>
        public SceneTransitionRequest WithOverlay()
        {
            UseOverlay = true;
            return this;
        }

        /// <summary>
        /// Builder method để bật giải phóng tài nguyên không sử dụng sau khi chuyển đổi cảnh.
        /// </summary>
        public SceneTransitionRequest WithClearUnusedAssets()
        {
            ClearUnusedAssets = true;
            return this;
        }

        /// <summary>
        /// Thực hiện yêu cầu chuyển đổi cảnh với các tham số đã cấu hình.
        /// </summary>
        public void Perform()
        {
            SceneController.Instance.PerformTransition(this).Forget();
        }
    }
}