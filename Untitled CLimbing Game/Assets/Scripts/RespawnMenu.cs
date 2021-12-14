using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnMenu : MonoBehaviour
{
    private SpawnBeacon spawnBeacon;
    private GameObject player;
    public GameObject respawnMenu;
    private Component[] componentList;
    public bool respawnMenuActive = false;

    void Start()
    {
        spawnBeacon = GameObject.FindGameObjectWithTag("SpawnPoint").GetComponent<SpawnBeacon>();
        player = GameObject.FindWithTag("Player");
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
        spawnBeacon.RespawnPlayer();
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
            if (player.activeInHierarchy == false)
            {
                respawnMenu.SetActive(true);
                respawnMenuActive = true;
            }
        }
    }
}
