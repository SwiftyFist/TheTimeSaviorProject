using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class goToPast_script : MonoBehaviour {

	//entrando nel trigger va al livello passato
	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
			SceneManager.LoadScene ("Level_Past");
	}
}
