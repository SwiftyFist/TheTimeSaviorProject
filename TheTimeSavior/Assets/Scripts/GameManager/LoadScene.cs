using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    //Velocità di movimento
    public float velocity = 8f;

    //Transforms
    Transform playerTransform, destroyerTransform, myTransform;

    //Variabile per le fasi
    int currentPhase = 1;
    bool isTimeToSkipPassed = false;
    bool canSkip;

    void Awake()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        destroyerTransform = GameObject.Find("Destroyer").GetComponent<Transform>();
        myTransform = GameObject.Find("Camera").GetComponent<Transform>();
        canSkip = GameObject.Find("GameMaster").GetComponent<save_GM_script>().loadScene;

    }
    
    void Start ()
    {
        GameObject.Find("Player").GetComponent<player_script>().enabled = false;//Blocco lo script del player
        GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        GameObject.Find("Player").GetComponent<Animator>().SetFloat("Horizontal_Speed", 0);
        GameObject.Find("Destroyer").GetComponent<DestroyerPlayerDistance>().enabled = false;//Blocco lo script del destroyer
        GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().enabled = false;
        GameObject.Find("Destroyer").GetComponent<DestroyerPlayerStandard>().enabled = false;
        GameObject.Find("Destroyer").GetComponent<DestroyerPlayerInactivity>().enabled = false;
        GameObject.Find("Camera").GetComponent<Camera_Script>().enabled = false;//Blocco lo script del movimento della camera
        StartCoroutine(WaitTimeToSkip());
    }

    void Update ()
    {
        if (Input.anyKeyDown && isTimeToSkipPassed && canSkip)//se preme un qualsiasi tasto e non è la prima volta che c`è l effetto 
            currentPhase = 4;//salta l effetto
        if (currentPhase == 1)//se è in fase uno
        {
            if (myTransform.position.x + 1 < playerTransform.position.x)//se la x della cam è minore della x del player
                MoveTo(playerTransform.position);//Muovi verso il player
            else//altrimenti 
            {
                //Setta la posizione della camera uguale a quella del player e termina la fase uno
                currentPhase++;
            }

        }
        else if (currentPhase == 2)//se in fase due
        {
            if (myTransform.position.x - 1 > destroyerTransform.position.x)//se la x della cam è maggiore della x del destroyer
                MoveTo(destroyerTransform.position);//Muovi verso il destroyer
            else//altrimenti
            {
                //Setta la posizione della camera uguale a quella del player e termina la fase due
                currentPhase++;
            }
        }
        else if (currentPhase == 3)//se in fase tre
        {
            Vector3 playerPos = new Vector3(playerTransform.position.x + 2f, playerTransform.position.y, playerTransform.position.z);
            if (myTransform.position.x + 0.05f < playerPos.x)//se la x della cam è minore della x del player
                MoveTo(playerPos);//Muovi verso il player
            else//altrimenti 
            {
                //Setta la posizione della camera uguale a quella del player e termina la fase uno
                currentPhase++;
            }
        }
        else
        {
            //Abilita gli script
            backToGame();

        }

    }

    void MoveTo (Vector3 position)
    {
        float xMovement;
        xMovement = Vector2.MoveTowards(myTransform.position, position, velocity * Time.deltaTime).x;
        myTransform.position = new Vector3(xMovement, myTransform.position.y, myTransform.position.z);
    }

    void backToGame()
    {
        GameObject.Find("Player").GetComponent<player_script>().enabled = true;
        GameObject.Find("Destroyer").GetComponent<DestroyerPlayerDistance>().enabled = true;
        GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().enabled = true;
        GameObject.Find("Destroyer").GetComponent<DestroyerPlayerStandard>().enabled = true;
       // GameObject.Find("Destroyer").GetComponent<DestroyerPlayerInactivity>().enabled = true;
        GameObject.Find("Camera").GetComponent<Camera_Script>().enabled = true;
        //Disattiva questo script
        GetComponent<LoadScene>().enabled = false;
        GameObject.Find("GameMaster").GetComponent<save_GM_script>().loadScene = true;
    }

    IEnumerator WaitTimeToSkip()
    {
        yield return new WaitForSeconds(0.10f);
        isTimeToSkipPassed = true;
    }

}
