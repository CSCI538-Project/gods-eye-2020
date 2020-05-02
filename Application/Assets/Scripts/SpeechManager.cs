using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using TMPro;

public class SpeechManager : MonoBehaviour
{
    public static SpeechManager instance;

    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    public GameObject cameraReticle;
    public GameObject screen;
    public GameObject continueButton;

    public GameObject facebookButton;
    public GameObject instagramButton;
    public GameObject twitterButton;
    public GameObject linkedinButton;

    public GameObject forwardImageButton;
    public GameObject backImageButton;
    public GameObject downTransactionButton;
    public GameObject upTransactionButton;


    [Header("Controllers")]
    public GameObject face;
    public GameObject ui;
    public GameObject username;
    public GameObject recentImages;
    public GameObject recentTransactions;


    // Start is called before the first frame update
    void Start(){
        
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //take a picture
        keywords.Add("Begin", () => {
            if (cameraReticle.activeInHierarchy){
                ImageCapture.instance.TakePicture();
            }
        });

        //button on initial face pane
        keywords.Add("Continue", () => {
            if (continueButton.activeInHierarchy){
                face.GetComponent<FacePaneController>().BeginMove();
                ui.GetComponent<UIController>().CategoryViewFromButton();
            }
        });

        //quit the app
        keywords.Add("Quit", () => {
            if (cameraReticle.activeInHierarchy){
                Application.Quit();
            } else if (continueButton.activeInHierarchy) {
                Application.Quit();
            } else if (screen.activeInHierarchy){
                ui.GetComponent<UIController>().CloseApp();
            }
        });

        //close the app if in camera scene or go back to the camera scene
        keywords.Add("Close", () => {
            if (screen.activeInHierarchy){
                ui.GetComponent<UIController>().BackToCamera();
            } else if (continueButton.activeInHierarchy){
                ui.GetComponent<UIController>().BackToCamera();
            }
        });

        //go to the category view
        keywords.Add("Tab One", () => {
            if (screen.activeInHierarchy){
                ui.GetComponent<UIController>().MainView();
            }
        });

        //go to usernames view
        keywords.Add("Tab Two", () => {
            if (screen.activeInHierarchy){
                ui.GetComponent<UIController>().UsernameView();
            }
        });

        //go to personal view
        keywords.Add("Tab Three", () => {
            if (screen.activeInHierarchy){
                ui.GetComponent<UIController>().PersonalView();
            }
        });

        //go to financial view
        keywords.Add("Tab Four", () => {
            if (screen.activeInHierarchy){
                ui.GetComponent<UIController>().FinancialView();
            }
        });

        //go to interests view
        keywords.Add("Tab Five", () => {
            if (screen.activeInHierarchy){
                ui.GetComponent<UIController>().InterestsView();
            }
        });

        //go to connections view
        keywords.Add("Tab Six", () => {
            if (screen.activeInHierarchy){
                ui.GetComponent<UIController>().ConnectionsView();
            }
        });

        //follow the facebook url link
        keywords.Add("Link One", () => {
            if (facebookButton.activeInHierarchy){
                username.GetComponent<UsernameController>().OpenFacebook();
            }
        });

        //follow the instagram url link
        keywords.Add("Link Two", () => {
            if (instagramButton.activeInHierarchy){
                username.GetComponent<UsernameController>().OpenInstagram();
            }
        });

        //follow the twitter url link
        keywords.Add("Link Three", () => {
            if (twitterButton.activeInHierarchy){
                username.GetComponent<UsernameController>().OpenTwitter();
            }
        });

        //follow the facebook url link
        keywords.Add("Link Four", () => {
            if (linkedinButton.activeInHierarchy){
                username.GetComponent<UsernameController>().OpenLinkedIn();
            }
        });

        keywords.Add("Next", () => {
            if (forwardImageButton.activeInHierarchy){
                //next image
                recentImages.GetComponent<RecentImagesController>().NextImage();
            } else if (downTransactionButton.activeInHierarchy){
                //next transaction
                recentTransactions.GetComponent<RecentTransactionsController>().nextTransaction();
            }
        });

        keywords.Add("Previous", () => {
            if (backImageButton.activeInHierarchy){
                //previous image
                recentImages.GetComponent<RecentImagesController>().PreviousImage();
            } else if (upTransactionButton.activeInHierarchy){
                //previous transaction
                recentTransactions.GetComponent<RecentTransactionsController>().previousTransaction();
            }
        });


        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args){
        System.Action keywordAction;

        if (keywords.TryGetValue(args.text, out keywordAction)){
            keywordAction.Invoke();
        }
    }

    //only for testing
    void Update(){
        if (Input.GetKeyDown(KeyCode.Q) && continueButton.activeInHierarchy){
            face.GetComponent<FacePaneController>().BeginMove();
            ui.GetComponent<UIController>().CategoryViewFromButton();
        }
    }

    public void ContinueToMain()
    {
        face.GetComponent<FacePaneController>().BeginMove();
        ui.GetComponent<UIController>().CategoryViewFromButton();
    }
}
