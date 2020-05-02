using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static ProfileParser;

//controller for showing recent venmo transactions in the financial view
public class UsernameController : MonoBehaviour
{
    string facebookURL;
    string instagramURL;
    string twitterURL;
    string linkedinURL;

    public GameObject facebookButton;
    public GameObject instagramButton;
    public GameObject twitterButton;
    public GameObject linkedinButton;


    public void Begin()
    {
        try
        {
            facebookButton.SetActive(true);
            instagramButton.SetActive(true);
            twitterButton.SetActive(true);
            linkedinButton.SetActive(true);

            loadUsernames();
        }
        catch(Exception ex)
        {
            Debug.Log("Username Begin = "+ex.ToString());
        }
    }

	void loadUsernames()
	{
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

        foreach (GameObject obj in allObjects){
            if (obj.tag == "FacebookUsername"){
                obj.GetComponent<TextMeshPro>().text = MainDataController.instance.currentProf.profile.social_media_info.facebook.username;
            }

            if (obj.tag == "InstagramUsername"){
                obj.GetComponent<TextMeshPro>().text = MainDataController.instance.currentProf.profile.social_media_info.instagram.username;
            }

            if (obj.tag == "LinkedinUsername"){
                obj.GetComponent<TextMeshPro>().text = MainDataController.instance.currentProf.profile.social_media_info.linkedin.username;
            }

            if (obj.tag == "TwitterUsername"){
                obj.GetComponent<TextMeshPro>().text = MainDataController.instance.currentProf.profile.social_media_info.twitter.username;
            }
        }

        facebookURL = MainDataController.instance.currentProf.profile.social_media_info.facebook.url;
        instagramURL = MainDataController.instance.currentProf.profile.social_media_info.instagram.url;
        twitterURL = MainDataController.instance.currentProf.profile.social_media_info.twitter.url;
        linkedinURL = MainDataController.instance.currentProf.profile.social_media_info.linkedin.url;

        if (facebookURL == ""){
            facebookButton.SetActive(false);
        }

        if (instagramURL == ""){
            instagramButton.SetActive(false);
        }

        if (twitterURL == ""){
            twitterButton.SetActive(false);
        }

        if (linkedinURL == ""){
            linkedinButton.SetActive(false);
        }
    }


    public void OpenFacebook(){
        Application.OpenURL(facebookURL);
    }

    public void OpenInstagram(){
        Application.OpenURL(instagramURL);
    }

    public void OpenTwitter(){
        Application.OpenURL(twitterURL);
    }

    public void OpenLinkedIn(){
        Application.OpenURL(linkedinURL);
    }
}
