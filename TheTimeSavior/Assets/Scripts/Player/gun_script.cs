using UnityEngine;
using System.Collections;

public class gun_script : MonoBehaviour
{
    #region Variabili
    //Bullet
    public Transform BulletMinigunPrefab;
    public Transform BulletSingleShotPrefab;
    public Transform FlashPrefab;
	Transform FirePoint;
    Coroutine Shooting1;

    //SingleShot
    public float singleShotWaitTime = 0.5f;
    private bool singleShootAvailable = true;
    private Coroutine singleShotCoroutine = null;

    //Camera Shake
    public float camShakeAmt = 0.1f; // <------ Questa qua
	public float camShakeLenght = 0.1f;
	public float minCamShake = 0.01f, maxCamShake = 0.2f;
    

    //Velocità di sparo
    bool enumerationStarted;
    bool isCoolingDown = false;
    bool isCold = true;
    bool isHolding;
    private float fireRateBackUp;
    public float shootStartDelay = 0.2f;
    public float maxFireRate = 0.4f;
    private float fireRate = 0.5f;
    private float timeToOverHeat = 0;
    public float maxShootSpeed = 0.2f; //Velocità massima di sparo
    public float shootSpeedIncrement = 0.02f; //Velocità con cui aumenta il rateo
    public float maxTimeToOverHeat = 3f; //Tempo di rateo massimo
    public float overHeatTime = 1f; //Tempo di raffreddamento
    private AudioManagerFmod audioManager;

    //Animazione sparo
    Animator GunRotation;
    public float minRotationVelocity = 0f, maxRotationVelocity = 10f;


    private Gun_Shell_Pool shellPool;


    public bool IsCold
    {
        get { return isCold; }
    }




    #endregion

    #region Funzioni per Unity

    void Awake ()
    {
        GunRotation = GameObject.Find("spr_gun").GetComponent<Animator>();
        fireRate = maxFireRate;
        fireRateBackUp = fireRate;
        FirePoint = transform.Find("FirePoint");
	
        if (FirePoint == null)
        {
            Debug.LogError("No Firepoint");
        }
	}

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManagerFmod>();
        shellPool = FindObjectOfType<Gun_Shell_Pool>();
    }


    private void FixedUpdate()
    {
        GunRotation.SetFloat("SpeedVelocity", GetRotationSpeed());
    }

    void Update ()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isHolding = true;
            if (enumerationStarted == false && isCold)
            {
                StopAllCoroutines();
                singleShootAvailable = true;
                enumerationStarted = true;
                StartCoroutine(StopHolding());
                StartCoroutine(StartShooting());
                audioManager.MinigunActivate();
            }
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            if (singleShootAvailable)
            {
                Shoot(BulletSingleShotPrefab);
                singleShootAvailable = false;
                singleShotCoroutine = StartCoroutine(singleShotCoolTime());
            }

        }
    }

    #endregion

    void Shoot(Transform bulletType)
    {
        Effect(bulletType);
        shellPool.ShootShell();
        GameObject.Find("Arm").GetComponent<KnockBackArm>().KnockBack();
    }

    void Effect(Transform bulletType)
    {
        // Crea il Bullet
        Instantiate(bulletType, FirePoint.position, FirePoint.rotation);
        // Crea il Muzzle Flash
        Transform clone = Instantiate(FlashPrefab, FirePoint.position, FirePoint.rotation) as Transform;
        clone.parent = FirePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        //Distrugge il Muzzle Flash
        Destroy(clone.gameObject, 0.04f);
        GetComponent<AudioSource>().Play();

        //Camera Shake
        GameObject.Find("Camera").GetComponent<Gun_Shake_Script>().Shake(GetCamShakeAmt(), camShakeLenght);
    }

    float GetCamShakeAmt()
    {
        float m, q;
        m = (minCamShake - maxCamShake) / (fireRate - maxShootSpeed);
        q = minCamShake - (m * fireRate);
        return (m * fireRate) + (q);
    }

    #region Minigun

    IEnumerator StopHolding () //Aspetta fino al rilascio del tasto di sparo poi inizia a decrementare la velocità
    {
        yield return new WaitUntil(() => Input.GetButtonUp("Fire1") == true);
        isHolding = false;
        isCoolingDown = false;
        timeToOverHeat = 0;
        StartCoroutine(ShootDecrease());
        enumerationStarted = false;
 
    }

    IEnumerator ShootDecrease() //Decrementa la velocità fintanto che il tasto non è premuto
    {
        if (!isHolding)
        {
            audioManager.MinigunDeactivate();
            if (fireRate < fireRateBackUp)
                fireRate = fireRate + shootSpeedIncrement;
        }

        yield return new WaitForSeconds(fireRate);

        if (!isHolding && fireRate <= fireRateBackUp)
        {
            StartCoroutine(ShootDecrease());
        }

    }

    IEnumerator StartShooting()
    {
        yield return new WaitForSeconds(shootStartDelay);

        if (isHolding && isCold)
        {
            Shoot(BulletMinigunPrefab);
            //TODO Chiamata al knock back
            StartCoroutine(Shooting());
        }
    }

    IEnumerator Shooting () //Si occupa dello sparo e dà inizio all overHeat
    {
        if (isHolding && isCold)
        {
            if (fireRate > maxShootSpeed)
                fireRate = fireRate - shootSpeedIncrement;
            else if (!isCoolingDown)
               StartCoroutine(ToOverHeat());
        }
                       
        yield return new WaitForSeconds(fireRate);

        if (isHolding && isCold)
        {
            Shoot(BulletMinigunPrefab);
            StartCoroutine(Shooting());
        }
    }
    
    IEnumerator ToOverHeat () //Arrivato al rateo massimo tiene controllata la scalata al surriscaldamento
    {
        isCoolingDown = true;

        if (isHolding && timeToOverHeat <= maxTimeToOverHeat)
        {
            timeToOverHeat = timeToOverHeat + 0.2f;
        }

        yield return new WaitForSeconds(0.2f);
        
        if (isHolding && enumerationStarted )
        {
            if (timeToOverHeat >= maxTimeToOverHeat)
            {
                isCold = false;
                StartCoroutine(CoolingDown());
            }
            else
            {
                StartCoroutine(ToOverHeat());
            }

        }
        
    }

    IEnumerator CoolingDown () //Gestisce il surriscaldamento
    {

        fireRate = fireRateBackUp;
        timeToOverHeat = 0;
        isCoolingDown = false;

        yield return new WaitForSeconds(overHeatTime);
 
        isCold = true;
    }

    public void StopShooting ()
    {
        enumerationStarted = false;
        isHolding = false;
        fireRate = fireRateBackUp;
        
    }

    //Converte il fire rate in velocità di rotazione per l animazione dello sparo
    public float GetRotationSpeed()
    {
        if (fireRate == maxFireRate)
            return 0;

        float m, q;
        m = ((minRotationVelocity - maxRotationVelocity) / (maxFireRate - maxShootSpeed));
        q = (minRotationVelocity - (m * maxFireRate));
        float rotationSpeed = ((m * fireRate) + q);
        return rotationSpeed;
    }
    #endregion

    #region SingleShot

    IEnumerator singleShotCoolTime()
    {
        yield return new WaitForSeconds(singleShotWaitTime);
        singleShootAvailable = true;
        StopCoroutine(singleShotCoroutine);
    }
    #endregion



}
