using UnityEngine;
using System.Collections;


public class save_GM_script : MonoBehaviour
{
    public bool loadScene = false;

    void Awake()
    {
        GameObject.DontDestroyOnLoad(gameObject);
    }
}
