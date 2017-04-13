using UnityEngine;

namespace Singleton
{
    public static class SingletonExt
    {

        public static int DontDestroyOnLoad<T>(this T component)
        {
            try
            {
                var type = typeof(T);
                var objects = MonoBehaviour.FindObjectsOfType(type);                

                if (objects.Length > 1)
                    for (int i = 1; i < objects.Length; i++)
                        MonoBehaviour.DestroyImmediate(objects[i]);
                else
                    MonoBehaviour.DontDestroyOnLoad(objects[0]);

                return 0;
            }
            catch
            {
                return 1;
            }

        }

    }
}

