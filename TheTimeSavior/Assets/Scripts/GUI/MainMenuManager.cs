using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {


    [SerializeField]
    private float loadingTime = 4f;
    //[SerializeField]
    private GameObject menuPanel;
    //[SerializeField]
    private GameObject loadingMenu;
    //[SerializeField]
    private TweenScale pcScreenTween;

    private UIRoot mainRoot;
    [SerializeField]
    private float timeToWait;
    [SerializeField]
    private float timeForExpand;
    [SerializeField]
    private float sizeMoltiplier;
    [SerializeField]
    private GameObject[] menuSprites;
    [SerializeField]
    private GameObject menuCanvas;
    [SerializeField]
    private GameObject fakeLoading;


    private bool afterStart = false;
    private AudioManagerFmod audioManager;



    public void Deactivate(GameObject toDeactivate)
    {
        toDeactivate.SetActive(false);
    }

    public void Activate(GameObject toActivate)
    {
        toActivate.SetActive(true);
    }

    /*private void Awake()
    {
        mainRoot = FindObjectOfType<UIRoot>();
        mainRoot.scalingStyle = UIRoot.Scaling.Constrained;
        mainRoot.manualHeight = Screen.currentResolution.height;
        mainRoot.manualWidth = Screen.currentResolution.width;
        
    }*/


    private void Start()
    {
        StartCoroutine(WaitCoroutine(timeToWait));

        audioManager = FindObjectOfType<AudioManagerFmod>();
    }

    private void PressStart()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //pcScreenTween.PlayForward();
            StartCoroutine(LerpCoroutine(menuSprites[1].transform.localScale, menuSprites[1].transform.localScale * 2, timeForExpand, 1));
            audioManager.StartMusicLoop();
        }
    }


    
    private void Update()
    {
        //if(afterStart)
            PressStart();
    }

    public void NewGame()
    {
        StartCoroutine(FakeLoading());
    }

    private IEnumerator LerpCoroutine(Vector2 startScale, Vector2 endScale, float t, int counter)
    {
        afterStart = false;
        float time = 0;
        float addingTime = Time.deltaTime;

        while (time < t)
        {
            menuSprites[counter].transform.localScale = Vector2.Lerp(startScale, endScale, (time / t));
            time += addingTime;
            addingTime += 0.001f;
            yield return null;
        }



        if (time >= t)
        {
            menuSprites[1].SetActive(false);
            menuSprites[2].SetActive(true);
            if (counter < 2)
                StartCoroutine(LerpCoroutine(menuSprites[2].transform.localScale * 2, menuSprites[2].transform.localScale, timeForExpand, 2));
            else
            {
                yield return new WaitForSeconds(0.3f);
                menuSprites[2].SetActive(false);
                menuSprites[3].SetActive(true);
                menuCanvas.SetActive(true);
            }
        }
    }

    private IEnumerator Wait()
    {   
        yield return new WaitForSeconds(0.3f);

        

    }

    private IEnumerator WaitCoroutine(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        menuSprites[0].SetActive(false);
        menuSprites[1].SetActive(true);
        afterStart = true;
        //StartCoroutine(LerpCoroutine(menuSprites[0].transform.localScale, menuSprites[0].transform.localScale / sizeMoltiplier, timeForExpand));
    }

    public IEnumerator FakeLoading()
    {
        
        audioManager.EnterGame();
        fakeLoading.SetActive(true);
        menuCanvas.SetActive(false);
        yield return new WaitForSeconds(loadingTime);
        audioManager.ReloadScene();
        SceneManager.LoadScene("Level_Hub");
        StartCoroutine(Wait());
    }

}
