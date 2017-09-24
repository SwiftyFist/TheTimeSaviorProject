using Destroyer;
using UnityEngine;

public class TriggerGate : MonoBehaviour
{
    public float offSetActivation = 3f;
    public bool Activating;
    private bool Activated = false;
    private Transform playerTransform;

    void Awake()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (playerTransform == null) return;
        if (playerTransform.position.x >= (transform.position.x + offSetActivation) && !Activated)
        {
            Activated = true;
            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerStandard>().SetActive(Activating);
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    //void OnTriggerEnter2D(Collider2D collision)
    //{

    //    if (collision.gameObject.tag == "Player")
    //    {
    //        GameObject.Find("Destroyer").GetComponent<DestroyerPlayerStandard>().SetActive(Activating);
    //        transform.GetChild(0).gameObject.SetActive(true);
    //    }

    //}

}
