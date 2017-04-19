using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_stop_script : MonoBehaviour {

	

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerDistance>().enabled = false;//Blocco lo script del destroyer
            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().enabled = false;
            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerStandard>().enabled = false;
            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerInactivity>().enabled = false;
            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerStandard>().myRigidBody2D.velocity = new Vector2(0,0);
            Debug.Log("IA Antivirus bloccata");
        }
    }

}
