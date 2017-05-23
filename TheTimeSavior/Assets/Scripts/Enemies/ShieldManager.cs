using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    float limSupAttivazioneScudiLaterali = 0.50f; //limite superiore per l attivazione degli scudi laterali
    float limInfAttivazioneScudiLaterali = -0.50f; //limite inferiore per l attivazione degli scudi laterali
    enum Shield //Enumerazione per indicare il tipo di shield
    {
        Front,
        Rear,
        Top,
        Bottom,
    }

    //Variabili del player
    Transform playerTransform;

    //Variabili dell oggetto 
    Transform myTransform;

    //Variabili dei figli
    public SpriteRenderer TopShield, BottomShield, FrontShield, RearShield;

    void Awake()
    {
        myTransform = GetComponent<Transform>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    } 

    public void SpawnShieldEnemy (Transform bullet)
    {
        limSetter();
        float xPosition = myTransform.position.x;

        if (bullet.position.y > limSupAttivazioneScudiLaterali) //Se la Y del bullet è maggiore del limSupAttivazioneScudiLaterali
            CreateShield(Shield.Top);//Spawna lo scudo alto
        else if (bullet.position.y < limInfAttivazioneScudiLaterali)//Altrimenti se minore del limInfAttivazioneScudiLaterali
            CreateShield(Shield.Bottom);//Spawna lo scudo basso
        else //Altrimenti se la Y del bullet è compresa tra i limiti rispetto la Y del nemico
            CreateShield(Shield.Front);
    }

    //Crea lo shield giusto
    void CreateShield (Shield myShield)//TODO Finire la fuznione CreateShield
    {
        SpriteRenderer mySprite = null;
        switch (myShield)
        {
            case Shield.Front:
                mySprite = FrontShield;
                break;
            case Shield.Top:
                mySprite = TopShield;
                break;
            case Shield.Bottom:
                mySprite = BottomShield;
                break;
            case Shield.Rear:
                mySprite = RearShield;
                break;
        }
        mySprite.enabled = true;
        StartCoroutine(ShieldDown(myShield, mySprite));
    }

    //Setta i limiti per l attivazione degli scudi laterali
    void limSetter () //TODO sistemare i valori
    {
        limSupAttivazioneScudiLaterali = myTransform.position.y + 1;
        limInfAttivazioneScudiLaterali = myTransform.position.y - 1;
    }

    IEnumerator ShieldDown(Shield myShield, SpriteRenderer mySprite)
    {
        yield return new WaitForSeconds(0.5f);
        mySprite.enabled = false;
    }
}
