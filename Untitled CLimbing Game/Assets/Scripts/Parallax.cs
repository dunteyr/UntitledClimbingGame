using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private GameObject player;
    private GameObject farMountains;
    private GameObject closeMountains;

    private Vector3 playerStartPos;
    private Vector3 playerCurrentPos;
    private Vector3 playerPrevPos;
    private Vector3 playerPosDiff;

    [SerializeField] public float closeMountSpd = 0.0225f;
    [SerializeField] public float farMountSpd = 0.01125f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        playerStartPos = player.transform.position;
        farMountains = GameObject.FindGameObjectWithTag("FarMountains");
        closeMountains = GameObject.FindGameObjectWithTag("CloseMountains");
    }

    // Update is called once per frame
    void Update()
    {
        //find amount of movement based on player position and previous player position
        playerCurrentPos = player.transform.position;
        playerPosDiff = playerCurrentPos - playerPrevPos;

        //horizontal movement
        closeMountains.transform.position -= new Vector3(playerPosDiff.x * closeMountSpd, 0, 0);
        farMountains.transform.position -= new Vector3(playerPosDiff.x * farMountSpd, 0, 0);

        //vertical movement
        closeMountains.transform.position -= new Vector3(0, playerPosDiff.y * closeMountSpd, 0);
        farMountains.transform.position -= new Vector3(0, playerPosDiff.y * farMountSpd, 0);

        //set the previous position for the next update to use
        playerPrevPos = playerCurrentPos;
    }
}
