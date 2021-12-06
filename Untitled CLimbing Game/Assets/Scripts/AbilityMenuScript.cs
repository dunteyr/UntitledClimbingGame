using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbilityMenuScript : MonoBehaviour
{
    //script is for canvas but this object is the UI panel child
    public GameObject abilityMenu;
    public bool abilityMenuActive = false;

    public GameObject infiniteJumpToggle;
    public bool infiniteJumpActive = false;

    // Start is called before the first frame update
    void Start()
    {
        //set the UI to be gone by default
        abilityMenu.SetActive(false);
        abilityMenuActive = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleAbilityMenu();
        }


    }

    public void ToggleAbilityMenu()
    {

        if (abilityMenuActive)
        {
            abilityMenuActive = false;
            //deactivate the panel UI object (child of canvas)
            abilityMenu.SetActive(false);         
        }

        else if (abilityMenuActive == false)
        {
            abilityMenuActive = true;
            //activate the panel UI object (child of canvas)
            abilityMenu.SetActive(true);  
        }
    }

    public void ToggleInfiniteJump()
    {
        if(infiniteJumpActive == false)
        {
            Debug.Log("Infinite Jump Enabled");
            infiniteJumpActive = true;
        }

        else if (infiniteJumpActive)
        {
            Debug.Log("Infinite Jump Disabled");
            infiniteJumpActive = false;
        }
        
    }
}
