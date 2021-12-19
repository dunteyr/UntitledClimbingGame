using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointPortal : MonoBehaviour
{
    private PortalShader portalShader;
    private CheckpointManager checkpointManager;
    private ParticleSystem[] particleSystems;
    private ParticleSystem burstParticles;
    private bool checkpointDissolvingIn;
    public bool checkpointMessageTriggered;

    public int checkpointID;
    public bool checkpointActive;

    // Start is called before the first frame update
    void Start()
    {
        checkpointMessageTriggered = false;

        portalShader = GetComponent<PortalShader>();
        checkpointManager = GetComponentInParent<CheckpointManager>();
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        GetBurstParticles();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (checkpointDissolvingIn)
        {
            portalShader.DissolveIn();

            if (portalShader.isDissolvedIn)
            {
                checkpointDissolvingIn = false;
            }
        }

        //once the particle burst stops, the checkpoint message dissapears
        if (burstParticles)
        {
            if(burstParticles.gameObject.activeInHierarchy == false && checkpointMessageTriggered == false)
            {
                checkpointManager.HideCheckPointMessage();
                checkpointMessageTriggered = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(portalShader.isDissolvedIn == false)
            {
                checkpointDissolvingIn = true;
                burstParticles.Play();
                checkpointManager.ActivateCheckpoint(checkpointID);
            }
        }
    }

    public void GetCheckpointID()
    {
        checkpointID = gameObject.GetInstanceID();
    }

    private void GetBurstParticles()
    {
        for (int i = 0; i < particleSystems.Length; i++)
        {
            if (particleSystems[i].name == "Burst Particles")
            {
                burstParticles = particleSystems[i];
            }
        }
    }
}
