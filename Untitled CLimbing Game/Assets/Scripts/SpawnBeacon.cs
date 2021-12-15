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
        if(playerHealth.playerIsDead == false)
        {

            Debug.LogWarning("Player wasnt dead.");

            //move player to spawn point, give him his health back and then activate him
            player.transform.SetPositionAndRotation(transform.position, player.transform.rotation);
            player.GetComponent<MovementScript>().SetRagdoll(false);
            playerHealth.SetPlayerHealth(playerHealth.playerMaxHealth);
            playerHealth.playerIsDead = false;

        }

        else if(playerHealth.playerIsDead)
        {
            //move player to spawn point, give him his health back and then activate him
            player.transform.SetPositionAndRotation(transform.position, playerRotation);
            playerHealth.SetPlayerHealth(playerHealth.playerMaxHealth);
            player.GetComponent<MovementScript>().SetRagdoll(false);
            playerHealth.playerIsDead = false;
        }
    }
}
