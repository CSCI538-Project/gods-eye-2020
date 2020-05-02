using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ProfileParser;
using UnityEngine.XR.WSA.Input;
using Assets.Scripts.ConnManager;
using Assets.Scripts.Parsers;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System.IO;
using Assets.Scripts.Configs;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using Assets.Scripts.Database;

public class VictimInfoController : MonoBehaviour
{

    [Header("All Views")]
    public GameObject VictimInfoPanel;
    public GameObject ActionView;
    //public GameObject VictimDataPrefab;

    [Header("Victim Components")]
    public GameObject FirstVictim;
    public GameObject SecondVictim;

    [Header("First Victim Components")]
    public GameObject FirstVictimName;
    public GameObject FirstVictimImage;
    public GameObject FirstVictimScore;
    public GameObject FirstVictimScoreCircle;

    [Header("Second Victim Components")]
    public GameObject SecondVictimName;
    public GameObject SecondVictimImage;
    public GameObject SecondVictimScore;
    public GameObject SecondVictimScoreCircle;

    [Header("Connection Manager")]
    private ConnectionManager connection;

    [Header("Button Components")]
    public GameObject ScrollUp;
    public GameObject ScrollDown;

    [Header("Filter Component")]
    public GameObject Filter1; //76-100
    public GameObject Filter2; //51-75
    public GameObject Filter3; //26-50
    public GameObject Filter4; //0-25
    public GameObject ResetFilter;


    int currentVictim = 0;
    int numVictims = 0;
    public static int victim_id = 0;
    private static string currentDisplayMode = "all";

    public List<IdentifiedVictimsParser.IdentifiedVictim> AllVictimsList;
    public List<IdentifiedVictimsParser.IdentifiedVictim> FilteredVictimList;


    public void ApplyFilter1()
    {
        currentDisplayMode = "filter1";
        ApplyVulnerabilityFilter();
        UpdateVictimInfo();
    }

    public void ApplyFilter2()
    {
        currentDisplayMode = "filter2";
        ApplyVulnerabilityFilter();
        UpdateVictimInfo();
    }
    public void ApplyFilter3()
    {
        currentDisplayMode = "filter3";
        ApplyVulnerabilityFilter();
        UpdateVictimInfo();
    }
    public void ApplyFilter4()
    {
        currentDisplayMode = "filter4";
        ApplyVulnerabilityFilter();
        UpdateVictimInfo();
    }

    public void ApplyResetFilter()
    {
        currentDisplayMode = "all";
        ApplyVulnerabilityFilter();
        UpdateVictimInfo();
    }

    public void ResetUIElements()
    {
        scrollUp(false);
        scrollDown(false);
        FirstVictim.SetActive(false);
        SecondVictim.SetActive(false);
    }

    void Start()
    {
        try
        {
            Debug.Log("VictimInfoController Started");
            ResetUIElements();

            connection = new ConnectionManager();

            if (MainDataController.instance.DataCenterInstance.GetIdentifiedVictimsParser() != null && MainDataController.instance.DataCenterInstance.GetIdentifiedVictimsParser().GetAllIdentifiedVictims() != null)
            {
                GetAllIdentifiedVictims();
            }

            MainDataController.instance.DataCenterInstance.UpdatedNotification += new MainDataController.DataCenter.UpdatedEventHandler((updatedType) =>
            {
                if (updatedType == MainDataController.DataCenter.UpdatedType.IdentifiedVictims)
                {
                    GetAllIdentifiedVictims();
                }
                else { Debug.Log("Still waiting to be notified for identified victim update."); }
            });

        }
        catch (Exception nre)
        {
            Debug.Log(nre.ToString());
        }
    }


    private void GetAllIdentifiedVictims()
    {
        if (MainDataController.instance.DataCenterInstance.GetIdentifiedVictimsParser() != null && MainDataController.instance.DataCenterInstance.GetIdentifiedVictimsParser().GetAllIdentifiedVictims() != null)
        {
            AllVictimsList = MainDataController.instance.DataCenterInstance.GetIdentifiedVictimsParser().GetAllIdentifiedVictims();
            AllVictimsList.Sort((record1, record2) => record2.ProfileInfo.VictimInfo.score.CompareTo(record1.ProfileInfo.VictimInfo.score));
            ApplyVulnerabilityFilter();
            UpdateVictimInfo();
        }
    }

