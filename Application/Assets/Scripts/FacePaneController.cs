using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FacePaneController : MonoBehaviour
{
    public GameObject faceButton;
    public GameObject nameText;
    public GameObject faceIndex;
    public GameObject screen;

    public float upForceScale;
    public float leftForceScale;

    public Transform topPosition;
    public Transform leftPosition;

    public GameObject colliderCollection;

    public GameObject score;
    
    Rigidbody rb;
    Vector3 leftForce;

    bool nextMove = false;
    bool doneMove = false;
    bool inactive = true;


    // Start is called before the first frame update
    void Start(){
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){
        if (nextMove){
            nextMove = false;
            leftForce = leftForceScale * Vector3.Normalize(leftPosition.position - topPosition.position);
            rb.AddForce(leftForce);
        }

        if (doneMove){
            transform.localPosition = new Vector3(-0.63f, 0.0f, 0.0f);
        }
        
        if (inactive){
            transform.localPosition = Vector3.zero;
        }

        gameObject.GetComponent<BoxCollider>().isTrigger = inactive;
        colliderCollection.transform.GetChild(2).GetComponent<BoxCollider>().isTrigger = inactive;
        colliderCollection.transform.GetChild(3).GetComponent<BoxCollider>().isTrigger = inactive;
    }

    public void Activate(){
        inactive = false;
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void BeginMove(){
        nextMove = true;
        faceButton.SetActive(false);
    }

    void OnTriggerEnter(Collider col){
        //collided with left collider, now stop moving
        if (col.gameObject.name == "LeftCollider"){
            rb.constraints |= RigidbodyConstraints.FreezeAll;
            doneMove = true;
            nameText.SetActive(true);
            faceIndex.SetActive(false);
            screen.SetActive(true);
            colliderCollection.SetActive(false);
            //score.transform.localPosition = new Vector3(0.3f, 0.35f, 0.0f);
            nextMove = false;
        }
    }

    public void Reset(){
        transform.localPosition = Vector3.zero;
        //score.transform.localPosition = new Vector3(0.0f, 0.35f, 0.0f);
        nameText.SetActive(false);
        screen.SetActive(false);
        colliderCollection.SetActive(true);
        faceIndex.SetActive(true);
        faceButton.SetActive(true);
        gameObject.SetActive(true);
        inactive = true;
        doneMove = false;
    }
}