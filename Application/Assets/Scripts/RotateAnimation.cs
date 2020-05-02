using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotateAnimation : MonoBehaviour
{
    //for every circle in the UI, continuously rotate it
    public List<GameObject> UICircles;

    //how fast to rotate the circles and in what direction, specified for each circle in the above list
    public List<float> RotateStep; 


    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        for (int i = 0; i < UICircles.Count; i++){
            if (UICircles[i].activeInHierarchy){ //only rotate the object if it is active
                UICircles[i].transform.Rotate(0.0f, 0.0f, RotateStep[i]);
            }
        }
    }
}
