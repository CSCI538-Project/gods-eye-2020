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

    //public static Dictionary<string, ProfileParser> profileMatcher = new Dictionary<string, ProfileParser>();
    //public static Dictionary<string, int> victimIdMatcher = new Dictionary<string, int>();
    //public static ProfileParser clickedOnProfile;

    public List<IdentifiedVictimsParser.IdentifiedVictim> AllVictimsList;
    public List<IdentifiedVictimsParser.IdentifiedVictim> FilteredVictimList;
    //public Dictionary<string, IdentifiedVictimsParser.IdentifiedVictim> victimDict = new Dictionary<string, IdentifiedVictimsParser.IdentifiedVictim>();


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
                Debug.Log("Called from top");
            }

            MainDataController.instance.DataCenterInstance.UpdatedNotification += new MainDataController.DataCenter.UpdatedEventHandler((updatedType) =>
            {
                if (updatedType == MainDataController.DataCenter.UpdatedType.IdentifiedVictims)
                {
                    GetAllIdentifiedVictims();
                    Debug.Log("Called from bottom");
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


    //public void SaveToHistory()
    //{
    //    try
    //    {
    //        string path = Application.persistentDataPath + "/victim_history.json";
    //        string output = @"{
    //        'profile_Gagan Vasudev': {
    //              'name':     'Gagan',
    //              'picture':  'DB/Gagan/gagan_face.jpg',
    //              'privacy':  'DB/Gagan/profile_Gagan Vasudev.json'
    //         },
    //        'profile_Yatin Gupta': {
    //              'name':     'Yatin',
    //              'picture':  'DB/Yatin/yatin_face.jpg',
    //              'privacy':  'DB/Yatin/profile_Yatin Gupta.json'
    //         },
    //        'profile_Maria Fernandes': {
    //              'name':     'Maria',
    //              'picture':  'DB/Maria/maria_face.jpg',
    //              'privacy':  'DB/Maria/profile_Maria Fernandes.json'
    //         },
    //        'profile_Victor Zamarian': {
    //              'name':     'Yash',
    //              'picture':  'DB/Victor/victor_face.jpg',
    //              'privacy':  'DB/Victor/profile_Victor Zamarian.json'
    //         }

    //        }";


    //        File.WriteAllText(path, output);
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.Log("SaveToHistory Error = " + ex.ToString());
    //    }
    //}

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
        Debug.Log("In Display");
        if (currentVictim < numVictims)
        {
            Debug.Log("In If Display");

            FirstVictimName.GetComponent<TextMeshPro>().text = FilteredVictimList[currentVictim].ProfileInfo.Name;
            Debug.Log("Displaying first victim");

            int score = FilteredVictimList[currentVictim].ProfileInfo.VictimInfo.score;
            Debug.Log("Displaying first victim-1");

            FirstVictimScore.GetComponent<TextMeshPro>().text = score.ToString();
            Debug.Log("Displaying first victim-2" + score.ToString());

            FirstVictimScore.GetComponent<TextMeshPro>().color = ReturnColor32(score);
            Debug.Log("Displaying first victim-3");

            FirstVictimScoreCircle.GetComponent<SpriteRenderer>().color = ReturnColor32(score);
            Debug.Log("Displaying first victim-4");

            StartCoroutine(connection.LoadRawImageTo(FirstVictimImage.GetComponent<RawImage>(), "/" + FilteredVictimList[currentVictim].ProfileInfo.Picture, 10, 10));
            Debug.Log("Displaying first victim-5");

            //FirstVictimImage.GetComponent<Image>().preserveAspect = false;
            Debug.Log("Displaying first victim-6");

            FirstVictim.SetActive(true);
            Debug.Log("Displaying first victim-7");

        }
        else
        {
            Debug.Log("In else of first victim");
            FirstVictim.SetActive(false);
        }

        //load the second victim's info (if exist)
        if (currentVictim + 1 < numVictims)
        {

            Debug.Log("Displaying second victim");
            int score = FilteredVictimList[currentVictim + 1].ProfileInfo.VictimInfo.score;
            Debug.Log("Displaying second victim-2");

            SecondVictimName.GetComponent<TextMeshPro>().text = FilteredVictimList[currentVictim + 1].ProfileInfo.Name;
            Debug.Log("Displaying second victim-3");

            SecondVictimScore.GetComponent<TextMeshPro>().text = score.ToString();
            Debug.Log("Displaying second victim-4");

            SecondVictimScore.GetComponent<TextMeshPro>().color = ReturnColor32(score);
            Debug.Log("Displaying second victim-5");

            SecondVictimScoreCircle.GetComponent<SpriteRenderer>().color = ReturnColor32(score);
            Debug.Log("Displaying second victim-6");

            StartCoroutine(connection.LoadRawImageTo(SecondVictimImage.GetComponent<RawImage>(), "/" + FilteredVictimList[currentVictim + 1].ProfileInfo.Picture, 10, 10));
            Debug.Log("Displaying second victim-7");

            //SecondVictimImage.GetComponent<Image>().preserveAspect = false;
            Debug.Log("Displaying second victim-8");

            SecondVictim.SetActive(true);
            Debug.Log("Displaying second victim-9");

        }
        else
        {
            Debug.Log("In else of second victim");

            SecondVictim.SetActive(false);
        }
    }

    void ApplyVulnerabilityFilter()
    {
        Debug.Log("applying filter");
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

    public void CloseApp()
    {
        Application.Quit();
    }

    public void BackToCamera()
    {
        //clickedOnProfile = null;
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
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


    //void loadMainScene()
    //{
    //    VictimInfoPanel.SetActive(false);

    //    FaceRecName.instance.recName = clickedOnProfile.profile.first_name + " " + clickedOnProfile.profile.last_name;
    //    MainDataController.instance.InsertData();
    //    SpeechManager.instance.ContinueToMain();
    //}


    public void loadActionScene(int index)
    {
        Debug.Log("load ap 1");
        if (FilteredVictimList.Count == 0) { return; } // Cannot load because there is no victims
        Debug.Log("load ap 2");

        if (index >= FilteredVictimList.Count || index < 0) { return; } // Cannot load becase the `index` is out of range
        Debug.Log("load ap 3");

        VictimInfoPanel.SetActive(false);
        Debug.Log("load ap 4");

        ActionsPageController.SetSessionID(FilteredVictimList[index].SessionID);
        Debug.Log("load ap 5");

        ActionView.SetActive(true);
        Debug.Log("load ap 6");

        ActionsPageController.instance.Begin();
        Debug.Log("load ap 7");

    }
}
