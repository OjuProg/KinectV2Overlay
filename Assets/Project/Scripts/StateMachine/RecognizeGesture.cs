using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

namespace KinectOverlay
{
    /// <summary>
    /// This class is the core of the recognition. 
    /// A list of recognized gestures is created, updated and compared
    /// with all complex gestures available. When there is a match, the
    /// recognized gesture is stored in order to be used in the UI scripts.
    /// </summary>
    public class RecognizeGesture : MonoBehaviour
    {
        /// <summary>
        /// List of recognized couples of gestures
        /// </summary>
        internal List<CoupleStruct> sequenceOfGestures = new List<CoupleStruct>();
        /// <summary>
        /// Size of sequenceOfGestures before a new couple of gestures is added in current frame
        /// </summary>
        internal int sizeOfSequence = 0;

        /// <summary>
        /// Reference to KinectManager script
        /// </summary>
        private KinectManager manager;
        /// <summary>
        /// Reference to CheckGestures script
        /// </summary>
        private CheckGestures checkGestures;
        /// <summary>
        /// Reference to SimpleGestures script
        /// </summary>
        private SimpleGestures recognizableGestures;
        /// <summary>
        /// Reference to ComplexGestures script
        /// </summary>
        private ComplexGestures complexGestures;

        // Use this for initialization
        void Start()
        {
            manager = GetComponent<KinectManager>();
            checkGestures = GetComponent<CheckGestures>();
            recognizableGestures = GetComponent<SimpleGestures>();
            complexGestures = GetComponent<ComplexGestures>();
        }

        /// <summary>
        /// Creates a null Gesture and stores recognized complex gesture in this object Gesture
        /// if a user is detected and a complex gesture is recognized. 
        /// </summary>
        /// <returns>Null if no user is detected, else recognized gesture</returns>
        public Gesture TryToGetGesture()
        {
            Gesture recognizedGesture = new Gesture();
            recognizedGesture.MakeNullGesture();

            if (manager.GetPrimaryUserID() != 0) //If a user is detected
            {
                CurrentVariablesValues();
                CheckJointPositions();
                recognizedGesture = CompareGestureSequenceWithAllComplexGestures();
            }
            return recognizedGesture;
        }

        /// <summary>
        /// Updates the values of sizeofSequence
        /// </summary>
        public void CurrentVariablesValues()
        {
            sizeOfSequence = sequenceOfGestures.Count;
        }

        /// <summary>
        /// Calls methods from SimpleGestures to check if a simple gesture is recognized.
        /// </summary>
        public void CheckJointPositions()
        {
            recognizableGestures.RightHandTranslationToTheRight();
            recognizableGestures.RightHandTranslationToTheLeft();
            recognizableGestures.RightHandTranslationUp();
            recognizableGestures.RightHandTranslationForward();
            recognizableGestures.StartPointWhenSitRight();
            recognizableGestures.RightHandDiagonalUp();
            recognizableGestures.RightHandFrontRunning();
            recognizableGestures.LeftHandTranslationUp();
            recognizableGestures.StartPointWhenSitLeft();
            recognizableGestures.LeftHandDiagonalUp();
            recognizableGestures.LeftHandFrontRunning();  
        }

        /// <summary>
        /// Compares the list of couples of simple gestures recognized so far with 
        /// all complex gestures' list of couple of gestures.
        /// When a complex gesture is recognized, it is stored in an object Gesture
        /// which is then used to give the appropriate answer in UI.
        /// </summary>
        /// <returns></returns>
        public Gesture CompareGestureSequenceWithAllComplexGestures()
        {
            Gesture recognizedGesture = new Gesture();
            recognizedGesture.MakeNullGesture();

            if (complexGestures.allComplexGestures != null)
            {
                foreach (Gesture gesture in complexGestures.allComplexGestures)
                {
                    if (sequenceOfGestures.Count >= gesture.Size && sequenceOfGestures.Count > 0)
                    {
                        bool match = true;
                        for (int i = 0; i < gesture.Size; i++)
                        {
                            if (CompareSimpleGestures(gesture.SequenceofGesturesToRecognize[gesture.Size - i - 1], sequenceOfGestures[sequenceOfGestures.Count - i - 1]))
                            {
                                match = false;
                            }
                        }
                        if (match)
                        {
                            switch (gesture.GestureName)
                            {
                                case "SwipeRight":
                                    recognizedGesture.GestureName = "SwipeRight";
                                    break;
                                case "SwipeLeft":
                                    recognizedGesture.GestureName = "SwipeLeft";
                                    break;
                                case "SwipeUp":
                                    recognizedGesture.GestureName = "SwipeUp";
                                    break;
                                case "Punch":
                                    recognizedGesture.GestureName = "Punch";
                                    break;
                                case "Run":
                                    Debug.Log("Hello!");
                                    recognizedGesture.GestureName = "Run";
                                    break;
                                case "PraiseTheSun":
                                    recognizedGesture.GestureName = "PraiseTheSun";
                                    break;
                                default:
                                    break;
                            }
                            sequenceOfGestures.Clear();
                            return recognizedGesture;
                        }
                    }
                }
            }
            return recognizedGesture;
        }

        /// <summary>
        /// Compares two couples of Gestures.
        /// If a gesture in the couple from the complex gesture is NONE, 
        /// the corresponding gesture in the other couple is not taken into account
        /// </summary>
        /// <param name="complexGesture">A couple of gestures from a recognizable complex gesture</param>
        /// <param name="recognizedGesture">A couple of gestures from the list of all recognized gestures</param>
        /// <returns>False if the couples are equal, else true.</returns>
        public bool CompareSimpleGestures(CoupleStruct complexGesture, CoupleStruct recognizedGesture)
        {
            return ((complexGesture.LeftAndRightHandSymbols[0] == GestureId.NONE && complexGesture.LeftAndRightHandSymbols[1] != recognizedGesture.LeftAndRightHandSymbols[1])
                || (complexGesture.LeftAndRightHandSymbols[0] != recognizedGesture.LeftAndRightHandSymbols[0] && complexGesture.LeftAndRightHandSymbols[1] == GestureId.NONE)
                || ((complexGesture.LeftAndRightHandSymbols[0] != recognizedGesture.LeftAndRightHandSymbols[0] || complexGesture.LeftAndRightHandSymbols[1] != recognizedGesture.LeftAndRightHandSymbols[1])
                    && complexGesture.LeftAndRightHandSymbols[0] != GestureId.NONE && complexGesture.LeftAndRightHandSymbols[1] != GestureId.NONE));
        }
        
    }
}