    private Color32 ReturnColor32(int score)
    {
        if (score <= 20 && score > 0)
            return new Color32(255, 0, 0, 255);
        else if (score > 20 && score <= 30)
            return new Color32(255, 64, 0, 255);
        else if (score > 30 && score <= 40)
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

    void Display()
    {
        if (currentVictim < numVictims)
        {
            FirstVictimName.GetComponent<TextMeshPro>().text = FilteredVictimList[currentVictim].ProfileInfo.Name;
            int score = FilteredVictimList[currentVictim].ProfileInfo.VictimInfo.score;
            FirstVictimScore.GetComponent<TextMeshPro>().text = score.ToString();
            FirstVictimScore.GetComponent<TextMeshPro>().color = ReturnColor32(score);
            FirstVictimScoreCircle.GetComponent<SpriteRenderer>().color = ReturnColor32(score);
            StartCoroutine(connection.LoadRawImageTo(FirstVictimImage.GetComponent<RawImage>(), "/" + FilteredVictimList[currentVictim].ProfileInfo.Picture, 10, 10));
            FirstVictim.SetActive(true);
        }
        else
        {
            FirstVictim.SetActive(false);
        }

        //load the second victim's info (if exist)
        if (currentVictim + 1 < numVictims)
        {
            int score = FilteredVictimList[currentVictim + 1].ProfileInfo.VictimInfo.score;
            SecondVictimName.GetComponent<TextMeshPro>().text = FilteredVictimList[currentVictim + 1].ProfileInfo.Name;
            SecondVictimScore.GetComponent<TextMeshPro>().text = score.ToString();
            SecondVictimScore.GetComponent<TextMeshPro>().color = ReturnColor32(score);
            SecondVictimScoreCircle.GetComponent<SpriteRenderer>().color = ReturnColor32(score);
            StartCoroutine(connection.LoadRawImageTo(SecondVictimImage.GetComponent<RawImage>(), "/" + FilteredVictimList[currentVictim + 1].ProfileInfo.Picture, 10, 10));

            SecondVictim.SetActive(true);
        }
        else
        {
            SecondVictim.SetActive(false);
        }
    }

    void ApplyVulnerabilityFilter()
    {
        Debug.Log("applying filter for " + currentDisplayMode);
        FilteredVictimList = new List<IdentifiedVictimsParser.IdentifiedVictim>();
        int low = 0;
        int high = 100;
        switch (currentDisplayMode)
        {
            case "all":
                low = 0;
                high = 100;
                break;
            case "filter1":
                low = 76;
                high = 100;
                break;
            case "filter2":
                low = 51;
                high = 75;
                break;
            case "filter3":
                low = 26;
                high = 50;
                break;
            case "filter4":
                low = 0;
                high = 25;
                break;
            default:
                low = 0;
                high = 100;
                break;
        }
        Debug.Log("low, high = " + low.ToString() + "," + high.ToString());

        for (int i = 0; i < AllVictimsList.Count; i++)
        {
            int score = AllVictimsList[i].ProfileInfo.VictimInfo.score;
            if (score >= low && score <= high)
            {
                Debug.Log("Score: " + score.ToString());
                FilteredVictimList.Add(AllVictimsList[i]);
            }
        }
        FilteredVictimList.Sort((record1, record2) => record2.ProfileInfo.VictimInfo.score.CompareTo(record1.ProfileInfo.VictimInfo.score));
        numVictims = FilteredVictimList.Count;
        currentVictim = 0;
    }


    public static bool IsArrayEmpty<T>(List<T> array)
    {
        if (array == null || array.Count == 0)
        {
            return true;
        }
        return false;
    }

    void UpdateVictimInfo()
    {

        Display();

        if (numVictims < 3)
        {
            scrollDown(false);
        }
        else
        {
            scrollDown(true);
        }
    }


    //display the next two victims
    public void NextVictim()
    {
        currentVictim += 2;
        scrollUp(true);

        //if only one next victim left, move one step
        if (currentVictim >= numVictims - 1)
        {
            currentVictim = numVictims - 2;
        }
        if (currentVictim == numVictims - 2)
        {
            scrollDown(false);
        }

        UpdateVictimInfo();
    }

    //display the previous transaction and move the top one to the bottom
    public void previousVictim()
    {
        currentVictim -= 2;
        scrollDown(true);

        //if only one previous victim left, move one step
        if (currentVictim < 0)
        {
            currentVictim = 0;
        }
        if (currentVictim == 0)
        {
            scrollUp(false);
        }

        UpdateVictimInfo();
    }

    //turn the up button on/off
    void scrollUp(bool state)
    {
        ScrollUp.SetActive(state);
    }

    //turn the down button on/off
    void scrollDown(bool state)
    {
        ScrollDown.SetActive(state);
    }

    public void Reset()
    {
        foreach (Transform t in FirstVictim.transform)
        {
            GameObject.Destroy(t.gameObject);
        }

        foreach (Transform t in SecondVictim.transform)
        {
            GameObject.Destroy(t.gameObject);
        }
    }


    public void ScrollDownVictims()
    {
        NextVictim();
    }

    public void ScrollUpVictims()
    {
        previousVictim();
    }


    public void FirstVictimOnHold()
    {
        Debug.Log("Clicked on First Victim");
        loadActionScene(currentVictim);
    }

    public void SecondVictimOnHold()
    {
        Debug.Log("Clicked on Second Victim");
        loadActionScene(currentVictim + 1);
    }

    void DisconnectFromServer()
    {
        connection.CloseConnection();
    }


    public void loadActionScene(int index)
    {
        if (FilteredVictimList.Count == 0) { return; } // Cannot load because there is no victims
        if (index >= FilteredVictimList.Count || index < 0) { return; } // Cannot load becase the `index` is out of range

        VictimInfoPanel.SetActive(false);
        ActionsPageController.SetSessionID(FilteredVictimList[index].SessionID);
        string recName = FilteredVictimList[index].ProfileInfo.VictimInfo.profile.first_name + " " + FilteredVictimList[index].ProfileInfo.VictimInfo.profile.last_name;
        ActionsPageController.SetNames(FilteredVictimList[index].ProfileInfo.VictimInfo.profile.first_name, FilteredVictimList[index].ProfileInfo.VictimInfo.profile.last_name, recName);
        ActionsPageController.SetShowDetails(true);
        ActionView.SetActive(true);
        ActionsPageController.instance.Begin();
    }
}
