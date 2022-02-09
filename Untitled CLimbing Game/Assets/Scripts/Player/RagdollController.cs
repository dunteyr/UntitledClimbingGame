using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class RagdollController : MonoBehaviour
{
    private MovementScript moveScript;
    public GameObject[] limbs;
    public Quaternion[] defaultRot;
    public Vector3[] defaultPos;
    private RagPose poseScript;
    public RagPose[] ragPoses;
    public Transform[] tPose;

    // Start is called before the first frame update
    void Start()
    {
        moveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementScript>();
        poseScript = GetComponent<RagPose>();
        limbs = GameObject.FindGameObjectsWithTag("Limb");
        defaultRot = new Quaternion[limbs.Length];
        defaultPos = new Vector3[limbs.Length];

        ragPoses = new RagPose[10];

        InitializeDefaultPose();
    }

    // Update is called once per frame
    void Update()
    {
        if(poseScript.storingPose == true)
        {
            StorePose();
        }
    }

    private void InitializeDefaultPose()
    {
        for (int i = 0; i < limbs.Length; i++)
        {
            defaultRot[i] = limbs[i].transform.localRotation;
            defaultPos[i] = limbs[i].transform.localPosition;
        }
    }

    //Sets all limbs to be affected by physics. Also resets them to default pose when turned off
    public void ConfigureLimbs(bool ragdollOn = true)
    {
        if (ragdollOn)
        {
            for (int i = 0; i < limbs.Length; i++)
            {
                if (limbs[i].GetComponent<Rigidbody2D>() != null)
                {
                    limbs[i].GetComponent<Rigidbody2D>().simulated = true;
                }
                else { Debug.LogError("A limb on the ragdoll is missing a rigidbody. Cant make it ragdoll."); }

                if (limbs[i].GetComponent<SpriteSkin>() != null)
                {
                    limbs[i].GetComponent<SpriteSkin>().enabled = false;
                }
            }
        }

        else if (ragdollOn == false)
        {
            for (int i = 0; i < limbs.Length; i++)
            {
                if (limbs[i].GetComponent<Rigidbody2D>() != null)
                {
                    limbs[i].GetComponent<Rigidbody2D>().simulated = false;
                }
                else { Debug.LogError("A limb on the ragdoll is missing a rigidbody. Cant make it ragdoll."); }

                if (limbs[i].GetComponent<SpriteSkin>() != null)
                {
                    limbs[i].GetComponent<SpriteSkin>().enabled = true;

                    //Setting limbs back to their tpose positions
                    limbs[i].transform.localRotation = defaultRot[i];
                    limbs[i].transform.localPosition = defaultPos[i];
                }
                else { Debug.LogError("Sprite skin was null. Couldnt turn off ragdoll"); }
            }
        }
    }

    private void StorePose()
    {
        int storeLocation = -1;
        bool locationFound = false;

        for (int i = 0; i < ragPoses.Length; i++)
        {
            if (ragPoses[i] == null && locationFound == false)
            {
                storeLocation = i;
                locationFound = true;
            }
        }
        if (locationFound && poseScript.poseName != null)
        {
            ragPoses[storeLocation] = poseScript.savedPose;
            poseScript.storingPose = false;
        }

        else if (locationFound && poseScript.poseName == null)
        {
            Debug.LogWarning("Enter a name for the pose first.");
        }

        else { Debug.LogError("Could not find store location for pose"); }
    }
}
