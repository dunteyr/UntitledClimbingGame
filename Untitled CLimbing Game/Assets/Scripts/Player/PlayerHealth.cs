using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private RespawnMenu respawnMenu;
    private AbilityMenuScript abilityMenu;
    private HeadsUpDisplay headsUpDisplay;
    private MovementScript movementScript;
    private HandScriptForReal handScript;

    public float playerMaxHealth;
    public float playerCurrentHealth;
    public bool playerIsDead;

    void Start()
    {
        respawnMenu = GameObject.FindGameObjectWithTag("Menu").GetComponentInChildren<RespawnMenu>();
        abilityMenu = GameObject.FindGameObjectWithTag("Menu").GetComponent<AbilityMenuScript>();
        headsUpDisplay = GameObject.FindGameObjectWithTag("HUD").GetComponent<HeadsUpDisplay>();
        movementScript = GetComponent<MovementScript>();
        handScript = GetComponentInChildren<HandScriptForReal>();

        playerIsDead = false;
        playerMaxHealth = 100;
        playerCurrentHealth = playerMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(playerCurrentHealth <= 0)
        {
            if (playerIsDead == false)
            {
                KillPlayer();
            }
            
        }
    }

    //can damage the player with a specific damage amount or by taking a percentage of max health
    public void DamagePlayer(float damageAmount, bool isPercentage = false)
    {
        //only damages if invincibility is off
        if(abilityMenu.invincibilityActive == false)
        {
            if (isPercentage)
            {
                //damageAmount will be a percentage of health taken
                float damagePercentage = damageAmount * 0.01f;
                playerCurrentHealth -= playerMaxHealth * damagePercentage;
                headsUpDisplay.SetHealthBar(playerCurrentHealth);
            }

            else if (isPercentage == false)
            {
                //damageAmount is a literal value taken from health
                playerCurrentHealth -= damageAmount;
                headsUpDisplay.SetHealthBar(playerCurrentHealth);
            }

        } else { Debug.Log("No damage taken. Player is invincible."); }
    }

    public void KillPlayer()
    {
        playerIsDead = true;
        //checks if the player is grabbing something and lets go using hand script
        LetGoOnDeath();
        //this is redundant if player is grabbing when he dies. but needed on a regular death
        movementScript.SetRagdoll(true);       
        respawnMenu.OnPlayerDeath();
    }

    public void SetPlayerHealth(float newHealth)
    {
        playerCurrentHealth = newHealth;
        headsUpDisplay.SetHealthBar(newHealth);

        if (playerCurrentHealth > 0 && playerIsDead) { playerIsDead = false; }
    }

    private void LetGoOnDeath()
    {
        if (handScript.isGrabbing)
        {
            if (handScript.terrainHingeJoint != null)
            {
                handScript.LetGo(handScript.terrainHingeJoint, true);
            }
            else if (handScript.handHingeJoint != null)
            {
                handScript.LetGo(handScript.handHingeJoint, true);
            }
            else if (handScript.ropeHingeJoint != null)
            {
                handScript.LetGo(handScript.ropeHingeJoint, true);
            }
        }      
    }
}
