using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_start_script : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerDistance>().enabled = true;//Riattivo lo script del destroyer
            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().enabled = true;
            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerStandard>().enabled = true;
            Debug.Log("IA Antivirus ripartita");

        }
    }
}
