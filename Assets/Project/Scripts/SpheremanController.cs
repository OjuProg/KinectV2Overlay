// Inspired from the "CubeMan Controller" given with the Kinect Assets Pack.
using UnityEngine;

using System;
using System.Collections.Generic;

namespace KinectOverlay
{
    public class SpheremanController : MonoBehaviour
    {
        public enum SphereManState { STANDARD, VALID, WRONG }; // The differents status of the sphereman - Influences its color.

        public SphereManState currentSphereManState;

        [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
        public int playerIndex = 0;

        [Tooltip("Whether the cubeman is allowed to move vertically or not.")]
        public bool verticalMovement = true;

        [Tooltip("Whether the cubeman is facing the player or not.")]
        public bool mirroredMovement = true;

        [Tooltip("Rate at which the cubeman will move through the scene.")]
        public float moveRate = 1f;

        [SerializeField]
        private Material standardGestureMaterial;  // Standard Sphere material - Articulations of the Player.
        [SerializeField]
        private LineRenderer standardSkeletonLine; // Standard Skeleton linking the spheres.

        [SerializeField]
        private Material validGestureMaterial;
        [SerializeField]
        private LineRenderer validSkeletonLine;

        [SerializeField]
        private Material wrongGestureMaterial;
        [SerializeField]
        private LineRenderer wrongSkeletonLine;

        [SerializeField]
        private List<int> jointToIgnoreIndexes;  // The user can chose which articulation he wants to hide.

        public GameObject Hip_Center;
        public GameObject Spine;
        public GameObject Neck;
        public GameObject Head;
        public GameObject Shoulder_Left;
        public GameObject Elbow_Left;
        public GameObject Wrist_Left;
        public GameObject Hand_Left;
        public GameObject Shoulder_Right;
        public GameObject Elbow_Right;
        public GameObject Wrist_Right;
        public GameObject Hand_Right;
        public GameObject Hip_Left;
        public GameObject Knee_Left;
        public GameObject Ankle_Left;
        public GameObject Foot_Left;
        public GameObject Hip_Right;
        public GameObject Knee_Right;
        public GameObject Ankle_Right;
        public GameObject Foot_Right;
        public GameObject Spine_Shoulder;
        public GameObject Hand_Tip_Left;
        public GameObject Thumb_Left;
        public GameObject Hand_Tip_Right;
        public GameObject Thumb_Right;

        private LineRenderer skeletonLine;
        public LineRenderer debugLine;

        private GameObject[] bones;
        private LineRenderer[] lines;

        private LineRenderer lineTLeft;
        private LineRenderer lineTRight;
        private LineRenderer lineFLeft;
        private LineRenderer lineFRight;

        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private Vector3 initialPosOffset = Vector3.zero;
        private Int64 initialPosUserID = 0;

        /// <summary>
        /// Update
        /// Standard Unity Methods, updates the articulations of the player's avatar.
        /// </summary>
        /// <param name="material">The material that will be applied to the articulations.</param>
        private void UpdateBonesMaterial(Material material)
        {
            Renderer rd;
            foreach (GameObject bone in bones)
            {
                rd = bone.GetComponent<Renderer>();
                if (rd != null)
                {
                    rd.material = material;
                }
            }
        }

        /// <summary>
        /// Updates the appareance of the Sphere man according to its status.
        /// </summary>
        /// <param name="state">The new state of the sphereman.</param>
        public void UpdateSphereManStatus(SphereManState state)
        {
            currentSphereManState = state;

            /* If a skeleton has been generated, we unload it to force 
             * the apparition of the adequate color of skeleton. 
             */
            if (skeletonLine != null)
            {
                for (int i = 0; i < bones.Length; i++)
                {
                    if (lines[i] != null)
                    {
                        GameObject.Destroy(lines[i]);
                    }
                }
            }

            // Each time, we load the adapted skeleton and spheres.
            switch (state)
            {
                case SphereManState.STANDARD:
                    if (standardGestureMaterial != null)
                    {
                        UpdateBonesMaterial(standardGestureMaterial);
                    }

                    if (standardSkeletonLine != null)
                    {
                        standardSkeletonLine.gameObject.SetActive(true);
                        skeletonLine = standardSkeletonLine;
                    }
                    if (validSkeletonLine != null)
                    {
                        validSkeletonLine.gameObject.SetActive(false);
                    }
                    if (wrongSkeletonLine != null)
                    {
                        wrongSkeletonLine.gameObject.SetActive(false);
                    }
                    break;

                case SphereManState.VALID:
                    if (validGestureMaterial != null)
                    {
                        UpdateBonesMaterial(validGestureMaterial);
                    }

                    if (standardSkeletonLine != null)
                    {
                        standardSkeletonLine.gameObject.SetActive(false);
                    }
                    if (validSkeletonLine != null)
                    {
                        validSkeletonLine.gameObject.SetActive(true);
                        skeletonLine = validSkeletonLine;
                    }
                    if (wrongSkeletonLine != null)
                    {
                        wrongSkeletonLine.gameObject.SetActive(false);
                    }
                    break;

                case SphereManState.WRONG:
                    if (wrongGestureMaterial != null)
                    {
                        UpdateBonesMaterial(wrongGestureMaterial);
                    }

                    if (validSkeletonLine != null)
                    {
                        validSkeletonLine.gameObject.SetActive(false);
                    }
                    if (standardSkeletonLine != null)
                    {
                        standardSkeletonLine.gameObject.SetActive(false);
                    }
                    if (wrongSkeletonLine != null)
                    {
                        wrongSkeletonLine.gameObject.SetActive(true);
                        skeletonLine = wrongSkeletonLine;
                    }
                    break;

                default:
                    break;
            }
        }

        void Start()
        {
            //store bones in a list for easier access
            bones = new GameObject[] {
            Hip_Center,
            Spine,
            Neck,
            Head,
            Shoulder_Left,
            Elbow_Left,
            Wrist_Left,
            Hand_Left,
            Shoulder_Right,
            Elbow_Right,
            Wrist_Right,
            Hand_Right,
            Hip_Left,
            Knee_Left,
            Ankle_Left,
            Foot_Left,
            Hip_Right,
            Knee_Right,
            Ankle_Right,
            Foot_Right,
            Spine_Shoulder,
            Hand_Tip_Left,
            Thumb_Left,
            Hand_Tip_Right,
            Thumb_Right
        };

            // array holding the skeleton lines
            lines = new LineRenderer[bones.Length];

            initialPosition = transform.position;
            initialRotation = transform.rotation;

            // For the visual aspect.
            currentSphereManState = SphereManState.STANDARD;
            UpdateSphereManStatus(currentSphereManState);
            jointToIgnoreIndexes = jointToIgnoreIndexes ?? new List<int>();
        }

        void Update()
        {
            KinectManager manager = KinectManager.Instance;

            // get 1st player
            Int64 userID = manager ? manager.GetUserIdByIndex(playerIndex) : 0;

            if (userID <= 0)
            {
                initialPosUserID = 0;
                initialPosOffset = Vector3.zero;

                // reset the pointman position and rotation
                if (transform.position != initialPosition)
                {
                    transform.position = initialPosition;
                }

                if (transform.rotation != initialRotation)
                {
                    transform.rotation = initialRotation;
                }

                for (int i = 0; i < bones.Length; i++)
                {
                    if (!jointToIgnoreIndexes.Contains(i))
                    {
                        bones[i].gameObject.SetActive(true);

                        bones[i].transform.localPosition = Vector3.zero;
                        bones[i].transform.localRotation = Quaternion.identity;

                        if (lines[i] != null)
                        {
                            lines[i].gameObject.SetActive(false);
                        }
                    }
                }

                return;
            }

            // set the position in space
            Vector3 posPointMan = manager.GetUserPosition(userID);
            Vector3 posPointManMP = new Vector3(posPointMan.x, posPointMan.y, !mirroredMovement ? -posPointMan.z : posPointMan.z);

            // store the initial position
            if (initialPosUserID != userID)
            {
                initialPosUserID = userID;
                initialPosOffset = posPointMan;
            }

            Vector3 relPosUser = (posPointMan - initialPosOffset);
            relPosUser.z = !mirroredMovement ? -relPosUser.z : relPosUser.z;

            transform.position = initialPosOffset +
                (verticalMovement ? relPosUser * moveRate : new Vector3(relPosUser.x, 0, relPosUser.z) * moveRate);

            // update the local positions of the bones
            for (int i = 0; i < bones.Length; i++)
            {
                // Displaying only the wanted joints.
                if (!jointToIgnoreIndexes.Contains(i))
                {
                    if (bones[i] != null)
                    {
                        int joint = !mirroredMovement ? i : (int)KinectInterop.GetMirrorJoint((KinectInterop.JointType)i);
                        if (joint < 0)
                            continue;

                        if (manager.IsJointTracked(userID, joint))
                        {
                            bones[i].gameObject.SetActive(true);

                            Vector3 posJoint = manager.GetJointPosition(userID, joint);
                            posJoint.z = !mirroredMovement ? -posJoint.z : posJoint.z;

                            Quaternion rotJoint = manager.GetJointOrientation(userID, joint, !mirroredMovement);
                            rotJoint = initialRotation * rotJoint;

                            posJoint -= posPointManMP;

                            if (mirroredMovement)
                            {
                                posJoint.x = -posJoint.x;
                                posJoint.z = -posJoint.z;
                            }

                            bones[i].transform.localPosition = posJoint;
                            bones[i].transform.rotation = rotJoint;

                            if (lines[i] == null && skeletonLine != null)
                            {
                                lines[i] = Instantiate((i == 22 || i == 24) && debugLine ? debugLine : skeletonLine) as LineRenderer;
                                lines[i].transform.parent = transform;
                            }

                            if (lines[i] != null)
                            {
                                lines[i].gameObject.SetActive(true);
                                Vector3 posJoint2 = bones[i].transform.position;

                                Vector3 dirFromParent = manager.GetJointDirection(userID, joint, false, false);
                                dirFromParent.z = !mirroredMovement ? -dirFromParent.z : dirFromParent.z;
                                Vector3 posParent = posJoint2 - dirFromParent;

                                lines[i].SetPosition(0, posParent);
                                lines[i].SetPosition(1, posJoint2);
                            }

                        }
                        else
                        {
                            bones[i].gameObject.SetActive(false);

                            if (lines[i] != null)
                            {
                                lines[i].gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }
}