using UnityEngine;
using System.Collections;

public class enemy_health_manager_script : MonoBehaviour {

	public static enemy_health_manager_script hl_script;
	public int enemyHealth;

	//public GameObject deathEffect;

	public int pointsOnDeath;

	void Awake () {
	
		hl_script = this;
	}
	


	void Update () {
	
		if (enemyHealth <= 0) {
			score_manager_script.AddPoints (pointsOnDeath);
            GetComponent<EnemySoundManager>().PlayOnDeath();
            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().VelocityModificatorByGame(2);
            Destroy (gameObject);
		}
	}
    

	public void giveDamage(int damageToGive)
		{
			enemyHealth -= damageToGive;
		}
	
}
