using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBeacon : MonoBehaviour
{
    private GameObject player;
    private PlayerHealth playerHealth;
    private Quaternion playerRotation;

    void Start()
    {
        //find players health script to reference player object
        player = FindObjectOfType<PlayerHealth>().gameObject;
        playerHealth = player.GetComponent<PlayerHealth>();
        playerRotation = player.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RespawnPlayer()
    {
        if(player.activeInHierarchy)
        {
            //deactivate player if respawn was called and player is active
            player.SetActive(false);

            Debug.LogWarning("Player was still active in scene. Was deactivated");

            //move player to spawn point, give him his health back and then activate him
            player.transform.SetPositionAndRotation(transform.position, player.transform.rotation);
            playerHealth.SetPlayerHealth(playerHealth.playerMaxHealth);
            player.SetActive(true);
            playerHealth.playerIsDead = false;

        }

        else if(player.activeInHierarchy == false)
        {
            //move player to spawn point, give him his health back and then activate him
            player.transform.SetPositionAndRotation(transform.position, playerRotation);
            playerHealth.SetPlayerHealth(playerHealth.playerMaxHealth);
            player.SetActive(true);
            playerHealth.playerIsDead = false;
        }
    }
}
