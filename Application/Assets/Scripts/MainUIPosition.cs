using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIPosition : MonoBehaviour
{
    //always keep the main UI at this y level
    public float yValue;

    // Update is called once per frame
    void Update(){
        this.transform.position = new Vector3(this.transform.position.x, yValue, this.transform.position.z);
    }
}
