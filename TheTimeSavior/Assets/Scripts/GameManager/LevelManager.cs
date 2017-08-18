using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManager
{
    class LevelManager : MonoBehaviour
    {
        private const string LevelPresent = "Level_Present";
        private const string LevelPast = "Level_Past";
        private const string LevelFuture = "Level_Future";
        private const string LevelTest = "Level_Test";
        private const string LevelHub = "Level_Hub";


        public static void GoToGameLevel(string level)
        {
            var playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
            var audioManager = FindObjectOfType<AudioManagerFmod>();

            playerAnimator.Play("Player_Idle");
            audioManager.StopFootstep();
            audioManager.StartInGameMusic();
            SceneManager.LoadScene(level);
            playerAnimator.Play("Player_Idle");
            GameObject.Find("Gun").GetComponent<gun_script>().StopShooting();
        }

        public static void GoToPresent()
        {
            GoToGameLevel(LevelPresent);          
        }

        public static void GoToFuture()
        {
            GoToGameLevel(LevelFuture);
        }

        public static void GoToPast()
        {
            GoToGameLevel(LevelPast);
        }

        public static void GoToTest()
        {
            GoToGameLevel(LevelTest);
        }

        public static void GoToHub()
        {
            SceneManager.LoadScene(LevelHub);
            var gun = GameObject.Find("Gun");
            if (gun != null)
                gun.GetComponent<gun_script>().StopShooting();
        }
    }
}
