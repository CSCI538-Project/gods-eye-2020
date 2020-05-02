using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class FinancialController : MonoBehaviour
{
    public void Begin(){
        try
        {
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

            foreach (GameObject obj in allObjects)
            {
                if (obj.tag == "CompanyName")
                {
                    obj.GetComponent<TextMeshPro>().text = MainDataController.instance.currentProf.profile.financial_info.current_company;
                }

                if (obj.tag == "CompanyPosition")
                {
                    obj.GetComponent<TextMeshPro>().text = MainDataController.instance.currentProf.profile.financial_info.position;
                }

                if (obj.tag == "Salary")
                {
                    obj.GetComponent<TextMeshPro>().text = MainDataController.instance.currentProf.profile.financial_info.salary_string;
                }
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Error Financial Begin" + ex.ToString());
        }
    }
}
