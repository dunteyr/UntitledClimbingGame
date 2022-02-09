using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagPose
{
    //private RagdollController ragController;

    public GameObject[] limbs;
    public Quaternion[] limbRotation;
    public Vector3[] limbPosition;
    [TextArea]
    public string poseName;

    public RagPose savedPose;
    public bool storingPose;

    public void Start()
    {
        //ragController = GetComponent<RagdollController>();
    }

    public RagPose(GameObject[] limbObjects, Quaternion[] rotations, Vector3[] positions, string name)
    {
        limbs = limbObjects;
        limbRotation = rotations;
        limbPosition = positions;
        poseName = name;
    }

    public int limbCount()
    {
        return limbs.Length;
    }

    [ContextMenu("Save Pose")]
    public void SavePose()
    {
        storingPose = false;

        GameObject[] limbObjects = GameObject.FindGameObjectsWithTag("Limb");
        Quaternion[] poseRotations = new Quaternion[limbObjects.Length];
        Vector3[] posePositions = new Vector3[limbObjects.Length];

        //get limb location data
        for (int i = 0; i < limbObjects.Length; i++)
        {
            poseRotations[i] = limbObjects[i].transform.localRotation;
            posePositions[i] = limbObjects[i].transform.localPosition;
        }

        savedPose = new RagPose(limbObjects, poseRotations, posePositions, poseName);
        storingPose = true;
        

    }
}
