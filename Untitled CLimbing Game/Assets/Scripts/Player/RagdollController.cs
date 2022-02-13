using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class RagdollController : MonoBehaviour
{
    private MovementScript moveScript;
    public RagdollPoseScriptableObject[] ragPose;
    public RagdollPoseScriptableObject storedPose;

    public GameObject[] limbs;
    public Quaternion[] defaultRot;
    public Vector3[] defaultPos;

    public int defaultPoseIndex;

    public string newPoseName;

    // Start is called before the first frame update
    void Start()
    {
        moveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementScript>();
        limbs = GameObject.FindGameObjectsWithTag("Limb");
        defaultRot = new Quaternion[limbs.Length];
        defaultPos = new Vector3[limbs.Length];

        InitializeDefaultPose(defaultPoseIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeDefaultPose(int poseIndex)
    {
        for (int i = 0; i < limbs.Length; i++)
        {
            //defaultRot[i] = limbs[i].transform.localRotation;
            //defaultPos[i] = limbs[i].transform.localPosition;

            defaultPos[i] = ragPose[poseIndex].limbPosition[i];
            defaultRot[i] = ragPose[poseIndex].limbRotation[i];
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
                    //force setting limbs velocities to 0
                    limbs[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
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

    [ContextMenu("Save Current Pose")]
    private void SaveCurrentPose()
    {
        if(newPoseName != null || newPoseName != "" || newPoseName != " ")
        {
            //create a ragdoll pose asset
            RagdollPoseScriptableObject newPose = ScriptableObject.CreateInstance<RagdollPoseScriptableObject>();
            newPose.poseName = newPoseName;

            //set the pose asset's array lengths to number of limbs
            newPose.limbPosition = new Vector3[limbs.Length];
            newPose.limbRotation = new Quaternion[limbs.Length];

            //get pose data from limbs for the pose asset
            for (int i = 0; i < limbs.Length; i++)
            {
                newPose.limbPosition[i] = limbs[i].transform.localPosition;
                newPose.limbRotation[i] = limbs[i].transform.localRotation;
            }

            //Remove position and rotation for pelvis so it stays aligned with player at all times
            newPose.limbPosition[0] = Vector3.zero;
            newPose.limbRotation[0] = new Quaternion(0, 0, 0, 0);

            storedPose = newPose;
            AssetDatabase.CreateAsset(newPose, "Assets/Ragdoll Poses/" + newPose.poseName + ".asset");
            Debug.Log("Pose Saved " + newPose);
        }
        
        else { Debug.LogError("Please enter a pose name first."); }
    }
}
