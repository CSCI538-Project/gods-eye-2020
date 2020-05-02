using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Parsers;
using Assets.Scripts.ConnManager;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using Newtonsoft.Json;
using System.IO;
using System;
using UnityEngine.UI;
using Assets.Scripts.Database;

public class VictimHistoryController : MonoBehaviour
{
    [Header("Victim Components")]
    public GameObject FirstVictim;
    public GameObject SecondVictim;
    public GameObject ThirdVictim;
    public GameObject FourthVictim;
    public GameObject FifthVictim;
    public GameObject SixthVictim;

    [Header("First History Victim Components")]
    public GameObject FirstHistoryVictimName;
    public GameObject FirstHistoryVictimImage;
    public GameObject FirstHistoryVictimPrivacy;
    public GameObject FirstHistoryCircle;

    [Header("Second History Victim Components")]
    public GameObject SecondHistoryVictimName;
    public GameObject SecondHistoryVictimImage;
    public GameObject SecondHistoryVictimPrivacy;
    public GameObject SecondHistoryCircle;

    [Header("Third History Victim Components")]
    public GameObject ThirdHistoryVictimName;
    public GameObject ThirdHistoryVictimImage;
    public GameObject ThirdHistoryVictimPrivacy;
    public GameObject ThirdHistoryCircle;

    [Header("Fourth History Victim Components")]
    public GameObject FourthHistoryVictimName;
    public GameObject FourthHistoryVictimImage;
    public GameObject FourthHistoryVictimPrivacy;
    public GameObject FourthHistoryCircle;

    [Header("Fifth History Victim Components")]
    public GameObject FifthHistoryVictimName;
    public GameObject FifthHistoryVictimImage;
    public GameObject FifthHistoryVictimPrivacy;
    public GameObject FifthHistoryCircle;

    [Header("Sixth History Victim Components")]
    public GameObject SixthHistoryVictimName;
    public GameObject SixthHistoryVictimImage;
    public GameObject SixthHistoryVictimPrivacy;
    public GameObject SixthHistoryCircle;

    private ConnectionManager connection;

    [Header("Button Components")]
    public GameObject ScrollUp;
    public GameObject ScrollDown;

    // Start is called before the first frame update
    public Dictionary<string, IdentifiedVictimsParser.Profile> victimDict = new Dictionary<string, IdentifiedVictimsParser.Profile>();
    public List<ProfileParser> VictimList = new List<ProfileParser>();

    int startIndex = 0;
    int numVictims = 0;
    public void Start()
    {
        this.connection = new ConnectionManager();
        this.loadHistory();
    }

    private void OnEnable()
    {
        this.loadHistory();
    }

    private void loadHistory()
    {
        Debug.Log("Load History!!!");
        History history = new History();
        List<VictimHistory> victims = history.GetAllReviewedVictims();
        VictimList.Clear();
        for (int i = 0; i < victims.Count; i++)
        {
            VictimList.Add(ProfileParser.parseProfile(victims[i].ProfileInJSON));
        }
        numVictims = VictimList.Count;

        if (numVictims != 0) { Display(); }
    }

    private void setSelectedVictimName(int index)
    {
        MainDataController.DataCenter.PersonName name = MainDataController.instance.DataCenterInstance.GetNamesOfSelectedVictim();
        name.FirstName = VictimList[index].profile.first_name;
        name.LastName = VictimList[index].profile.last_name;
        MainDataController.instance.DataCenterInstance.SetNamesOfSelectedVictim(name);
    }

