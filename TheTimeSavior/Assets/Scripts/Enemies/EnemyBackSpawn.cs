using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBackSpawn : MonoBehaviour
{

    public Transform Enemy;
    private bool spawned = false;
    
    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.tag != "Player" || Enemy == null || spawned) return;
        var spawnTransform = transform.GetChild(0);
        var enemy = (Instantiate(Enemy, spawnTransform.position, spawnTransform.rotation));

        if (enemy.GetComponent<DroneAI_v2>() != null)
            enemy.GetComponent<DroneAI_v2>().SetTriggerOn();
        else if (enemy.GetComponent<EnemyAI>() != null)
            enemy.GetComponent<EnemyAI>().SetTriggerOn();
        else if (enemy.GetComponent<enemy2_script>() != null)
            enemy.GetComponent<enemy2_script>().SetTriggerOn();
        else
            return;
        
        spawned = true;


    }
}
