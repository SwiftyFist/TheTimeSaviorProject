using UnityEngine;
using Singleton;

public class save_GM_script : MonoBehaviour
{ 
    public bool loadScene = false;

    void Awake()
    {
        this.DontDestroyOnLoad();
    }
}
