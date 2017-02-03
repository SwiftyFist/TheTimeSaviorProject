using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBack : MonoBehaviour
{
    //Variabili dell oggetto
    Transform myTransform;
    Transform playerTransform;
    public float pushBackDistance = 0.5f;

    void Awake()
    {
        myTransform = GetComponent<Transform>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        Physics2D.IgnoreLayerCollision(12, 8);
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            PushEnemyBack();
        }
    }

    void PushEnemyBack()
    {
        if (myTransform.position.x > playerTransform.position.x)//se il nemico é a destra del player
        {
            myTransform.position = new Vector3(myTransform.position.x + pushBackDistance, myTransform.position.y, myTransform.position.z);//imposta la x calcolando pushBackDistance positivo
        }
        else
        {
            myTransform.position = new Vector3(myTransform.position.x - pushBackDistance, myTransform.position.y, myTransform.position.z);//imposta la x calcolando pushBackDistance negativo
        }

        if (transform.name == "Enemy_Type_2")
        {
            if (myTransform.position.y > playerTransform.position.y)
            {
                myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y + pushBackDistance, myTransform.position.z);
            }
            else if (myTransform.position.y < playerTransform.position.y && myTransform.position.y - pushBackDistance > -3)
            {
                myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y - pushBackDistance, myTransform.position.z);
            }
        }
    }
}
