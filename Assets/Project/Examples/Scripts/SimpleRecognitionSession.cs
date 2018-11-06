/* LevelChanger.cs
 * Made for the Kinect Project of JIN 2018
 */
using System.Collections;
using UnityEngine;

namespace KinectOverlayDemonstration
{
    /// <summary>
    /// GestureRecognitionSession
    /// Used to offer an example of the use of the Custom Kinect Overlay.
    /// In this case, the scene will recongnized the gestures and notices the user
    /// which gesture it has recognized..
    /// </summary>
    public class SimpleRecognitionSession : MonoBehaviour
    {
        // Used to check the gesture.
        private KinectOverlay.RecognizeGesture recognizer;
        private GesturesForDemo lastRecognizedGesture;

        [SerializeField]
        private SimpleRecognitionUI recognitionUI;

        [SerializeField]
        private LevelChanger levelChanger;

        private bool processingGesture;

        /// <summary>
        /// ProcessGesture
        /// Given a gesture, this function will make all the actions related 
        /// to its identification on the gameloop.
        /// </summary>
        /// <param name="gesture">The gesture to process.</param>
        /// <returns></returns>
        public IEnumerator ProcessGesture(GesturesForDemo gesture)
        {
            processingGesture = true;
            lastRecognizedGesture = gesture;

            // Updating the UI
            if (recognitionUI != null)
            {
                recognitionUI.UpdateRecognizedGesture(gesture);
                // We let the interface readable for the user a few seconds
                yield return new WaitForSeconds(3.0f);

                /* In case the player has made a praise the sun, 
                 * doing another one will take him back to menu
                 */
                if(gesture == GesturesForDemo.PraiseTheSun)
                {
                    recognitionUI.UpdateRecognizedGesture(GesturesForDemo.PraiseToMenu);
                }
                else
                {
                    recognitionUI.UpdateRecognizedGesture(GesturesForDemo.NONE);
                }
            }

            processingGesture = false;
        }

        /// <summary>
        /// OnGestureRecognition
        /// Given a gesture, this function will identify it and, if no other gesture is being process,
        /// will process to call the recognition procedure.
        /// </summary>
        /// <param name="recognizedGesture"></param>
        public void OnGestureRecognition(KinectOverlay.Gesture recognizedGesture)
        {
            if (!processingGesture)
            {
                GesturesForDemo gestureName = GesturesForDemo.NONE;
                switch (recognizedGesture.GestureName)
                {
                    case "SwipeRight":
                        gestureName = GesturesForDemo.RightSwipe;
                        break;
                    case "SwipeLeft":
                        gestureName = GesturesForDemo.LeftSwipe;
                        break;
                    case "SwipeUp":
                        gestureName = GesturesForDemo.SwipeUp;
                        break;
                    case "Punch":
                        gestureName = GesturesForDemo.Punch;
                        break;
                    case "Run":
                        gestureName = GesturesForDemo.Run;
                        break;
                    case "PraiseTheSun":
                        gestureName = GesturesForDemo.PraiseTheSun;
                        break;
                    default:
                        break;
                }

                // In case the user made the praise the sun gesture two times, it gets back to the menue.
                if (levelChanger != null && lastRecognizedGesture == GesturesForDemo.PraiseTheSun && 
                    gestureName == GesturesForDemo.PraiseTheSun)
                {
                    levelChanger.LoadMainMenu();
                }
                else
                {
                    StartCoroutine(ProcessGesture(gestureName));
                }
            }
        }

        /// <summary>
        /// Start
        /// Unity Standard Method. Use to initialize the gameloop components.
        /// </summary>
        void Start()
        {
            recognizer = GameObject.FindObjectOfType<KinectOverlay.RecognizeGesture>();


            if (recognizer != null)
            {
                processingGesture = false;
                lastRecognizedGesture = GesturesForDemo.NONE;
                if (recognitionUI != null)
                {
                    recognitionUI.UpdateRecognizedGesture(GesturesForDemo.NONE);
                }
            }
            else
            {
                Debug.LogError("Error, no RecognizeGesture component can be found. Recognition can't be done without it.");
            }
        }

        /// <summary>
        /// Update
        /// Standard Unity Method, if it can, it researches a gesture. 
        /// </summary>
        void Update()
        {
            if (recognizer != null)
            {
                KinectOverlay.Gesture recognizedGesture = recognizer.TryToGetGesture();

                if (recognizedGesture.GestureName != null)
                {
                    OnGestureRecognition(recognizedGesture);
                }
            }
        }
    }
}