    public void FirstVictimOnHold()
    {
        FaceRecName.instance.recName = VictimList[startIndex].profile.first_name + " " + VictimList[startIndex].profile.last_name;
        NewMainUIController.instance.newmainUI.SetActive(false);
        this.setSelectedVictimName(startIndex);
        MainDataController.instance.InsertData();
        SpeechManager.instance.ContinueToMain();
    }
    public void SecondVictimOnHold()
    {
        FaceRecName.instance.recName = VictimList[startIndex + 1].profile.first_name + " " + VictimList[startIndex + 1].profile.last_name;
        NewMainUIController.instance.newmainUI.SetActive(false);
        this.setSelectedVictimName(startIndex + 1);
        MainDataController.instance.InsertData();
        SpeechManager.instance.ContinueToMain();
    }
    public void ThirdVictimOnHold()
    {
        FaceRecName.instance.recName = VictimList[startIndex + 2].profile.first_name + " " + VictimList[startIndex + 2].profile.last_name;
        NewMainUIController.instance.newmainUI.SetActive(false);
        this.setSelectedVictimName(startIndex + 2);
        MainDataController.instance.InsertData();
        SpeechManager.instance.ContinueToMain();
    }
    public void FourthVictimOnHold()
    {
        FaceRecName.instance.recName = VictimList[startIndex + 3].profile.first_name + " " + VictimList[startIndex + 3].profile.last_name;
        NewMainUIController.instance.newmainUI.SetActive(false);
        this.setSelectedVictimName(startIndex + 3);
        MainDataController.instance.InsertData();
        SpeechManager.instance.ContinueToMain();
    }
    public void FifthVictimOnHold()
    {
        FaceRecName.instance.recName = VictimList[startIndex + 4].profile.first_name + " " + VictimList[startIndex + 4].profile.last_name;
        NewMainUIController.instance.newmainUI.SetActive(false);
        this.setSelectedVictimName(startIndex + 4);
        MainDataController.instance.InsertData();
        SpeechManager.instance.ContinueToMain();
    }
    public void SixthVictimOnHold()
    {
        FaceRecName.instance.recName = VictimList[startIndex + 5].profile.first_name + " " + VictimList[startIndex + 5].profile.last_name;
        NewMainUIController.instance.newmainUI.SetActive(false);
        this.setSelectedVictimName(startIndex + 5);
        MainDataController.instance.InsertData();
        SpeechManager.instance.ContinueToMain();
    }

    public void ScrollUpButton()
    {
        startIndex -= 6;
        Display();
    }

