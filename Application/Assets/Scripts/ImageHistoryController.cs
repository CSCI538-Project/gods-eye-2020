using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI;
using Assets.Scripts.ConnManager;

//controller for showing the recent social media images in the personal view
public class ImageHistoryController : MonoBehaviour
{
    [Header("Base")]
    public GameObject ImageBaseLocation; //location where image prefabs will be instantiated
    public GameObject BaseImagePrefab;

    [Header("Buttons")]
    public GameObject backButton; //previous button
    public GameObject forwardButton; //next button

    private int numImages = 0;
    private int currentImage = 0;
    private ConnectionManager connection;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Load ImageHistoryController");
        toggleBackButton(false);

        this.connection = new ConnectionManager();

        MainDataController.instance.DataCenterInstance.UpdatedNotification += new MainDataController.DataCenter.UpdatedEventHandler((updatedType) => {
            // Handling the updated info
            if (updatedType == MainDataController.DataCenter.UpdatedType.ObtainedScreenshotHistory)
            {
                this.Reset();
                this.createAllImageObjectsBy(MainDataController.instance.DataCenterInstance.GetHistoryOfImagesInURLs());
            }
        });
    }

    private void createAllImageObjectsBy(List<string> imageURLs) 
    {
        //if (imageURLs.Count == 0) { return; }
        this.numImages = imageURLs.Count;
        //if (this.numLoadedImages == this.numImages) { Debug.Log("Already loaded."); return; }
        
        for (int i = 0; i < this.numImages; i++) 
        {
            GameObject imageCopy = Instantiate(BaseImagePrefab, ImageBaseLocation.transform);
            StartCoroutine(connection.LoadRawImageTo(imageCopy.GetComponentInChildren<RawImage>(), imageURLs[i], 400, 300));

            if (i != 0)
            { //hide all other images except the first one
                imageCopy.SetActive(false);
                //ImageBaseLocation.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        if (numImages < 2) { toggleForwardButton(false); }
        else { toggleForwardButton(true); }
    }

    //display the next image in the sequence
    public void NextImage()
    {
        ImageBaseLocation.transform.GetChild(currentImage).gameObject.SetActive(false);

        currentImage++;
        toggleBackButton(true);

        if (currentImage == numImages - 1)
        {
            toggleForwardButton(false);
        }

        ImageBaseLocation.transform.GetChild(currentImage).gameObject.SetActive(true);
    }

    //display the previous image in the sequence
    public void PreviousImage()
    {
        ImageBaseLocation.transform.GetChild(currentImage).gameObject.SetActive(false);

        currentImage--;
        toggleForwardButton(true);

        if (currentImage == 0)
        {
            toggleBackButton(false);
        }

        ImageBaseLocation.transform.GetChild(currentImage).gameObject.SetActive(true);
    }

    //turn the back button on/off
    void toggleBackButton(bool state)
    {
        backButton.SetActive(state);
    }

    //turn the forward button on/off
    void toggleForwardButton(bool state)
    {
        forwardButton.SetActive(state);
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable in ImageHistoryController.");
        this.Reset();
        
    }

    public void Reset()
    {
        foreach (Transform t in ImageBaseLocation.transform)
        {
            Debug.Log(t.name);
            GameObject.Destroy(t.gameObject);
        }

        this.numImages = 0;
        this.currentImage = 0;
    }

    private string getJSON(string firstName) 
    {
        return "{\"command\":\"execution\", \"content\": { \"session_id\":-1, \"action\": \"history_images\", \"para\":\""+firstName+"\" }}";
    }
}
