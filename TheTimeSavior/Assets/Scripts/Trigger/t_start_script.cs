using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_start_script : MonoBehaviour {

    public Sprite PortaChiusa;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var Destroyer = GameObject.Find("Destroyer");
            Destroyer.GetComponent<DestroyerPlayerDistance>().enabled = true;//Riattivo lo script del destroyer
            Destroyer.GetComponent<DestroyerPlayerGame>().enabled = true;
            Destroyer.GetComponent<DestroyerPlayerStandard>().enabled = true;
            Debug.Log("IA Antivirus ripartita");
            StartCoroutine(CloseDoor());
        }
    }
    IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(0.3f);
        transform.GetChild(0).gameObject.SetActive(true);

    }
}
