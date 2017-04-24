using UnityEngine;
using UnityEngine.UI;

public class multiplier_number : MonoBehaviour {

    Text number;
	
	void Awake () {

        number = GetComponent<Text>();
    }
	
    public int SetTextMultiplier(int enemyCount)
    {
        var multiplier = 0;

        if (enemyCount < 5)
            multiplier = 1;
        else if (enemyCount >= 5 && enemyCount < 10)
            multiplier = 2;
        else if (enemyCount >= 10 && enemyCount < 15)
            multiplier = 3;
        else if (enemyCount >= 15)
            multiplier = 4;

        number.text = multiplier.ToString();
        return multiplier;
    }
}
