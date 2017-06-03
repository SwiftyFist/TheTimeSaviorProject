using UnityEngine;

public class TriggerGate : MonoBehaviour
{

    public bool Activating;
    private Transform playerTransform;

    void Awake()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (playerTransform.position.x >= (transform.position.x + 3))
        {
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
