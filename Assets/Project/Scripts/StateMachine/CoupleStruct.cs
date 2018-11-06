using System;
using System.Collections.Generic;

namespace KinectOverlay
{
    [Serializable]
    public class CoupleStruct
    {
        public List<GestureId> leftAndRightHandSymbols = new List<GestureId>();

        public List<GestureId> LeftAndRightHandSymbols
        {
            get
            {
                return leftAndRightHandSymbols;
            }

            set
            {
                leftAndRightHandSymbols = value;
            }
        }

        public CoupleStruct()
        {
            LeftAndRightHandSymbols.Add(GestureId.NONE);
            LeftAndRightHandSymbols.Add(GestureId.NONE);
        }

        public CoupleStruct(GestureId left, GestureId right)
        {
            LeftAndRightHandSymbols.Add(left);
            LeftAndRightHandSymbols.Add(right);
        }
    }

}