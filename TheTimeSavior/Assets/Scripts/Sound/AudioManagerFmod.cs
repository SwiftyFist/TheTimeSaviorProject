using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD;

public class AudioManagerFmod : MonoBehaviour {

    [HideInInspector]
    public List<EnemyAI> normalEnemyList;
    [HideInInspector]
    public List<DroneAI_v2> droneList;
    [HideInInspector]
    public Transform player;

    [EventRef]
    public string enemyBank;
    [EventRef]
    public string droneBank;
    [EventRef]
    public string playerShoot;
    [EventRef]
    public string playerMove;


    private FMOD.Studio.EventInstance enemyInstance;
    private FMOD.Studio.EventInstance droneInstance;
    private FMOD.Studio.EventInstance playerInstance;


    private void Start()
    {
        enemyInstance = FMODUnity.RuntimeManager.CreateInstance(enemyBank);
        droneInstance = FMODUnity.RuntimeManager.CreateInstance(droneBank);
        
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
        FMOD.Studio.Bus droneBus = RuntimeManager.GetBus("bus:/drone");
        FMOD.Studio.Bus enemyBus = RuntimeManager.GetBus("bus:/enemy");
        droneBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        enemyBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
        enemyInstance.release();
        droneInstance.release();
    }

    private void OnDestroy()
    {
        Stop();
    }


}
