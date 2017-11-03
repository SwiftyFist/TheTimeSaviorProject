using System.Collections;
using GameManager;
using UnityEngine;

namespace Destroyer
{
    public class DestroyerPlayerStandard : MonoBehaviour
    {

        #region Variabili

        public Rigidbody2D MyRigidBody2D; //RigidBody2D del destroyer
        private Transform _myTransform; //Transform del destroyer

        public static float antivirVelocity = 5f; //Velocità antivirus attuale
        public float _antivirVelocity; //Velocità antivirus iniziale

        public float velocityModificatorByTime = 0.09f; //Modificatore di velocità over time
        public float maxVelocity = 7; //Velocità massima overtime
        public float minVelocity = -50; // Velocità minima

        public score_manager_script ScoreManager;

        private GameObject _player;

        #endregion

        #region Funzioni per Unity

        void Awake()
        {
            ModificatorSet();
            _myTransform = GetComponent<Transform>();
            MyRigidBody2D = GetComponent<Rigidbody2D>();
            MyRigidBody2D.velocity = Vector2.right * antivirVelocity;
            StartCoroutine(VelocityModificatorByTime()); //Aumenta la velocità overtime
            ScoreManager = GameObject.Find("Score_Manager").GetComponent<score_manager_script>();
            _player = GameObject.Find("Player");
        }

        void Update()
        {
            //Debug.Log("Velocità " + myRigidBody2D.velocity.x);
            MyRigidBody2D.velocity = Vector2.right * AntivirVelocity();
            var player = GameObject.Find("Player");
            if ( player != null &&
                 player.GetComponent<Transform>().position.x
                 < _myTransform.position.x)
            {
                LevelManager.LevelReset();
            }

        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("NeverDestroy") 
                && !collision.gameObject.CompareTag("Shield")
                && !collision.gameObject.CompareTag("Player")
                && !collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(collision.gameObject); //Distrugge quello che incontra per alleggerire il gioco
            }

        }

        #endregion

        #region Funzioni Interne

        IEnumerator VelocityModificatorByTime() //Aumenta la velocità ogni 0.5 secondi
        {
            yield return new WaitForSeconds(0.5f);

            if (antivirVelocity < maxVelocity)
            {
                antivirVelocity = antivirVelocity + velocityModificatorByTime;
            }

            StartCoroutine(VelocityModificatorByTime());
        }

        void ModificatorSet()
        {
            antivirVelocity = _antivirVelocity;
        }

        float AntivirVelocity ()
        {
            var velocity = antivirVelocity + DestroyerPlayerDistance.velocityModificatorByDistance + 
                           DestroyerPlayerGame.velocityModificatorByGame + DestroyerPlayerInactivity.velocityModificatorByInactivity;
            return velocity < minVelocity ? minVelocity : velocity;
        }


        //Attiva o disattiva il destroyer
        public void SetActive (bool activating)
        {
            GetComponent<DestroyerPlayerDistance>().enabled = activating;
            GetComponent<DestroyerPlayerGame>().enabled = activating;
            GetComponent<DestroyerPlayerStandard>().enabled = activating;

            if (!activating)
            {
                GetComponent<DestroyerPlayerInactivity>().enabled = activating;
                GetComponent<DestroyerPlayerStandard>().MyRigidBody2D.velocity = new Vector3(0, 0, 0);
            }
        }

        #endregion

    }
}


