using UnityEngine;
using System.Collections;

public class Platform_Script : MonoBehaviour {

	private Rigidbody2D myRigidBody;
	private Animator myAnimator;
	private bool Gate;

	void Awake () {
	
		myRigidBody = GetComponent<Rigidbody2D> ();
		myAnimator = GetComponent<Animator> ();

	}
	
	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.name == "Player" && player_script.pl_script.isGrounded == true) 
		{
			Gate = true;
			myAnimator.SetBool ("Gate", Gate);
		}


    }
	void OnCollisionExit2D (Collision2D other)
	{
		if (other.gameObject.name == "Player") 
		{
			Gate = false;
			myAnimator.SetBool ("Gate", Gate);
		}


	}

}
