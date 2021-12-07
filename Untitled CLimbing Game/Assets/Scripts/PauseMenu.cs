using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public bool pauseMenuActive = false;

    private AbilityMenuScript abilityMenu;

    // Start is called before the first frame update
    void Start()
    {
        //set the UI to be gone by default
        pauseMenu.SetActive(false);
        pauseMenuActive = false;

        abilityMenu = GetComponent<AbilityMenuScript>();
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
}
