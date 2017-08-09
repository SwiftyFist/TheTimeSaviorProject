using UnityEngine;

namespace Enemies
{
    public class EnemyBackSpawn : MonoBehaviour
    {
        public Transform Enemy;
        private bool _spawned = false;
    
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("Player") || Enemy == null || _spawned) return;
            var spawnTransform = transform.GetChild(0);
            var enemy = (Instantiate(Enemy, spawnTransform.position, spawnTransform.rotation));

            if (enemy.GetComponent<DroneAI_v2>() != null)
                enemy.GetComponent<DroneAI_v2>().SetTrigger();
            else if (enemy.GetComponent<EnemyAI>() != null)
                enemy.GetComponent<EnemyAI>().SetTrigger();
            else if (enemy.GetComponent<enemy2_script>() != null)
                enemy.GetComponent<enemy2_script>().SetTriggerOn();
            else
                return;
        
            _spawned = true;
        }
    }
}
