using Assets.Scripts.Destroyer;
using UnityEngine;

namespace Assets.Scripts.Trigger
{
    public class TriggerGate : MonoBehaviour
    {

        public bool Activating;

        void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.gameObject.tag == "Player")
            {
                GameObject.Find("Destroyer").GetComponent<DestroyerPlayerStandard>().SetActive(Activating);
                transform.GetChild(0).gameObject.SetActive(true);
            }

        }

    }
}
