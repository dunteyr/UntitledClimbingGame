using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerBehavior : MonoBehaviour
{
    public AbilityMenuScript abilityMenu;

    public Camera mainCamera;
    private Vector3 cameraPosition;
    private Vector3 newCameraPosition;

    private Vector3 playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = transform.position;
        cameraPosition = mainCamera.transform.position;

        newCameraPosition.Set(playerPosition.x, playerPosition.y, cameraPosition.z);

        cameraPosition = newCameraPosition;

        mainCamera.transform.position = newCameraPosition;

    }
}
