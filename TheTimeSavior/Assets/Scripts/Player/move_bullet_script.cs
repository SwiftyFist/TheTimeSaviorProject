using UnityEngine;
using System.Collections;
using Enemies;

public class move_bullet_script : MonoBehaviour
{
	public int MoveSpeed = 55;
	public int PointsToAdd = 10;
	public int DamageToGive =1;
	public float EnemyShakeAmt;
	public float EnemyShakeLenght;

	void Update () 
	{
		if (GetComponent<Transform>().position.x > Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x)
			Destroy(gameObject);
		transform.Translate (Vector3.right * Time.deltaTime * MoveSpeed);
		Destroy (gameObject, 0.5f);
	}

	void OnTriggerEnter2D(Collider2D colInfo)
	{
		if (colInfo.tag == "Enemy")
		{
            var playerPosition = GameObject.Find("Player").transform.position;
            colInfo.gameObject.GetComponent<Enemy>().ActiveShield(playerPosition);
            Destroy(gameObject);

            GameObject.Find("Camera").GetComponent<Camera_Shake_Script>().Shake(EnemyShakeAmt, EnemyShakeLenght);

            colInfo.GetComponent<EnemySoundManager>().PlayOnHitByBullet();
            colInfo.GetComponent<enemy_health_manager_script>().giveDamage(DamageToGive);
            colInfo.GetComponent<Enemy>().SetTrigger();
        }

		if (colInfo.tag == "LevelObject")
			Destroy (gameObject);
	}
}



