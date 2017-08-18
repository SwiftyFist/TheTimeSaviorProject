using UnityEngine;
using System.Collections;
using Enemies;

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
		SpawnEffect = transform.Find("Spawn_Effect");
	}

	void Update () 
	{
		if (GetComponent<Transform>().position.x > Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x)
			Destroy(gameObject);
		bulletDir = this.gameObject.transform.forward;
		transform.Translate (Vector3.right * Time.deltaTime * moveSpeed);
		Destroy (gameObject, 0.5f);
	}

	void OnTriggerEnter2D(Collider2D colInfo)
	{
		if (colInfo.tag == "Enemy")
		{
            colInfo.gameObject.GetComponent<Enemy>().ActiveShield(transform.position);
            Destroy(gameObject);

            GameObject.Find("Camera").GetComponent<Camera_Shake_Script>().Shake(EnemyShakeAmt, EnemyShakeLenght);

            colInfo.GetComponent<EnemySoundManager>().PlayOnHitByBullet();
            colInfo.GetComponent<enemy_health_manager_script>().giveDamage(damageToGive);
            colInfo.GetComponent<Enemy>().SetTrigger();
        }

		if (colInfo.tag == "LevelObject")
        {
			Destroy (gameObject);
		}
	}



	   

 }



