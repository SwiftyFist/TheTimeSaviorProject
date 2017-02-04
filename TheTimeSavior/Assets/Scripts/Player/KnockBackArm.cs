using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBackArm : MonoBehaviour
{
    Transform myTransform;
    Rigidbody2D myRigidBody2D;

    public float KnockBackVelocity = 2;

    private Vector3 initialPosition;
    private Vector3 maxPosition;

    public void Awake()
    {
        myTransform = GetComponent<Transform>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
        initialPosition = myTransform.position;

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
        Vector3 dir = Quaternion.AngleAxis(myTransform.rotation.y, Vector3.forward) * Vector3.right;
        if (canKnockBack())
            myRigidBody2D.AddForce(dir * KnockBackVelocity);

            
    }

    void BackInPosition()
    {
        if (myTransform.position != initialPosition)//se la transform dell arma é diversa dalla transform iniziale
            myTransform.position = Vector3.MoveTowards(myTransform.position, initialPosition, KnockBackVelocity * Time.deltaTime);//muovi verso la transform iniziale
    }

    bool IsInPosition()
    {
        if (myTransform.position == initialPosition)
            return true;
        else
            return false;
    }

    bool canKnockBack()
    {
        if (myTransform.position.x < maxPosition.x && myTransform.position.y < maxPosition.y)
            return true;
        else
            return false;
    }
}
