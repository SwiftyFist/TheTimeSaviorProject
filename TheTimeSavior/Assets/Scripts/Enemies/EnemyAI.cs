using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyAI : MonoBehaviour
{
    public enum EStatus //Tiene conto dello stato in cui si trova il nemico 
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
    public EStatus myStatus;
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

    private bool notAtEdge;
    public Transform edgeCheck;
    [SerializeField]
    private bool stayOnCurrentPlatform; //Choose via inspector if the current enemy can stay over the current platform(the field [serializefield] mean that it can be modified by inspector, also if the var is private or protected
    [SerializeField]
    private float[] minMaxX = new float[2]; //Max and min x for clamp the enemy's transform. min = 0, max = 1(array position)

    //private Transform platformUnderMe; //The tranform of the platform under the current enemy

    //private bool moveClamped = false; //bool that make the enemy move clamped on a platform

    private score_manager_script ScoreManager;

    void Awake()
    {
        ScoreManager = GameObject.Find("Score_Manager").GetComponent<score_manager_script>();
        myAnimator = GetComponent<Animator>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        SetStatus();
    }

    void FixedUpdate()
    {
        notAtEdge = Physics2D.OverlapCircle(edgeCheck.position, 0.2f, groundLayer);


        isGrounded = Physics2D.OverlapCircle(Enemy_Ground.position, 0.1f, groundLayer); //Controlla se è a terra
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
        var collidedGameObject = collision.gameObject;
        if (collidedGameObject.name == "Player")
        {
            var playerScript = collidedGameObject.GetComponent<player_script>();
            //Se collide con il player modifica la velocità del Destroyer
            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().VelocityModificatorByGame(0);
            //Riporta il moltiplicatore a 1
           ScoreManager.EnemyDeathCountReset();
            //if (!playerScript.isInvincible)
            //    playerScript.SetInvincible();
        }

        if (collidedGameObject.tag == "TriggerGate")
        {
            GetComponent<EnemyDeath>().DestroyEnemy(0);
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
        myStatus = EStatus.Running;
        //imposta le variabili dell animator
        myAnimator.SetBool("Triggered", true);
        myAnimator.SetBool("Rotate", true);
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

        //control if the enemy have to move clamped
        if (stayOnCurrentPlatform)
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, minMaxX[0] + 28.55f, minMaxX[1] + 28.55f), transform.position.y);
    }

    //Agisce seguendo il runningScheme
    void RunningScheme()
    {
        //Avvia il controllo dato dall accelerazione nello stato triggered
        if (!called)
        {
            lastRunningVelIncreaser = StartCoroutine(RunningVelIncreaser());
        }

        //control if the enemy have to move clamped
        if (stayOnCurrentPlatform)
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, minMaxX[0] + 28.55f, minMaxX[1] + 28.55f), transform.position.y);
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

    float CalcDistanceFromPlayer()
    {
        return Mathf.Abs(myTransform.position.x - playerTransform.position.x);
    }

    IEnumerator RunningVelIncreaser()
    {
        called = true;
        runningVelIncreaser = lastRunningVelIncreaser;
        yield return new WaitForSeconds(0.2f);
        if (bIsFacingLeft && myCurrentVelocity > (maxRunningVelocity * -1))
        {
            myCurrentVelocity -= accelerationOnRun;//accelera verso sinistra

        }
        //Se il player è a sinistra

        else if (myCurrentVelocity < maxRunningVelocity)//Se il player è a destra
            myCurrentVelocity += accelerationOnRun;//accelera verso destra
        myAnimator.SetFloat("Velocity", Mathf.Abs(myCurrentVelocity));
        lastRunningVelIncreaser = StartCoroutine(RunningVelIncreaser());
        StopCoroutine(runningVelIncreaser);
    }

    /// <summary>
    /// Function to clamp the position of the current enemy if the variable stayOnCurrentPlatform is true
    /// </summary>
    /*private void ClampPositionBetweenPlatformEdge()
    {
        //"shoot" a ray under the enemy, if the tag of the hitted obj, set to clamp the position
        if (!moveClamped && Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.1f), Vector2.down, 1))//.transform.CompareTag("LevelObject"))
        {
            //Take the box collider of the platform under the enemy
            BoxCollider2D platformColl = Physics2D.Raycast(transform.position, Vector2.down, 1).transform.GetComponent<BoxCollider2D>();
            minMaxX[0] = platformColl.transform.position.x - platformColl.bounds.extents.x;
            minMaxX[1] = platformColl.transform.position.x + platformColl.bounds.extents.x;
            moveClamped = true;
            Debug.Log("Son clampato");
        }
        /*else
        {
            moveClamped = false;
            stayOnCurrentPlatform = false;
        }
        
    }*/
}
