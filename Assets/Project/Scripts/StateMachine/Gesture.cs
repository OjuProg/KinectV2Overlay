using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KinectOverlay
{
    /// <summary>
    /// Enum used to identify simple gestures
    /// </summary>
    public enum GestureId
    {
        TRHR, // Translation right hand to the right
        TRHL, // Translation right hand to the left
        TRHU, // Translation right hand to the top
        TRHD, // Translation right hand to the bottom
        TLHU, // Translation left hand to the top
        TLHD, // Translation left hand to the bottom
        RHRS, // Right hand to right shoulder
        LHLS, // Left hand to left shoulder
        TRHF, // Translation right hand forward
        RRHF, // Run right hand front
        RLHF, // Run left hand front
        RRHB, // Run right hand back
        RLHB, // Run left hand back
        RHSP, // Right hand start point
        LHSP, // Left hand start point
        RHDU, // Right hand diagonal up
        LHDU, // Left hand diagonal up 
        NONE // No gesture has to be recognized
    }

    /// <summary>
    /// Gesture is a struct that contains the data used to define complex gestures :
    /// - a name
    /// - a list of couples of simple gestures
    /// - the size of this list
    /// </summary>
    public struct Gesture
    {
        private List<CoupleStruct> sequenceofGesturesToRecognize;
        private int size;
        private string gestureName;
        
        public Gesture(List<CoupleStruct> list_, string name_) : this()
        {
            SequenceofGesturesToRecognize = list_;
            Size = list_.Count;
            GestureName = name_;
        }

        #region Getters and Setters

        public List<CoupleStruct> SequenceofGesturesToRecognize
        {
            get
            {
                return sequenceofGesturesToRecognize;
            }

            set
            {
                sequenceofGesturesToRecognize = value;
            }
        }

        public int Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
            }
        }

        public string GestureName
        {
            get
            {
                return gestureName;
            }

            set
            {
                gestureName = value;
            }
        }
        #endregion

        /// <summary>
        /// Creates a Null gesture, used when no gesture is recognized.
        /// </summary>
        public void MakeNullGesture()
        {
            sequenceofGesturesToRecognize = new List<CoupleStruct>();
            size = 0;
            gestureName = null;
        }

    }
}
