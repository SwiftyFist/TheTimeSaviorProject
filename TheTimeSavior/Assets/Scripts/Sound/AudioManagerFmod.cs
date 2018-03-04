using Enemies;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AudioManagerFmod : MonoBehaviour {

    public static AudioManagerFmod instance;


    [SerializeField]
    private gun_script _currentGun;
    [SerializeField]
    private StudioEventEmitter _gunEmitter;
    [SerializeField]
    private GameObject _scoreManagerCanvas;
    [SerializeField]
    private StudioEventEmitter _musicEmitter;
    [SerializeField]
    private StudioEventEmitter _backgroundEmitter;

    [HideInInspector]
    public List<EnemyAI> normalEnemyList;
    [HideInInspector]
    public List<DroneAI_v2> droneList;
    [HideInInspector]
    public Transform player;


    [EventRef]
    public string EnemyBank;
    [EventRef]
    public string DroneBank;
    [EventRef]
    public string PlayerShoot;
    [EventRef]
    public string PlayerMove = "event:/Footstep";
    [EventRef]
    public string MusicBank = "event:/Music_Future";
    [EventRef]
    public string MinigunBank = "event:/Minigun";
   


    [HideInInspector]
    public bool IsMainMenu;

    private StudioEventEmitter _footEmitter;

    


    //public delegate void OnChangeSceneHub();
   

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
        //musicEmitter = GetComponent<StudioEventEmitter>();
        if (_musicEmitter != null)
        {
            _musicEmitter.Play();
            _musicEmitter.SetParameter("Intro_Loop", 0);
            SceneManager.activeSceneChanged += OnChangeSceneHub;
        }
    }


    public void ReloadScene()
    {
        _scoreManagerCanvas.SetActive(true);
        isMainMenu = false;
        EnterGame();
        StartCoroutine(Reload());
        
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(0.1f);
        
        _currentGun = FindObjectOfType<gun_script>();
        _gunEmitter = GameObject.Find("InitialPoint").GetComponent<StudioEventEmitter>();
        player = FindObjectOfType<player_script>().transform;
        _footEmitter = player.gameObject.GetComponent<StudioEventEmitter>();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "Menu_Main")
            isMainMenu = false;
        else
            isMainMenu = true;
        enemyInstance = RuntimeManager.CreateInstance(enemyBank);
        droneInstance = RuntimeManager.CreateInstance(droneBank);
        musicInstance = RuntimeManager.CreateInstance(MusicBank);
        playerInstance = RuntimeManager.CreateInstance(playerMove);
        gunInstance = RuntimeManager.CreateInstance(minigunBank);
        if(!isMainMenu)
            _currentGun = FindObjectOfType<gun_script>();

        
        StartMusic();
        instance = this;
       
        //MinigunActivate();
    }

    public void MinigunActivate()
    {
        if(_currentGun.GetRotationSpeed() <= 1)
            _gunEmitter.Play();
    }

    public void MinigunDeactivate()
    {
        StartCoroutine(WaitMinigun()); 
       
    }

    private IEnumerator WaitMinigun()
    {
        while (_currentGun.GetRotationSpeed() > 0.2f)
        {
            yield return new WaitForFixedUpdate();
        }
        _gunEmitter.Stop();
    }

    private void Update()
    {
        if (!isMainMenu)
        {
            if(_currentGun == null)
            {
                _currentGun = FindObjectOfType<gun_script>();
                _gunEmitter = GameObject.Find("InitialPoint").GetComponent<StudioEventEmitter>();
                player = FindObjectOfType<player_script>().transform;
                _footEmitter = player.gameObject.GetComponent<StudioEventEmitter>();
            }
            
            _gunEmitter.SetParameter("rotationSpeed", _currentGun.GetRotationSpeed());
            if (!_currentGun.IsCold)
                _gunEmitter.SetParameter("isCold", 1);
            else
                _gunEmitter.SetParameter("isCold", 0);
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

    public void StartFootstep()
    {
        StartCoroutine(WaitFootstep());
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
        _musicEmitter.SetParameter("Intro_Loop", value);
    }

    public void StartMusic()
    {
        //musicInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        musicInstance.setParameterValue("Intro_Loop", 0f);
        //StartCoroutine(StartMusicLoop());
    }



    public void StartMusicLoop()
    {
        //yield return new WaitForSeconds(30.4f);
        if(SceneManager.GetActiveScene().name == "Menu_Main")
            _musicEmitter.SetParameter("Intro_Loop", 0.5f);

    }

    public void StartInGameMusic()
    {
        //yield return new WaitForSeconds(0);
        _musicEmitter.SetParameter("Intro_Loop", 2f);
    }

    public void EnterGame()
    {
        _musicEmitter.SetParameter("Intro_Loop", 1f);
        //StartCoroutine(StartInGameMusic());
    }

    public void StopFootstep()
    {
        _footEmitter.Stop();
    }

    public void OnChangeSceneHub(Scene oldScene, Scene newScene)
    {
        if (newScene.name == "Level_Present")
        {
            _backgroundEmitter.Play();
        }
        else if (newScene.name == "Level_Hub")
        {
            _backgroundEmitter.Stop();
            EnterGame();
        }
    }
    
    public IEnumerator WaitFootstep()
    {
        yield return new WaitForSeconds(00.4f);
        _footEmitter.Play();
    }
}
