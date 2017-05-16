using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

    [SerializeField]
    private float timeToWait;
    [SerializeField]
    private float sizeMoltiplier;
    [SerializeField]
    private GameObject[] menuSprites;


    private int counter = 0;


    private IEnumerator LerpCoroutine(Vector2 startScale, Vector2 endScale, float t)
    {
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
            menuSprites[counter].SetActive(false);
            counter++;
            menuSprites[counter].SetActive(true);
        }
    }

}
