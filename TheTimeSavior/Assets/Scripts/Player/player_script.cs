using UnityEngine;
using System.Collections;

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

    public float JumpForce = 1400f; //Forza del salto
    private float jumpingScaleRate = 0.2f;
    private bool isHoldingJump = false;
    private float maxJumpTime = 0.8f;
    public float maxSpeed = 10f; //Velocità massima di movimento
    public Transform player_ground; //Transform della sottoclasse del player
    public LayerMask Layer_Ground; //Layer mask di tutto ciò che è terreno

    Coroutine lastJumping = null;

    #endregion

    #region Funzioni per Unity

    void Awake()
    {

		if (pl_script != null) 
		{
			GameObject.Destroy (gameObject);
		} 
		else 
		{
			GameObject.DontDestroyOnLoad (gameObject);
			pl_script = this;
		}

        myRigidBody2d = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myTransform = GetComponent<Transform>();
		playerPosition = myTransform.position;
        playerSoundManager = GetComponent<PlayerSoundManager>();
    }

    void Update()
    {
        myRigidBody2d.velocity = new Vector2(horizontalAxes * maxSpeed, myRigidBody2d.velocity.y);
        myAnimator.SetFloat("Horizontal_Speed", Mathf.Abs(myRigidBody2d.velocity.x));

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
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
        myRigidBody2d.AddForce(Vector2.up * JumpForce);
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
			myRigidBody2d.AddForce(Vector2.up * 200, ForceMode2D.Force);
            jumpingScaleRate = jumpingScaleRate + 0.1f;
        }

        yield return new WaitForSeconds(0.1f);

        if (jumpingScaleRate < maxJumpTime && isHoldingJump && !isGrounded)
           lastJumping = StartCoroutine(Jumping());
    }


}
