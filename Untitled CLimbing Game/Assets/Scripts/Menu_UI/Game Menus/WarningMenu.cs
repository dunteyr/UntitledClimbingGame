using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WarningMenu : MonoBehaviour
{
    private Component[] componentList;
    private GameObject warningMenu;
    private TextMeshPro warningText;

    public bool warningMenuActive;

    // Start is called before the first frame update
    void Start()
    {
        FindMenu();
        warningText = GameObject.FindGameObjectWithTag("WarningText").GetComponent<TextMeshPro>();

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

    //calls togglewarning and sets a warning message
    public void ShowWarning(string warningMessage)
    {
        warningText.text = warningMessage;
    }
}