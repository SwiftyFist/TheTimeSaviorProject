using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_stop_script : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var Destroyer = GameObject.Find("Destroyer");
            Destroyer.GetComponent<DestroyerPlayerDistance>().enabled = false;//Blocco lo script del destroyer
            Destroyer.GetComponent<DestroyerPlayerGame>().enabled = false;
            Destroyer.GetComponent<DestroyerPlayerStandard>().enabled = false;
            Destroyer.GetComponent<DestroyerPlayerInactivity>().enabled = false;
            Destroyer.GetComponent<DestroyerPlayerStandard>().myRigidBody2D.velocity = new Vector3(0,0, 0);
            Debug.Log("IA Antivirus bloccata");
            transform.GetChild(0).gameObject.SetActive(true);

        }

    }

}
