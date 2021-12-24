using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbilityMenuScript : MonoBehaviour
{
    private Component[] componentList;
    private MovementScript movementScript;

    //script is for canvas but this object is the UI panel child
    public GameObject abilityMenu;
    public bool abilityMenuActive = false;

    public GameObject infiniteJumpToggle;
    public bool infiniteJumpActive = false;

    public GameObject invincibilityToggle;
    public bool invincibilityActive = false;

    public GameObject ragdollToggle;
    public bool ragdollActive = false;


    // Start is called before the first frame update
    void Start()
    {
        FindMenu();
        movementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementScript>();

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

        ManageAbilityInputs();

    }
    private void FindMenu()
    {
        //get a list of every Canvas Renderer in menu's children
        componentList = GetComponentsInChildren<CanvasRenderer>();

        //check every component in that list for the abilities menu and set it equal to abilitiesMenu
        for (int i = 0; i < componentList.Length; i++)
        {
            if (componentList[i].gameObject.tag == "Abilities")
            {
                abilityMenu = componentList[i].gameObject;
            }
        }
    }

    private void ManageAbilityInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleInfiniteJump();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ToggleInvincibility();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ToggleRagdoll();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {

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

    public void ToggleInvincibility()
    {
        if (invincibilityActive == false)
        {
            Debug.Log("Invincibility Enabled");
            invincibilityActive = true;
        }

        else if (invincibilityActive)
        {
            Debug.Log("Invincibility Disabled");
            invincibilityActive = false;
        }

    }

    public void ToggleRagdoll()
    {
        if (ragdollActive == false)
        {
            Debug.Log("Ragdoll Enabled");
            movementScript.SetRagdoll(true, true);
            ragdollActive = true;
        }

        else if (ragdollActive)
        {
            Debug.Log("Ragdoll Disabled");
            movementScript.SetRagdoll(false);
            ragdollActive = false;
        }
    }
}
