using UnityEngine;
using System.Collections;

public class move_bullet_script : MonoBehaviour {

	//Velocità del proiettile 
	public int moveSpeed ;
	public int pointsToAdd ;
	public int damageToGive;


	//Effetto Colpo Scudo
	Transform SpawnEffect;
	public Transform EffectPrefab;

	//Camera Shake
	public float EnemyShakeAmt ;
	public float EnemyShakeLenght ;

	Vector3 bulletDir;

	void Awake()
	{
		SpawnEffect = transform.FindChild("Spawn_Effect");
	}


	void Update () 
	{
		if (GetComponent<Transform>().position.x > Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x)
			Destroy(gameObject);
		bulletDir = this.gameObject.transform.forward;
		//Fa un translate del proiettile e dopo due secondi viene distrutto il proiettile
		transform.Translate (Vector3.right * Time.deltaTime * moveSpeed);
		Destroy (gameObject, 0.5f);


	}



	// Se il proiettile collide con il nemico questo viene danneggiato di 1 attraverso la funzione giveDamage del enemy health manager
	// Ogni volta che il proiettile collide con nemici o elementi di gioco questo viene distrutto

	void OnTriggerEnter2D(Collider2D colInfo)
	{
		if (colInfo.tag == "Enemy")
		{
			if (colInfo.name == "Enemy") {
				

				//Effetto Scudo
				Transform test = Instantiate(EffectPrefab, SpawnEffect.position, SpawnEffect.rotation) as Transform;

				test.localScale = new Vector3 (1,1,1);
				Destroy (test.gameObject,0.03f);

				Destroy (gameObject);
				//Camera Shake
				GameObject.Find("Camera").GetComponent<Camera_Shake_Script>().Shake (EnemyShakeAmt, EnemyShakeLenght);
                


				//Suono Collisione proiettile --> nemico 1
				colInfo.GetComponent<EnemySoundManager>().PlayOnHitByBullet();
				//Il nemico perde vita
				colInfo.GetComponent<enemy_health_manager_script>().giveDamage (damageToGive);
	            //Se il nemico viene colpito si triggera in automatico
				colInfo.GetComponent<EnemyAI>().SetTriggerOn();
			}
			if (colInfo.name == "Enemy_Type_2") {


				Transform test = Instantiate(EffectPrefab, SpawnEffect.position, SpawnEffect.rotation) as Transform;

				test.localScale = new Vector3 (1,1,1);
				Destroy (test.gameObject,0.05f);


				Destroy (gameObject);
				//Camera Shake
				GameObject.Find("Camera").GetComponent<Camera_Shake_Script>().Shake (EnemyShakeAmt, EnemyShakeLenght);


                
				//Suono Collisione proiettile --> nemico 2
				colInfo.GetComponent<EnemySoundManager>().PlayOnHitByBullet();
				//Il nemico perde vita
				colInfo.GetComponent<enemy_health_manager_script> ().giveDamage (damageToGive);
                //Se il nemico viene colpito si triggera in automatico
                colInfo.GetComponent<DroneAI_v2>().SetTriggerOn();

            }

           // GameObject.Find("Destroyer").GetComponent<DestroyerPlayerInactivity>().bulletCollided();
		}

		if (colInfo.tag == "LevelObject"){
			Destroy (gameObject);
		}
	}



	   

 }



