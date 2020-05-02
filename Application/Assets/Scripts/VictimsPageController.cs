using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Parsers;
using Assets.Scripts.ConnManager;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;

// This script control the display of active victims basic info, like ip address and username...
public class VictimsPageController : MonoBehaviour
{
    [Header("All Views")]
    public GameObject VictimsView;
    public GameObject ActionView;

    [Header("Scroll Buttons")]
    public GameObject VictimsUp;
    public GameObject VictimsDown;

    [Header("All Victims")]
    public GameObject FirstVictim;
    public GameObject MiddleVictim;
    public GameObject LastVictim;

    [Header("Victim Info Labels")]
    public GameObject Hostname;
    public GameObject IPAddress;
    public GameObject Username;
    public GameObject OS;

    [Header("Action Button")]
    public GameObject ActionButton;


    private List<VictimsParser.Victim> victimsList;
    private List<GameObject> victims_btns = new List<GameObject>();
    private int currVictim = 0;
    private int currVictimInfo = 0;
    private int victims_num = 0;
    private ConnectionManager connection;

    void Start()
    {
        Debug.Log("Victims Page started!");
        connection = new ConnectionManager();
        //connection.Connect();
        victims_btns.Add(FirstVictim);
        victims_btns.Add(MiddleVictim);
        victims_btns.Add(LastVictim);
        reset();

        // Get all Victims from MainDataController and display
        if (MainDataController.instance.DataCenterInstance.GetVictimsParser() != null)
        {
            victimsList = MainDataController.instance.DataCenterInstance.GetVictimsParser().GetAllVictims();
            victims_num = victimsList.Count;
            displayVictims();
        }
        else { Debug.Log("is Null!"); }

        // Bind an event to instance. When the HoloLens received messages from the server and finished the parsing, a notification with its type will be generaged.
        MainDataController.instance.DataCenterInstance.UpdatedNotification += new MainDataController.DataCenter.UpdatedEventHandler((updatedType) =>
        {
            // Handling the updated info
            if (updatedType == MainDataController.DataCenter.UpdatedType.Victims)
            {
                victimsList = MainDataController.instance.DataCenterInstance.GetVictimsParser().GetAllVictims();
                victims_num = victimsList.Count;
                displayVictims();
            }
            else { Debug.Log("Not for me."); }
        });
    }

    // Display the Victims
    void displayVictims()
    {
        currVictim = 0;
        currVictimInfo = 0;
        updateVictims();
        showVictimInfo();
    }

    // Hide the victims and Disable the scroll buttons
    void reset()
    {
        FirstVictim.SetActive(false);
        MiddleVictim.SetActive(false);
        LastVictim.SetActive(false);
        VictimsUp.GetComponent<Interactable>().Enabled = false;
        VictimsDown.GetComponent<Interactable>().Enabled = false;
    }

    // Update the three Active Victims' ID
    void updateVictims()
    {
        Debug.Log("Update Victims");
        reset();
        if (victims_num == 0)
        {
            Debug.Log("No Victims Showing Up!");
            return;
        }
        else if (victims_num < 3)
        {
            // When there are less than three victims showing up
            for (int i = 0; i < victims_num; i++)
            {
                victims_btns[i].transform.GetChild(1).GetComponent<TextMeshPro>().text = (currVictim + i + 1).ToString();
                victims_btns[i].SetActive(true);
            }
        }
        else
        {
            // When there are more
            for (int i = 0; i < 3; i++)
            {
                victims_btns[i].transform.GetChild(1).GetComponent<TextMeshPro>().text = (currVictim + i + 1).ToString();
                victims_btns[i].SetActive(true);
            }
        }
        if (currVictim > 0)
        {
            VictimsUp.GetComponent<Interactable>().Enabled = true;
        }
        if (currVictim + 2 < victims_num)
        {
            VictimsDown.GetComponent<Interactable>().Enabled = true;
        }
        return;
    }

    // Display victims info of victim_id
    void showVictimInfo()
    {
        Debug.Log("Show Victim Info");
        if (currVictimInfo < victimsList.Count)
        {
            Hostname.GetComponent<TextMeshPro>().text = victimsList[currVictimInfo].VictimInfo.HostName;
            IPAddress.GetComponent<TextMeshPro>().text = victimsList[currVictimInfo].Address.IPAddress + ": " + victimsList[currVictimInfo].Address.Port.ToString();
            Username.GetComponent<TextMeshPro>().text = victimsList[currVictimInfo].VictimInfo.UserName;
            OS.GetComponent<TextMeshPro>().text = victimsList[currVictimInfo].VictimInfo.Type;
        }
    }

    // Buttons Onhold Functions

    public void scrollUp()
    {
        Debug.Log("Scroll Up");

        // If No More Victims to Scroll
        if (currVictim < 3)
        {
            currVictim = 0;
        }
        else
        {
            currVictim -= 3;
        }
        updateVictims();
    }

    public void scrollDown()
    {
        Debug.Log("Scroll Down");

        // If No More Victims to Scroll
        if (currVictim + 5 >= victims_num)
        {
            currVictim = (victims_num - 3) < 0 ? 0 : (victims_num - 3);
        }
        else
        {
            currVictim += 3;
        }
        updateVictims();
    }

    // Show Each Victim
    public void showFirstVictim()
    {
        currVictimInfo = currVictim;
        showVictimInfo();
    }

    public void showMiddleVictim()
    {
        currVictimInfo = currVictim + 1;
        showVictimInfo();
    }

    public void showLastVictim()
    {
        currVictimInfo = currVictim + 2;
        showVictimInfo();
    }

    // Action Button OnHold

    public void action()
    {
        if (victimsList.Count == 0)
        {
            Debug.Log("There is no victim loaded.");
            return;
        }
        VictimsView.SetActive(false);
        ActionsPageController.SetSessionID(victimsList[currVictimInfo].SessionID);
        ActionsPageController.SetShowDetails(false);
        ActionView.SetActive(true);
        ActionsPageController.instance.Begin();
    }
}
