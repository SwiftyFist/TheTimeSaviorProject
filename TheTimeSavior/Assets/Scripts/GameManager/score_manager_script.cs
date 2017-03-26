using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class score_manager_script : MonoBehaviour {
    public static score_manager_script _score;
    public static int multiplier;
    public int enemyCount;
	public static int score;

	Text text;

	void Start () 
	{
        _score = this;	
     text = GetComponent<Text> ();

	}

	void Update () 
	{
		if (score < 0)
			score = 0;

		text.text = "" + score;

        if (enemyCount < 5) multiplier = 1;
        if (enemyCount >= 5) multiplier = 2;
        if (enemyCount >= 10) multiplier = 3;
        if (enemyCount >= 15) multiplier = 4;
    }

	public void AddPoints (int pointsToAdd)
	{
        score += pointsToAdd * multiplier;
        
	}

    public int MultiplierValue()
    {
       return multiplier;
    }

    public void EnemyDeathCount()
    {
        enemyCount += 1;
    }
    public void EnemyDeathCountReset()
    {
        enemyCount = 0;
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
