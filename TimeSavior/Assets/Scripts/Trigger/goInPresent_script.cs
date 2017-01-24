using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class goInPresent_script : MonoBehaviour {

	//entrando nel trigger va al livello presente
	public void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Level_Present");
            GameObject.Find("Gun").GetComponent<gun_script>().StopShooting();
        }
	}
}
