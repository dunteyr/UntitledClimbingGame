using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private RespawnMenu respawnMenu;
    private AbilityMenuScript abilityMenu;
    private HeadsUpDisplay headsUpDisplay;
    private MovementScript movementScript;

    public float playerMaxHealth;
    public float playerCurrentHealth;
    public bool playerIsDead;

    void Start()
    {
        respawnMenu = GameObject.FindGameObjectWithTag("Menu").GetComponentInChildren<RespawnMenu>();
        abilityMenu = GameObject.FindGameObjectWithTag("Menu").GetComponent<AbilityMenuScript>();
        headsUpDisplay = GameObject.FindGameObjectWithTag("HUD").GetComponent<HeadsUpDisplay>();
        movementScript = GetComponent<MovementScript>();

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
        movementScript.SetRagdoll(true);
        playerIsDead = true;
        respawnMenu.OnPlayerDeath();
    }

    public void SetPlayerHealth(float newHealth)
    {
        playerCurrentHealth = newHealth;
        headsUpDisplay.SetHealthBar(newHealth);
    }
}
