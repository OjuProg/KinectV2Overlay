/* CustomKinectController.cs
 * Made for the Kinect Project of JIN 2018
 */
using UnityEngine;
using UnityEngine.UI;

namespace KinectOverlay
{
    /// <summary>
    /// CustomKinectController
    /// Manages the custom kinect manager made for this project.
    /// Can only exist in one instance.
    /// </summary>
    public class CustomKinectController : MonoBehaviour
    {
        [SerializeField]
        private GameObject customKinectControllerPrefab;
        private KinectManager kinectManager;  // The standard controller offered by Microsoft.

        // These four variables are used to display the current state of the Controller.
        [SerializeField]
        private Text kinectStateText;
        [SerializeField]
        private Color undetectedColor;
        [SerializeField]
        private Color userDetectedColor;

        /// <summary>
        /// Public getter for the Instance of the Custom Kinect Controller
        /// </summary>
        public static CustomKinectController Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Awake
        /// Standard Unity Method.
        /// Used to create the 
        /// </summary>
        private void Awake()
        {
            // An instance has already been made.
            if (Instance != null)
            {
                Debug.LogWarning("An instance of Custom Kinect Controller exists already.");
                Destroy(this.gameObject);
                return;
            }

            // If we do have the needed prefab.
            if(customKinectControllerPrefab != null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
                GameObject go = Instantiate(customKinectControllerPrefab);
                go.transform.parent = this.gameObject.transform;
                kinectManager = go.GetComponent<KinectManager>();
            }
            else
            {
                Debug.LogError("The prefab for the Custom Kinect Controller does not exist. Please bind it to this object.");
                Destroy(this.gameObject);
            }
        }

        /// <summary>
        /// UpdateKinectUIStatus
        /// Display on screen whether a user is detected by the Kinect.
        /// </summary>
        private void UpdateKinectUIStatus()
        {
            if(kinectStateText != null)
            {
                if (kinectManager.IsUserDetected())
                {
                    kinectStateText.text = "Kinect: User Detected";
                    kinectStateText.color = userDetectedColor;
                }
                else
                {
                    kinectStateText.text = "Kinect: No User";
                    kinectStateText.color = undetectedColor;
                }
            }
        }

        /// <summary>
        /// Update.
        /// Standard Unity Methods, used here to check for the user detection.
        /// </summary>
        private void Update()
        {
            UpdateKinectUIStatus();
        }
    }
}
