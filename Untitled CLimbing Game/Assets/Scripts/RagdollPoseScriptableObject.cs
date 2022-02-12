using UnityEngine;

[CreateAssetMenu(fileName = "RagdollPose", menuName = "Ragdoll Pose")]
public class RagdollPoseScriptableObject : ScriptableObject
{
    public string poseName;

    public Quaternion[] limbRotation;
    public Vector3[] limbPosition;
}
