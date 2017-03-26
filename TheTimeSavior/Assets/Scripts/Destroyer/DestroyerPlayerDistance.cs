using UnityEngine;
using System.Collections;

public class DestroyerPlayerDistance : MonoBehaviour
{
    public static float velocityModificatorByDistance;
    public float angularCoefficent =0.1f;

    void Update ()
    {
        VelocityModificatorByDistanceCalc();
        Debug.Log("Distanza " + DistancePlayerDestroyer());
    }

    void VelocityModificatorByDistanceCalc ()
    {
        velocityModificatorByDistance = (DistancePlayerDestroyer() + 5) * angularCoefficent;
    }

    float DistancePlayerDestroyer ()
    {
        return player_script.pl_script.transform.position.x - GetComponent<Transform>().position.x;
    }

}
