using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Viene abilitato in DestroyerPlayerStandard
public class LoadScene : MonoBehaviour
{
    //Velocità di movimento
    public float velocity = 8f;

    //Transforms
    Transform playerTransform, destroyerTransform, myTransform;

    //Variabile per le fasi
    int currentPhase = 1;

    void Start ()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        destroyerTransform = GameObject.Find("Destroyer").GetComponent<Transform>();
        myTransform = GetComponent<Transform>();
        GameObject.Find("Player").GetComponent<platform_movement_script>().enabled = false;//Blocco lo script del player
        GameObject.Find("Destroyer").GetComponent<DestroyerPlayerDistance>().enabled = false;//Blocco lo script del destroyer
        GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().enabled = false;
        GameObject.Find("Destroyer").GetComponent<DestroyerPlayerStandard>().enabled = false;
        GameObject.Find("Destroyer").GetComponent<DestroyerPlayerInactivity>().enabled = false;
        GetComponent<Camera_Script>().enabled = false;//Blocco lo script del movimento della camera
    }

    void Update ()
    {
        if (currentPhase == 1)//se è in fase uno
        {
            if (myTransform.position.x < playerTransform.position.x)//se la x della cam è minore della x del player
                MoveTo(playerTransform);//Muovi verso il player
            else//altrimenti 
            {
                //Setta la posizione della camera uguale a quella del player e termina la fase uno
                SetPositionTo(playerTransform);
                currentPhase++;
            }

        }
        else if (currentPhase == 2)//se in fase due
        {
            if (myTransform.position.x > destroyerTransform.position.x)//se la x della cam è maggiore della x del destroyer
                MoveTo(destroyerTransform);//Muovi verso il destroyer
            else//altrimenti
            {
                //Setta la posizione della camera uguale a quella del player e termina la fase due
                SetPositionTo(destroyerTransform);
                currentPhase++;
            }
        }
        else if (currentPhase == 3)//se in fase tre
        {
            if (myTransform.position.x < playerTransform.position.x)//se la x della cam è minore della x del player
                MoveTo(playerTransform);//Muovi verso il player
            else//altrimenti 
            {
                //Setta la posizione della camera uguale a quella del player e termina la fase uno
                SetPositionTo(playerTransform);
                currentPhase++;
                //Abilita gli script
                GameObject.Find("Player").GetComponent<platform_movement_script>().enabled = true;
                GameObject.Find("Destroyer").GetComponent<DestroyerPlayerDistance>().enabled = true;
                GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().enabled = true;
                GameObject.Find("Destroyer").GetComponent<DestroyerPlayerStandard>().enabled = true;
                GameObject.Find("Destroyer").GetComponent<DestroyerPlayerInactivity>().enabled = true;
                GetComponent<Camera_Script>().enabled = true;
                //Disattiva questo script
                GetComponent<LoadScene>().enabled = false;
            }
        }

    }

    void MoveTo (Transform targetPosition)
    {
        float xMovement;
        xMovement = Vector2.MoveTowards(myTransform.position, targetPosition.position, velocity * Time.deltaTime).x;
        myTransform.position = new Vector2(xMovement, myTransform.position.y);
    }

    void SetPositionTo (Transform targetPosition)
    {
        float xPosition;
        xPosition = targetPosition.position.x;
        myTransform.position = new Vector2(xPosition, myTransform.position.y);
    }

}
