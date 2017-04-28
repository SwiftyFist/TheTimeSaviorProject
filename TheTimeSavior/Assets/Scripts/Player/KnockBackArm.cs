using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackArm : MonoBehaviour
{
    Transform myTransform;
    Transform backPointTransform;

    public float backVelocity = 1;
    public float knockBackVelocity = 4;

    private Transform initialPointTransform;

    public void Awake()
    {
        myTransform = GetComponent<Transform>();
        backPointTransform = GameObject.Find("BackPoint").GetComponent<Transform>();
        initialPointTransform = GameObject.Find("InitialPoint").GetComponent<Transform>();

    }

    void Update()
    {
        BackInPosition();
    }

    public void KnockBack()
    {
        //muovo con il movetowards verso quel punto
        myTransform.position = Vector3.MoveTowards(myTransform.position, backPointTransform.position, knockBackVelocity * Time.deltaTime);        
                    
    }

    void BackInPosition()
    {
        if (myTransform.position != GetInitialPosition())//se la transform dell arma é diversa dalla transform iniziale
            myTransform.position = Vector3.MoveTowards(myTransform.position, GetInitialPosition(), backVelocity * Time.deltaTime);//muovi verso la transform iniziale
    }

    bool IsInPosition()
    {
        if (myTransform.position == initialPointTransform.position)
            return true;
        else
            return false;
    }

    Vector3 GetInitialPosition()
    {
        return initialPointTransform.position;
    }
}