    public void ScrollDownButton()
    {
        startIndex += 6;
        Display();
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
        try
        {
            var baseImagePath = "/DB/";
            if (startIndex == 0)
            {
                ScrollUp.SetActive(false);
            }
            else
            {
                ScrollUp.SetActive(true);
            }
            if (startIndex + 6 < numVictims)
            {
                ScrollDown.SetActive(true);
            }
            else
            {
                ScrollDown.SetActive(false);
            }
            //load the first victim's info
            if (startIndex < numVictims)
            {
                Debug.Log("FirstVictimShow");
                FirstHistoryVictimName.GetComponent<TextMeshPro>().text = VictimList[startIndex].profile.first_name + " " + VictimList[startIndex].profile.last_name;
                FirstHistoryVictimPrivacy.GetComponent<TextMeshPro>().text = VictimList[startIndex].score.ToString();
                FirstHistoryVictimPrivacy.GetComponent<TextMeshPro>().color = ReturnColor32(VictimList[startIndex].score);
                FirstHistoryCircle.GetComponent<SpriteRenderer>().color = ReturnColor32(VictimList[startIndex].score);
                StartCoroutine(connection.LoadRawImageTo(FirstHistoryVictimImage.GetComponent<RawImage>(), baseImagePath + VictimList[startIndex].profile.first_name + "/" + VictimList[startIndex].profile.face_image + ".jpg", 12, 12));
                FirstVictim.SetActive(true);
            }
            else
            {
                FirstVictim.SetActive(false);
            }
            //load the second victim's info (if exist)
            if (startIndex + 1 < numVictims)
            {
                Debug.Log("SecondVictimShow");
                SecondHistoryVictimName.GetComponent<TextMeshPro>().text = VictimList[startIndex + 1].profile.first_name + " " + VictimList[startIndex + 1].profile.last_name;
                SecondHistoryVictimPrivacy.GetComponent<TextMeshPro>().text = VictimList[startIndex + 1].score.ToString();
                SecondHistoryVictimPrivacy.GetComponent<TextMeshPro>().color = ReturnColor32(VictimList[startIndex + 1].score);
                SecondHistoryCircle.GetComponent<SpriteRenderer>().color = ReturnColor32(VictimList[startIndex + 1].score);
                StartCoroutine(connection.LoadRawImageTo(SecondHistoryVictimImage.GetComponent<RawImage>(), baseImagePath + VictimList[startIndex + 1].profile.first_name + "/" + VictimList[startIndex + 1].profile.face_image + ".jpg", 12, 12));
                SecondVictim.SetActive(true);
            }
            else
            {
                Debug.Log("SecondVictimHide");
                SecondVictim.SetActive(false);
            }
            if (startIndex + 2 < numVictims)
            {
                ThirdHistoryVictimName.GetComponent<TextMeshPro>().text = VictimList[startIndex + 2].profile.first_name + " " + VictimList[startIndex + 2].profile.last_name;
                ThirdHistoryVictimPrivacy.GetComponent<TextMeshPro>().text = VictimList[startIndex + 2].score.ToString();
                ThirdHistoryVictimPrivacy.GetComponent<TextMeshPro>().color = ReturnColor32(VictimList[startIndex + 2].score);
                ThirdHistoryCircle.GetComponent<SpriteRenderer>().color = ReturnColor32(VictimList[startIndex + 2].score);
                StartCoroutine(connection.LoadRawImageTo(ThirdHistoryVictimImage.GetComponent<RawImage>(), baseImagePath + VictimList[startIndex + 2].profile.first_name + "/" + VictimList[startIndex + 2].profile.face_image + ".jpg", 12, 12));
                ThirdVictim.SetActive(true);
            }
            else
            {
                ThirdVictim.SetActive(false);
            }

            if (startIndex + 3 < numVictims)
            {
                Debug.Log("Fourth Victim Show");
                FourthHistoryVictimName.GetComponent<TextMeshPro>().text = VictimList[startIndex + 3].profile.first_name + " " + VictimList[startIndex + 3].profile.last_name;
                FourthHistoryVictimPrivacy.GetComponent<TextMeshPro>().text = VictimList[startIndex + 3].score.ToString();
                FourthHistoryVictimPrivacy.GetComponent<TextMeshPro>().color = ReturnColor32(VictimList[startIndex + 3].score);
                FourthHistoryCircle.GetComponent<SpriteRenderer>().color = ReturnColor32(VictimList[startIndex + 3].score);
                StartCoroutine(connection.LoadRawImageTo(FourthHistoryVictimImage.GetComponent<RawImage>(), baseImagePath + VictimList[startIndex + 3].profile.first_name + "/" + VictimList[startIndex + 3].profile.face_image + ".jpg", 12, 12));
                FourthVictim.SetActive(true);
            }
            else
            {
                Debug.Log("Fourth Victim Hide");
                FourthVictim.SetActive(false);
            }

            if (startIndex + 4 < numVictims)
            {
                Debug.Log("Fifth Victim Show");
                FifthHistoryVictimName.GetComponent<TextMeshPro>().text = VictimList[startIndex + 4].profile.first_name + " " + VictimList[startIndex + 4].profile.last_name;
                FifthHistoryVictimPrivacy.GetComponent<TextMeshPro>().text = VictimList[startIndex + 4].score.ToString();
                FifthHistoryVictimPrivacy.GetComponent<TextMeshPro>().color = ReturnColor32(VictimList[startIndex + 4].score);
                FifthHistoryCircle.GetComponent<SpriteRenderer>().color = ReturnColor32(VictimList[startIndex + 4].score);
                StartCoroutine(connection.LoadRawImageTo(FifthHistoryVictimImage.GetComponent<RawImage>(), baseImagePath + VictimList[startIndex + 4].profile.first_name + "/" + VictimList[startIndex + 4].profile.face_image + ".jpg", 12, 12));
                FifthVictim.SetActive(true);
            }
            else
            {
                Debug.Log("Fifth Victim Hide");
                FifthVictim.SetActive(false);
            }

            if (startIndex + 5 < numVictims)
            {
                SixthHistoryVictimName.GetComponent<TextMeshPro>().text = VictimList[startIndex + 5].profile.first_name + " " + VictimList[startIndex + 5].profile.last_name;
                SixthHistoryVictimPrivacy.GetComponent<TextMeshPro>().text = VictimList[startIndex + 5].score.ToString();
                SixthHistoryVictimPrivacy.GetComponent<TextMeshPro>().color = ReturnColor32(VictimList[startIndex + 5].score);
                SixthHistoryCircle.GetComponent<SpriteRenderer>().color = ReturnColor32(VictimList[startIndex + 5].score);
                StartCoroutine(connection.LoadRawImageTo(SixthHistoryVictimImage.GetComponent<RawImage>(), baseImagePath + VictimList[startIndex + 5].profile.first_name + "/" + VictimList[startIndex + 5].profile.face_image + ".jpg", 12, 12));
                SixthVictim.SetActive(true);
            }
            else
            {
                SixthVictim.SetActive(false);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("History Display Error = " + ex.ToString());
        }
    }
}
