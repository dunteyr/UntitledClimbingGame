using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private GameObject player;
    private GameObject mainCamera;
    private GameObject farMountains;
    private GameObject closeMountains;
    /*
    private Vector3 playerStartPos;
    private Vector3 playerCurrentPos;
    private Vector3 playerPrevPos;
    private Vector3 playerPosDiff;
    */

    private Vector3 camCurrentPos;
    private Vector3 camPrevPos;
    private Vector3 camPosDiff;

    [SerializeField] public float closeMountSpd = 0.0225f;
    [SerializeField] public float farMountSpd = 0.01125f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        //playerStartPos = player.transform.position;
        farMountains = GameObject.FindGameObjectWithTag("FarMountains");
        closeMountains = GameObject.FindGameObjectWithTag("CloseMountains");
    }

    // Update is called once per frame
    void Update()
    {
        //find amount of movement based on player position and previous player position
        camCurrentPos = mainCamera.transform.position;
        camPosDiff = camCurrentPos - camPrevPos;

        //horizontal movement
        closeMountains.transform.position -= new Vector3(camPosDiff.x * closeMountSpd, 0, 0);
        farMountains.transform.position -= new Vector3(camPosDiff.x * farMountSpd, 0, 0);

        //vertical movement
        closeMountains.transform.position -= new Vector3(0, camPosDiff.y * closeMountSpd, 0);
        farMountains.transform.position -= new Vector3(0, camPosDiff.y * farMountSpd, 0);

        //set the previous position for the next update to use
        camPrevPos = camCurrentPos;
    }
}
