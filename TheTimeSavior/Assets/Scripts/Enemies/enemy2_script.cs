/*//Modifiche del 03/01/2017
 * 
 * Invece che modificare la velocità del destroyer quando colpisce il player
 * modifica il coefficente angolare per il calcolo della velocità 
 * 
 */

using UnityEngine;
using System.Collections;

public class enemy2_script : MonoBehaviour {
	private Transform _transform;
	private Animator _animator;
	private player_script thePlayer;

	public float moveSpeed;

	public float playerRange;
	[SerializeField]
	private bool Attack;
	private bool Triggered;
	private bool Acc;
	private bool Dead;
	public LayerMask playerLayer;
	bool facingLeft = true; 
	public bool playerInRange;


	void Start () {
	
		thePlayer = FindObjectOfType<player_script> ();
		_animator = GetComponent<Animator>();
		_transform = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		playerInRange = Physics2D.OverlapCircle (transform.position, playerRange, playerLayer);

		if (playerInRange) {
			Triggered = true;
			Acc = true;
			Attack = true;
			transform.position = Vector3.MoveTowards (transform.position, thePlayer.transform.position, moveSpeed * Time.deltaTime);
			if (facingLeft && player_script.pl_script.transform.position.x > _transform.position.x || !facingLeft && player_script.pl_script.transform.position.x < _transform.position.x) {
				Gira ();
			}
			_animator.SetBool ("Triggered", Triggered);
			_animator.SetBool ("Acc", Acc);
			_animator.SetBool ("Attack", Attack);
		} else {
			Triggered = false;
			Acc = false;
			Attack = false;
			_animator.SetBool ("Triggered", Triggered);
			_animator.SetBool ("Acc", Acc);
			_animator.SetBool ("Attack", Attack);
		}
		if (enemy_health_manager_script.hl_script.enemyHealth <= 0) {
			Dead = true;
			_animator.SetBool ("Dead", Dead);
		}
		}

	void OnDrawGizmosSelected(){
	
		Gizmos.DrawSphere (transform.position , playerRange);

	}
	public void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.name == "Player" )
        {
            //destroyer_playerV2.velocityModificatorByGame += destroyer_playerV2.velocityVariationByGame[1];
            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().VelocityModificatorByGame(1);
        }
    }

	public void Gira()
	{
		
		_transform.localScale = new Vector3(_transform.localScale.x * -1, _transform.localScale.y, _transform.localScale.z);
		facingLeft = !facingLeft;
	}
}
