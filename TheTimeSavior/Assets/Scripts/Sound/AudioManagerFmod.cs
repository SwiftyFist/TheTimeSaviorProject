using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD;

public class AudioManagerFmod : MonoBehaviour {

    [SerializeField]
    private gun_script currentGun;
    [SerializeField]
    private FMODUnity.StudioEventEmitter gunEmitter;


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


    private void Start()
    {
        enemyInstance = FMODUnity.RuntimeManager.CreateInstance(enemyBank);
        droneInstance = FMODUnity.RuntimeManager.CreateInstance(droneBank);
        musicInstance = RuntimeManager.CreateInstance(MusicBank);
        playerInstance = RuntimeManager.CreateInstance(playerMove);
        gunInstance = RuntimeManager.CreateInstance(minigunBank);
        
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
        gunEmitter.SetParameter("Gatling", currentGun.GetRotationSpeed());
        
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

    private IEnumerator StartInGameMusic()
    {
        yield return new WaitForSeconds(2);
        musicInstance.setParameterValue("Intro_Loop", 2);
    }

    public void EnterGame()
    {
        musicInstance.setParameterValue("Intro_Loop", 1);
        StartCoroutine(StartInGameMusic());
    }
    
}
