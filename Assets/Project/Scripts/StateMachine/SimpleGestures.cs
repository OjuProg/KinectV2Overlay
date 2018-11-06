using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KinectOverlay
{
    /// <summary>
    /// This class implements tests to recognize the simple gestures
    /// that are available in the enum GestureId. 
    /// </summary>
    public class SimpleGestures : MonoBehaviour
    {
        /// <summary>
        /// Reference to CheckGestures script.
        /// </summary>
        private CheckGestures checkGestures;
        /// <summary>
        /// Reference to RecognizeGesture script.
        /// </summary>
        private RecognizeGesture recognizeGesture;
        /// <summary>
        /// Reference to ComplexGestures script.
        /// </summary>
        private ComplexGestures complexGestures;

        private void Start()
        {
            recognizeGesture = GetComponent<RecognizeGesture>();
            checkGestures = GetComponent<CheckGestures>();
            complexGestures = GetComponent<ComplexGestures>();
        }

        /// <summary>
        /// Adds the couple of currently recognized gestures to the list of all recognized gestures. 
        /// If a couple has already been added at this frame, the couple is updated.
        /// </summary>
        /// <param name="addLeftGesture">Left hand gesture to add</param>
        /// <param name="addRightGesture">Right hand gesture to add</param>
        public void AddGesture(GestureId addLeftGesture, GestureId addRightGesture)
        {
            if (recognizeGesture.sequenceOfGestures.Count == 0)
            {
                recognizeGesture.sequenceOfGestures.Add(new CoupleStruct(addLeftGesture, addRightGesture));
            }
            else if (recognizeGesture.sequenceOfGestures.Count - recognizeGesture.sizeOfSequence == 0
                && (recognizeGesture.sequenceOfGestures[recognizeGesture.sequenceOfGestures.Count-1].LeftAndRightHandSymbols[0] != addLeftGesture
                || recognizeGesture.sequenceOfGestures[recognizeGesture.sequenceOfGestures.Count - 1].LeftAndRightHandSymbols[1] != addRightGesture))
            {
                //if (!CheckTransitionGesture(addLeftGesture, addRightGesture))
                //{
                //    recognizeGesture.sequenceOfGestures.Add(new CoupleStruct(addLeftGesture, addRightGesture));
                //}
                recognizeGesture.sequenceOfGestures.Add(new CoupleStruct(addLeftGesture, addRightGesture));
            }
            else if (recognizeGesture.sequenceOfGestures.Count - recognizeGesture.sizeOfSequence > 0)
            {
                recognizeGesture.sequenceOfGestures[recognizeGesture.sequenceOfGestures.Count - 1].LeftAndRightHandSymbols[0] = addLeftGesture;
                recognizeGesture.sequenceOfGestures[recognizeGesture.sequenceOfGestures.Count - 1].LeftAndRightHandSymbols[1] = addRightGesture;
                
            }
        }

        /// <summary>
        /// Checks if current gesture is a transition gesture
        /// </summary>
        /// <param name="currentLeftGesture">current left gesture</param>
        /// <param name="currentRightGesture">current right gesture</param>
        /// <returns></returns>
        public bool CheckTransitionGesture(GestureId currentLeftGesture, GestureId currentRightGesture)
        {
            foreach(Gesture cg in complexGestures.allComplexGestures)
            {
                if (cg.Size >= 2 && recognizeGesture.sequenceOfGestures.Count >= 1) {
                    for (int i = 0; i < cg.Size; i++) {
                        if (PreviousGestureIsIncludedInAComplexGesture(cg, i-1)
                            && CheckNextGestureInComplexGesture(cg, i+1, currentLeftGesture, currentRightGesture)
                            && IsNotIncludedInOtherComplexGesture(currentLeftGesture, currentRightGesture))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if a gesture is part of a complex gesture
        /// </summary>
        /// <param name="cg">A complex gesture</param>
        /// <param name="i">Index of gesture from complex gesture to test</param>
        /// <param name="leftGesture">Left gesture to test</param>
        /// <param name="rightGesture">Right gesture to test</param>
        /// <returns></returns>
        public bool CheckNextGestureInComplexGesture(Gesture cg, int i, GestureId leftGesture, GestureId rightGesture)
        {
            if (i <= cg.SequenceofGesturesToRecognize.Count - 1)
            {
                return (cg.SequenceofGesturesToRecognize[i].LeftAndRightHandSymbols[0] == leftGesture
                    || cg.SequenceofGesturesToRecognize[i].LeftAndRightHandSymbols[1] == rightGesture);
            }
            return false;
        }

        /// <summary>
        /// Checks if current gesture is contained in a complexe gesture.
        /// If it is not the case, current gesture will be considered a transition gesture.
        /// Otherwise, it counts as a normal gesture.
        /// </summary>
        /// <param name="leftGesture">Symbol corresponding to current left hand's gesture</param>
        /// <param name="rightGesture">Symbol corresponding to current right hand's gesture</param>
        /// <returns></returns>
        public bool IsNotIncludedInOtherComplexGesture(GestureId leftGesture, GestureId rightGesture)
        {
            foreach (Gesture cg in complexGestures.allComplexGestures)
            {
                for (int i = 0; i < cg.SequenceofGesturesToRecognize.Count; i++)
                {
                    if (cg.SequenceofGesturesToRecognize[i].LeftAndRightHandSymbols[0] == leftGesture && cg.SequenceofGesturesToRecognize[i].LeftAndRightHandSymbols[1] == rightGesture)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Tests if previous couple of gestures of the list of recognized gesture matches a couple of gesture in a complex gesture
        /// </summary>
        /// <param name="cg">complex gesture to check</param>
        /// <param name="i">Index of couple of gestures in the list of recognized gestures to test</param>
        /// <returns>True if current gesture is equal to previous gesture, else false</returns>
        public bool PreviousGestureIsIncludedInAComplexGesture(Gesture cg, int i)
        {
            if (i >= 0 && recognizeGesture.sequenceOfGestures.Count - 2 >= 0)
            {
                return (cg.SequenceofGesturesToRecognize[i].LeftAndRightHandSymbols[0] == recognizeGesture.sequenceOfGestures[recognizeGesture.sequenceOfGestures.Count - 2].LeftAndRightHandSymbols[0]
                    && cg.SequenceofGesturesToRecognize[i].LeftAndRightHandSymbols[1] == recognizeGesture.sequenceOfGestures[recognizeGesture.sequenceOfGestures.Count - 2].LeftAndRightHandSymbols[1]);
            }
            return false;
        }

        /// <summary>
        /// Test if right hand is near right hip.
        /// This position is used as a resting position (so a starting posiition)
        /// </summary>
        public void StartPointWhenSitRight()
        {
            List<CoupleStruct> sequence = recognizeGesture.sequenceOfGestures;
            if (checkGestures.CheckForNoTranslation(checkGestures.RightHandPos.x, checkGestures.RightHipPos.x, 0.2f)
                && checkGestures.CheckForNoTranslation(checkGestures.RightHandPos.y, checkGestures.RightHipPos.y, 0.3f)
                && checkGestures.CheckForNoTranslation(checkGestures.RightHandPos.z, checkGestures.RightHipPos.z, 0.3f))
            {
                if (sequence.Count == 0)
                {
                    AddGesture(GestureId.NONE, GestureId.RHSP);
                }
                else if (sequence.Count >= 1 && sequence[sequence.Count - 1].LeftAndRightHandSymbols[1] != GestureId.RHSP)
                {
                    AddGesture(recognizeGesture.sequenceOfGestures[recognizeGesture.sequenceOfGestures.Count - 1].LeftAndRightHandSymbols[0], GestureId.RHSP);
                }
            }
        }

        /// <summary>
        /// Test if left hand is near left hip.
        /// This position is used as a resting position (so a starting posiition)
        /// </summary>
        public void StartPointWhenSitLeft()
        {
            List<CoupleStruct> sequence = recognizeGesture.sequenceOfGestures;
            if (checkGestures.CheckForNoTranslation(checkGestures.LeftHandPos.x, checkGestures.LeftHipPos.x, 0.2f)
                && checkGestures.CheckForNoTranslation(checkGestures.LeftHandPos.y, checkGestures.LeftHipPos.y, 0.3f)
                && checkGestures.CheckForNoTranslation(checkGestures.LeftHandPos.z, checkGestures.LeftHipPos.z, 0.3f))
            {
                if (sequence.Count == 0)
                {
                    AddGesture(GestureId.LHSP, GestureId.NONE);
                }
                else if (sequence.Count >= 1 && sequence[sequence.Count - 1].LeftAndRightHandSymbols[0] != GestureId.LHSP)
                {
                    AddGesture(GestureId.LHSP, recognizeGesture.sequenceOfGestures[recognizeGesture.sequenceOfGestures.Count - 1].LeftAndRightHandSymbols[1]);
                }
            }
        }

        /// <summary>
        /// Test is right hand is close to right shoulder
        /// </summary>
        public void RightHandToRightShoulder()
        {
            List<CoupleStruct> sequence = recognizeGesture.sequenceOfGestures;
            if (checkGestures.CheckForNoTranslation(checkGestures.RightHandPos.x, checkGestures.RightShoulderPos.x, 0.07f)
                && checkGestures.CheckForNoTranslation(checkGestures.RightHandPos.y, checkGestures.RightShoulderPos.y, 0.07f)
                && checkGestures.CheckForNoTranslation(checkGestures.RightHandPos.z, checkGestures.RightShoulderPos.z, 0.07f))
            {
                if (sequence.Count == 0)
                {
                    AddGesture(GestureId.NONE, GestureId.RHRS);
                }
                else if (sequence.Count >= 1 && sequence[sequence.Count - 1].LeftAndRightHandSymbols[1] != GestureId.RHRS)
                {
                    AddGesture(recognizeGesture.sequenceOfGestures[recognizeGesture.sequenceOfGestures.Count - 1].LeftAndRightHandSymbols[0], GestureId.RHRS);
                }
            }
        }

        /// <summary>
        /// Test if right arm is extended to the right
        /// </summary>
        public void RightHandTranslationToTheRight()
        {
            List<CoupleStruct> sequence = recognizeGesture.sequenceOfGestures;
            if (checkGestures.CheckForTranslation(checkGestures.RightHandPos.x, checkGestures.RightShoulderPos.x, 0.25f)
                && checkGestures.CheckForNoTranslation(checkGestures.RightHandPos.y, checkGestures.RightShoulderPos.y, 0.15f)
                && checkGestures.CheckForNoTranslation(checkGestures.RightHandPos.z, checkGestures.RightShoulderPos.z, 0.15f))
            {
                if (checkGestures.FindTranslationDirection(checkGestures.RightHandPos - checkGestures.RightShoulderPos) == Vector3.right)
                {
                    if (sequence.Count == 0)
                    {
                        AddGesture(GestureId.NONE, GestureId.TRHR);
                    }
                    else if (sequence.Count >= 1 && sequence[sequence.Count - 1].LeftAndRightHandSymbols[1] != GestureId.TRHR)
                    {
                        AddGesture(recognizeGesture.sequenceOfGestures[recognizeGesture.sequenceOfGestures.Count - 1].LeftAndRightHandSymbols[0], GestureId.TRHR);
                    }

                }
            } 
        }

        /// <summary>
        /// Test if right arm is extended to the left
        /// </summary>
        public void RightHandTranslationToTheLeft()
        {
            List<CoupleStruct> sequence = recognizeGesture.sequenceOfGestures;
            if (checkGestures.CheckForTranslation(checkGestures.RightHandPos.x, checkGestures.RightShoulderPos.x, 0.10f)
                && checkGestures.CheckForNoTranslation(checkGestures.RightHandPos.y, checkGestures.RightShoulderPos.y, 0.15f)
                && checkGestures.CheckForNoTranslation(checkGestures.RightHandPos.z, checkGestures.RightShoulderPos.z, 0.15f))
            {
                if (checkGestures.FindTranslationDirection(checkGestures.RightHandPos - checkGestures.RightShoulderPos) == -Vector3.right)
                {
                    if (sequence.Count == 0)
                    {
                        AddGesture(GestureId.NONE, GestureId.TRHL);
                    }
                    else if (sequence.Count >= 1 && sequence[sequence.Count - 1].LeftAndRightHandSymbols[1] != GestureId.TRHL)
                    {
                        AddGesture(recognizeGesture.sequenceOfGestures[recognizeGesture.sequenceOfGestures.Count - 1].LeftAndRightHandSymbols[0], GestureId.TRHL);
                    }
                }
            }
        }

        /// <summary>
        /// Test if right arm is extended in front of the used
        /// </summary>
        public void RightHandTranslationForward()
        {
            List<CoupleStruct> sequence = recognizeGesture.sequenceOfGestures;
            if (checkGestures.CheckForNoTranslation(checkGestures.RightHandPos.x, checkGestures.RightShoulderPos.x, 0.05f)
                && checkGestures.CheckForNoTranslation(checkGestures.RightHandPos.y, checkGestures.RightShoulderPos.y, 0.2f)
                && checkGestures.CheckForTranslation(checkGestures.RightHandPos.z, checkGestures.RightShoulderPos.z, 0.4f))
            {
                if (checkGestures.FindTranslationDirection(checkGestures.RightHandPos - checkGestures.RightShoulderPos) == -Vector3.forward)
                {
                    if (sequence.Count == 0)
                    {
                        AddGesture(GestureId.NONE, GestureId.TRHF);
                    }
                    else if (sequence.Count >= 1 && sequence[sequence.Count - 1].LeftAndRightHandSymbols[1] != GestureId.TRHF)
                    {
                        AddGesture(recognizeGesture.sequenceOfGestures[recognizeGesture.sequenceOfGestures.Count - 1].LeftAndRightHandSymbols[0], GestureId.TRHF);
                    }
                }
            }
        }

        /// <summary>
        /// Test if right hand is above the user's head
        /// </summary>
        public void RightHandTranslationUp()
        {
            List<CoupleStruct> sequence = recognizeGesture.sequenceOfGestures;
            if (checkGestures.CheckForNoTranslation(checkGestures.RightHandPos.x, checkGestures.RightShoulderPos.x, 0.20f) 
                && checkGestures.CheckForTranslation(checkGestures.RightHandPos.y, checkGestures.RightShoulderPos.y, 0.5f)
                /*&& checkGestures.CheckForNoTranslation(checkGestures.RightHandPos.z, checkGestures.RightShoulderPos.z, 0.27f)*/)
            {
                if (checkGestures.FindTranslationDirection(checkGestures.RightHandPos - checkGestures.RightShoulderPos) == Vector3.up)
                {
                    if (sequence.Count == 0)
                    {
                        AddGesture(GestureId.NONE, GestureId.TRHU);
                    }
                    else if (sequence.Count >= 1 && sequence[sequence.Count - 1].LeftAndRightHandSymbols[1] != GestureId.TRHU)
                    {
                        AddGesture(recognizeGesture.sequenceOfGestures[recognizeGesture.sequenceOfGestures.Count - 1].LeftAndRightHandSymbols[0], GestureId.TRHU);
                    }

                }
            }
        }

        /// <summary>
        /// Test if right hand is above the user's head, in diagonal
        /// </summary>
        public void RightHandDiagonalUp()
        {
            List<CoupleStruct> sequence = recognizeGesture.sequenceOfGestures;
            if (checkGestures.CheckForTranslation(checkGestures.RightHandPos.x, checkGestures.RightShoulderPos.x, 0.3f)
                && checkGestures.CheckForTranslation(checkGestures.RightHandPos.y, checkGestures.RightShoulderPos.y, 0.2f)
                && checkGestures.CheckForNoTranslation(checkGestures.RightHandPos.z, checkGestures.RightShoulderPos.z, 0.15f))
            {
                if (checkGestures.FindTranslationDirection(checkGestures.RightHandPos - checkGestures.RightShoulderPos) == Vector3.up)
                {
                    if (sequence.Count == 0)
                    {
                        AddGesture(GestureId.NONE, GestureId.RHDU);
                    }
                    else if (sequence.Count >= 1 && sequence[sequence.Count - 1].LeftAndRightHandSymbols[1] != GestureId.RHDU)
                    {
                        AddGesture(recognizeGesture.sequenceOfGestures[recognizeGesture.sequenceOfGestures.Count - 1].LeftAndRightHandSymbols[0], GestureId.RHDU);
                    }

                }
            }
            }

        /// <summary>
        /// Test if left hand is close to left shoulder
        /// </summary>
        public void LeftHandToLeftShoulder()
        {
            List<CoupleStruct> sequence = recognizeGesture.sequenceOfGestures;
            if (checkGestures.CheckForNoTranslation(checkGestures.LeftHandPos.x, checkGestures.LeftShoulderPos.x, 0.07f)
                && checkGestures.CheckForNoTranslation(checkGestures.LeftHandPos.y, checkGestures.LeftShoulderPos.y, 0.07f)
                && checkGestures.CheckForNoTranslation(checkGestures.LeftHandPos.z, checkGestures.LeftShoulderPos.z, 0.07f))
            {
                if (sequence.Count == 0)
                {
                    AddGesture(GestureId.LHLS, GestureId.NONE);
                }
                else if (sequence.Count >= 1 && sequence[sequence.Count - 1].LeftAndRightHandSymbols[0] != GestureId.LHLS)
                {
                    AddGesture(GestureId.LHLS, recognizeGesture.sequenceOfGestures[recognizeGesture.sequenceOfGestures.Count - 1].LeftAndRightHandSymbols[1]);
                }
            }
        }

        /// <summary>
        /// Test if left hand is above the user's head, in diagonal
        /// </summary>
        public void LeftHandDiagonalUp()
        {
            List<CoupleStruct> sequence = recognizeGesture.sequenceOfGestures;
            if (checkGestures.CheckForTranslation(checkGestures.LeftHandPos.x, checkGestures.LeftShoulderPos.x, 0.3f)
                && checkGestures.CheckForTranslation(checkGestures.LeftHandPos.y, checkGestures.LeftShoulderPos.y, 0.2f)
                && checkGestures.CheckForNoTranslation(checkGestures.LeftHandPos.z, checkGestures.LeftShoulderPos.z, 0.15f))
            {
                if (checkGestures.FindTranslationDirection(checkGestures.LeftHandPos - checkGestures.LeftShoulderPos) == Vector3.up)
                {
                    if (sequence.Count == 0)
                    {
                        AddGesture(GestureId.LHDU, GestureId.NONE);
                    }
                    else if (sequence.Count >= 1 && sequence[sequence.Count - 1].LeftAndRightHandSymbols[0] != GestureId.LHDU)
                    {
                        AddGesture(GestureId.LHDU, recognizeGesture.sequenceOfGestures[recognizeGesture.sequenceOfGestures.Count - 1].LeftAndRightHandSymbols[1]);
                    }

                }
            }
        }

        /// <summary>
        /// Test if left hand is above the user's head
        /// </summary>
        public void LeftHandTranslationUp()
        {
            List<CoupleStruct> sequence = recognizeGesture.sequenceOfGestures;
            if (checkGestures.CheckForNoTranslation(checkGestures.LeftHandPos.x, checkGestures.LeftShoulderPos.x, 0.20f)
                && checkGestures.CheckForTranslation(checkGestures.LeftHandPos.y, checkGestures.LeftShoulderPos.y, 0.5f)
                /*&& checkGestures.CheckForNoTranslation(checkGestures.LeftHandPos.z, checkGestures.LeftShoulderPos.z, 0.27f)*/)
            {
                if (checkGestures.FindTranslationDirection(checkGestures.LeftHandPos - checkGestures.LeftShoulderPos) == Vector3.up)
                {
                    if (sequence.Count == 0)
                    {
                        AddGesture(GestureId.TLHU, GestureId.NONE);
                    }
                    else if (sequence.Count >= 1 && sequence[sequence.Count - 1].LeftAndRightHandSymbols[0] != GestureId.TLHU)
                    {
                        AddGesture(GestureId.TLHU, recognizeGesture.sequenceOfGestures[recognizeGesture.sequenceOfGestures.Count - 1].LeftAndRightHandSymbols[1]);
                    }

                }
            }
        }

        /// <summary>
        /// Test if right fore arm is vertical in front of the user, elbow bent, and left hand is at half chest height, almost aligned with left elbow along the depth axis.
        /// </summary>
        public void RightHandFrontRunning()
        {
            List<CoupleStruct> sequence = recognizeGesture.sequenceOfGestures;
            if (checkGestures.CheckForNoTranslation(checkGestures.RightHandPos.x, checkGestures.RightElbowPos.x, 0.15f)
                && checkGestures.CheckForTranslation(checkGestures.RightHandPos.y, checkGestures.RightElbowPos.y, 0.2f)
                
                && checkGestures.CheckForNoTranslation(checkGestures.LeftHandPos.x, checkGestures.LeftElbowPos.x, 0.15f)
                && checkGestures.CheckForNoTranslation(checkGestures.LeftHandPos.y, checkGestures.LeftElbowPos.y, 0.1f)
                )
            {
                if (sequence.Count == 0)
                {
                    AddGesture(GestureId.NONE, GestureId.RRHF);
                }
                else if (sequence.Count >= 1 && sequence[sequence.Count - 1].LeftAndRightHandSymbols[1] != GestureId.RRHF)
                {
                    AddGesture(recognizeGesture.sequenceOfGestures[recognizeGesture.sequenceOfGestures.Count - 1].LeftAndRightHandSymbols[0], GestureId.RRHF);
                }
            }
        }

        /// <summary>
        /// Test if left fore arm is vertical in front of the user, elbow bent, and right hand is at half chest height, almost aligned with right elbow along the depth axis.
        /// </summary>
        public void LeftHandFrontRunning()
        {
            List<CoupleStruct> sequence = recognizeGesture.sequenceOfGestures;
            if (checkGestures.CheckForNoTranslation(checkGestures.LeftHandPos.x, checkGestures.LeftElbowPos.x, 0.15f)
                && checkGestures.CheckForTranslation(checkGestures.LeftHandPos.y, checkGestures.LeftElbowPos.y, 0.2f)

                && checkGestures.CheckForNoTranslation(checkGestures.RightHandPos.x, checkGestures.RightElbowPos.x, 0.15f)
                && checkGestures.CheckForNoTranslation(checkGestures.RightHandPos.y, checkGestures.RightElbowPos.y, 0.1f)
                )
            {
                if (sequence.Count == 0)
                {
                    AddGesture(GestureId.RLHF, GestureId.NONE);
                }
                else if (sequence.Count >= 1 && sequence[sequence.Count - 1].LeftAndRightHandSymbols[0] != GestureId.RLHF)
                {
                    AddGesture(GestureId.RLHF, recognizeGesture.sequenceOfGestures[recognizeGesture.sequenceOfGestures.Count - 1].LeftAndRightHandSymbols[1]);
                }
            }
        }
    }
}
