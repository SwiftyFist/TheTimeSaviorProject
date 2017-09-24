using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class score_manager_script : MonoBehaviour
{
    public int Multiplier;
    public int EnemyCount;
	public static int Score;
    private multiplier_number _multiplierObj;

	Text text;

	void Start () 
	{
        text = GetComponent<Text> ();
        _multiplierObj = GameObject.Find("Multiplier").GetComponent<multiplier_number>();

	}

	void Update () 
	{
		if (Score < 0)
			Score = 0;

		text.text = "" + Score;
    }

	public void AddPoints (int pointsToAdd)
	{
        Score += pointsToAdd * Multiplier;
        
	}

    public void EnemyDeathCount()
    {
        EnemyCount += 1;
        Multiplier = _multiplierObj.SetTextMultiplier(EnemyCount);
    }
    public void EnemyDeathCountReset()
    {
        EnemyCount = 0;
        Multiplier = _multiplierObj.SetTextMultiplier(EnemyCount);
    }

    public void Reset()
	{
		Score = 0;
        EnemyCount = 0;
        Multiplier =  _multiplierObj.SetTextMultiplier(1);
    }

	public static void SendToHub()
	{
		player_script.pl_script.upgradePoints = player_script.pl_script.upgradePoints + Score;
	}
}
