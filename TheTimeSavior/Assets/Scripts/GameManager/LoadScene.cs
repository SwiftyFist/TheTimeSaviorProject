using System.Collections;
using Destroyer;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.LevelMaking;

namespace GameManager
{
    public class LoadScene : MonoBehaviour
    {
        //Velocità di movimento
        public float Velocity = 8f;
        
        private Transform _playerTransform, _destroyerTransform, _myTransform;
        
        private int _currentPhase = 1;
        private bool _isTimeToSkipPassed;
        private bool _canSkip;       

        public void Awake()
        {
            _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
            _destroyerTransform = GameObject.Find("Destroyer").GetComponent<Transform>();
            _myTransform = GameObject.Find("MainCamera").GetComponent<Transform>();
            _canSkip = GameObject.Find("GameMaster").GetComponent<save_GM_script>().loadScene;
        }
    
        public void Start ()
        {
            GameObject.Find("Player").GetComponent<player_script>().enabled = false;//Blocco lo script del player
            GameObject.Find("Player").GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            GameObject.Find("Player").GetComponent<Animator>().SetFloat("Horizontal_Speed", 0);
            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerDistance>().enabled = false;//Blocco lo script del destroyer
            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().enabled = false;
            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerStandard>().enabled = false;
            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerInactivity>().enabled = false;
            GameObject.Find("MainCamera").GetComponent<Camera_Script>().enabled = false;//Blocco lo script del movimento della camera
            StartCoroutine(WaitTimeToSkip());
        }

        public void Update ()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _isTimeToSkipPassed && _canSkip)//se preme un qualsiasi tasto e non è la prima volta che c`è l effetto 
                _currentPhase = 4;//salta l effetto
            if (_currentPhase == 1)//se è in fase uno
            {
                if (_myTransform.position.x + 1 < _playerTransform.position.x)//se la x della cam è minore della x del player
                    MoveTo(_playerTransform.position);//Muovi verso il player
                else//altrimenti 
                {
                    //Setta la posizione della camera uguale a quella del player e termina la fase uno
                    _currentPhase++;
                }

            }
            else if (_currentPhase == 2)//se in fase due
            {
                if (_myTransform.position.x - 1 > _destroyerTransform.position.x)//se la x della cam è maggiore della x del destroyer
                    MoveTo(_destroyerTransform.position);//Muovi verso il destroyer
                else//altrimenti
                {
                    //Setta la posizione della camera uguale a quella del player e termina la fase due
                    _currentPhase++;
                }
            }
            else if (_currentPhase == 3)//se in fase tre
            {
                Vector3 playerPos = new Vector3(_playerTransform.position.x + 2f, _playerTransform.position.y, _playerTransform.position.z);
                if (_myTransform.position.x + 0.05f < playerPos.x)//se la x della cam è minore della x del player
                    MoveTo(playerPos);//Muovi verso il player
                else//altrimenti 
                {
                    //Setta la posizione della camera uguale a quella del player e termina la fase uno
                    _currentPhase++;
                }
            }
            else
            {
                //Abilita gli script
                BackToGame();

            }

        }

        private void MoveTo (Vector3 position)
        {
            var xMovement = Vector2.MoveTowards(_myTransform.position, position, Velocity * Time.deltaTime).x;
            _myTransform.position = new Vector3(xMovement, _myTransform.position.y, _myTransform.position.z);
        }

        private void BackToGame()
        {
            GameObject.Find("Player").GetComponent<player_script>().enabled = true;
            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerDistance>().enabled = true;
            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerGame>().enabled = true;
            GameObject.Find("Destroyer").GetComponent<DestroyerPlayerStandard>().enabled = true;
            // GameObject.Find("Destroyer").GetComponent<DestroyerPlayerInactivity>().enabled = true;
            GameObject.Find("MainCamera").GetComponent<Camera_Script>().enabled = true;
            //Disattiva questo script
            GetComponent<LoadScene>().enabled = false;
            GameObject.Find("GameMaster").GetComponent<save_GM_script>().loadScene = true;
        }

        private IEnumerator WaitTimeToSkip()
        {
            yield return new WaitForSeconds(0.10f);
            _isTimeToSkipPassed = true;
        }

    }
}
