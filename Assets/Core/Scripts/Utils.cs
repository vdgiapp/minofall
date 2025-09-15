using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minofall
{
    /// <summary>
    /// Static class dùng để chứa các hàm tiện ích chung.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Trả về giá trị nằm trong khoảng [min, max], nếu vượt quá sẽ được giới hạn theo kiểu wrap-around.
        /// </summary>
        /// <param name="val">Giá trị cần wrap</param>
        /// <param name="min">Giá trị tối thiểu</param>
        /// <param name="max">Giá trị tối đa</param>
        public static float WrapValue(float val, float min, float max)
        {
            float range = max - min;
            if (val < min) val += range * Mathf.Ceil((min - val) / range);
            else if (val > max) val -= range * Mathf.Ceil((val - max) / range);
            return val;
        }

        /// <summary>
        /// Trả về giá trị nằm trong khoảng [min, max], nếu vượt quá sẽ được giới hạn theo kiểu wrap-around.
        /// </summary>
        /// <param name="val">Giá trị cần wrap</param>
        /// <param name="min">Giá trị tối thiểu</param>
        /// <param name="max">Giá trị tối đa</param>
        public static int WrapValue(int val, int min, int max)
        {
            if (val < min) return max - (min - val) % (max - min);
            else return min + (val - min) % (max - min);
        }

        /// <summary>
        /// Định dạng số với dấu phẩy phân cách hàng nghìn.
        /// </summary>
        /// <param name="number">Số cần định dạng</param>
        public static string NumberFormat(long number) => number.ToString("N0");

        /// <summary>
        /// Trả về một danh sách tên của tất cả các scene đã được load.
        /// </summary>
        public static List<string> GetLoadedSceneNames()
        {
            List<string> scenes = new();
            int count = SceneManager.sceneCount;
            for (int i = 0; i < count; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.isLoaded)
                    scenes.Add(scene.name);
            }
            return scenes;
        }

        /// <summary>
        /// Trả về 'true' nếu scene có tên đã được load.
        /// </summary>
        /// <param name="sceneName">Tên scene cần kiểm tra</param>
        /// <returns></returns>
        public static bool IsSceneLoaded(string sceneName)
        {
            int count = SceneManager.sceneCount;
            for (int i = 0; i < count; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.isLoaded && scene.name == sceneName)
                    return true;
            }
            return false;
        }
    }
}