using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyAI : MonoBehaviour
{ 
    enum EStatus //Tiene conto dello stato in cui si trova il nemico 
    {
        Inactive,
        Walking,
        Running
    }

    //Variabili per lo stato e ìl movimento
    public float rangeToRun = 8.5f, rangeToActivate = 14; //Range di attivazione della corsa e del movimento
    public float walkVelocity = 6f;
    public float maxRunningVelocity = 10f;
    public float accelerationOnRun = 2f; //variabile usata per ricavare il tempo
    float myCurrentVelocity = 0f;
    EStatus myStatus;
    public float pushBackOnHit = 6;
    
    //Variabili dell'oggetto
    Rigidbody2D myRigidBody2D;
    Transform myTransform;
    bool bIsFacingLeft = true;
    bool isGrounded;
    public LayerMask groundLayer;
    public Transform Enemy_Ground;

    //Variabili del player
    Transform playerTransform;

    //Variabili per l animazione
    Animator myAnimator;

    //Variabili per l'enumeratore del'incremento di velocità
    Coroutine lastRunningVelIncreaser, runningVelIncreaser;
    bool called = false;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        SetStatus();
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(Enemy_Ground.position, 0.1f, groundLayer); //Controlla se è a terra
        if (myStatus != EStatus.Running)
            SetStatus();//Aggiorna myStatus
    }

    void Update ()
    {
        SetTheRightFacing();
        //Controlla lo stato       
        switch (myStatus)
        {
            case EStatus.Inactive://Se è su inattivo imposto la velocità a 0
                InactiveScheme();
                break;
            case EStatus.Walking://Se è su walking deve avere una certa velocità
                WalkingScheme();
                break;
            case EStatus.Running://Se è su Running deve arrivare a una certa velocità attraverso una certa accelerazione
                RunningScheme();
                break;
        }

        myRigidBody2D.velocity = new Vector2(myCurrentVelocity, myRigidBody2D.velocity.y);
    }

    //Cambia colore quando il player è in range
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, rangeToActivate);
    //    Gizmos.color = Color.black;
    //    Gizmos.DrawSphere(transform.position, rangeToRun);
    //}

    //Quando il nemico collide con il player il destroyer player si velocizza
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            //Se collide con il player modifica la velocità del Destroyer
            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().VelocityModificatorByGame(0);
        }
    }

    //Gira il nemico mettendolo verso il player
    public void SetTheRightFacing()
    {
        if (bIsFacingLeft && playerTransform.position.x > myTransform.position.x 
            || !bIsFacingLeft && playerTransform.position.x < myTransform.position.x)
        {
            myTransform.localScale = new Vector3(myTransform.localScale.x * -1, myTransform.localScale.y, myTransform.localScale.z);
            bIsFacingLeft = !bIsFacingLeft;
        }
    }

    public void SetTriggerOn ()
    {
        myStatus = EStatus.Running;
        //imposta le variabili dell animator
        myAnimator.SetBool("Triggered", true);
        myAnimator.SetBool("Rotate", true);
    }

    //Agisce seguendo l`InactiveScheme
    void InactiveScheme ()
    {
        //imposta la velocità a 0
        myRigidBody2D.velocity = new Vector2(0, myRigidBody2D.velocity.y);
    }

    //Agisce seguendo il walkingScheme
    void WalkingScheme ()
    {
        //imposta la velocità in base a dove guarda il nemico
        if (bIsFacingLeft)
            myCurrentVelocity = walkVelocity * -1;
        else
            myCurrentVelocity = walkVelocity;
    }

    //Agisce seguendo il runningScheme
    void RunningScheme ()
    {
        //Avvia il controllo dato dall accelerazione nello stato triggered
        if (!called)
        {
            lastRunningVelIncreaser = StartCoroutine(RunningVelIncreaser());
        }
    }

    //Imposta lo status del player
    void SetStatus()
    {
        float distance = CalcDistanceFromPlayer();
        if (distance >= rangeToActivate)//se il nemico è a distanza maggiore di rangeToActive
        {
            myStatus = EStatus.Inactive;
            //imposta le variabili dell animator
            myAnimator.SetBool("Triggered", false);
            myAnimator.SetBool("Rotate", false);
        }
        else if (distance < rangeToActivate && distance >= rangeToRun)//se il nemico è a distanza minore di rangeToActive e minore di rangeToRun
        {
            myStatus = EStatus.Walking;
            //imposta le variabili dell animator
            myAnimator.SetBool("Triggered", false);
            myAnimator.SetBool("Rotate", false);
        }
        else if (distance < rangeToRun)//se il nemico è a distanza minore di rangeToRun 
        {
            myStatus = EStatus.Running;
            //imposta le variabili dell animator
            myAnimator.SetBool("Triggered", true);
            myAnimator.SetBool("Rotate", true);
        }
    }

    float CalcDistanceFromPlayer ()
    {
        return Mathf.Abs(myTransform.position.x - playerTransform.position.x);
    } 

    IEnumerator RunningVelIncreaser ()
    {
        called = true;
        runningVelIncreaser = lastRunningVelIncreaser;
        yield return new WaitForSeconds(0.2f);
        if (bIsFacingLeft && myCurrentVelocity > (maxRunningVelocity * -1))//Se il player è a sinistra
            myCurrentVelocity -= accelerationOnRun;//accelera verso sinistra
        else if (myCurrentVelocity < maxRunningVelocity)//Se il player è a destra
            myCurrentVelocity += accelerationOnRun;//accelera verso destra
        lastRunningVelIncreaser = StartCoroutine(RunningVelIncreaser());
        StopCoroutine(runningVelIncreaser);
    }
}
