using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class EnemyDeath : MonoBehaviour
    {
        Animator myAnimator;
        public GameObject DeathParticle0;
        public GameObject DeathParticle1;
        public Transform SpawnParticle;
        private score_manager_script ScoreManager;

        void Awake()
        {
            ScoreManager = GameObject.Find("Score_Manager").GetComponent<score_manager_script>();
            myAnimator = GetComponent<Animator>();
            SpawnParticle = transform.FindChild("SpawnParticle");
        }

        public void DestroyEnemy(int pointsOnDeath, bool byPlayer = false)
        {
            GetComponent<enemy_health_manager_script>().stillAlive = false;
            // score_manager_script._score.EnemyDeathCount();
            // score_manager_script._score.AddPoints(pointsOnDeath);
            GetComponent<EnemySoundManager>().PlayOnDeath();

            if(byPlayer)
            {
                //Aggiunge 1 al contatore di nemici uccisi
                ScoreManager.EnemyDeathCount();
                GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().VelocityModificatorByGame(2);
                //Aggiunge 10 allo score (da utilizzare una variabile) questo valore verrà diviso in base ai nemici e boss fight
                //Viene poi moltiplicato allo score
                ScoreManager.AddPoints(10);
            }

            //TODO aggiungere variabile per animazione;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
            myAnimator.SetBool("Dead", true);

            StartCoroutine(DeathDelay());       
        }

        IEnumerator DeathDelay()
        {
            if (transform.name == "Enemy")
                GetComponent<EnemyAi>().enabled = false;
            else  
                GetComponent<DroneAI_v2>().enabled = false;

      
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<BoxCollider2D>().enabled = false;
            yield return new WaitForSeconds(0.7f);
       
            //Spawn del particellare della morte
            GameObject clone0 = Instantiate(DeathParticle0, SpawnParticle.position, SpawnParticle.rotation) as GameObject;
            GameObject clone1 = Instantiate(DeathParticle1, SpawnParticle.position, SpawnParticle.rotation) as GameObject;
            //Distruzione del particellare dopo tot tempo
            Destroy(clone0.gameObject, 1f);
            Destroy(clone1.gameObject, 1f);
            Destroy(gameObject);
        }

    }
}
