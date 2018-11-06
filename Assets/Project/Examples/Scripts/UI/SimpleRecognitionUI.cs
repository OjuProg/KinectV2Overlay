/* SimpleRecognitionUI.cs
 * Made for the Kinect Project of JIN 2018
 */
using UnityEngine;
using UnityEngine.UI;

namespace KinectOverlayDemonstration
{
    /// <summary>
    /// GestureComparisonUI
    /// Scrit managing the UI for the SimpleRecognition Scene.
    /// </summary>
    public class SimpleRecognitionUI : MonoBehaviour
    {
        [SerializeField]
        private Text recognizedGestureText;

        /// <summary>
        /// UpdateRecognizedGestureText
        /// Updates the text of the UI element registered as holder of
        /// the recognized gesture's name.
        /// </summary>
        /// <param name="gesture">The gesture's type we've recognized.</param>
        public void UpdateRecognizedGesture(GesturesForDemo gesture)
        {
            if (recognizedGestureText != null)
            {
                switch (gesture)
                {
                    case GesturesForDemo.RightSwipe:
                        recognizedGestureText.text = "Right Hand Right Swipe";
                        break;
                    case GesturesForDemo.LeftSwipe:
                        recognizedGestureText.text = "Right Hand Left Swipe";
                        break;
                    case GesturesForDemo.SwipeUp:
                        recognizedGestureText.text = "Swipe Up";
                        break;
                    case GesturesForDemo.Punch:
                        recognizedGestureText.text = "Punch";
                        break;
                    case GesturesForDemo.Run:
                        recognizedGestureText.text = "Run";
                        break;
                    case GesturesForDemo.PraiseTheSun:
                        recognizedGestureText.text = "Praise the Sun! \\[T]/";
                        break;
                    case GesturesForDemo.PraiseToMenu:
                        recognizedGestureText.text = "\\[T]/ for Menu";
                        break;
                    case GesturesForDemo.NONE:
                        recognizedGestureText.text = "None";
                        break;
                    default:
                        recognizedGestureText.text = "Unknown Gesture";
                        break;
                }
            }
        }
    }
}
