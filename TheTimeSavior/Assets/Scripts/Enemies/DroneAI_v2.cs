using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAI_v2 : MonoBehaviour
{
    #region Prova1
    //enum EStatus
    //{
    //    Inactive,
    //    Walking,
    //    Triggered
    //}

    ////Variabili per lo stato e il movimento
    //public float rangeToRun = 8.5f, rangeToActivate = 14;
    //public float walkVelocity = 6f;
    //public float maxRunningVelocity = 10f;
    //public float accelerationOnRun = 2f;
    //float myCurrentVelocity = 0f;
    //EStatus myStatus;

    ////Variabili dell oggetto
    //Transform myTransform;
    //Animator myAnimator;
    //bool bIsFacingRight = false;

    ////Variabili del player
    //Transform playerTransform;

    ////Variabili Coroutine
    //Coroutine lastRunVelocityUpdater;
    //bool bRunVelocityUpdaterCalled = false;

    //private void Awake()
    //{
    //    playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    //    myTransform = GetComponent<Transform>();
    //    myAnimator = GetComponent<Animator>();
    //    SetStatus();
    //}

    //private void FixedUpdate()
    //{
    //    if(myStatus != EStatus.Triggered)
    //    {
    //        SetStatus();
    //    }
    //}

    //private void Update()
    //{
    //    SetTheRightFacing();

    //    if (myStatus == EStatus.Inactive)//se è inattivo 
    //        InactiveScheme();//rimane fermo
    //    else if (myStatus == EStatus.Walking)//se è in walking
    //        WalkingScheme();//muovi ad una data velocità
    //    else//se è triggerato
    //        TriggerScheme();//Schema di movimento principale

    //    myTransform.position =  Vector3.MoveTowards(myTransform.position, playerTransform.position, myCurrentVelocity * Time.deltaTime);
    //}

    //private void InactiveScheme()
    //{
    //    myCurrentVelocity = 0;
    //}

    //private void WalkingScheme()
    //{
    //    if (!bIsFacingRight)//Se è girato verso destra
    //        myCurrentVelocity = walkVelocity;//Setta la velocità ad un valore dato
    //    else//Se è girato verso sinistra
    //        myCurrentVelocity = walkVelocity * -1;//muovi a velocità standard verso sinistra
    //}

    //private void TriggerScheme()
    //{
    //    if(!bRunVelocityUpdaterCalled)
    //    {
    //        lastRunVelocityUpdater = StartCoroutine(RunVelocityUpdater());
    //    }

    //}

    //IEnumerator RunVelocityUpdater()
    //{
    //    if (Mathf.Abs(myCurrentVelocity) < maxRunningVelocity)
    //    {
    //        if (!bIsFacingRight)
    //            myCurrentVelocity += accelerationOnRun;
    //        else
    //            myCurrentVelocity -= accelerationOnRun;
    //    }
    //    yield return new WaitForSeconds(0.2f);
    //    lastRunVelocityUpdater = StartCoroutine(RunVelocityUpdater());

    //}

    //private void SetTheRightFacing()
    //{
    //    if (bIsFacingRight && myTransform.position.x > playerTransform.position.x )
    //    {
    //        bIsFacingRight = !bIsFacingRight;
    //        myTransform.localScale = new Vector2(myTransform.localScale.x * -1, myTransform.localScale.y);

    //    }
    //    else if (!bIsFacingRight && myTransform.position.x < playerTransform.position.x)
    //    {
    //        bIsFacingRight = !bIsFacingRight;
    //        myTransform.localScale = new Vector2(myTransform.localScale.x * -1, myTransform.localScale.y);
    //    }
    //}

    ////Imposta lo status del player
    //void SetStatus()
    //{
    //    float distance = CalcDistanceFromPlayer();
    //    if (distance >= rangeToActivate)//se il nemico è a distanza maggiore di rangeToActive
    //    {
    //        myStatus = EStatus.Inactive;
    //        myAnimator.SetBool("Triggered", false);
    //    }
    //    else if (distance < rangeToActivate && distance >= rangeToRun)//se il nemico è a distanza minore di rangeToActive e minore di rangeToRun
    //    {
    //        myStatus = EStatus.Walking;
    //        myAnimator.SetBool("Triggered", false);
    //    }
    //    else if (distance < rangeToRun)//se il nemico è a distanza minore di rangeToRun 
    //    {
    //        myStatus = EStatus.Triggered;
    //        myAnimator.SetBool("Triggered", true);
    //    }
    //}

    //float CalcDistanceFromPlayer()
    //{
    //    return Mathf.Abs(myTransform.position.x - playerTransform.position.x);
    //}

    //public void SetTriggerOn()
    //{
    //    myStatus = EStatus.Triggered;
    //    //TODO impostare le variabili da animator
    //}

    ////Quando il nemico collide con il player il destroyer player si velocizza
    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.name == "Player")
    //    {
    //        //Se collide con il player modifica la velocità del Destroyer
    //        GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().VelocityModificatorByGame(0);
    //    }
    //}
    #endregion
    enum EStatus //Tiene conto dello stato in cui si trova il nemico 
    {
        Inactive,
        Walking,
        Triggered
    }

    //Variabili per lo stato e ìl movimento
    public float rangeToRun = 8.5f, rangeToActivate = 14; //Range di attivazione della corsa e del movimento
    public float walkVelocity = 6f;
    public float maxRunningVelocity = 10f;
    public float accelerationOnRun = 2f; //variabile usata per ricavare il tempo
    float myCurrentVelocity = 0f;
    EStatus myStatus;
   

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
        //isGrounded = Physics2D.OverlapCircle(Enemy_Ground.position, 0.1f, groundLayer); //Controlla se è a terra
        if (myStatus != EStatus.Triggered)
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
            case EStatus.Triggered://Se è su Running deve arrivare a una certa velocità attraverso una certa accelerazione
                RunningScheme();
                break;
        }

        myRigidBody2D.velocity = new Vector2(myCurrentVelocity, myRigidBody2D.velocity.y);
        float y = Vector3.MoveTowards(myTransform.position, playerTransform.position, Mathf.Abs(myCurrentVelocity) * Time.deltaTime).y;//MoveTowards
        myTransform.position = new Vector2(myTransform.position.x, y);
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
            //Riporta il moltiplicatore a 1
            score_manager_script._score.EnemyDeathCountReset();
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

    public void SetTriggerOn()
    {
        myStatus = EStatus.Triggered;
        //TODO imposta le variabili dell animator
        myAnimator.SetBool("Triggered", true);
        myAnimator.SetBool("Run", true);
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
            //TODO imposta le variabili dell animator
            myAnimator.SetBool("Triggered", false);
            myAnimator.SetBool("Run", false);
        }
        else if (distance < rangeToActivate && distance >= rangeToRun)//se il nemico è a distanza minore di rangeToActive e minore di rangeToRun
        {
            myStatus = EStatus.Walking;
            //TODO imposta le variabili dell animator
            myAnimator.SetBool("Triggered", false);
            myAnimator.SetBool("Run", false);
        }
        else if (distance < rangeToRun)//se il nemico è a distanza minore di rangeToRun 
        {
            myStatus = EStatus.Triggered;
            //TODO imposta le variabili dell animator
           myAnimator.SetBool("Triggered", true);
           myAnimator.SetBool("Run", true);
        }
    }

    float CalcDistanceFromPlayer()
    {
        return Mathf.Abs(myTransform.position.x - playerTransform.position.x);
    }

    IEnumerator RunningVelIncreaser()
    {
        called = true;
        runningVelIncreaser = lastRunningVelIncreaser;
        yield return new WaitForSeconds(0.2f);
        if (bIsFacingLeft && myCurrentVelocity > (maxRunningVelocity * -1))//Se il player è a sinistra
            myCurrentVelocity -= accelerationOnRun;//accelera verso sinistra
        else if (myCurrentVelocity < maxRunningVelocity)//Se il player è a destra
            myCurrentVelocity += accelerationOnRun;//accelera verso destra
        myAnimator.SetFloat("Velocity", Mathf.Abs(myCurrentVelocity));
        lastRunningVelIncreaser = StartCoroutine(RunningVelIncreaser());
        StopCoroutine(runningVelIncreaser);
    }
}
