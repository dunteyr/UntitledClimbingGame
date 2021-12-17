using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraBlend : MonoBehaviour
{
    private CinemachineBrain cineBrain;
    private CinemachineVirtualCamera mainVCam;
    private CinemachineVirtualCamera farVCam;
    //private float blendDuration = 1;
    private float blendTime;
    private CinemachineBlendDefinition blendDef;
    //private CinemachineBlend farCameraBlend;

    public bool farCamActive = false;
    
    void Start()
    {
        cineBrain = GetComponentInChildren<CinemachineBrain>();
        mainVCam = GameObject.FindGameObjectWithTag("Vcam1").GetComponent<CinemachineVirtualCamera>();
        farVCam = GameObject.FindGameObjectWithTag("FarCam").GetComponent<CinemachineVirtualCamera>();
        blendDef = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.HardOut, 1);
        //farCameraBlend = new CinemachineBlend(mainVCam, farVCam, blendDef.BlendCurve, blendDuration, blendTime);

        //make the default blend be the far cam blend
        cineBrain.m_DefaultBlend = blendDef;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (farCamActive)
            {
                farCamActive = false;
                ToggleFarCam(false);
            }

            else if (farCamActive == false)
            {
                farCamActive = true;
                ToggleFarCam(true);
            }
        }

        else if (Input.GetKeyUp(KeyCode.Q))
        {
            //farCamActive = false;
        }
    }

    private void ToggleFarCam(bool turnOn)
    {
        if (turnOn)
        {
            //turning on the far cam means deactivating the main camera. cinemachine brain automatically blends 
            //to far cam because it is the only other camera. If there are more cameras it basis it on priority
            mainVCam.gameObject.SetActive(false);
        }

        else if (turnOn == false)
        {
            mainVCam.gameObject.SetActive(true);
        }
    }
}
