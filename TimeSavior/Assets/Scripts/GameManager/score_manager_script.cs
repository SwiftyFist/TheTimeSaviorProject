using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class score_manager_script : MonoBehaviour {


	public static int score;

	Text text;

	void Start () 
	{
		
     text = GetComponent<Text> ();

	}

	void Update () 
	{
		if (score < 0)
			score = 0;

		text.text = "" + score;
	}

	public static void AddPoints (int pointsToAdd)
	{
		score += pointsToAdd; 
	}

	public static void Reset()
	{
		score = 0;
	}

	public static void SendToHub()
	{
		player_script.pl_script.upgradePoints = player_script.pl_script.upgradePoints + score;
	}
}
