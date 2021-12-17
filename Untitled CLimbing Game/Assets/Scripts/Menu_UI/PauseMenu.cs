using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private Component[] componentList;

    private SpawnBeacon spawnBeacon;
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
                abilityMenu.ToggleAbilityMenu();
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
        spawnBeacon.RespawnPlayer();
        TogglePauseMenu();
    }

    public void OnPauseQuitButton()
    {
        //this needs to be called so that time isnt still frozen when going through menus and starting new levels
        TogglePauseMenu();
        SceneManager.LoadScene("MainMenu");
    }
}
