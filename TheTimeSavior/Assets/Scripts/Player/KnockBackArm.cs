using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackArm : MonoBehaviour
{
    Transform myTransform;
    Transform limitTransform;
    Transform initialTransform;

    public float KnockBackVelocity = 2;

    void Update()
    {
        //Tora alla posizione;
    }

    public void KnockBack()
    {
        if (Mathf.Abs(myTransform.position.x) > Mathf.Abs(limitTransform.position.x) || Mathf.Abs(myTransform.position.y) > Mathf.Abs(limitTransform.position.y))//se il transform dell arma non è minore di un certo punto 
            Vector3.MoveTowards(myTransform.position, limitTransform.position, KnockBackVelocity * Time.deltaTime);//Aggiungi una forza negativa/positiva all arm in quella direzione

    }

    void BackInPosition()
    {
        //se la transform dell arma é diversa dalla transform iniziale
            //muovi verso la transform iniziale
    }
}
