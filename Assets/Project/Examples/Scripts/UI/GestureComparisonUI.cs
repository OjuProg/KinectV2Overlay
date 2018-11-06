/* GestureComparisonUI.cs
 * Made for the Kinect Project of JIN 2018
 */
using UnityEngine;
using UnityEngine.UI;

namespace KinectOverlayDemonstration
{
    /// <summary>
    /// GestureComparisonUI
    /// Scrit managing the UI for the GestureComparison Scene.
    /// </summary>
    public class GestureComparisonUI : MonoBehaviour
    {

        [SerializeField]
        private Image gestureComparisonPanel;

        [SerializeField]
        private Text expectedGestureText;
        [SerializeField]
        private GesturesForDemo expectedGesture;

        [SerializeField]
        private Text recognizedGestureText;
        private GesturesForDemo recognizedGesture;

        [SerializeField]
        private Color matchingGesturesColor;
        [SerializeField]
        private Color differentGesturesColor;
        [SerializeField]
        private Color standardGesturesColor;

        /// <summary>
        /// Start
        /// Standard Unity Method. Use to initialize the UI.
        /// </summary>
        private void Start()
        {
            UpdateExpectedGestureText(expectedGesture);
            UpdateRecognizedGestureText(GesturesForDemo.NONE);
        }

        /// <summary>
        /// UpdateUIColor
        /// Updates the UI's color according to the matching of the gesture.
        /// </summary>
        private void UpdateUIColor()
        {
            if (gestureComparisonPanel != null)
            {
                // In case we're in a disposition where we need to restart the UI's color.
                if (recognizedGesture == GesturesForDemo.NONE ||
                    expectedGesture == GesturesForDemo.NONE)
                {
                    gestureComparisonPanel.color = standardGesturesColor;
                }
                // Matching gestures -> Green Feedback
                else if (expectedGesture == recognizedGesture)
                {
                    gestureComparisonPanel.color = matchingGesturesColor;
                }
                // Different gestures -> Red Feedback
                else
                {
                    gestureComparisonPanel.color = differentGesturesColor;
                }
            }
        }

        /// <summary>
        /// Update Text Gesture
        /// Updates the given text element to describe the given gesture.
        /// </summary>
        /// <param name="gesture">The gesture's type we want to display through the UI.</param>
        /// <param name="uiText">The ui text element using to display the gesture's type.</param>
        private void UpdateTextGesture(GesturesForDemo gesture, Text uiText)
        {
            if (uiText != null)
            {
                switch (gesture)
                {
                    case GesturesForDemo.RightSwipe:
                        uiText.text = "Right Hand Right Swipe";
                        break;
                    case GesturesForDemo.LeftSwipe:
                        uiText.text = "Right Hand Left Swipe";
                        break;
                    case GesturesForDemo.SwipeUp:
                        uiText.text = "Swipe Up";
                        break;
                    case GesturesForDemo.Punch:
                        uiText.text = "Punch";
                        break;
                    case GesturesForDemo.Run:
                        uiText.text = "Run";
                        break;
                    case GesturesForDemo.PraiseTheSun:
                        uiText.text = "Praise the Sun! \\[T]/";
                        break;
                    case GesturesForDemo.PraiseToMenu:
                        uiText.text = "\\[T]/ To Menu!";
                        break;
                    case GesturesForDemo.NONE:
                        uiText.text = "None";
                        break;
                    default:
                        uiText.text = "Unknown Gesture";
                        break;
                }
                UpdateUIColor();
            }
        }

        /// <summary>
        /// UpdateExpectedGestureText
        /// Updates the text of the UI element registered as holder of
        /// the expected gesture's name.
        /// </summary>
        /// <param name="gesture">The gesture's type we're expecting.</param>
        public void UpdateExpectedGestureText(GesturesForDemo gesture)
        {
            expectedGesture = gesture;
            UpdateTextGesture(gesture, expectedGestureText);
        }

        /// <summary>
        /// UpdateRecognizedGestureText
        /// Updates the text of the UI element registered as holder of
        /// the recognized gesture's name.
        /// </summary>
        /// <param name="gesture">The gesture's type we've recognized.</param>
        public void UpdateRecognizedGestureText(GesturesForDemo gesture)
        {
            recognizedGesture = gesture;
            UpdateTextGesture(gesture, recognizedGestureText);
        }
    }
}
