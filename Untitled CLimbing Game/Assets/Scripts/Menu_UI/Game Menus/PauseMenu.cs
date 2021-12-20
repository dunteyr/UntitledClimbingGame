using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private Component[] componentList;

    private SpawnBeacon spawnBeacon;
    private CheckpointManager checkpointManager;
    public GameObject pauseMenu;
    public bool pauseMenuActive = false;

    private AbilityMenuScript abilityMenu;

    // Start is called before the first frame update
    void Start()
    {
        FindMenu();
        //set the UI to be gone by default
        pauseMenu.SetActive(false);
        pauseMenuActive = false;

        abilityMenu = GetComponent<AbilityMenuScript>();
        spawnBeacon = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<SpawnBeacon>();
        checkpointManager = GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if(abilityMenu.abilityMenuActive == true && pauseMenuActive == false)
            {
                TogglePauseMenu();
            }
            else if (abilityMenu.abilityMenuActive == false && pauseMenuActive == false)
            {
                TogglePauseMenu();
                abilityMenu.ToggleAbilityMenu();
            }
            else if(abilityMenu.abilityMenuActive == true && pauseMenuActive == true)
            {
                TogglePauseMenu();
            }
            
        }
    }
    private void FindMenu()
    {
        //get a list of every Canvas Renderer in menu's children
        componentList = GetComponentsInChildren<CanvasRenderer>();

        //check every component in that list for the abilities menu and set it equal to abilitiesMenu
        for (int i = 0; i < componentList.Length; i++)
        {
            if (componentList[i].gameObject.tag == "Pause")
            {
                pauseMenu = componentList[i].gameObject;
            }
        }
    }

    public void TogglePauseMenu()
    {
        if (pauseMenuActive)
        {
            //time resumes
            Time.timeScale = 1f;

            pauseMenuActive = false;
            //deactivate the panel UI object (child of canvas)
            pauseMenu.SetActive(false);

            //toggling the pause menu while the ability menu is open turns ability menu off too
            if(abilityMenu.abilityMenuActive) { abilityMenu.ToggleAbilityMenu(); }
        }

        else if (pauseMenuActive == false)
        {
            //time pauses
            Time.timeScale = 0f;

            pauseMenuActive = true;
            //activate the panel UI object (child of canvas)
            pauseMenu.SetActive(true);
        }
    }

    public void OnPauseRespawnButton()
    {
        checkpointManager.RespawnPlayer();
        TogglePauseMenu();
    }

    public void OnPauseRestartButton()
    {
        TogglePauseMenu();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void OnPauseQuitButton()
    {
        //this needs to be called so that time isnt still frozen when going through menus and starting new levels
        TogglePauseMenu();
        SceneManager.LoadScene("MainMenu");
    }
}
