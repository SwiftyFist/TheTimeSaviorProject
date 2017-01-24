using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{

    public void DestroyEnemy(int pointsOnDeath)
    {
        GetComponent<EnemySoundManager>().PlayOnDeath();
        GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().VelocityModificatorByGame(2);
        //TODO aggiungere variabile per animazione;
        StartCoroutine(DeathDelay());
    }

    IEnumerator DeathDelay()
    {
        if (transform.name == "Enemy")
            GetComponent<EnemyAI>().enabled = false;
        else
            GetComponent<DroneAI_v2>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

}
