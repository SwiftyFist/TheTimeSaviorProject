using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemyBullet : MonoBehaviour
    {
        public int MoveSpeed = 40;
        public float PushBackForce = 30;

        private GameObject Player;

        private void Awake()
        {
            Player = GameObject.Find("Player");
        }

        void Update()
        {
            transform.Translate(Vector3.right * Time.deltaTime * MoveSpeed);
            var collisionDetection = Utils.IsColliding(Player.transform, transform);
            if (collisionDetection)
            {
                Debug.Log("Beccato!");
                Player
                    .GetComponent<Rigidbody2D>()
                    .AddForce(new Vector2(PushBackForce * -1, 0), ForceMode2D.Impulse);
                var playerScript = Player.GetComponent<player_script>();

                GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().VelocityModificatorByGame(0);

                GameObject.Find("Score_Manager").GetComponent<score_manager_script>().EnemyDeathCountReset();

                if (!playerScript.isInvincible)
                    playerScript.SetInvincible();

                Destroy(this.gameObject);
            }
        }

        void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }

}

