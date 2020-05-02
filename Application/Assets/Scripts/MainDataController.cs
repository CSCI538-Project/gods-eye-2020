using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using Assets.Scripts.ConnManager;
using Newtonsoft.Json.Linq;
using Assets.Scripts.Parsers;
using static ProfileParser;
using UnityEngine.Networking;
using Assets.Scripts.Configs;
using System;
using Assets.Scripts.Database;

/**
 * Usually, the MainDataController should be loaded at the first because some components will depend on the data saved in DataCenter.
 */
public class MainDataController : MonoBehaviour
{
    public static MainDataController instance;

    /**
     * DataCenter
     * It maintains the connections between the server and the hololens, and the related data sent from the server will be stored as well.
     * You should NOT bind any other events to the connections outside this class.
     * If you want, you can add a new type of NotificationType; otherwise, you can try to use another way to implement same functions.
     */
    public class DataCenter
    {
        private ConnectionManager connection;
        private VictimsParser victims;
        private IdentifiedVictimsParser identifiedVictims;

        private string screenshotFileName;
        private List<string> imagesURLs;
        private string photoFileName;
        public struct PersonName 
        {
            public string FirstName;
            public string LastName;
        }
        private PersonName selectedVictimName;

        public delegate void UpdatedEventHandler(UpdatedType type);
        public event UpdatedEventHandler UpdatedNotification;

        public enum UpdatedType
        {
            Victims,
            IdentifiedVictims,      // it will get a list contains all identified victims
            ObtainedScreenshot,
            ObtainedPhoto,
            IdentifiedVictim,       // it represents that a victim has been identified
            ObtainedScreenshotHistory
        }

        public DataCenter()
        {
            connection = new ConnectionManager();
            connection.Callback += new ConnectionManager.CallbackEventHandler((received) => UnityMainThreadDispatcher.Instance().Enqueue(handleData(received)));
            connection.Connect();       // It should be call only once in this whole appliction workflow.

            this.selectedVictimName = new PersonName();
            this.imagesURLs = new List<string>();

            // Load Database file
            Assets.Scripts.Database.Initialization initializationDatabase = new Assets.Scripts.Database.Initialization();
            initializationDatabase.ObtainDatabaseFile();
        }

        public VictimsParser GetVictimsParser() { return this.victims; }
        public IdentifiedVictimsParser GetIdentifiedVictimsParser() { return this.identifiedVictims; }
        //public Dictionary<string, ProfileParser> GetIdentifiedVictimProfiles() { return this.profileMatcher; }
        public string GetScreenshotFileName() { return this.screenshotFileName; }
        public string GetPhotoFileName() { return this.photoFileName; }
        private void setScreenshotFileName(string fileName) { this.screenshotFileName = fileName; }
        private void setPhotoFileName(string fileName) { this.photoFileName = fileName; }
        public List<string> GetHistoryOfImagesInURLs() { return this.imagesURLs; }
        public void UpdateHistoryOfImagesInURLs(string firstName) { connection.SendData("{\"command\":\"execution\", \"content\": { \"session_id\":-1, \"action\": \"history_images\", \"para\":\"" + firstName + "\" }}"); }
        public void SetNamesOfSelectedVictim(PersonName name) { this.selectedVictimName = name; }
        public PersonName GetNamesOfSelectedVictim() { return this.selectedVictimName; }

        private IEnumerator handleData(byte[] receivedData)
        {
            JObject results = JObject.Parse(System.Text.Encoding.UTF8.GetString(receivedData));

            if (results["status"].ToString() != "Success") { yield break; }

            // Parsing and analyzing the messages obtained from the server. The format of response message should follow the API document published by the server side.
            if (results["content_type"].ToString() == "json")
            {
                JObject contentFromJson = (JObject)results["content"];
                if (contentFromJson["victims"] != null)
                {
                    // Victims List
                    this.victims = new VictimsParser(contentFromJson);
                    this.PublishNotification(UpdatedType.Victims);

                    // Request to update the identified victims list
                    connection.SendData(" {\"command\": \"fetch\",\"victims_type\": \"identified victims\",\"content\": \"\"} ");
                }
                else if (contentFromJson["identified_victims"] != null)
                {
                    // Identified Victims List
                    this.identifiedVictims = new IdentifiedVictimsParser(contentFromJson);

                    // This line is commented because this notification will be published after all identified victims' profile received and parsed.
                    // this.PublishNotification(UpdatedType.IdentifiedVictims);

                }
                // handle the history of all images
                else if (contentFromJson["images"] != null) 
                {
                    this.imagesURLs.Clear();
                    this.parseImagesHistoryResponse(contentFromJson);
                    this.PublishNotification(UpdatedType.ObtainedScreenshotHistory);
                }
                else
                {
                    Debug.Log("Unsupported Message in MainDataController. Detail: " + System.Text.Encoding.UTF8.GetString(receivedData));
                }
            }
            else if (results["content_type"].ToString() == "text")
            {
                if (results["content"].ToString() != null)
                {
                    if (results["content"].ToString().IndexOf("screenshot") != -1)
                    {
                        this.setScreenshotFileName(results["content"].ToString());
                        this.PublishNotification(UpdatedType.ObtainedScreenshot);
                    }
                    // Photo took from Mac
                    else if (results["content"].ToString().IndexOf("isight") != -1)
                    {
                        this.setPhotoFileName(results["content"].ToString());
                        this.PublishNotification(UpdatedType.ObtainedPhoto);
                    }
                }
            }
            else if (results["content_type"].ToString() == "boolean")
            {
                if (results["content"].Value<bool>("content") == true)
                {
                    // Request to update the identified victims list
                    connection.SendData(" {\"command\": \"fetch\",\"victims_type\": \"identified victims\",\"content\": \"\"} ");
                    this.PublishNotification(UpdatedType.IdentifiedVictim);
                }
                else
                {
                    // Other
                }
            }

            Debug.Log("MainDataController Received Data in string: " + System.Text.Encoding.UTF8.GetString(receivedData));
        }

