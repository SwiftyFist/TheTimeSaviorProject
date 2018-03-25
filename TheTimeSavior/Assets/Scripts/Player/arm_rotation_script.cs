//Prova GitHub

using UnityEngine;
using System.Collections;
using System;

public class arm_rotation_script : MonoBehaviour {
    //rotationOffset al momento è inutile possibile rimozione
	public int rotationOffset;
	public bool isFacingRight;
    private Transform _transform;
    private Vector3 mousePositionArm;
    private Transform _playerTransform;
	//Dal'inspector gli viene inserito l'oggetto vuoto dal qualche parte il proiettile
	public Transform FirePoint;

    public bool UsingMouse = true;
    [Range(0, 1)]
    public float R_analog_threshold = 0.0f;


    //La variabile bool direction viene impostata a true perchè il player una volta avviato il gioco è diretto a destra (true=destra,false=sinistra)
    void Awake()
    {
		isFacingRight = true;
        _transform = GetComponent<Transform>();
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button7))
            UsingMouse = false;

        if (Input.GetKeyDown(KeyCode.Mouse0))
            UsingMouse = true;
    }

    void Update()
    {
        var rightSitickX = Input.GetAxis("RightStickX");
        var rightSitickY = Input.GetAxis("RightStickY");
        var aimAngle = 0.0f;        

        if (Mathf.Abs(rightSitickX) < R_analog_threshold) { rightSitickX = 0.0f; }
        if (Mathf.Abs(rightSitickY) < R_analog_threshold) { rightSitickY = 0.0f; }

        if (!UsingMouse)
        {
            if (rightSitickX != 0.0f || rightSitickY != 0.0f)
            {
                aimAngle = Mathf.Atan2(rightSitickY, rightSitickX) * Mathf.Rad2Deg;
                rotationOffset = isFacingRight ? 90 : -90;

			    _transform.rotation = Quaternion.Euler(0f,0f,aimAngle+rotationOffset);

			    if (aimAngle >= 0f && Mathf.Abs(aimAngle) <= 90f || aimAngle <= 0 && aimAngle >= -90f)
			    {
				    if (!isFacingRight)
				    {
					    isFacingRight = true;
					    Flip();
				    }
			    }
			    if (aimAngle >= 100f && aimAngle <= 180f || aimAngle <= -100f && aimAngle >= -180f) 
			    {
				    if (isFacingRight)
				    {
					    isFacingRight = false;
					    Flip ();
				    }
			    }
            }
        }
		else
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePositionArm = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _transform.position;
            var mousePositionPlayer = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _playerTransform.position;
            mousePositionArm.Normalize();

            var angle = Mathf.Atan2(mousePositionArm.y, mousePositionArm.x) * Mathf.Rad2Deg;

            rotationOffset = isFacingRight ? 90 : -90;

		    _transform.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);
            Debug.Log(mousePositionArm.x);
            if (mousePositionPlayer.x >= 0 && !isFacingRight)
		    {
                isFacingRight = true;
				Flip();
		    }
		    else if (mousePositionPlayer.x < 0 && isFacingRight) 
		    {
                isFacingRight = false;
				Flip();
		    }
        }
	}

	//Funzione che va a richiamare la funzione Gira() del player per far ruotare il player 
	void Flip()
	{
		if (isFacingRight == false && player_script.pl_script.lookRight == true || isFacingRight == true && player_script.pl_script.lookRight == false) 
		{
			player_script.pl_script.Flip ();
		}	
	
		//Fa ruotare lo sprite dell'arma correttamente
		_transform.localScale = new Vector3(_transform.localScale.x * -1, _transform.localScale.y * -1, _transform.localScale.z);
	}


}

