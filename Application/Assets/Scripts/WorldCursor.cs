using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldCursor : MonoBehaviour
{
    Canvas pointerCanvas;

    // Start is called before the first frame update
    void Start()
    {
        pointerCanvas = gameObject.GetComponentInChildren<Canvas>();
        pointerCanvas.sortingOrder = 2;
    }

    void Update()
    {
        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;

        pointerCanvas.enabled = false;

        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo)){
            GameObject objectHit = hitInfo.transform.gameObject;

            if (objectHit.activeInHierarchy && objectHit.tag != "AnimCollider"){
                // If the raycast hit a hologram...
                // Display the cursor mesh.
                pointerCanvas.enabled = true;

                // Move the cursor to the point where the raycast hit.
                this.transform.position = hitInfo.point;
            }
        } else {
            // If the raycast did not hit a hologram, hide the cursor mesh.
            pointerCanvas.enabled = false;
        }
    }
}
