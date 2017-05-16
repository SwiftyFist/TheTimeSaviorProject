using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy2_script : MonoBehaviour {

    public float movespeed = 4;
    Rigidbody2D myRigidBody2D;
    Transform myTransform;
    Animator myAnimator;
    Transform playerTransform;
    private score_manager_script ScoreManager;
    public enum EStatus //Tiene conto dello stato in cui si trova il nemico 
    {
        Inactive,
        Walking,
        Triggered
    }
    public EStatus myStatus;


    // Use this for initialization
    void Awake () {

        myRigidBody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myTransform = GetComponent<Transform>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        ScoreManager = GameObject.Find("Score_Manager").GetComponent<score_manager_script>();
    }

    // Update is called once per frame
    void Update()
    {

        if (myStatus == EStatus.Triggered)
        {
             myAnimator.SetBool("Triggered", true);
            if (myTransform.position.x < playerTransform.position.x)
            {
                myTransform.localScale = new Vector3(-1, myTransform.localScale.y, myTransform.localScale.z);
                myRigidBody2D.velocity = new Vector2(1 * movespeed, myRigidBody2D.velocity.y);
            }
            if (myTransform.position.x > playerTransform.position.x)
            {
                myTransform.localScale = new Vector3(1, myTransform.localScale.y, myTransform.localScale.z);
                myRigidBody2D.velocity = new Vector2(-1 * movespeed, myRigidBody2D.velocity.y);
            }
        }
        if (myStatus != EStatus.Triggered)
        {
             myAnimator.SetBool("Triggered", false);
            myRigidBody2D.velocity = new Vector2(0, myRigidBody2D.velocity.y);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            SetTriggerOn();
        }
        }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            myStatus = EStatus.Walking;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            //Se collide con il player modifica la velocità del Destroyer
            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().VelocityModificatorByGame(0);
            //Riporta il moltiplicatore a 1
            ScoreManager.EnemyDeathCountReset();
        }
    }

    public void SetTriggerOn()
    {
        myStatus = EStatus.Triggered;
    }
}
