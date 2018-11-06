/* LevelChanger.cs
 * Made for the Kinect Project of JIN 2018
 */
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KinectOverlayDemonstration
{
    /// <summary>
    /// LevelChanger
    /// Manages the transition between scenes. 
    /// Used to make a fadein-fadeout animation.
    /// </summary>
    public class LevelChanger : MonoBehaviour
    {

        [SerializeField]
        private Animator animator; // The animator responsible of the fade effect.

        private int levelToLoad;

        /// <summary>
        /// OnFadeComplete.
        /// Loads the level to load after the fading effect.
        /// </summary>
        private void OnFadeComplete()
        {
            SceneManager.LoadScene(levelToLoad);
        }

        /// <summary>
        /// Creates a fading effect to load a level.
        /// </summary>
        /// <param name="levelIndex">The level to load.</param>
        private void FadeToLevel(int levelIndex)
        {
            levelToLoad = levelIndex;
            animator.SetTrigger("FadeOut");
        }

        /// <summary>
        /// LoadMainMenu
        /// Loads the main menu of the application.
        /// </summary>
        public void LoadMainMenu()
        {
            FadeToLevel(0);
        }

        /// <summary>
        /// LoadRecognitionSession
        /// Loads the Recognition Session Level.
        /// </summary>
        public void LoadRecognitionSession()
        {
            FadeToLevel(1);
        }

        /// <summary>
        /// LoadSimpleRecognition
        /// Loads the Free Recognition Level.
        /// </summary>
        public void LoadSimpleRecognition()
        {
            FadeToLevel(2);
        }

        /// <summary>
        /// QuitApplication
        /// Shutdown the game.
        /// </summary>
        public void QuitApplication()
        {
            Application.Quit();
        }

        /// <summary>
        /// Update.
        /// Unity Standard Function, listen for an eventual return to main menu.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LoadMainMenu();
            }
        }
    }
}