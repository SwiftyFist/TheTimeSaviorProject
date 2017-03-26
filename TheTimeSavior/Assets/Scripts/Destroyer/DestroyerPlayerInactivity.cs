using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerPlayerInactivity : MonoBehaviour
{

    public float inactivityTime;
    bool isInactive = false;
    public static float velocityModificatorByInactivity = 0;
    public float velocityVariationTimeByInactivity = 0.2f;
    Coroutine lastInactivityDetection = null;
    Coroutine lastVelocityModificatorByInactivity = null;

    void Start()
    {
        lastInactivityDetection = StartCoroutine(InactivityDetection());
    }

    void Update()
    {
    }

    public void bulletCollided ()
    {
        isInactive = false;
        StopCoroutine(lastInactivityDetection);
        lastInactivityDetection = StartCoroutine(InactivityDetection());
    }

    IEnumerator InactivityDetection ()
    {
        yield return new WaitForSeconds(inactivityTime);
        isInactive = true;
        lastVelocityModificatorByInactivity = StartCoroutine(VelocityModificatorByInactivity());
    }

    IEnumerator VelocityModificatorByInactivity ()
    {
        yield return new WaitForSeconds(0.5f);
        if (isInactive)
        {
            velocityModificatorByInactivity += velocityVariationTimeByInactivity;
            lastVelocityModificatorByInactivity = StartCoroutine(VelocityModificatorByInactivity());
        }
        else
        {
            velocityModificatorByInactivity = 0;
        }
    }

}
