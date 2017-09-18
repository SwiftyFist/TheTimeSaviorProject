using UnityEngine;

public class Platform_Script : MonoBehaviour {

	private Animator _myAnimator;
	private bool _gate;
    private player_script _playerScript;

	private void Awake ()
    {
		_myAnimator = GetComponent<Animator> ();
        _playerScript = GameObject.Find("Player").GetComponent<player_script>();
    }
	
	private void OnCollisionEnter2D (Collision2D other)
	{
	    if (other.gameObject.name != "Player" || !_playerScript.isGrounded) return;
	    _gate = true;
	    _myAnimator.SetBool ("Gate", _gate);
	}

	private void OnCollisionExit2D (Collision2D other)
	{
	    if (other.gameObject.name != "Player") return;
	    _gate = false;
	    _myAnimator.SetBool ("Gate", _gate);
	}

}
