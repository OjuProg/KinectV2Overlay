using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KinectOverlay
{
    /// <summary>
    /// This class retrieves all necessary joints positions from Kinect data
    /// and implements the operations we will make between these joints (translations).
    /// </summary>
    public class CheckGestures : MonoBehaviour
    {
        //Text to print debug
        public Text text;

        internal int leftHandIndex;
        internal int rightHandIndex;

        internal int leftElbowIndex;
        internal int rightElbowIndex;

        internal int leftShoulderIndex;
        internal int rightShoulderIndex;

        internal int hipCenterIndex;
        internal int shoulderCenterIndex;

        internal int leftHipIndex;
        internal int rightHipIndex;

        internal int leftKneeIndex;
        internal int rightKneeIndex;

        internal int leftAnkleIndex;
        internal int rightAnkleIndex;

        private Vector3 leftHandPos = new Vector3();
        private Vector3 rightHandPos = new Vector3();

        private Vector3 leftElbowPos = new Vector3();
        private Vector3 rightElbowPos = new Vector3();

        private Vector3 leftShoulderPos = new Vector3();
        private Vector3 rightShoulderPos = new Vector3();

        internal Vector3 hipCenterPos = new Vector3();
        private Vector3 shoulderCenterPos = new Vector3();

        private Vector3 leftHipPos = new Vector3();
        private Vector3 rightHipPos = new Vector3();

        private KinectManager manager;
        /// <summary>
        /// Max value of scalar product between tracked position and world axes 
        /// </summary>
        private float maxProjection = 0.0f;
        /// <summary>
        /// Vector used to know the direction of a translation
        /// </summary>
        private Vector3 direction = Vector3.zero;

        #region Getters and setters
        internal Vector3 RightHandPos
        {
            get
            {
                return rightHandPos;
            }

            set
            {
                rightHandPos = value;
            }
        }

        internal Vector3 LeftHandPos
        {
            get
            {
                return leftHandPos;
            }

            set
            {
                leftHandPos = value;
            }
        }

        internal Vector3 LeftElbowPos
        {
            get
            {
                return leftElbowPos;
            }

            set
            {
                leftElbowPos = value;
            }
        }

        internal Vector3 RightElbowPos
        {
            get
            {
                return rightElbowPos;
            }

            set
            {
                rightElbowPos = value;
            }
        }

        internal Vector3 LeftShoulderPos
        {
            get
            {
                return leftShoulderPos;
            }

            set
            {
                leftShoulderPos = value;
            }
        }

        internal Vector3 RightShoulderPos
        {
            get
            {
                return rightShoulderPos;
            }

            set
            {
                rightShoulderPos = value;
            }
        }

        internal Vector3 ShoulderCenterPos
        {
            get
            {
                return shoulderCenterPos;
            }

            set
            {
                shoulderCenterPos = value;
            }
        }

        internal Vector3 LeftHipPos
        {
            get
            {
                return leftHipPos;
            }

            set
            {
                leftHipPos = value;
            }
        }

        internal Vector3 RightHipPos
        {
            get
            {
                return rightHipPos;
            }

            set
            {
                rightHipPos = value;
            }
        }
        #endregion

        private void Start()
        {
            manager = FindObjectOfType<KinectManager>();
        }


        /// <summary>
        /// Gets the list of joint indexes.
        /// </summary>
        /// <returns>The needed joint indexes.</returns>
        /// <param name="manager">The KinectManager instance</param>
        public int[] GetNeededJointIndexes(KinectManager manager)
        {
            leftHandIndex = manager.GetJointIndex(KinectInterop.JointType.HandLeft);
            rightHandIndex = manager.GetJointIndex(KinectInterop.JointType.HandRight);

            leftElbowIndex = manager.GetJointIndex(KinectInterop.JointType.ElbowLeft);
            rightElbowIndex = manager.GetJointIndex(KinectInterop.JointType.ElbowRight);

            leftShoulderIndex = manager.GetJointIndex(KinectInterop.JointType.ShoulderLeft);
            rightShoulderIndex = manager.GetJointIndex(KinectInterop.JointType.ShoulderRight);

            hipCenterIndex = manager.GetJointIndex(KinectInterop.JointType.SpineBase);
            shoulderCenterIndex = manager.GetJointIndex(KinectInterop.JointType.SpineShoulder);

            leftHipIndex = manager.GetJointIndex(KinectInterop.JointType.HipLeft);
            rightHipIndex = manager.GetJointIndex(KinectInterop.JointType.HipRight);

            leftKneeIndex = manager.GetJointIndex(KinectInterop.JointType.KneeLeft);
            rightKneeIndex = manager.GetJointIndex(KinectInterop.JointType.KneeRight);

            leftAnkleIndex = manager.GetJointIndex(KinectInterop.JointType.AnkleLeft);
            rightAnkleIndex = manager.GetJointIndex(KinectInterop.JointType.AnkleRight);

            int[] neededJointIndexes = {
            leftHandIndex, rightHandIndex, leftElbowIndex, rightElbowIndex, leftShoulderIndex, rightShoulderIndex,
            hipCenterIndex, shoulderCenterIndex, leftHipIndex, rightHipIndex, leftKneeIndex, rightKneeIndex,
            leftAnkleIndex, rightAnkleIndex
            };

            return neededJointIndexes;
        }

        /// <summary>
        /// Assigns the positions of each joints based on Kinect data.
        /// </summary>
        public void GetAllJointsPosition()
        {
            LeftHandPos = manager.GetJointPosition(manager.GetPrimaryUserID(), leftHandIndex);
            RightHandPos = manager.GetJointPosition(manager.GetPrimaryUserID(), rightHandIndex);

            LeftElbowPos = manager.GetJointPosition(manager.GetPrimaryUserID(), leftElbowIndex);
            RightElbowPos = manager.GetJointPosition(manager.GetPrimaryUserID(), rightElbowIndex);

            LeftShoulderPos = manager.GetJointPosition(manager.GetPrimaryUserID(), leftShoulderIndex);
            RightShoulderPos = manager.GetJointPosition(manager.GetPrimaryUserID(), rightShoulderIndex);

            hipCenterPos = manager.GetJointPosition(manager.GetPrimaryUserID(), hipCenterIndex);
            ShoulderCenterPos = manager.GetJointPosition(manager.GetPrimaryUserID(), shoulderCenterIndex);

            LeftHipPos = manager.GetJointPosition(manager.GetPrimaryUserID(), leftHipIndex);
            RightHipPos = manager.GetJointPosition(manager.GetPrimaryUserID(), rightHipIndex);
        }

        /// <summary>
        /// Prints values on screen for easy debug
        /// </summary>
        public void PrintJointsPositionsOnScreen()
        {
            //Debug.Log(manager.GetJointPosition(manager.GetPrimaryUserID(), rightHandIndex));
            if (text != null && RightHandPos != null)
            {
                //text.text = "Right hand position : " + RightHandPos.x.ToString("000") + ", " + RightHandPos.y.ToString("000") + ", " + RightHandPos.z.ToString("000");
                text.text = "right hand : " + RightHandPos.ToString() + "\n" + "left shoulder : " + LeftShoulderPos.ToString();
            }
        }

        /// <summary>
        /// Checks if the distance between two points is above a certain value
        /// </summary>
        /// <param name="p1">first value to compare</param>
        /// <param name="p2">second value to compare</param>
        /// <param name="distance">minimal difference between values</param>
        /// <returns></returns>
        public bool CheckForTranslation(float p1, float p2, float distance)
        {
            return (Mathf.Abs(p1 - p2) >= distance);
        }

        /// <summary>
        /// Checks if the distance between two points is below a certain value
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public bool CheckForNoTranslation(float p1, float p2, float distance)
        {
            return (Mathf.Abs(p1 - p2) <= distance);
        }

        /// <summary>
        /// Calculate the projection of a translation vector on a direction vector
        /// </summary>
        /// <param name="directionToTest">direction vector</param>
        /// <param name="translation">translation vector</param>
        public void TestDirection(Vector3 directionToTest, Vector3 translation)
        {
            if (Vector3.Dot(directionToTest, translation) >= maxProjection)
            {
                maxProjection = Vector3.Dot(directionToTest, translation);
                direction = directionToTest;
            }
        }

        /// <summary>
        /// Resets variables used for translation calculation
        /// </summary>
        public void ResetVariables()
        {
            maxProjection = 0.0f;
            direction = Vector3.zero;
        }

        /// <summary>
        /// Determines the direction of a translation
        /// </summary>
        /// <param name="translation">vector whose direction has to be determined</param>
        /// <returns></returns>
        public Vector3 FindTranslationDirection(Vector3 translation)
        {
            ResetVariables();
            TestDirection(Vector3.up, translation);
            TestDirection(-Vector3.up, translation);
            TestDirection(Vector3.right, translation);
            TestDirection(-Vector3.right, translation);
            TestDirection(Vector3.forward, translation);
            TestDirection(-Vector3.forward, translation);
            return direction;
        }

        private void Update()
        {
            GetNeededJointIndexes(manager);
            GetAllJointsPosition();
        }
    }
}