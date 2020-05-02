using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.UI
{
    //controls how long a button can be focused on before its state is set to not be focused
    public class ButtonFocusTime : MonoBehaviour
    {
        public float focusTime;

        public float GetFocusTime(){
            return focusTime;
        }
    }
}
