using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script only starts and stops the loading display on the cursor
public class ButtonCursorController : MonoBehaviour
{
    public void BeginLoadingCursor(float speed){
        RadialProgress.instance.Begin(speed);
    }

    public void EndLoadingCursor(){
        RadialProgress.instance.End();
    }
}
