using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackArm : MonoBehaviour
{
    Transform myTransform;
    Transform backPointTransform;

    public float backVelocity = 1;
    public float knockBackVelocity = 4;

    private Transform playerTransform;
    private Vector3 maxPosition;

    public void Awake()
    {
        myTransform = GetComponent<Transform>();
        backPointTransform = GameObject.Find("BackPoint").GetComponent<Transform>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();

    }

    private void FixedUpdate()
    {
        //TODO settare max position
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
        if (myTransform.position == playerTransform.position)
            return true;
        else
            return false;
    }

    bool canKnockBack()
    {
        return true;
        if (myTransform.position.x < maxPosition.x && myTransform.position.y < maxPosition.y)
            return true;
        else
            return false;
    }

    Vector3 GetInitialPosition()
    {
        Vector3 initialPosition;
        initialPosition.x = playerTransform.position.x - 0.033f;
        initialPosition.y = playerTransform.position.y + 0.748f;
        initialPosition.z = playerTransform.position.z;
        return initialPosition;
    }
}
