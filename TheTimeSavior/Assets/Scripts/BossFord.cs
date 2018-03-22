using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossFord : MonoBehaviour
{
    private Vector3 centralRayCast;
    private Vector3 leftRayCast;
    private Vector3 rightRayCast;

    public enum State
    {
        MOVE,
        SHOOT,
        RELOAD,
        JUMP,
        DASHPUNCH
    }

    private float timeToMove;
    public float timeToWait = 3.0f;
    

    public State state;
    private bool alive;
    public float changePatternTime = 10;
    private Rigidbody2D rb;

    [Header("variables for Standard Movement")]
    //variables for Standard Movement
    public List<Transform> positionPoints = new List<Transform>();
    float fracJourney = 0;
    private int positionPointsInd = 1;
    float currentLerpTime = 0;
    float lerpTime = 1f;
    public float moveSpeed = 2.0f;
    float Journey = 1;

    [Header("variables for Shooting")]
    //variables for shooting
    public float bulletSpeed = 3f;
    public GameObject target;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    [Header("variables for Reloading")]
    //variables for reloading
    private float timeToShoot= 0.0f;

    [Header("variables for Jumping")]
    //variables for jumping
    public float landingTime;
    public float gravity;
    private float time;
    public float angle = 40.0f;
    public float space;

    //float dragDown = 100f;
    //float privateRotation = 20f;

    [Header("variables for Dashing")]
    //variables for DashPunch
    public float speedDash;
    public float timeToChargeDash;

    // Use this for initialization
    void Start ()
    {
        //state = State.MOVE;

        alive = true;
        StartCoroutine("FSM");
	}

    IEnumerator FSM()
    {
        while(alive)
        {
            switch(state)
            {
                case State.MOVE:
                    StandardMove();
                    break;
                case State.SHOOT:
                    Shoot();
                    break;
                case State.RELOAD:
                    Reload();
                    break;
                case State.JUMP:
                    Jump();
                    break;
                case State.DASHPUNCH:
                    DashPunch();
                    break;
            }
            yield return null;
        }
    }

    public void StandardMove()
    {
        float deltaSpeed = Time.deltaTime * moveSpeed;
        Journey += deltaSpeed;
        transform.position = Vector3.MoveTowards(transform.position, positionPoints[positionPointsInd].position, Journey);
        
        WaitTimer();
        Journey = 0;

        if (positionPointsInd >= positionPoints.Count)
        {
            positionPoints.Reverse();
            positionPointsInd = 0;
        }

    }

    public void Shoot()
    {
        bulletSpawnPoint.LookAt(target.transform);

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(bulletSpawnPoint.forward * bulletSpeed, ForceMode2D.Impulse);
        state = State.RELOAD;
       
    }

    public void Reload()
    {
        if (timeToShoot < timeToWait)
        {
            timeToShoot += Time.deltaTime;
        }
        else
        {
            timeToShoot = 0.0f;
            state = State.SHOOT;
        }
    }

    public void Jump()
    {

        float x = transform.position.x;
        float y =  Mathf.Cos(angle);
        float deltaSpeed = /*Time.deltaTime*/ space / landingTime;
        Journey += deltaSpeed;
        transform.position = Vector3.MoveTowards(new Vector3(x, y, transform.position.z), positionPoints[positionPointsInd].position, Journey);

        if (Vector3.Distance(transform.position, positionPoints[positionPointsInd].position) <= 0.1f)
        {

            if (timeToMove < timeToWait)
            {
                timeToMove += Time.deltaTime;
                
            }
            else
            {
                Journey = 0;
                positionPointsInd++;
                if (positionPointsInd >= positionPoints.Count)
                {
                    positionPointsInd = 0;
                    positionPoints.Reverse();
                }
                timeToMove = 0.0f;
            }
        }

    }

    void DashPunch()
    {
        if (timeToMove < timeToChargeDash)
        {
            timeToMove += Time.deltaTime;
        }
        else
        {
            float deltaSpeed = Time.deltaTime * moveSpeed;
            Journey += deltaSpeed;
            transform.position = Vector3.MoveTowards(transform.position, positionPoints[positionPointsInd].position, Journey);

            if (Vector3.Distance(transform.position, positionPoints[positionPointsInd].position) <= 0.1f)
            {
                
                Journey = 0;
                positionPointsInd++;
                timeToMove = 0.0f;
            }
        }


        if (positionPointsInd >= positionPoints.Count)
        {
            positionPoints.Reverse();
            positionPointsInd = 0;
        }
    }

    void WaitTimer()
    {
        if (Vector3.Distance(transform.position, positionPoints[positionPointsInd].position) <= 0.1f)
        {

            if (timeToMove < timeToWait)
            {
                timeToMove += Time.deltaTime;
            }
            else
            {
                fracJourney = 0;
                currentLerpTime = 0;
                positionPointsInd++;
                timeToMove = 0.0f;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //if(other.tag=="Player")
        //{
        //    state = State.INVESTIGATE;
        //    investigateSpot = other.gameObject.transform.position;
        //}
    }

}
