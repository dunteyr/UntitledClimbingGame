using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteMenu : MonoBehaviour
{
    private Component[] componentList;
    private GameObject levelCompleteMenu;
    private WarningMenu warningMenu;
    private WarningMessages warningMessages;

    public bool levelCompleteMenuActive;

    private int currentLevelIndex;
    private int nextLevelIndex;

    // Start is called before the first frame update
    void Start()
    {
        FindMenu();

        warningMenu = GetComponent<WarningMenu>();
        warningMessages = GetComponent<WarningMessages>();

        levelCompleteMenu.SetActive(false);
        levelCompleteMenuActive = false;
    }

    private void FindMenu()
    {
        //get a list of every Canvas Renderer in menu's children
        componentList = GetComponentsInChildren<CanvasRenderer>();

        //check every component in that list for the respawn menu and set it equal to respawnMenu
        for (int i = 0; i < componentList.Length; i++)
        {
            if (componentList[i].gameObject.tag == "LevelComplete")
            {
                levelCompleteMenu = componentList[i].gameObject;
            }
        }
    }

    public void ToggleLevelComplete()
    {
        if(levelCompleteMenuActive == false)
        {
            levelCompleteMenu.SetActive(true);
            levelCompleteMenuActive = true;
        }

        else if (levelCompleteMenuActive)
        {
            levelCompleteMenu.SetActive(false);
            levelCompleteMenuActive = false;
        }
    }

    public void OnNextLevelButton()
    {
        //get the index of this level to get the next level
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        nextLevelIndex = currentLevelIndex + 1;

        //checks if there is a level after the current one
        if(nextLevelIndex+1 > SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("There is no level to load after this one. Loading main menu.");
            SceneManager.LoadScene("MainMenu");
        }

        //if there is a level after the current one, load it
        else
        {
            SceneManager.LoadScene(nextLevelIndex);
        }
    }

    public void OnLevelSelectButton()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void OnRestartLevelButton()
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentLevelIndex);
    }

    public void OnQuitToMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
