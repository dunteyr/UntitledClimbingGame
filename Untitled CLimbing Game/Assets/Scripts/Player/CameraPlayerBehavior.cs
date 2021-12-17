using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraPlayerBehavior : MonoBehaviour
{
    public AbilityMenuScript abilityMenu;

    public CinemachineBrain cameraBrain;
    public CinemachineVirtualCamera virtualCamera;
    public LensSettings currentLensSettings;
    public LensSettings newLensSettings;
    private Rigidbody2D player;

    [SerializeField] private float defaultOrthoSize = 9;
    [SerializeField] private float maxOrthoSize = 15;
    private float orthoSize;
    [SerializeField] private float orthoAmount = 0.3f;
    [SerializeField] private float orthoTriggerPoint = 14;
    private float newOrthoSize;

    [SerializeField] private float smoothTime = 1f;
    float yVelocity = 0.0f;

    private float playerVelocity;

    // Start is called before the first frame update
    void Start()
    {
        cameraBrain = GameObject.FindWithTag("MainCamera").GetComponent<CinemachineBrain>();
        virtualCamera = GameObject.FindWithTag("Vcam1").GetComponent<CinemachineVirtualCamera>();
        currentLensSettings = virtualCamera.m_Lens;
        player = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ControlOrthoSize();
    }

    //controls distance of camera from player based on player velocity
    private void ControlOrthoSize()
    {
        currentLensSettings = virtualCamera.m_Lens;
        //get player velocity and orthographic size of the virtual camera
        playerVelocity = player.velocity.magnitude;
        orthoSize = currentLensSettings.OrthographicSize;

        //at a certain velocity, start changing ortho size
        if(playerVelocity >= orthoTriggerPoint)
        {
            //add player velocity * a changeable amount constant to ortho
            newOrthoSize = orthoSize + playerVelocity * orthoAmount;

            //make sure new ortho doesnt go over the max
            if(newOrthoSize >= maxOrthoSize)
            {
                newOrthoSize = maxOrthoSize;
            }
        }

        //below the trigger velocity, target ortho size is set to the default
        else if(playerVelocity < orthoTriggerPoint)
        {
            newOrthoSize = defaultOrthoSize;
        }

        //after newOrthoSize is found, give the camera new lens settings that have the smoothed new ortho size
        virtualCamera.m_Lens = CreateLensSettings();
    }

    //function that returns LensSettings after setting all values
    private LensSettings CreateLensSettings()
    {
        newLensSettings = new LensSettings(
            currentLensSettings.FieldOfView,
            //smooths the transition to new ortho size
            Mathf.SmoothDamp(orthoSize, newOrthoSize, ref yVelocity, smoothTime),
            currentLensSettings.NearClipPlane,
            currentLensSettings.FarClipPlane,
            currentLensSettings.Dutch);

        return newLensSettings;
    }

}
