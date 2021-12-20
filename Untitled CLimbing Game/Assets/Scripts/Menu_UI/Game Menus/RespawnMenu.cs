using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RespawnMenu : MonoBehaviour
{
    private SpawnBeacon spawnBeacon;
    private CheckpointManager checkpointManager;
    private GameObject player;
    public GameObject respawnMenu;
    private PlayerHealth playerHealth;

    private WarningMenu warningMenu;
    private WarningMessages warningMessages;

    private Component[] componentList;
    public bool respawnMenuActive = false;

    void Start()
    {
        spawnBeacon = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<SpawnBeacon>();
        checkpointManager = GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointManager>();

        //warningMenu = GameObject.FindGameObjectWithTag("PopupMenu").GetComponent<WarningMenu>();
        //warningMessages = warningMenu.GetComponent<WarningMessages>();

        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        FindMenu();

        //set the UI to be gone by default
        respawnMenu.SetActive(false);
        respawnMenuActive = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnRespawnButton()
    {
        respawnMenu.SetActive(false);
        respawnMenuActive = false;
        checkpointManager.RespawnPlayer();
    }

    public void OnQuitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void FindMenu()
    {
        //get a list of every Canvas Renderer in menu's children
        componentList = GetComponentsInChildren<CanvasRenderer>();

        //check every component in that list for the respawn menu and set it equal to respawnMenu
        for (int i = 0; i < componentList.Length; i++)
        {
            if (componentList[i].gameObject.tag == "Respawn")
            {
                respawnMenu = componentList[i].gameObject;
            }
        }
    }

    public void OnPlayerDeath()
    {
        if(respawnMenuActive == false)
        {
            if (player.activeInHierarchy == false || playerHealth.playerIsDead)
            {
                respawnMenu.SetActive(true);
                respawnMenuActive = true;
            }
        }
    }
}