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
    public Transform[] tPose;

    // Start is called before the first frame update
    void Start()
    {
        moveScript = GameObject.FindGameObjectWithTag("Player").GetComponent<MovementScript>();
        limbs = GameObject.FindGameObjectsWithTag("Limb");
        defaultRot = new Quaternion[limbs.Length];
        defaultPos = new Vector3[limbs.Length];

        InitializeDefaultPose();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeDefaultPose()
    {
        for (int i = 0; i < limbs.Length; i++)
        {
            defaultRot[i] = limbs[i].transform.localRotation;
            defaultPos[i] = limbs[i].transform.localPosition;
        }
    }

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

                    Debug.Log(defaultRot[i] + " " + defaultPos[i]);

                }
                else { Debug.LogError("Sprite skin was null. Couldnt turn off ragdoll"); }
            }
        }
    }
}
