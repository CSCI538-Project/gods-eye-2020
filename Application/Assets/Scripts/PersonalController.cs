using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static ProfileParser;

public class PersonalController : MonoBehaviour
{
    public void Begin(){
        try
        {
            PersonalInfo personalInfo = MainDataController.instance.currentProf.profile.personal_info;

            GameObject[] allObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

            foreach (GameObject obj in allObjects)
            {
                if (obj.tag == "AgeTag")
                {
                    obj.GetComponent<TextMeshPro>().text = personalInfo.age.ToString();
                }

                if (obj.tag == "ResidenceStreet")
                {
                    obj.GetComponent<TextMeshPro>().text = personalInfo.residence.apt;
                }

                if (obj.tag == "ResidenceCity")
                {
                    obj.GetComponent<TextMeshPro>().text = personalInfo.residence.city;
                }

                if (obj.tag == "ResidenceState")
                {
                    obj.GetComponent<TextMeshPro>().text = personalInfo.residence.state;
                }

                if (obj.tag == "Edu1TextDegree")
                {
                    obj.GetComponent<TextMeshPro>().text = personalInfo.education[0].degree;
                }

                if (obj.tag == "Edu1TextMajor")
                {
                    obj.GetComponent<TextMeshPro>().text = personalInfo.education[0].major;
                }

                if (obj.tag == "Edu1TextTenure")
                {
                    obj.GetComponent<TextMeshPro>().text = personalInfo.education[0].tenure;
                }

                if (obj.tag == "Edu2TextDegree")
                {
                    obj.GetComponent<TextMeshPro>().text = personalInfo.education[1].degree;
                }

                if (obj.tag == "Edu2TextMajor")
                {
                    obj.GetComponent<TextMeshPro>().text = personalInfo.education[1].major;
                }

                if (obj.tag == "Edu2TextTenure")
                {
                    obj.GetComponent<TextMeshPro>().text = personalInfo.education[1].tenure;
                }

                if (obj.tag == "EmailTextTag")
                {
                    obj.GetComponent<TextMeshPro>().text = personalInfo.email;
                }

                if (obj.tag == "PhoneTextTag")
                {
                    obj.GetComponent<TextMeshPro>().text = personalInfo.contact_info.mobile;
                }
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Personal Begin = " + ex.ToString());
        }
    }
}
