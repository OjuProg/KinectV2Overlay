/* GestureRecognitionSession.cs
 * Made for the Kinect Project of JIN 2018
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KinectOverlayDemonstration
{
    /// <summary>
    /// GestureRecognitionSession
    /// Used to offer an example of the use of the Custom Kinect Overlay.
    /// In this case, we're asking the user to make the gesture display on screen.
    /// </summary>
    public class GestureRecognitionSession : MonoBehaviour
    {
        // Used to check the gesture.
        private KinectOverlay.RecognizeGesture recognizer;

        [SerializeField]
        private KinectOverlay.SpheremanController sphereman; // The User's avatar.

        [SerializeField]
        private GestureComparisonUI gestureComparisonUI;

        [SerializeField]
        private List<GesturesForDemo> gesturesToCheck;
        private int[] numberOfSuccessPerGesture;
        private bool processingGesture; // Is the Script already processing a gesture?
        private bool successfulCheck;

        [SerializeField]
        private int trialsPerGesture;
        private int currentTrial;

        [SerializeField]
        private LevelChanger levelChanger;

        /// <summary>
        /// UpdateListOfSuccesses
        /// Check if the given gesture is the one expected.
        /// If so, increments the number of success associated.
        /// </summary>
        /// <param name="gesture">The gesture we want to compare to the expected one.</param>
        /// <returns></returns>
        private bool UpdateListOfSuccesses(GesturesForDemo gesture)
        {
            bool isASuccess = false;
            // CurrentTrial can be virtually infinite, since not capped, hence the min.
            int expectedGestureIndex = Mathf.Min(currentTrial / trialsPerGesture, gesturesToCheck.Count - 1);
            if (gesture == gesturesToCheck[expectedGestureIndex])
            {
                numberOfSuccessPerGesture[expectedGestureIndex] += 1;
                isASuccess = true;
            }

            currentTrial++;
            return isASuccess;
        }

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
            successfulCheck = UpdateListOfSuccesses(gesture);
            int expectedGestureIndex = Mathf.Min(currentTrial / trialsPerGesture, gesturesToCheck.Count - 1);

            // Displaying the feedback.
            if (gestureComparisonUI != null || sphereman != null)
            {
                // Updating the recognized gesture's name.
                if(gestureComparisonUI != null)
                {
                    gestureComparisonUI.UpdateRecognizedGestureText(gesture);
                }

                // Updating the appareance of the sphereman according to the matching result/
                if (successfulCheck && sphereman != null)
                {
                    sphereman.UpdateSphereManStatus(KinectOverlay.SpheremanController.SphereManState.VALID);
                }
                else if (sphereman != null)
                {
                    sphereman.UpdateSphereManStatus(KinectOverlay.SpheremanController.SphereManState.WRONG);
                }

                // We let the interface readable for the user a few seconds
                yield return new WaitForSeconds(3.0f);

                // Restarting the UI to a standard state.
                gestureComparisonUI.UpdateRecognizedGestureText(GesturesForDemo.NONE);
                
                if (currentTrial % trialsPerGesture == 0)
                {
                    gestureComparisonUI.UpdateExpectedGestureText(gesturesToCheck[expectedGestureIndex]);
                }
                if (currentTrial > gesturesToCheck.Count * trialsPerGesture - 1)
                {
                    gestureComparisonUI.UpdateExpectedGestureText(GesturesForDemo.PraiseToMenu);
                }

                if (sphereman != null)
                {
                    sphereman.UpdateSphereManStatus(KinectOverlay.SpheremanController.SphereManState.STANDARD);
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
                switch(recognizedGesture.GestureName)
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
                    case "SwipeUp2":
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
                if(currentTrial < gesturesToCheck.Count * trialsPerGesture)
                {
                    StartCoroutine(ProcessGesture(gestureName));
                }
                else
                {
                    // In case we're at the end of the level and we use the gesture to get back to the mainMenu.
                    if(gestureName == GesturesForDemo.PraiseTheSun && levelChanger != null)
                    {
                        levelChanger.LoadMainMenu();
                    }
                }
            }
        }

        /// <summary>
        /// Start
        /// Unity Standard Method. Use to initialize the gameloop components.
        /// </summary>
        private void Start()
        {
            recognizer = GameObject.FindObjectOfType<KinectOverlay.RecognizeGesture>();
            if (recognizer != null)
            {
                if (gesturesToCheck != null)
                {
                    numberOfSuccessPerGesture = new int[gesturesToCheck.Count];
                }

                if (gestureComparisonUI != null)
                {
                    if (gesturesToCheck != null)
                    {
                        gestureComparisonUI.UpdateExpectedGestureText(gesturesToCheck[0]);
                    }
                    else
                    {
                        gestureComparisonUI.UpdateExpectedGestureText(GesturesForDemo.NONE);
                    }
                }

                trialsPerGesture = Mathf.Max(1, trialsPerGesture);
                currentTrial = 0;
                processingGesture = false;
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
        private void Update()
        {
            if(recognizer != null)
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