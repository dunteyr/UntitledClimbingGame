using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalShader : MonoBehaviour
{
    private Material material;
    private Shader shader;

    public bool isSpawnPoint;

    public bool isDissolvedIn;
    private float maxDissolveValue = 1;

    [SerializeField] public float dissolveSpeed = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        shader = material.shader;

        //differentiate between spawn beacon and checkpoint
        if(gameObject.tag == "SpawnPoint") { isSpawnPoint = true; }
        
    }

    // Update is called once per frame
    void Update()
    {
        //makes the default spawn beacon dissolve in immediately when the game starts
        if (isSpawnPoint && isDissolvedIn == false)
        {
            DissolveIn();
        }
    }

    public void DissolveIn()
    {
        if (isDissolvedIn == false)
        {
            //bool that needs to be one for the dissolve to happen
            material.SetInteger("_StartDissolve", 1);
            float currentDissolveValue = material.GetFloat("_Dissolve");

            if(currentDissolveValue < maxDissolveValue)
            {
                float newDissolveValue = currentDissolveValue;
                newDissolveValue += dissolveSpeed * Time.deltaTime;

                material.SetFloat("_Dissolve", newDissolveValue);
            }

            else if(currentDissolveValue >= maxDissolveValue)
            {
                isDissolvedIn = true;
                material.SetFloat("_Dissolve", maxDissolveValue);
            }
        }

        else { Debug.Log("Portal is already dissolved in"); }
    }
}
