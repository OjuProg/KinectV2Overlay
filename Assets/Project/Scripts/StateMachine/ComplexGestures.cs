using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KinectOverlay
{
    /// <summary>
    /// This class creates complex gestures, i.e. assigns a name and 
    /// a list of couple of static gestures and stores them in a custom
    /// Gesture object.
    /// </summary>
    public class ComplexGestures : MonoBehaviour
    {
        /// <summary>
        /// List of all complex gestures the application can recognize.
        /// </summary>
        internal List<Gesture> allComplexGestures = new List<Gesture>();

        /// <summary>
        /// Creates a swipe right gesture : right hand swipes to the right and comes back to origin position
        /// </summary>
        private Gesture swipeRight = new Gesture();
        private List<CoupleStruct> swipeRightGestures = new List<CoupleStruct>();

        /// <summary>
        /// Creates a swipe left gesture : right hand swipes to the left and comes back to origin position
        /// </summary>
        private Gesture swipeLeft = new Gesture();
        private List<CoupleStruct> swipeLeftGestures = new List<CoupleStruct>();

        /// <summary>
        /// Creates a swipe up gesture : both hands swipe to the top and come back to origin position
        /// </summary>
        private Gesture swipeUp = new Gesture();
        private List<CoupleStruct> swipeUpGestures = new List<CoupleStruct>();

        /// <summary>
        /// Creates a swipe up gesture : both hands swipe to the top and come back to origin position
        /// </summary>
        private Gesture swipeUp2 = new Gesture();
        private List<CoupleStruct> swipeUp2Gestures = new List<CoupleStruct>();

        /// <summary>
        /// Creates a punch gesture : right hand translates forward
        /// </summary>
        private Gesture punch = new Gesture();
        private List<CoupleStruct> punchGestures = new List<CoupleStruct>();

        /// <summary>
        /// Creates a run gesture : both arms move back and forth 
        /// </summary>
        private Gesture run = new Gesture();
        private List<CoupleStruct> runGestures = new List<CoupleStruct>();

        /// <summary>
        /// Creates a praise the sun gesture : both arms are raised above the head, in diagonal, toward the outside
        /// </summary>
        private Gesture sun = new Gesture();
        private List<CoupleStruct> sunGestures = new List<CoupleStruct>();

        #region Getters and setters
        public Gesture SwipeRight
        {
            get
            {
                return swipeRight;
            }

            private set
            {
                swipeRight = value;
            }
        }

        public Gesture SwipeUp
        {
            get
            {
                return swipeUp;
            }

            set
            {
                swipeUp = value;
            }
        }

        public Gesture SwipeLeft
        {
            get
            {
                return swipeLeft;
            }

            set
            {
                swipeLeft = value;
            }
        }

        public Gesture Punch
        {
            get
            {
                return punch;
            }

            set
            {
                punch = value;
            }
        }

        public Gesture Run
        {
            get
            {
                return run;
            }

            set
            {
                run = value;
            }
        }

        public Gesture Sun
        {
            get
            {
                return sun;
            }

            set
            {
                sun = value;
            }
        }

        public Gesture SwipeUp2
        {
            get
            {
                return swipeUp2;
            }

            set
            {
                swipeUp2 = value;
            }
        }
        #endregion
        private void Start()
        {
            InitializeComplexGestures();
            BuildComplexGestures();
            MakeListOfAllComplexGestures();
        }

        /// <summary>
        /// Fills the list of couplse of simple gestures of each complex gesture
        /// Elements have to be added in the order they will be recognized. 
        /// </summary>
        public void InitializeComplexGestures()
        {
            swipeRightGestures.Add(new CoupleStruct(GestureId.NONE, GestureId.RHSP));
            swipeRightGestures.Add(new CoupleStruct(GestureId.NONE, GestureId.TRHR));
            swipeRightGestures.Add(new CoupleStruct(GestureId.NONE, GestureId.RHSP));

            swipeLeftGestures.Add(new CoupleStruct(GestureId.NONE, GestureId.RHSP));
            swipeLeftGestures.Add(new CoupleStruct(GestureId.NONE, GestureId.TRHL));
            swipeLeftGestures.Add(new CoupleStruct(GestureId.NONE, GestureId.RHSP));

            swipeUpGestures.Add(new CoupleStruct(GestureId.NONE, GestureId.RHSP));
            swipeUpGestures.Add(new CoupleStruct(GestureId.TLHU, GestureId.TRHU));
            swipeUpGestures.Add(new CoupleStruct(GestureId.NONE, GestureId.RHSP));

            swipeUp2Gestures.Add(new CoupleStruct(GestureId.LHSP, GestureId.NONE));
            swipeUp2Gestures.Add(new CoupleStruct(GestureId.TLHU, GestureId.TRHU));
            swipeUp2Gestures.Add(new CoupleStruct(GestureId.LHSP, GestureId.NONE));

            punchGestures.Add(new CoupleStruct(GestureId.NONE, GestureId.TRHF));

            runGestures.Add(new CoupleStruct(GestureId.RLHB, GestureId.RRHF));
            runGestures.Add(new CoupleStruct(GestureId.RLHF, GestureId.RRHB));
            runGestures.Add(new CoupleStruct(GestureId.RLHB, GestureId.RRHF));
            runGestures.Add(new CoupleStruct(GestureId.RLHF, GestureId.RRHB));

            //runGestures.Add(new CoupleStruct(GestureId.TLHD, GestureId.RHRS));
            //runGestures.Add(new CoupleStruct(GestureId.LHLS, GestureId.TRHD));
            //runGestures.Add(new CoupleStruct(GestureId.TLHD, GestureId.RRHF));
            //runGestures.Add(new CoupleStruct(GestureId.LHLS, GestureId.RHRS));

            sunGestures.Add(new CoupleStruct(GestureId.LHDU, GestureId.RHDU));
        }

        /// <summary>
        /// Updates the complex gestures with a name and a list of simple gestures
        /// </summary>
        public void BuildComplexGestures()
        {
            SwipeRight = new Gesture(swipeRightGestures, "SwipeRight");
            SwipeLeft = new Gesture(swipeLeftGestures, "SwipeLeft");
            SwipeUp = new Gesture(swipeUpGestures, "SwipeUp");
            SwipeUp2 = new Gesture(swipeUpGestures, "SwipeUp2");
            Punch = new Gesture(punchGestures, "Punch");
            Run = new Gesture(runGestures, "Run");
            Sun = new Gesture(sunGestures, "PraiseTheSun");
        }

        /// <summary>
        /// Adds each complex gesture to the list of complex gestures the application can recognize.
        /// </summary>
        public void MakeListOfAllComplexGestures()
        {
            allComplexGestures.Add(SwipeRight);
            allComplexGestures.Add(SwipeLeft);
            allComplexGestures.Add(SwipeUp);
            allComplexGestures.Add(SwipeUp2);
            allComplexGestures.Add(Punch);
            allComplexGestures.Add(Run);
            allComplexGestures.Add(Sun);
        }
    }
}
