using UnityEngine;
using System.Collections;


public class save_GM_script : MonoBehaviour {
	//Script che fa in modo che il game master non venga ricaricato durante i cambi di scena
	public static save_GM_script instance;


	void Start () 
	{
		if (instance != null) 
		{
			GameObject.Destroy (gameObject);
		} 
		else 
		{
			GameObject.DontDestroyOnLoad (gameObject);
			instance = this;
		}
	
	}
	

	void Update () {
	
	}
}
