using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static ProfileParser;

public class InterestsController : MonoBehaviour
{
    public GameObject checkinsPrefab;
    public GameObject checkinlocation;

    public GameObject likesPrefab;
    public GameObject likesLocation;


    Vector3 checkin1Pos = new Vector3(0.0f, 0.2225f, 0.0f);
    Vector3 checkin2Pos = new Vector3(0.0f, 0.075f, 0.0f);
    Vector3 checkin3Pos = new Vector3(0.0f, -0.075f, 0.0f);
    Vector3 checkin4Pos = new Vector3(0.0f, -0.225f, 0.0f);

    //likes position vectors
    Vector3 likes1Pos = new Vector3(0.0f, 0.222f, 0.0f);
    Vector3 likes2Pos = new Vector3(0.0f, 0.074f, 0.0f);
    Vector3 likes3Pos = new Vector3(0.0f, -0.074f, 0.0f);
    Vector3 likes4Pos = new Vector3(0.0f, -0.222f, 0.0f);

    public void Begin(){
        try
        {
            List<Checkin> checkins = MainDataController.instance.currentProf.profile.interests_info.checkins;
            List<string> likes = MainDataController.instance.currentProf.profile.interests_info.recent_likes;

            int numCheckins = Mathf.Min(checkins.Count, 4);
            int numLikes = Mathf.Min(likes.Count, 4);

            for (int i = 0; i < numCheckins; i++)
            {
                GameObject checkinCopy = Instantiate(checkinsPrefab, checkinlocation.transform);

                if (i == 0)
                {
                    checkinCopy.transform.localPosition = checkin1Pos;
                }
                else if (i == 1)
                {
                    checkinCopy.transform.localPosition = checkin2Pos;
                }
                else if (i == 2)
                {
                    checkinCopy.transform.localPosition = checkin3Pos;
                }
                else if (i == 3)
                {
                    checkinCopy.transform.localPosition = checkin4Pos;
                }

                //set the text values for each transaction
                checkinCopy.transform.GetChild(1).GetComponent<TextMeshPro>().text = checkins[i].location;
                checkinCopy.transform.GetChild(2).GetComponent<TextMeshPro>().text = checkins[i].date;
            }


            for (int i = 0; i < numLikes; i++)
            {
                GameObject likesCopy = Instantiate(likesPrefab, likesLocation.transform);

                if (i == 0)
                {
                    likesCopy.transform.localPosition = likes1Pos;
                }
                else if (i == 1)
                {
                    likesCopy.transform.localPosition = likes2Pos;
                }
                else if (i == 2)
                {
                    likesCopy.transform.localPosition = likes3Pos;
                }
                else if (i == 3)
                {
                    likesCopy.transform.localPosition = likes4Pos;
                }

                //set the text values for each transaction
                likesCopy.transform.GetChild(1).GetComponent<TextMeshPro>().text = likes[i];
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Interest Begin Error" + ex.ToString());
        }
    }

    public void Reset(){
        foreach (Transform t in checkinlocation.transform){
            GameObject.Destroy(t.gameObject);
        }

        foreach (Transform t in likesLocation.transform){
            GameObject.Destroy(t.gameObject);
        }
    }
}
