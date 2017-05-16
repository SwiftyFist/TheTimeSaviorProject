using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD;
using UnityEngine.SceneManagement;


public class AudioManagerFmod : MonoBehaviour {

    [SerializeField]
    private gun_script currentGun;
    [SerializeField]
    private FMODUnity.StudioEventEmitter gunEmitter;
    [SerializeField]
    private GameObject scoreManagerCanvas;


    [HideInInspector]
    public List<EnemyAI> normalEnemyList;
    [HideInInspector]
    public List<DroneAI_v2> droneList;
    [HideInInspector]
    public Transform player;


    [FMODUnity.EventRef]
    public string enemyBank;
    [FMODUnity.EventRef]
    public string droneBank;
    [FMODUnity.EventRef]
    public string playerShoot;
    [FMODUnity.EventRef]
    public string playerMove;
    [FMODUnity.EventRef]
    public string MusicBank;
    [EventRef]
    public string minigunBank;
   


    [HideInInspector]
    public bool isMainMenu;

    StudioEventEmitter musicEmitter;

    /*private void Start()
    {
        musicEmitter = GetComponent<StudioEventEmitter>();
    }*/

    FMOD.Studio.EventInstance enemyInstance;
    FMOD.Studio.EventInstance droneInstance;
    FMOD.Studio.EventInstance playerInstance;
    FMOD.Studio.EventInstance gunInstance;
    FMOD.Studio.EventInstance musicInstance;



    private void Awake()
    {
        musicEmitter = GetComponent<StudioEventEmitter>();
        musicEmitter.Play();
    }


    public void ReloadScene()
    {
        scoreManagerCanvas.SetActive(true);
        isMainMenu = false;
        EnterGame();
        StartCoroutine(Reload());
        
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(0.1f);
        
        currentGun = FindObjectOfType<gun_script>();
        gunEmitter = currentGun.GetComponent<StudioEventEmitter>();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Menu_Main")
            isMainMenu = false;
        else
            isMainMenu = true;
        enemyInstance = FMODUnity.RuntimeManager.CreateInstance(enemyBank);
        droneInstance = FMODUnity.RuntimeManager.CreateInstance(droneBank);
        musicInstance = RuntimeManager.CreateInstance(MusicBank);
        playerInstance = RuntimeManager.CreateInstance(playerMove);
        gunInstance = RuntimeManager.CreateInstance(minigunBank);
        if(!isMainMenu)
            currentGun = FindObjectOfType<gun_script>();
       
        //MinigunActivate();
    }

    public void MinigunActivate()
    {
        gunEmitter.Play();
        
    }

    public void MinigunDeactivate()
    {
        StartCoroutine(WaitMinigun()); 
       
    }

    private IEnumerator WaitMinigun()
    {
        while (currentGun.GetRotationSpeed() > 0.2f)
        {
            yield return new WaitForFixedUpdate();
        }
        gunEmitter.Stop();
    }

    private void Update()
    {
        if (!isMainMenu)
        {
            if(currentGun == null)
            {
                currentGun = FindObjectOfType<gun_script>();
                gunEmitter = currentGun.GetComponent<StudioEventEmitter>();
            }
            gunEmitter.SetParameter("Gatling", currentGun.GetRotationSpeed());
            if (!currentGun.IsCold)
                gunEmitter.SetParameter("Surriscaldamento", 1);
            else
                gunEmitter.SetParameter("Surriscaldamento", 0);
        }
    }

    public void EnemySound(Transform enemy, float value)
    {
        enemyInstance.set3DAttributes(RuntimeUtils.To3DAttributes(enemy));
        enemyInstance.setParameterValue("value", value);
    }


    public void DroneSound(Transform drone, float value)
    {
        droneInstance.set3DAttributes(RuntimeUtils.To3DAttributes(drone));
        droneInstance.setParameterValue("value", value);

    }

    private void Stop()
    {
        /*FMOD.Studio.Bus droneBus = RuntimeManager.GetBus("bus:/drone");
        FMOD.Studio.Bus enemyBus = RuntimeManager.GetBus("bus:/enemy");
        droneBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        enemyBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        enemyInstance.release();
        droneInstance.release();*/

    }

    private void OnDestroy()
    {
        Stop();
    }
    
    public void MusicGestion(float value)
    {
        musicEmitter.SetParameter("Intro_Loop", value);
    }

    public void StartMusic()
    {
        //musicInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        musicInstance.setParameterValue("Intro_Loop", 0.1f);
        StartCoroutine(StartMusicLoop());
    }



    private IEnumerator StartMusicLoop()
    {
        yield return new WaitForSeconds(30.4f);
        musicInstance.setParameterValue("Intro_Loop", 0.5f);

    }

    public void StartInGameMusic()
    {
        //yield return new WaitForSeconds(0);
        musicEmitter.SetParameter("Intro_Loop", 2f);
    }

    public void EnterGame()
    {
        musicEmitter.SetParameter("Intro_Loop", 1);
        //StartCoroutine(StartInGameMusic());
    }
    
}
