using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public CheckpointPortal[] checkpoints;
    public CheckpointPortal currentActiveCheckpoint;
    public SpawnBeacon defaultSpawn;
    public CameraPlayerBehavior mainCam;
    private Canvas checkpointMessages;

    private GameObject player;
    private PlayerHealth playerHealth;
    private AbilityMenuScript abilityMenu;
    private Quaternion playerRotation;

    // Start is called before the first frame update
    void Start()
    {
        //find players health script to reference player object
        player = FindObjectOfType<PlayerHealth>().gameObject;
        playerHealth = player.GetComponent<PlayerHealth>();
        abilityMenu = GameObject.FindGameObjectWithTag("Menu").GetComponent<AbilityMenuScript>();
        playerRotation = player.transform.rotation;

        defaultSpawn = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<SpawnBeacon>();
        checkpointMessages = GetComponentInChildren<Canvas>();
        checkpointMessages.enabled = false;
        GetCheckpoints();

        mainCam = GameObject.FindGameObjectWithTag("Player").GetComponent<CameraPlayerBehavior>();

    }

    private void GetCheckpoints()
    {
        //gets an array of all the checkpoint scripts in the scene
        checkpoints = GetComponentsInChildren<CheckpointPortal>();

        //sets each one with an ID
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].GetCheckpointID();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //called from individual checkpoint script when player collides with it
    public void ActivateCheckpoint(int activatedID)
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            //makes the portal the player touched the active checkpoint that he will respawn at
            if(checkpoints[i].checkpointID == activatedID)
            {
                checkpoints[i].checkpointActive = true;
                currentActiveCheckpoint = checkpoints[i];
                ShowCheckpointMessage();
            }
        }
    }

    //respawns player at the currently active checkpoint
    public void RespawnPlayer()
    {
        if (playerHealth.playerIsDead == false)
        {
            Debug.LogWarning("Player wasnt dead.");

            //move player to spawn point, give him his health back and then activate him
            player.transform.SetPositionAndRotation(GetSpawnLocation(), player.transform.rotation);
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0); //sets players velocity to zero
            if (abilityMenu.ragdollActive == false) { player.GetComponent<MovementScript>().SetRagdoll(false); } //player stays ragolled if ragdoll ability is on
            playerHealth.SetPlayerHealth(playerHealth.playerMaxHealth);
            playerHealth.playerIsDead = false;
        }

        else if (playerHealth.playerIsDead)
        {
            //move player to spawn point, give him his health back and then activate him
            player.transform.SetPositionAndRotation(GetSpawnLocation(), playerRotation);
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0); //sets players velocity to zero
            playerHealth.SetPlayerHealth(playerHealth.playerMaxHealth);
            if (abilityMenu.ragdollActive == false) { player.GetComponent<MovementScript>().SetRagdoll(false); } //player stays ragolled if ragdoll ability is on
            playerHealth.playerIsDead = false;
        }
    }

    //returns the position of the active checkpoint or the default spawn if there is no checkpoint
    public Vector3 GetSpawnLocation()
    {
        Vector3 spawnLocation;

        if (currentActiveCheckpoint == null)
        {
            spawnLocation = defaultSpawn.transform.position;
        }
        else
        {
            spawnLocation = currentActiveCheckpoint.transform.position;
        }

        return spawnLocation;
    }

    public void ShowCheckpointMessage()
    {
        checkpointMessages.transform.SetPositionAndRotation(GetSpawnLocation(), checkpointMessages.transform.rotation);
        checkpointMessages.enabled = true;
    }

    public void HideCheckPointMessage()
    {
        checkpointMessages.enabled = false;
    }

}
