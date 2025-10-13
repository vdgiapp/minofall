using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minofall
{
    public static class Utils
    {
        public static float WrapValue(float val, float min, float max)
        {
            float range = max - min;
            if (val < min)
            {
                val += range * Mathf.Ceil((min - val) / range);
            }
            else if (val > max)
            {
                 val -= range * Mathf.Ceil((val - max) / range);
            }
            return val;
        }

        public static int WrapValue(int val, int min, int max)
        {
            if (val < min)
            {
                return max - (min - val) % (max - min);
            }
            else
            {
                return min + (val - min) % (max - min);
            }
        }

        public static List<string> GetLoadedSceneNames()
        {
            List<string> scenes = new();
            int count = SceneManager.sceneCount;
            for (int i = 0; i < count; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.isLoaded)
                {
                    scenes.Add(scene.name);
                }
            }
            return scenes;
        }

        public static bool IsSceneLoaded(string sceneName)
        {
            int count = SceneManager.sceneCount;
            for (int i = 0; i < count; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.isLoaded && scene.name == sceneName)
                {
                    return true;
                }
            }
            return false;
        }

        public static T[] To1DArray<T>(T[,] array2D)
        {
            int width = array2D.GetLength(0);
            int height = array2D.GetLength(1);
            T[] array1D = new T[width * height];
            int index = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    array1D[index++] = array2D[x, y];
                }
            }
            return array1D;
        }

        public static T[,] To2DArray<T>(T[] array1D, int width, int height)
        {
            T[,] array2D = new T[width, height];
            int index = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    array2D[x, y] = array1D[index++];
                }
            }
            return array2D;
        }
    }
}