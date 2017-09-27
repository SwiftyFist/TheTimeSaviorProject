using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManager
{
    public class FpsCounter : MonoBehaviour
    {
        private float _deltaTime;
        private bool _show;
 
        private void Update()
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
            if (Input.GetButtonDown("FpsShow"))
                _show = !_show;
        }
 
        private void OnGUI()
        {
            if (SceneManager.GetActiveScene().name == "Menu_Main" || !_show) return;
            var width = Screen.width;
            var height = Screen.height;
            GUI.Label(
                new Rect(0, 0, width, height * 2/ 100),
                string.Format(
                    "{0:0.0} ms ({1:0.} fps)", 
                    _deltaTime * 1000.0f, 
                    1.0f / _deltaTime
                ),
                new GUIStyle
                {
                    alignment = TextAnchor.UpperRight,
                    fontSize = height * 4 / 100,
                    normal = {textColor = Color.white}
                }
            );
        }
    }
}