using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAI : MonoBehaviour
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
    public float decelerationOnChangingDirection = 4f;
    bool bChangingDirection = false;
    float myCurrentVelocity = 0f;
    EStatus myStatus;

    //Variabili dell'oggetto
    Rigidbody2D myRigidBody2D;
    Transform myTransform;
    bool bIsFacingLeft = true;
    bool bFacingChanged = false; //Diversa in EnemyAi aggiunta al posto di isGrounded

    //Variabili del player
    Transform playerTransform;

    //Variabili per l animazione
    Animator myAnimator;

    //Variabili per l'enumeratore del'incremento di velocità
    Coroutine lastRunningVelIncreaser, runningVelIncreaser;
    bool bRunningVelIncreaserCalled = false;

    //Variabili per l'enumeratore del cambio di direzione
    Coroutine lastChangingDirectionDecreaser, changingDirectionDecreaser;
    bool bChangingDirectionDecreaserCalled = false;
    int changingDirectionDirection = -1;

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
        if (myStatus != EStatus.Running)
            SetStatus();//Aggiorna myStatus
    }

    void Update()
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

        ////////////////////////////
        //Parte diversa da EnemyAI//
        ////////////////////////////

        //if(bFacingChanged)
        //{
        //    myCurrentVelocity = 3;
        //    //bFacingChanged = false;
        //}        

        //se la X del player e la y del player sono abbastanza vicini alla X e Y del drone o è c`è stato
        if (CalcDistanceFromPlayer() < 1f || bChangingDirection)
            ChangeDirection();//Muovi solo verso destra o sinistra con una certa velocità decresente fino a 0
        else//altrimenti
            myTransform.position = Vector3.MoveTowards(myTransform.position, playerTransform.position, Mathf.Abs(myCurrentVelocity) * Time.deltaTime);//MoveTowards

        ///////////////////////////
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
            //GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().VelocityModificatorByGame(0);
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
            //bFacingChanged = true;
        }
    }

    public void SetTriggerOn()
    {
        myStatus = EStatus.Running;
        myAnimator.SetBool("Triggered", true);
    }

    //Agisce seguendo l`InactiveScheme
    void InactiveScheme()
    {
        //imposta la velocità a 0
        myRigidBody2D.velocity = new Vector2(0, myRigidBody2D.velocity.y);
    }

    //Agisce seguendo il walkingScheme
    void WalkingScheme()
    {
        //imposta la velocità in base a dove guarda il nemico
        if (bIsFacingLeft)
            myCurrentVelocity = walkVelocity * -1;
        else
            myCurrentVelocity = walkVelocity;
    }

    //Agisce seguendo il runningScheme
    void RunningScheme()
    {
        //Avvia il controllo dato dall accelerazione nello stato triggered
        if (!bRunningVelIncreaserCalled)
        {
            myCurrentVelocity = walkVelocity;
            bRunningVelIncreaserCalled = true;
            lastRunningVelIncreaser = StartCoroutine(RunningVelIncreaser());
        }
    }

    //Cambia direzione
    void ChangeDirection()
    {
        StopCoroutine(lastRunningVelIncreaser);
        //Deve diminuire la velocità fino ad arrivare a 0 
        //Quindi chiamo una coroutine 
        if (!bChangingDirectionDecreaserCalled)
        {
            bChangingDirectionDecreaserCalled = true;
            lastChangingDirectionDecreaser = StartCoroutine(ChangingDirectionDecreaser());

            if (bIsFacingLeft)
                changingDirectionDirection = -1;
            else
                changingDirectionDirection = 1;
        }

        myRigidBody2D.velocity = new Vector2(myCurrentVelocity * changingDirectionDirection, 0);
    }

    //Imposta lo status del player
    void SetStatus()
    {
        float distance = CalcDistanceFromPlayer();
        if (distance >= rangeToActivate)//se il nemico è a distanza maggiore di rangeToActive
        {
            myStatus = EStatus.Inactive;
            myAnimator.SetBool("Triggered", false);
        }
        else if (distance < rangeToActivate && distance >= rangeToRun)//se il nemico è a distanza minore di rangeToActive e minore di rangeToRun
        {
            myStatus = EStatus.Walking;
            myAnimator.SetBool("Triggered", false);
        }
        else if (distance < rangeToRun)//se il nemico è a distanza minore di rangeToRun 
        {
            myStatus = EStatus.Running;
            myAnimator.SetBool("Triggered", true);
        }
    }

    float CalcDistanceFromPlayer()
    {
        return Mathf.Abs(myTransform.position.x - playerTransform.position.x);
    }

    IEnumerator RunningVelIncreaser() //Diversa da EnemyAI
    {
        runningVelIncreaser = lastRunningVelIncreaser;
        if (myCurrentVelocity < maxRunningVelocity)
            myCurrentVelocity += accelerationOnRun;
        yield return new WaitForSeconds(0.2f);
        if (myCurrentVelocity > maxRunningVelocity)
        {
            bRunningVelIncreaserCalled = false;
            StopCoroutine(runningVelIncreaser);
            StopCoroutine(lastRunningVelIncreaser);
        }
        else
        {
            lastRunningVelIncreaser = StartCoroutine(RunningVelIncreaser());
            StopCoroutine(runningVelIncreaser);
        }
    }

    IEnumerator ChangingDirectionDecreaser()
    {
        changingDirectionDecreaser = lastChangingDirectionDecreaser;
        if (myCurrentVelocity > 0)
            myCurrentVelocity -= decelerationOnChangingDirection;

        yield return new WaitForSeconds(0.2f);

        if (myCurrentVelocity < 0)
        {
            bChangingDirectionDecreaserCalled = false;
            bChangingDirection = false;
            StopCoroutine(changingDirectionDecreaser);
            StopCoroutine(lastChangingDirectionDecreaser);
        }
        else
        {
            lastChangingDirectionDecreaser = StartCoroutine(ChangingDirectionDecreaser());
            StopCoroutine(changingDirectionDecreaser);
        }
    }
}

