using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI;


//controller for showing the recent social media images in the personal view
public class RecentImagesController : MonoBehaviour
{
    public GameObject ImageBaseLocation; //location where image prefabs will be instantiated
    public GameObject BaseImagePrefab;

    int numImages = 0;

    List<Sprite> RecentImages;

    int currentImage = 0;

    public GameObject backButton; //previous button
    public GameObject forwardButton; //next button

    //recent social media images will be stored in this directory when pulling in user data
    string recentImagesDirBase = "recent_images/";


    // Start is called before the first frame update
    void Start(){
        //CreateImageArrays();

        //toggleBackButton(false);
    }

    public void Begin()
    {
        try
        {
            CreateImageArrays();
            toggleBackButton(false);
        }
        catch(System.Exception ex)
        {
            Debug.Log("RecentImages Begin = " + ex.ToString());
        }
    }

    void ImportImages(){
        RecentImages = new List<Sprite>();

        string profileImages = MainDataController.instance.currentProf.profile.personal_info.recent_images;

        Object[] images = Resources.LoadAll(recentImagesDirBase + profileImages);

        //for all images in the directory, create a sprite for them
        foreach (Object obj in images){
            Texture2D tex = (Texture2D)obj;
            RecentImages.Add(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero));
        }

        numImages = RecentImages.Count;
    }

    void CreateImageArrays(){
        ImportImages();
        currentImage = 0;

        for (int i = 0; i < numImages; i++){
            //add the image prefab to the scene inside the recent images data element
            GameObject imageCopy = Instantiate(BaseImagePrefab, ImageBaseLocation.transform);

            //set the sprite
            imageCopy.GetComponentInChildren<Image>().sprite = RecentImages[i];
            imageCopy.GetComponentInChildren<Image>().preserveAspect = true;
           
            if (i != 0){ //hide all other images except the first one
                ImageBaseLocation.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        if (numImages < 2){
            toggleForwardButton(false);
        } else {
            toggleForwardButton(true);
        }
    }

    //display the next image in the sequence
    public void NextImage(){
        ImageBaseLocation.transform.GetChild(currentImage).gameObject.SetActive(false);

        currentImage++;
        toggleBackButton(true);

        if (currentImage == numImages - 1){
            toggleForwardButton(false);
        }

        ImageBaseLocation.transform.GetChild(currentImage).gameObject.SetActive(true);
    }

    //display the previous image in the sequence
    public void PreviousImage(){
        ImageBaseLocation.transform.GetChild(currentImage).gameObject.SetActive(false);

        currentImage--;
        toggleForwardButton(true);

        if (currentImage == 0){
            toggleBackButton(false);
        }

        ImageBaseLocation.transform.GetChild(currentImage).gameObject.SetActive(true);
    }

    //turn the back button on/off
    void toggleBackButton(bool state){
        backButton.SetActive(state);
    }

    //turn the forward button on/off
    void toggleForwardButton(bool state){
        forwardButton.SetActive(state);
    }

    public void Reset(){
        foreach (Transform t in ImageBaseLocation.transform){
            GameObject.Destroy(t.gameObject);
        }
    }
}
