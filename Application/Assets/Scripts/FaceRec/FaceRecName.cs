using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceRecName : MonoBehaviour
{
    public static FaceRecName instance;

    [HideInInspector]
    public string recName;

    public Text displayText;
    public float delay;


    // Start is called before the first frame update
    void Start(){
        if (instance == null){
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        //Begin();
        recName = "";
    }

    // Update is called once per frame
    void Update(){
        //only for testing in unity editor
        if (Input.GetKeyDown(KeyCode.Z))
        {
            displayText.text = "Found Face:\nTESTING 1";
            Begin();
            recName = "Gagan Vasudev";
        }

        if (Input.GetKeyDown(KeyCode.X)){
            displayText.text = "Found Face:\nTESTING 2";
            Begin();
            recName = "Victor Zamarian";
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            displayText.text = "Found Face:\nTESTING 3";
            Begin();
            recName = "Santhosh Narayanan";
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            displayText.text = "Found Face:\nTESTING 4";
            Begin();
            recName = "Shwetha Rao";
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            displayText.text = "Found Face:\nTESTING 5";
            Begin();
            recName = "Maria Fernandes";
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            displayText.text = "Found Face:\nTESTING 6";
            Begin();
            recName = "Yash Patel";
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            displayText.text = "Found Face:\nTESTING 7";
            Begin();
            recName = "Yatin Gupta";
        }
    }

    public void Begin(){
        Invoke("DelayedBegin", delay);
    }

    void DelayedBegin(){
        displayText.text = "";
        MainDataController.instance.InsertData();
    }

    public void RemoveName(){
        recName = "";
    }
}
