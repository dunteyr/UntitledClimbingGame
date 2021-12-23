using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WarningMenu : MonoBehaviour
{
    private Component[] componentList;
    private GameObject warningMenu;
    public TextMeshProUGUI warningText;
    private WarningMessages warningMessages;
    public string messageType;

    public bool warningMenuActive;
    public bool clickedYes;

    // Start is called before the first frame update
    void Start()
    {       
        FindMenu();
        warningText = GameObject.FindGameObjectWithTag("WarningText").GetComponent<TextMeshProUGUI>();

        warningMessages = GetComponent<WarningMessages>();

        warningMenu.SetActive(false);
        warningMenuActive = false;
    }

    private void FindMenu()
    {
        //get a list of every Canvas Renderer in menu's children
        componentList = GetComponentsInChildren<CanvasRenderer>();

        //check every component in that list for the respawn menu and set it equal to respawnMenu
        for (int i = 0; i < componentList.Length; i++)
        {
            if (componentList[i].gameObject.tag == "Warning")
            {
                warningMenu = componentList[i].gameObject;
            }
        }
    }

    //turns the menu on and off
    public void ToggleWarning()
    {
        if (warningMenuActive == false)
        {
            warningMenu.SetActive(true);
            warningMenuActive = true;
        }

        else if (warningMenuActive)
        {
            warningMenu.SetActive(false);
            warningMenuActive = false;
        }
    }

    //calls togglewarning and sets specified warning message
    public void ShowWarning(string whichMessage)
    {
        messageType = whichMessage;
        
        if (whichMessage == "Quit")
        {
            warningText.text = warningMessages.QuitLevelWarning();
        }

        else if(whichMessage == "Restart")
        {
            warningText.text = warningMessages.RestartLevelWarning();
        }

        else if(whichMessage == "EraseSave")
        {
            warningText.text = warningMessages.EraseSaveWarning();
        }

        else
        {
            Debug.LogWarning("ShowWarning() was called with an invalid warning type");
        }

        ToggleWarning();
    }

    public void YesButton()
    {
        if(messageType == "Quit")
        {
            SceneManager.LoadScene("MainMenu");
            if(Time.timeScale != 1f) { Time.timeScale = 1f; } //set time to normal if it isnt already
        }
        else if (messageType == "Restart")
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
            if (Time.timeScale != 1f) { Time.timeScale = 1f; } //set time to normal if it isnt already
        }
        else if (messageType == "EraseSave")
        {
            Debug.LogWarning("There is no erase save function yet.");
        }
    }

    public void NoButton()
    {
        ToggleWarning();
    }
}