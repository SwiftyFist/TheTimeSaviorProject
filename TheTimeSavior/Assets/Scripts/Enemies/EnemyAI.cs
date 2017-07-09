using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyAI : MonoBehaviour
{
    public enum EStatus
    {
        Inactive,
        Walking,
        Running
    }

    //Variabili per lo stato e ìl movimento
    public float rangeToRun = 8.5f, rangeToActivate = 14; 
    public float walkVelocity = 6f;
    public float maxRunningVelocity = 10f;
    public float accelerationOnRun = 2f; 
    float myCurrentVelocity = 0f;
    public EStatus myStatus;
    public float pushBackOnHit = 6;
    public bool StayOnPlatform = false;
    public Transform RightLimit, LeftLimit;
    public Vector3 RightLimitPosition, LeftLimitPosition;
    bool IsOutOfPosition = false;
    public float LimitPushBackVelocity = 2f;

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

    private score_manager_script ScoreManager;

    public float DistanceFromPlayerToDeath;

    void Awake()
    {
        ScoreManager = GameObject.Find("Score_Manager").GetComponent<score_manager_script>();
        myAnimator = GetComponent<Animator>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        RightLimitPosition = RightLimit.position;
        LeftLimitPosition = LeftLimit.position;
        Destroy(RightLimit);
        Destroy(LeftLimit);
        SetStatus();
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(Enemy_Ground.position, 0.1f, groundLayer); //Controlla se è a terra
        if (myStatus != EStatus.Running)
            SetStatus();
    }

    void Update()
    {
        IsOutOfPosition = (myTransform.position.x >= RightLimitPosition.x) || (myTransform.position.x <= LeftLimitPosition.x);
        SetTheRightFacing();
        switch (myStatus)
        {
            case EStatus.Inactive:
                InactiveScheme();
                break;
            case EStatus.Walking:
                WalkingScheme();
                break;
            case EStatus.Running:
                RunningScheme();
                break;
        }

        
        myRigidBody2D.velocity = new Vector2(myCurrentVelocity, myRigidBody2D.velocity.y);
        if (IsOutOfPosition && StayOnPlatform)
            myRigidBody2D.velocity = new Vector2(
                LimitPushBackVelocity * (myTransform.position.x >= RightLimitPosition.x ? -1 : 1), myRigidBody2D.velocity.y);

        if (CalcDistanceFromPlayer() > DistanceFromPlayerToDeath)
            GetComponent<EnemyDeath>().DestroyEnemy(0);
    }

    //Cambia colore quando il player è in range
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, rangeToActivate);
    //    Gizmos.color = Color.black;
    //    Gizmos.DrawSphere(transform.position, rangeToRun);
    //}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var collidedGameObject = collision.gameObject;
        if (collidedGameObject.name == "Player")
        {
            var playerScript = collidedGameObject.GetComponent<player_script>();

            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().VelocityModificatorByGame(0);

            ScoreManager.EnemyDeathCountReset();

            if (!playerScript.isInvincible)
                playerScript.SetInvincible();
        }

        if (collidedGameObject.tag == "TriggerGate")
            GetComponent<EnemyDeath>().DestroyEnemy(0);
    }

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
        myAnimator.SetBool("Triggered", true);
        myAnimator.SetBool("Rotate", true);
    }

    void InactiveScheme()
    {
        myRigidBody2D.velocity = new Vector2(0, myRigidBody2D.velocity.y);
    }
    
    void WalkingScheme()
    {
        if (bIsFacingLeft)
            myCurrentVelocity = walkVelocity * -1;
        else
            myCurrentVelocity = walkVelocity;
    }
    
    void RunningScheme()
    {
        if (!called)
            lastRunningVelIncreaser = StartCoroutine(RunningVelIncreaser());
    }
    
    void SetStatus()
    {
        float distance = CalcDistanceFromPlayer();
        if (distance >= rangeToActivate)
        {
            myStatus = EStatus.Inactive;
            myAnimator.SetBool("Triggered", false);
            myAnimator.SetBool("Rotate", false);
        }
        else if (distance < rangeToActivate && distance >= rangeToRun)
        {
            myStatus = EStatus.Walking;
            myAnimator.SetBool("Triggered", false);
            myAnimator.SetBool("Rotate", false);
        }
        else if (distance < rangeToRun)
        {
            myStatus = EStatus.Running;
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
            myCurrentVelocity -= accelerationOnRun;
        else if (myCurrentVelocity < maxRunningVelocity)
            myCurrentVelocity += accelerationOnRun;

        myAnimator.SetFloat("Velocity", Mathf.Abs(myCurrentVelocity));
        lastRunningVelIncreaser = StartCoroutine(RunningVelIncreaser());
        StopCoroutine(runningVelIncreaser);
    }
}
