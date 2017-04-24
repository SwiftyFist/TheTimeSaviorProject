using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    Animator myAnimator;
    public GameObject DeathParticle;
    public Transform SpawnParticle;
    private score_manager_script ScoreManager;

    void Awake()
    {
      ScoreManager = GameObject.Find("Score_Manager").GetComponent<score_manager_script>();
      myAnimator = GetComponent<Animator>();
      SpawnParticle = transform.FindChild("SpawnParticle");
    }

    public void DestroyEnemy(int pointsOnDeath)
    {
        GetComponent<enemy_health_manager_script>().stillAlive = false;
       // score_manager_script._score.EnemyDeathCount();
       // score_manager_script._score.AddPoints(pointsOnDeath);
        GetComponent<EnemySoundManager>().PlayOnDeath();
        GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().VelocityModificatorByGame(2);
        //TODO aggiungere variabile per animazione;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        myAnimator.SetBool("Dead", true);

        //Aggiunge 1 al contatore di nemici uccisi
        ScoreManager.EnemyDeathCount();

        //Aggiunge 10 allo score (da utilizzare una variabile) questo valore verrà diviso in base ai nemici e boss fight
        //Viene poi moltiplicato allo score
        ScoreManager.AddPoints(10);

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
        yield return new WaitForSeconds(0.7f);
       
        //Spawn del particellare della morte
        GameObject clone = Instantiate(DeathParticle, SpawnParticle.position, SpawnParticle.rotation) as GameObject;
        //Distruzione del particellare dopo tot tempo
        Destroy(clone.gameObject, 2f);
        Destroy(gameObject);
    }

}
