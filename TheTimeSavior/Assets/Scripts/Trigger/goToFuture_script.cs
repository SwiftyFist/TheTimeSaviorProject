using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class goToFuture_script : MonoBehaviour {

	//entrando nel trigger va al livello futuro
	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
			SceneManager.LoadScene ("Level_Future");
	}
}