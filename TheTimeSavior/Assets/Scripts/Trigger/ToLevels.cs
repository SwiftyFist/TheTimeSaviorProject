using GameManager;
using UnityEngine;

namespace Trigger
{
    public class ToLevels : MonoBehaviour
    {
        private bool _canGo;
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            _canGo = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            _canGo = false;
        }

        private void Update()
        {
            if (_canGo && Input.GetButtonDown("Submit"))
                LevelManager.GoToTest();
        }
    }
}
