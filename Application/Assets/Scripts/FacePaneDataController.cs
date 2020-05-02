using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class FacePaneDataController : MonoBehaviour
{
    string faceImgDir = "face_images/";

    Sprite ImportImage(string imagePath){
        Texture2D image = Resources.Load(imagePath) as Texture2D;
        return Sprite.Create(image, new Rect(0, 0, image.width, image.height), Vector2.zero);
    }

    private Color32 ReturnColor32(int score)
    {
        if (score <= 10 && score > 0) 
            return new Color32(255, 0, 0, 255);
        else if (score > 20 && score <= 30)
            return new Color32(255, 64, 0, 255);
        else if(score > 30 && score <= 40)
            return new Color32(255, 128, 0, 255);
        else if (score > 40 && score <= 50)
            return new Color32(255, 191, 0, 255);
        else if (score > 50 && score <= 60)
            return new Color32(255, 255, 0, 255);
        else if (score > 60 && score <= 70)
            return new Color32(191, 255, 0, 255);
        else if (score > 70 && score <= 80)
            return new Color32(128, 255, 0, 255);
        else if (score > 80 && score <= 90)
            return new Color32(64, 255, 0, 255);
        else
            return new Color32(0, 255, 0, 255);
    }

    private string GetVulnerabilityText(int score)
    {
        if (score < 26 && score > 0){
            return "Fortified";
        } else if (score >= 26 && score < 51){
            return "Low Vulnerability";
        } else if (score >= 51 && score < 76){
            return "Medium Vulnerability";
        } else {
            return "Highly Vulnerable";
        }
    }

    public void Begin()
    {
        try
        {
            string faceImg = MainDataController.instance.currentProf.profile.face_image;
            Debug.Log("FaceIMG in FACEPANE"+faceImg.ToString());
            Sprite face = ImportImage(faceImgDir + faceImg);

            int score = MainDataController.instance.currentProf.score;
            string firstName = MainDataController.instance.currentProf.profile.first_name;
            string lastName = MainDataController.instance.currentProf.profile.last_name;

            GameObject[] allObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

            foreach (GameObject obj in allObjects)
            {
                if (obj != null)
                {
                    if (obj.tag == "FacePane")
                    {
                        Debug.Log("FacePane");
                        if (obj.GetComponentInChildren<Image>() != null)
                        {
                            obj.GetComponentInChildren<Image>().sprite = face;
                            obj.GetComponentInChildren<Image>().preserveAspect = true;
                        }
                        Debug.Log("FacePane End");
                    }

                    if (obj.tag == "FacePaneScore")
                    {
                        Debug.Log("FacePaneScore");
                        obj.GetComponent<TextMeshPro>().text = score.ToString();
                        obj.GetComponent<TextMeshPro>().color = ReturnColor32(score);
                        Debug.Log("FacePaneScore End");
                    }

                    if (obj.tag == "FacePaneIndex")
                    {
                        Debug.Log("FacePaneIndex");
                        obj.GetComponent<TextMeshPro>().text = GetVulnerabilityText(score);
                        Debug.Log("FacePaneIndex End");
                    }

                    if (obj.tag == "FullNameTag")
                    {
                        Debug.Log("FacePaneFullName" + firstName + " " + lastName);
                        obj.GetComponent<TextMeshPro>().text = firstName + " " + lastName;
                        Debug.Log("FacePaneFullName End " + firstName + " " + lastName);
                    }

                    if (obj.tag == "ScoreDisplayBackground")
                    {
                        Debug.Log("ScoreDisplayBackground");
                        obj.GetComponent<SpriteRenderer>().color = ReturnColor32(score);
                        Debug.Log("ScoreDisplayBackground End");
                    }
                }
            }
        }
        catch(Exception ex)
        {
            Debug.Log("FacePane Begin = " + ex.ToString());
        }
    }
}
