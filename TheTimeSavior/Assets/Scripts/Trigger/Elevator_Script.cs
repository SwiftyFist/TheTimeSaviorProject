using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator_Script : MonoBehaviour
{
    private bool Trigger = false;
    private Animator myAnimator;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Trigger = true;
            myAnimator.SetBool("Elevator_Trigger",Trigger);
            
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Trigger = false;
            myAnimator.SetBool("Elevator_Trigger", Trigger);
        }
    }

     void Update()
    {
        if (Trigger == true && Input.GetButtonDown("Submit"))
        {
            FindObjectOfType<AudioManagerFmod>().StartInGameMusic();
            SceneManager.LoadScene("Level_Present");
            GameObject.Find("Gun").GetComponent<gun_script>().StopShooting();
            
        }
    }
}