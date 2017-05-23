using UnityEngine;

namespace Assets.Scripts.Destroyer
{
    public class DestroyerPlayerDistance : MonoBehaviour
    {
        public static float VelocityModificatorByDistance;
        public float AngularCoefficent =0.1f;

        void Update ()
        {
            VelocityModificatorByDistanceCalc();
        }

        void VelocityModificatorByDistanceCalc ()
        {
            VelocityModificatorByDistance = (DistancePlayerDestroyer() + 5) * AngularCoefficent;
        }

        float DistancePlayerDestroyer ()
        {
            return player_script.pl_script.transform.position.x - GetComponent<Transform>().position.x;
        }

    }
}
