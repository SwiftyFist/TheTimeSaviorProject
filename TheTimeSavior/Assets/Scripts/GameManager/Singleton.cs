using UnityEngine;

namespace Singleton
{
    public static class SingletonExt
    {

        public static int DontDestroyOnLoad<T>(this T component)
        {
            var type = typeof(T);
            var objects = Object.FindObjectsOfType(type);                

            if (objects.Length > 1)
                for (int i = 1; i < objects.Length; i++)
                    Object.Destroy(objects[i]);
            else
                Object.DontDestroyOnLoad(objects[0]);

            return 0;
        }

    }
}