        private void parseImagesHistoryResponse(JObject content) 
        {
            JArray images = content.Value<JArray>("images");
            for (int i = 0; i < content.Value<int>("total_images"); i++) 
            {
                this.imagesURLs.Add("/" + content.Value<string>("image_path") + images[i].ToString());
            }
        }

        public void PublishNotification(UpdatedType type)
        {
            if (this.UpdatedNotification != null) this.UpdatedNotification(type);
        }
    }

    public GameObject mainUI;
    public GameObject cameraUI;

    public GameObject initialFacePane;

    [HideInInspector]
    public ProfileParser currentProf;
    public DataCenter DataCenterInstance;

    //put all data controllers here
    [Header("All Data Controllers")]
    public GameObject FaceData;
    public GameObject UsernameData;
    public GameObject PersonalData;
    public GameObject RecentImagesData;
    public GameObject FinancialData;
    public GameObject CompanyLogoData;
    public GameObject RecentTransactionsData;
    public GameObject InterestsData;
    public GameObject FamilyMemberData;
    public GameObject TopFriendData;
    public GameObject ImageHistoryData;

    [Header("All Controllers")]
    public GameObject VictimsPageController;
    public GameObject ActionsPageController;
    public GameObject VictimInfoController;
    public GameObject VictimHistoryController;
    public GameObject ImageHistoryController;

    //public void TakePhoto(int sessionID, Action<byte[]> callback) {}

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Load MainDataController");

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        this.DataCenterInstance = new DataCenter();

        mainUI.transform.parent = Camera.main.transform;

        //this is temporary, probably won't have the UI be linked directly to the camera
        cameraUI.transform.parent = Camera.main.transform;

        // Load other controllers one by one which could rely on the MainDataController.
        VictimsPageController.SetActive(true);
        ActionsPageController.SetActive(true);
        VictimInfoController.SetActive(true);
        ImageHistoryController.SetActive(true);
        VictimHistoryController.SetActive(true);
    }

    //parse the json here
    //then call all data controller's Begin() methods so they add data to the UI
    public void InsertData()
    {
        try
        {
            Display();

            History history = new History();
            List<VictimHistory> victims = history.GetAllReviewedVictims();
            if (victims.Count != 0)
            {
                Debug.Log("Load from database");
                MainDataController.DataCenter.PersonName name = instance.DataCenterInstance.GetNamesOfSelectedVictim();
                currentProf = ProfileParser.parseProfile(history.GetProfileJSONBy(name.FirstName, name.LastName));
            }
            else 
            {
                string profileJSON = "profile_" + FaceRecName.instance.recName;
                Debug.Log("Insert data ProfileJson " + profileJSON);

                TextAsset jsonObj = Resources.Load("profile_dir/" + profileJSON) as TextAsset;
                currentProf = ProfileParser.parseProfile(jsonObj.text);
            }

            FaceData.GetComponent<FacePaneDataController>().Begin();
            UsernameData.GetComponent<UsernameController>().Begin();
            PersonalData.GetComponent<PersonalController>().Begin();
            RecentImagesData.GetComponent<RecentImagesController>().Begin();
            FinancialData.GetComponent<FinancialController>().Begin();
            CompanyLogoData.GetComponent<LogoController>().Begin();
            RecentTransactionsData.GetComponent<RecentTransactionsController>().Begin();
            InterestsData.GetComponent<InterestsController>().Begin();
            FamilyMemberData.GetComponent<FamilyMembersController>().Begin();
            TopFriendData.GetComponent<TopFriendsController>().Begin();
        }
        catch(Exception ex)
        {
            Debug.Log("Insert Data = " + ex.ToString());
        }
    }

    void Display()
    {
        mainUI.GetComponent<RadialView>().enabled = true;
        mainUI.transform.parent = null;
        mainUI.SetActive(true);

        initialFacePane.GetComponent<FacePaneController>().Activate();
        cameraUI.SetActive(false);
    }
}
