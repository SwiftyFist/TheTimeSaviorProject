using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DestroyerPlayerStandard : MonoBehaviour
{

    #region Variabili

    public Rigidbody2D myRigidBody2D; //RigidBody2D del destroyer
    private Transform myTransform; //Transform del destroyer

    public static float antivirVelocity = 5f; //Velocità antivirus attuale
    public float _antivirVelocity; //Velocità antivirus iniziale

    public float velocityModificatorByTime = 0.09f; //Modificatore di velocità over time
    public float maxVelocity = 7; //Velocità massima overtime
    public float minVelocity = -50; // Velocità minima

    public score_manager_script ScoreManager;

    #endregion

    #region Funzioni per Unity

    void Awake()
    {
        ModificatorSet();
        myTransform = GetComponent<Transform>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
        myRigidBody2D.velocity = Vector2.right * antivirVelocity;
        StartCoroutine(VelocityModificatorByTime()); //Aumenta la velocità overtime
        ScoreManager = GameObject.Find("Score_Manager").GetComponent<score_manager_script>();
    }

    void Update()
    {
        //Debug.Log("Velocità " + myRigidBody2D.velocity.x);
        myRigidBody2D.velocity = Vector2.right * AntivirVelocity();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            LevelReset(); //Resetta il livello se collide con il player
        }
        else if (collision.gameObject.tag!="NeverDestroy")
        {
            Destroy(collision.gameObject); //Distrugge quello che incontra per alleggerire il gioco
        }

    }

    #endregion

    #region Funzioni Interne

    void LevelReset() //Reimposta il livello e la velocità del virus
    {
        //audio_manager_script._audioM.StopSound("test");
        SceneManager.LoadScene("Level_Hub");
        GameObject.Find("Gun").GetComponent<gun_script>().StopShooting();
        DestroyerPlayerInactivity.velocityModificatorByInactivity = 0;
        player_script.pl_script.myTransform.position = player_script.pl_script.playerPosition;
        antivirVelocity = _antivirVelocity;
        score_manager_script.SendToHub();
        ScoreManager.Reset();
    }

    IEnumerator VelocityModificatorByTime() //Aumenta la velocità ogni 0.5 secondi
    {
        yield return new WaitForSeconds(0.5f);

        if (antivirVelocity < maxVelocity)
        {
            antivirVelocity = antivirVelocity + velocityModificatorByTime;
        }

        StartCoroutine(VelocityModificatorByTime());
    }

    void ModificatorSet()
    {
        antivirVelocity = _antivirVelocity;
    }

    float AntivirVelocity ()
    {
        var velocity = antivirVelocity + DestroyerPlayerDistance.velocityModificatorByDistance + 
            DestroyerPlayerGame.velocityModificatorByGame + DestroyerPlayerInactivity.velocityModificatorByInactivity;
        return velocity < minVelocity ? minVelocity : velocity;
    }


    //Attiva o disattiva il destroyer
    public void SetActive (bool activating)
    {
        GetComponent<DestroyerPlayerDistance>().enabled = activating;
        GetComponent<DestroyerPlayerGame>().enabled = activating;
        GetComponent<DestroyerPlayerStandard>().enabled = activating;

        if (!activating)
        {
            GetComponent<DestroyerPlayerInactivity>().enabled = activating;
            GetComponent<DestroyerPlayerStandard>().myRigidBody2D.velocity = new Vector3(0, 0, 0);
        }
    }

    #endregion

}


