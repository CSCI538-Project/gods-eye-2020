using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    //how long after pressing the face button the categories show up
    public float categoryViewTime;

    [Header("All Views")]
    public GameObject mainUI;
    public GameObject cameraUI;

    public GameObject facePane;
    public GameObject centerFacePane;
    public GameObject rightScoreFacePane;

    public GameObject navBar;
    public GameObject backgroundScreen;

    public GameObject categoryView;
    public GameObject usernameView;
    public GameObject personalView;
    public GameObject financialView;
    public GameObject interestsView;
    public GameObject connectionsView;
    public GameObject imageHistoryView;

    List<GameObject> viewList = new List<GameObject>();


    [Header("Navigation Bar Buttons")]
    public GameObject categoryButton;
    public GameObject usernameButton;
    public GameObject personalButton;
    public GameObject financialButton;
    public GameObject interestsButton;
    public GameObject connectionsButton;
    public GameObject imageHistoryButton;

    List<GameObject> navList = new List<GameObject>();

    [Header("Navigation Bar Icons")]
    public GameObject usernameIcon;
    public GameObject personalIcon;
    public GameObject financialIcon;
    public GameObject interestsIcon;
    public GameObject connectionsIcon;
    public GameObject imageHistoryIcon;

    List<GameObject> iconList = new List<GameObject>();

    [Header("Reset GameObjects")]
    public GameObject recentImagesController;
    public GameObject recentTransactionsController;
    public GameObject interestsController;
    public GameObject familyMembersController;
    public GameObject topFriendsController;
    

    // Start is called before the first frame update
    void Start(){
        viewList.Add(facePane);
        viewList.Add(centerFacePane);
        viewList.Add(rightScoreFacePane);
        viewList.Add(navBar);
        viewList.Add(categoryView);
        viewList.Add(usernameView);
        viewList.Add(personalView);
        viewList.Add(financialView);
        viewList.Add(interestsView);
        viewList.Add(connectionsView);
        viewList.Add(imageHistoryView);

        navList.Add(categoryButton);
        navList.Add(usernameButton);
        navList.Add(personalButton);
        navList.Add(financialButton);
        navList.Add(interestsButton);
        navList.Add(connectionsButton);
        navList.Add(imageHistoryButton);

        iconList.Add(usernameIcon);
        iconList.Add(personalIcon);
        iconList.Add(financialIcon);
        iconList.Add(interestsIcon);
        iconList.Add(connectionsIcon);
        iconList.Add(imageHistoryIcon);
    }

    public void CategoryViewFromButton(){
        Invoke("MainView", categoryViewTime);
    }


    void DeactivateAll(){
        DeactivateAllViews();
        DeactivateAllNavButtons();
        DeactivateAllNavIcons();
    }
    
    //disable all views
    void DeactivateAllViews(){
        //this will be replaced by an animation
        foreach (GameObject obj in viewList){
            obj.SetActive(false);
        }
    }

    //disable all nav bar buttons
    void DeactivateAllNavButtons(){
        foreach (GameObject obj in navList){
            obj.SetActive(false);
        }
    }

    //disable all nave bar icons
    void DeactivateAllNavIcons(){
        foreach (GameObject obj in iconList){
            obj.SetActive(false);
        }
    }


    void ActivateView(int viewNum){
        viewList[viewNum].SetActive(true);
    }

    void ActivateAllNavButtons(){
        foreach (GameObject obj in navList){
            obj.SetActive(true);
        }
    }

    void DeactivateNavButton(int btnNum){
        navList[btnNum].SetActive(false);
    }

    void ActivateIcon(int iconNum){
        iconList[iconNum].SetActive(true);
    }


    //display the categories after the face is clicked
    //also go back to the category view from the back button in any of the category views (except the usrenames)
    public void MainView(){
        DeactivateAllViews();

        ActivateView(viewList.IndexOf(categoryView));
        ActivateView(viewList.IndexOf(facePane));
    }

    
    //show the usernames
    public void UsernameView(){
        DeactivateAll();

        ActivateView(viewList.IndexOf(usernameView));
        ActivateView(viewList.IndexOf(centerFacePane));

        ActivateView(viewList.IndexOf(navBar));

        ActivateAllNavButtons();
        DeactivateNavButton(navList.IndexOf(usernameButton));
        ActivateIcon(iconList.IndexOf(usernameIcon));
    }


    //methods for category buttons as well as navBar buttons
    public void PersonalView(){
        DeactivateAll();

        ActivateView(viewList.IndexOf(personalView));
        ActivateView(viewList.IndexOf(rightScoreFacePane));

        ActivateView(viewList.IndexOf(navBar));

        ActivateAllNavButtons();
        DeactivateNavButton(navList.IndexOf(personalButton));
        ActivateIcon(iconList.IndexOf(personalIcon));
    }

    public void FinancialView(){
        DeactivateAll();

        ActivateView(viewList.IndexOf(financialView));
        ActivateView(viewList.IndexOf(centerFacePane));

        ActivateView(viewList.IndexOf(navBar));

        ActivateAllNavButtons();
        DeactivateNavButton(navList.IndexOf(financialButton));
        ActivateIcon(iconList.IndexOf(financialIcon));
    }

    public void InterestsView(){
        DeactivateAll();

        ActivateView(viewList.IndexOf(interestsView));
        ActivateView(viewList.IndexOf(centerFacePane));

        ActivateView(viewList.IndexOf(navBar));

        ActivateAllNavButtons();
        DeactivateNavButton(navList.IndexOf(interestsButton));
        ActivateIcon(iconList.IndexOf(interestsIcon));
    }

    public void ConnectionsView(){
        DeactivateAll();

        ActivateView(viewList.IndexOf(connectionsView));
        ActivateView(viewList.IndexOf(rightScoreFacePane));

        ActivateView(viewList.IndexOf(navBar));

        ActivateAllNavButtons();
        DeactivateNavButton(navList.IndexOf(connectionsButton));
        ActivateIcon(iconList.IndexOf(connectionsIcon));
    }

    public void ImageHistoryView()
    {
        DeactivateAll();
        MainDataController.instance.DataCenterInstance.UpdateHistoryOfImagesInURLs(MainDataController.instance.DataCenterInstance.GetNamesOfSelectedVictim().FirstName);

        ActivateView(viewList.IndexOf(imageHistoryView));

        ActivateView(viewList.IndexOf(navBar));

        ActivateAllNavButtons();
        DeactivateNavButton(navList.IndexOf(imageHistoryButton));
        ActivateIcon(iconList.IndexOf(imageHistoryIcon));
    }


    //quit the application from the main UI
    public void CloseApp(){
        //NEED TO CHANGE
        DeactivateAll();
        facePane.GetComponent<FacePaneController>().Reset();

        recentImagesController.GetComponent<RecentImagesController>().Reset();
        recentTransactionsController.GetComponent<RecentTransactionsController>().Reset();
        interestsController.GetComponent<InterestsController>().Reset();
        familyMembersController.GetComponent<FamilyMembersController>().Reset();
        topFriendsController.GetComponent<TopFriendsController>().Reset();
        mainUI.SetActive(false);

        MainDataController.instance.VictimHistoryController.GetComponent<VictimHistoryController>().Start();
        NewMainUIController.instance.newmainUI.SetActive(true);
        //Application.Quit();
    }

    //go back to camera
    public void BackToCamera(){
        FaceRecName.instance.RemoveName();

        DeactivateAll();
        facePane.GetComponent<FacePaneController>().Reset();

        recentImagesController.GetComponent<RecentImagesController>().Reset();
        recentTransactionsController.GetComponent<RecentTransactionsController>().Reset();
        interestsController.GetComponent<InterestsController>().Reset();
        familyMembersController.GetComponent<FamilyMembersController>().Reset();
        topFriendsController.GetComponent<TopFriendsController>().Reset();

        MainDataController.instance.mainUI.transform.parent = Camera.main.transform;

        mainUI.SetActive(false);
        cameraUI.SetActive(true);
    }
}
