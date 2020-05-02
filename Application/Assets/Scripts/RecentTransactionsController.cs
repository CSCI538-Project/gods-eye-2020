using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static ProfileParser;

//controller for showing recent venmo transactions in the financial view
public class RecentTransactionsController : MonoBehaviour
{
    public GameObject transactionDataPrefab;

    public GameObject upperLocation; //location of top data block
    public GameObject lowerLocation; //location of bottom data block

    public GameObject upButton; //previous button
    public GameObject downButton; //next button

    int numTransactions;
    int currentTransaction = 0;

    List<VenmoTx> transactionData;


    public void Begin(){
        try
        {
            CreateTransactionsArrays();
            toggleUpButton(false);
        }
        catch(Exception ex)
        {
            Debug.Log("Recent Tran Error Begin"+ex.ToString());
        }
    }

    void CreateTransactionsArrays(){
        numTransactions = MainDataController.instance.currentProf.profile.financial_info.venmo_tx.Count;
        transactionData = MainDataController.instance.currentProf.profile.financial_info.venmo_tx;

        currentTransaction = 0;

        for (int i = 0; i < numTransactions; i++){
            //add transaction data prefab to scene
            if (i == 0)
            {
                GameObject transactionCopy = Instantiate(transactionDataPrefab, upperLocation.transform);

                //set the text values for each transaction
                transactionCopy.transform.GetChild(0).GetComponent<TextMeshPro>().text = transactionData[i].amount;
                transactionCopy.transform.GetChild(1).GetComponent<TextMeshPro>().text = transactionData[i].date;
                transactionCopy.transform.GetChild(2).GetComponent<TextMeshPro>().text = transactionData[i].recipient;
            } else if (i == numTransactions - 1)
            {
                GameObject transactionCopy = Instantiate(transactionDataPrefab, lowerLocation.transform);

                transactionCopy.transform.GetChild(0).GetComponent<TextMeshPro>().text = transactionData[i].amount;
                transactionCopy.transform.GetChild(1).GetComponent<TextMeshPro>().text = transactionData[i].date;
                transactionCopy.transform.GetChild(2).GetComponent<TextMeshPro>().text = transactionData[i].recipient;
            } else
            {
                GameObject transactionCopyUpper = Instantiate(transactionDataPrefab, upperLocation.transform);
                GameObject transactionCopyLower = Instantiate(transactionDataPrefab, lowerLocation.transform);

                transactionCopyUpper.transform.GetChild(0).GetComponent<TextMeshPro>().text = transactionData[i].amount;
                transactionCopyUpper.transform.GetChild(1).GetComponent<TextMeshPro>().text = transactionData[i].date;
                transactionCopyUpper.transform.GetChild(2).GetComponent<TextMeshPro>().text = transactionData[i].recipient;

                transactionCopyLower.transform.GetChild(0).GetComponent<TextMeshPro>().text = transactionData[i].amount;
                transactionCopyLower.transform.GetChild(1).GetComponent<TextMeshPro>().text = transactionData[i].date;
                transactionCopyLower.transform.GetChild(2).GetComponent<TextMeshPro>().text = transactionData[i].recipient;
            }
        }

        //hide all other data blocks except the first one in each location
        for (int j = 1; j < upperLocation.transform.childCount; j++){
            upperLocation.transform.GetChild(j).gameObject.SetActive(false);
        }

        for (int k = 1; k < lowerLocation.transform.childCount; k++){
            lowerLocation.transform.GetChild(k).gameObject.SetActive(false);
        }

        if (numTransactions < 3){
            toggleDownButton(false);
        } else {
            toggleDownButton(true);
        }
    }

    //display the next transaction and move the bottom one to the top
    public void nextTransaction(){
        upperLocation.transform.GetChild(currentTransaction).gameObject.SetActive(false);
        lowerLocation.transform.GetChild(currentTransaction).gameObject.SetActive(false);

        currentTransaction++;
        toggleUpButton(true);

        if (currentTransaction == numTransactions - 2){
            toggleDownButton(false);
        }

        upperLocation.transform.GetChild(currentTransaction).gameObject.SetActive(true);
        lowerLocation.transform.GetChild(currentTransaction).gameObject.SetActive(true);
    }

    //display the previous transaction and move the top one to the bottom
    public void previousTransaction(){
        upperLocation.transform.GetChild(currentTransaction).gameObject.SetActive(false);
        lowerLocation.transform.GetChild(currentTransaction).gameObject.SetActive(false);

        currentTransaction--;
        toggleDownButton(true);

        if (currentTransaction == 0){
            toggleUpButton(false);
        }

        upperLocation.transform.GetChild(currentTransaction).gameObject.SetActive(true);
        lowerLocation.transform.GetChild(currentTransaction).gameObject.SetActive(true);
    }

    //turn the up button on/off
    void toggleUpButton(bool state){
        upButton.SetActive(state);
    }

    //turn the down button on/off
    void toggleDownButton(bool state){
        downButton.SetActive(state);
    }

    public void Reset(){
        foreach (Transform t in upperLocation.transform){
            GameObject.Destroy(t.gameObject);
        }

        foreach (Transform t in lowerLocation.transform){
            GameObject.Destroy(t.gameObject);
        }
    }
}
