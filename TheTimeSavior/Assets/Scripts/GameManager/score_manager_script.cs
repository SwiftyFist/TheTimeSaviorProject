using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class score_manager_script : MonoBehaviour
{
    public int multiplier;
    public int enemyCount;
	public static int score;
    private multiplier_number MultiplierObj;

	Text text;

	void Start () 
	{
        text = GetComponent<Text> ();
        MultiplierObj = GameObject.Find("Multiplier").GetComponent<multiplier_number>();
	}

	void Update () 
	{
		if (score < 0)
			score = 0;

		text.text = "" + score;
    }

	public void AddPoints (int pointsToAdd)
	{
        score += pointsToAdd * multiplier;
        
	}

    public void EnemyDeathCount()
    {
        enemyCount += 1;
        multiplier = MultiplierObj.SetTextMultiplier(enemyCount);
    }
    public void EnemyDeathCountReset()
    {
        enemyCount = 0;
        multiplier = MultiplierObj.SetTextMultiplier(enemyCount);
    }

    public void Reset()
	{
		score = 0;
        enemyCount = 0;
        multiplier =  MultiplierObj.SetTextMultiplier(1);
    }

	public static void SendToHub()
	{
		player_script.pl_script.upgradePoints = player_script.pl_script.upgradePoints + score;
	}
}
