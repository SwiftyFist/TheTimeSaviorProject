using UnityEngine;
using System.Collections;

public class Camera_Script : MonoBehaviour {


	public GameObject LevelStart;
	public GameObject LevelEnd;
	float min;
	float max;
	float CamPos;

	void Start () 
	{
		min = LevelStart.transform.position.x + 17.44f;
		max = LevelEnd.transform.position.x - 17.44f;
	}
	

	void Update () {
	
		CamPos = player_script.pl_script.myTransform.position.x + 2f;
		gameObject.transform.position = new Vector3 (Mathf.Clamp (CamPos, min, max), gameObject.transform.position.y, gameObject.transform.position.z);
	}
}
