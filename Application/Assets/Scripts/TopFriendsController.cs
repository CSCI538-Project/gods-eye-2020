using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static ProfileParser;
using System;

//show up to 3 top friends in the connections view
public class TopFriendsController : MonoBehaviour
{
    public GameObject friendPrefab;
    public GameObject friendLocation;
    List<GameObject> friendPrefabs = new List<GameObject>();

    Vector3 friend1Pos = new Vector3(0.0f, 0.2f, 0.0f);
    Vector3 friend2Pos = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 friend3Pos = new Vector3(0.0f, -0.2f, 0.0f);

    string friendsImagesDirBase = "friend_pictures/";


    public void Begin(){
        try
        {
            CreateFriendArrays();
        }
        catch(Exception ex)
        {
            Debug.Log("Error in Top Friends" + ex.ToString());
        }
    }

    Sprite ImportImage(string imagePath){
        Texture2D image = Resources.Load(imagePath) as Texture2D;
        return Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f), 40);
    }

    void CreateFriendArrays(){
        List<SocialMediaFriend> friends = MainDataController.instance.currentProf.profile.connections.social_media_friends;
        int numFriends = Mathf.Min(friends.Count, 3);

        GameObject[] allObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

        foreach (GameObject obj in allObjects){
            if (obj.tag == "RelationshipStatus")
            {
                obj.GetComponent<TextMeshPro>().text = MainDataController.instance.currentProf.profile.connections.relationship_status;
            }
        }

        string friendsImagesDir = MainDataController.instance.currentProf.profile.connections.social_media_friends_images_dir;

        for (int i = 0; i < numFriends; i++){
            GameObject friendCopy = Instantiate(friendPrefab, friendLocation.transform);

            if (i == 0)
            {
                friendCopy.transform.localPosition = friend1Pos;
            }
            else if (i == 1)
            {
                friendCopy.transform.localPosition = friend2Pos;
            }
            else if (i == 2)
            {
                friendCopy.transform.localPosition = friend3Pos;
            }

            Sprite friendImage = ImportImage(friendsImagesDirBase + friendsImagesDir + "/" + friends[i].url);

            friendCopy.GetComponentInChildren<Image>().sprite = friendImage;
            friendCopy.GetComponentInChildren<Image>().preserveAspect = true;
            friendCopy.transform.GetChild(1).GetComponent<TextMeshPro>().text = friends[i].name;
        }
    }

    public void Reset(){
        foreach (Transform t in friendLocation.transform){
            GameObject.Destroy(t.gameObject);
        }
    }
}
