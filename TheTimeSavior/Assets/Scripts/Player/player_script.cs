using UnityEngine;
using System.Collections;
using GameManager;
using Singleton;
using UnityEngine.SceneManagement;

public class player_script : MonoBehaviour
{
    #region Variabili

    public static player_script pl_script; //Variabile usata per entrare da script esterni
	public float upgradePoints = 0; //Tiene conto del punteggio
	public Transform myTransform; //Contiene la propria transform
	public Vector3 playerPosition; //Contiene la posizione iniziale del player
    private Animator myAnimator; //Contiene l animator del player
    private Rigidbody2D myRigidBody2d; //Contiene il rigidbody2d del player
    private PlayerSoundManager playerSoundManager;
    private float horizontalAxes; //Valore dell asse verticale 
    public bool lookRight = true; //Dove guarda
    public bool isGrounded = false; //Se è a terra
    public float InvincibleTime = 1f;
    public float OffSetBecomInvulnerable = 0.5f;
    public bool isInvincible = false;
    public bool IsInMenu = false; //Controlla se il player sta guardando un Menù in caso positivo lo blocca
    public Coroutine BackVulnerable = null;
    public Coroutine BecomeInvulnerable = null;

    public float JumpForce; //Forza del salto
    private float jumpingScaleRate = 0.2f;
    private bool isHoldingJump;
    public float maxJumpTime;
    



    public float maxSpeed = 10f; //Velocità massima di movimento
    public Transform player_ground; //Transform della sottoclasse del player
    public LayerMask Layer_Ground; //Layer mask di tutto ciò che è terreno

    Coroutine lastJumping = null;

    Transform ArmTransform, InitialArmPositionTransform;

    Vector3 StartArmPosition, WalkArmPosition, RunArmPosition, JumpUpArmPosition, JumpDownArmPosition;

    private bool footstepStarted = false;
    #endregion

    #region Funzioni per Unity

    void Awake()
    {

        if (pl_script != null) 
		{
			GameObject.Destroy(gameObject);
		} 
		else 
		{
			GameObject.DontDestroyOnLoad (gameObject);
			pl_script = this;
		}
        isInvincible = false;
        myRigidBody2d = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myTransform = GetComponent<Transform>();
		playerPosition = myTransform.position;
        playerSoundManager = GetComponent<PlayerSoundManager>();
        ArmTransform = transform.GetChild(1);
        InitialArmPositionTransform = GameObject.Find("InitialPoint").GetComponent<Transform>();
        StartArmPosition = InitialArmPositionTransform.localPosition;
        RunArmPosition = new Vector3(StartArmPosition.x + 0.37f, StartArmPosition.y + 0.04f, StartArmPosition.z);
        WalkArmPosition = new Vector3(StartArmPosition.x, StartArmPosition.y + 0.10f, StartArmPosition.z);
        JumpUpArmPosition = new Vector3(StartArmPosition.x, StartArmPosition.y, StartArmPosition.z);
        JumpDownArmPosition = new Vector3(StartArmPosition.x -0.05f, StartArmPosition.y - 0.10f, StartArmPosition.z);
    }

    void Update()
    {
        if(!IsInMenu)
        {
            myRigidBody2d.velocity = new Vector2(horizontalAxes * maxSpeed, myRigidBody2d.velocity.y);
            myAnimator.SetFloat("Horizontal_Speed", Mathf.Abs(myRigidBody2d.velocity.x));

            SetArmPosition();

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                Jump();
            }
            if (!footstepStarted && isGrounded && myRigidBody2d.velocity.magnitude > 0.2f)
            {
                footstepStarted = true;
                AudioManagerFmod.instance.StartFootstep();
            }
            else if (myRigidBody2d.velocity.magnitude < 0.4f)
            {
                footstepStarted = false;
                AudioManagerFmod.instance.StopFootstep();
            }
            else if (!isGrounded)
            {
                footstepStarted = false;
                AudioManagerFmod.instance.StopFootstep();
            }
        }
     
