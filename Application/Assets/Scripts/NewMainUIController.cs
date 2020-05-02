using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;

//this script to manipulate the actions performed to victims' machine
public class NewMainUIController : MonoBehaviour
{
    public static NewMainUIController instance;

    [Header("All Views")]
    public GameObject newmainUI;
    public GameObject cameraUI;

    public GameObject VictimPage;
    public GameObject ActionsPage;
    public GameObject VictimInfoPane;
    public GameObject HistoryPane;

    List<GameObject> viewList = new List<GameObject>();

    void Start()
    {
        viewList.Add(VictimPage);
        viewList.Add(ActionsPage);
        viewList.Add(VictimInfoPane);
        viewList.Add(HistoryPane);
        Debug.Log("Start New Main UI!");
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //Display New Main UI
        newmainUI.transform.parent = Camera.main.transform;
        Display();
    }

    void Display()
    {
        newmainUI.GetComponent<RadialView>().enabled = true;
        newmainUI.transform.parent = null;
        newmainUI.SetActive(true);

        cameraUI.SetActive(false);
    }

    void DeactivateAllViews()
    {
        //this will be replaced by an animation
        foreach (GameObject obj in viewList)
        {
            obj.SetActive(false);
        }
    }

    public void VictimPageView()
    {
        DeactivateAllViews();
        ActivateView(viewList.IndexOf(VictimPage));
    }

    public void VictimInfoPaneView()
    {
        DeactivateAllViews();
        ActivateView(viewList.IndexOf(VictimInfoPane));
    }

    public void HistoryPaneView()
    {
        DeactivateAllViews();
        ActivateView(viewList.IndexOf(HistoryPane));
    }

    void ActivateView(int viewNum)
    {
        viewList[viewNum].SetActive(true);
    }
}