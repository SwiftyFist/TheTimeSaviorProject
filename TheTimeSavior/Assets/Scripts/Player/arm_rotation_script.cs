//Prova GitHub

using UnityEngine;
using System.Collections;

public class arm_rotation_script : MonoBehaviour {
    //rotationOffset al momento è inutile possibile rimozione
	public int rotationOffset;
	public bool direction;
    private Transform _transform;
    private Vector3 difference;
	//Dal'inspector gli viene inserito l'oggetto vuoto dal qualche parte il proiettile
	public Transform FirePoint;

    public bool UsingMouse = true;
    [Range(0, 1)]
    public float R_analog_threshold = 0.0f;


    //La variabile bool direction viene impostata a true perchè il player una volta avviato il gioco è diretto a destra (true=destra,false=sinistra)
    void Awake()
    {
		direction = true;
        _transform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            UsingMouse = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
            UsingMouse = true;
        
    }

    void Update()
    {
		//Controller Rotation (Right Stick )  FUNZIONANTE
		//Asse x e y
        float x = Input.GetAxis("RightStickX");
        float y = Input.GetAxis("RightStickY");

        
        float aim_angle = 0.0f;
        // CANCEL ALL INPUT BELOW THIS FLOAT
        

        if (Mathf.Abs(x) < R_analog_threshold) { x = 0.0f; }

        if (Mathf.Abs(y) < R_analog_threshold) { y = 0.0f; }

        // CALCULATE ANGLE AND ROTATE
        if (!UsingMouse)
        {
            if (x != 0.0f || y != 0.0f)
            {
                aim_angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
                Debug.Log(aim_angle);
                if (direction == true) 
			    {
				    rotationOffset = 90;
			    }
			    if (direction == false) 
			    {
				    rotationOffset = -90;
			    }
			    _transform.rotation = Quaternion.Euler(0f,0f,aim_angle+rotationOffset);

			    //Due If che fanno ruotare il player in base a dove si trova l'arma
			    //Mirando a sinistra con lo stick il player ruota a sinistra in automatico, lo stesso a destra
			    if (aim_angle >= 0f && aim_angle <= 90f || aim_angle <= 0 && aim_angle >= -90f)
			    {
				    if (direction == false)
				    {
					    direction = true;
					    Flip();
				    }
			    }
			    if (aim_angle >= 100f && aim_angle <= 180f || aim_angle <= -100f && aim_angle >= -180f) 
			    {
				    if (direction == true)
				    {
					    direction = false;
					    Flip ();
				    }
			    }
            }
        }
		//Mouse Rotation FUNZIONANTE
		else
        {
            //Sottrae dalla posizione del mouse la posizione  del player e poi la normalizza
            difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _transform.position;
            difference.Normalize();

		    //Calcola l'angolo
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; 
		    //Debug.Log ("rotation = "+rotZ);
		    if (direction == true) 
		    {
			    rotationOffset = 90;
		    }
		    if (direction == false) 
		    {
			    rotationOffset = -90;
		    }
			    //Ruota il braccio
		    _transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);

			    //Due If che fanno ruotare il player in base a dove si trova l'arma
			    //Mirando a sinistra con il mouse il player ruota a sinistra in automatico, lo stesso a destra
		    if (rotZ > 0f && rotZ < 90f || rotZ < 0 && rotZ > -90f)
		    {
			    if (direction == false)
			    {
				    direction = true;
				    Flip();
			    }
		    }
		    if (rotZ > 100f && rotZ < 180f || rotZ < -100f && rotZ > -180f) 
		    {
			    if (direction == true)
			    {
				    direction = false;
				    Flip ();
			    }
		    }
        }
	}

	//Funzione che va a richiamare la funzione Gira() del player per far ruotare il player 
	void Flip()
	{
		if (direction == false && player_script.pl_script.lookRight == true || direction == true && player_script.pl_script.lookRight == false) 
		{
			player_script.pl_script.Flip ();
		}	
	
		//Fa ruotare lo sprite dell'arma correttamente
		_transform.localScale = new Vector3(_transform.localScale.x * -1, _transform.localScale.y * -1, _transform.localScale.z);
	}


}

