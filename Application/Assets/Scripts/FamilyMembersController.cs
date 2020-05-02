using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static ProfileParser;
using System;

//show up to 3 family members in the connections view
public class FamilyMembersController : MonoBehaviour
{
    Vector3 member1Pos = new Vector3(0.0f, 0.2f, 0.0f);
    Vector3 member2Pos = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 member3Pos = new Vector3(0.0f, -0.2f, 0.0f);

    public GameObject familyMemberPrefab;
    public GameObject familyMemberLocation;

    string familyImagesDirBase = "family_pictures/";


    public void Begin(){
        try
        {
            CreateMemberArrays();
        }
        catch(Exception ex)
        {
            Debug.Log("Familt Begin Error"+ex.ToString());
        }
    }

    Sprite ImportImage(string imagePath){
        Texture2D image = Resources.Load<Texture2D>(imagePath);
        return Sprite.Create(image, new Rect(0, 0, image.width, image.height), Vector2.zero);
    }

    void CreateMemberArrays(){
        //familyMembersPrefabs = new List<GameObject>();

        List<FamilyMember> familyMembers = MainDataController.instance.currentProf.profile.connections.family_members;
        int numFamilyMembers = Mathf.Min(familyMembers.Count, 3);

        string familyImagesDir = MainDataController.instance.currentProf.profile.connections.family_members_images_dir;

        for (int i = 0; i < numFamilyMembers; i++){
            GameObject memberCopy = Instantiate(familyMemberPrefab, familyMemberLocation.transform);

            if (i == 0)
            {
                memberCopy.transform.localPosition = member1Pos;
            }
            else if (i == 1)
            {
                memberCopy.transform.localPosition = member2Pos;
            }
            else if (i == 2)
            {
                memberCopy.transform.localPosition = member3Pos;
            }

            Sprite familyImage = ImportImage(familyImagesDirBase + familyImagesDir + "/" + familyMembers[i].url);
            memberCopy.GetComponentInChildren<Image>().sprite = familyImage;
            memberCopy.GetComponentInChildren<Image>().preserveAspect = true;

            memberCopy.transform.GetChild(1).GetComponent<TextMeshPro>().text = familyMembers[i].name;
            memberCopy.transform.GetChild(2).GetComponent<TextMeshPro>().text = familyMembers[i].relation;
        }
    }

    public void Reset(){
        foreach (Transform t in familyMemberLocation.transform){
            GameObject.Destroy(t.gameObject);
        }
    }
}