        if (Input.GetButtonDown("Invincibility"))
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (isInvincible)
            {
                foreach (var enemy in enemies)
                    if (enemy != null)
                        Physics2D.IgnoreLayerCollision(gameObject.layer, enemy.layer, true);
            }
            else
            {
                foreach (var enemy in enemies)
                    if (enemy != null)
                        Physics2D.IgnoreLayerCollision(gameObject.layer, enemy.layer, false);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
                LevelManager.LevelReset();
        }
    }

    void FixedUpdate()
    {
        horizontalAxes = Input.GetAxis("Horizontal");
        isGrounded = Physics2D.OverlapCircle(player_ground.position, 0.2f, Layer_Ground);
        myAnimator.SetBool("Ground", isGrounded);
        myAnimator.SetFloat("Vertical_Speed", myRigidBody2d.velocity.y);       
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Codice per le piattaforme mobili
        if ((other.transform.name == "mobile_platform" && player_ground.position.y > other.transform.position.y))
        {
            transform.parent = other.transform;
        }

        //Codice per la riproduzione del suono quando viene colpito da un nemico
        if(other.collider.tag == "Enemy")
        {
            playerSoundManager.PlayEnemyHitSound();
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.name == "mobile_platform")
        {
            transform.parent = null;
        }
    }

    #endregion

    public void Flip()
    {
        lookRight = !lookRight;
        myTransform.localScale = new Vector3 (myTransform.localScale.x*-1, myTransform.localScale.y, myTransform.localScale.z);
        
    }

    void Jump()
    {
        myRigidBody2d.velocity = new Vector2(myRigidBody2d.velocity.x, 0);
        // myRigidBody2d.AddForce(Vector2.up * JumpForce);
        myRigidBody2d.velocity = new Vector2(myRigidBody2d.velocity.x, JumpForce);
        playerSoundManager.PlayJumpSound();
        isGrounded = false;
        isHoldingJump = true;
        if (lastJumping != null)
            StopCoroutine(lastJumping);
        StartCoroutine(StopHolding());
        lastJumping = StartCoroutine(Jumping());
    }

    IEnumerator StopHolding() //Aspetta fino al rilascio del tasto di sparo poi inizia a decrementare la velocità
    {
        yield return new WaitUntil(() => Input.GetButtonUp("Jump") == true);
        jumpingScaleRate = 0f;
        isHoldingJump = false;
    }

    IEnumerator Jumping()
    {
        if (jumpingScaleRate < maxJumpTime && isHoldingJump && !isGrounded)
        {
            //myRigidBody2d.AddForce(Vector2.up * 400, ForceMode2D.Force);
            myRigidBody2d.velocity = new Vector2(myRigidBody2d.velocity.x, JumpForce);
            jumpingScaleRate = jumpingScaleRate + 0.1f;
        }

        yield return new WaitForSeconds(0.04f);

        if (jumpingScaleRate < maxJumpTime && isHoldingJump && !isGrounded)
           lastJumping = StartCoroutine(Jumping());
    }

    public void SetArmPosition()
    {
        var absVelocity = Mathf.Abs(myRigidBody2d.velocity.x);

        if (!isGrounded)
        {
            if (myRigidBody2d.velocity.y > 0)
            {
                ArmTransform.localPosition = JumpUpArmPosition;
                InitialArmPositionTransform.localPosition = JumpUpArmPosition;
            }
            else
            {
                ArmTransform.localPosition = JumpDownArmPosition;
                InitialArmPositionTransform.localPosition = JumpDownArmPosition;
            }

        }
        else
        {
            if (absVelocity > 9)
            {
                ArmTransform.localPosition = RunArmPosition;
                InitialArmPositionTransform.localPosition = RunArmPosition;
            }
            else if (absVelocity > 0.1f)
            {
                ArmTransform.localPosition = WalkArmPosition;
                InitialArmPositionTransform.localPosition = WalkArmPosition;
            }
            else
            {
                ArmTransform.localPosition = StartArmPosition;
                InitialArmPositionTransform.localPosition = StartArmPosition;
            }
        }
    }

    public void SetInvincible()
    {
        if (BecomeInvulnerable == null)
            BecomeInvulnerable = StartCoroutine(
                BecomeInvulnerableEnumerator(GameObject.FindGameObjectsWithTag("Enemy"))
            );
    }

    public IEnumerator BackVulnerableEnumerator(GameObject[] enemies)
    {
        yield return new WaitForSeconds(InvincibleTime);
        SetIgnoreCollisionWithEnemy(enemies, false);
        StopCoroutine(BackVulnerable);
        BackVulnerable = null;
    }

    public IEnumerator BecomeInvulnerableEnumerator(GameObject[] enemies)
    {
        yield return new WaitForSeconds(OffSetBecomInvulnerable);
        if (BackVulnerable != null) yield break;
        SetIgnoreCollisionWithEnemy(enemies, true);
        BackVulnerable = StartCoroutine(BackVulnerableEnumerator(enemies));
        StopCoroutine(BecomeInvulnerable);
        BecomeInvulnerable = null;
    }

    public void SetIgnoreCollisionWithEnemy(GameObject[] enemies, bool active)
    {
        foreach (var enemy in enemies)
            if (enemy != null)
                Physics2D.IgnoreLayerCollision(gameObject.layer, enemy.layer, active);
        isInvincible = active;
    }
}
