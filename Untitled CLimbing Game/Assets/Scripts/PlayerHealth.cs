using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private RespawnMenu respawnMenu;
    private HeadsUpDisplay headsUpDisplay;

    public float playerMaxHealth;
    public float playerCurrentHealth;
    public bool playerIsDead;
    private CapsuleCollider2D player;

    void Start()
    {
        respawnMenu = GameObject.FindGameObjectWithTag("Menu").GetComponentInChildren<RespawnMenu>();
        headsUpDisplay = GameObject.FindGameObjectWithTag("HUD").GetComponent<HeadsUpDisplay>();

        player = GetComponent<CapsuleCollider2D>();
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

    //can damage the player with a specific damage amount or by taking a percentage of health
    public void DamagePlayer(float damageAmount, bool isPercentage = false)
    {
        if (isPercentage)
        {
            //damageAmount will be a percentage of health taken
            float damagePercentage = damageAmount * 0.01f;
            playerCurrentHealth -= playerCurrentHealth * damagePercentage;
            headsUpDisplay.SetHealthBar(playerCurrentHealth);
        }

        else if(isPercentage == false)
        {
            //damageAmount is a literal value taken from health
            playerCurrentHealth -= damageAmount;
            headsUpDisplay.SetHealthBar(playerCurrentHealth);
        }
    }

    public void KillPlayer()
    {
        gameObject.SetActive(false);
        playerIsDead = true;
        respawnMenu.OnPlayerDeath();
    }

    public void SetPlayerHealth(float newHealth)
    {
        playerCurrentHealth = newHealth;
        headsUpDisplay.SetHealthBar(newHealth);
    }
}
