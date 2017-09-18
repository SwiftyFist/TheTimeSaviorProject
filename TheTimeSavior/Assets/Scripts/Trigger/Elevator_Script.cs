using UnityEngine;
using UnityEngine.SceneManagement;

namespace Trigger
{
    public class Elevator_Script : MonoBehaviour
    {
        private bool _trigger;
        private Animator _myAnimator;

        private void Awake()
        {
            _myAnimator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            _trigger = true;
            _myAnimator.SetBool("Elevator_Trigger",_trigger);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            _trigger = false;
            _myAnimator.SetBool("Elevator_Trigger", _trigger);
        }

        private void Update()
        {
            if (_trigger != true || !Input.GetButtonDown("Submit")) return;
            var audioManager = FindObjectOfType<AudioManagerFmod>();
            var gun = GameObject.Find("Gun");
            if (audioManager != null)
                audioManager.StartInGameMusic();
            SceneManager.LoadScene("Level_Present");
            if(gun != null)
               gun.GetComponent<gun_script>().StopShooting();
        }
    }
}