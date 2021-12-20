using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBeaconShader : MonoBehaviour
{
    private Material material;
    private Shader shader;

    private bool twirlForward = true;
    private float twirlValue;
    private Vector2 twirlValueRange;
    private float twirlSpeed = 1.3f;
    private string twirlSine = "_Twirl_Sine";

    private bool noiseForward;
    private float noiseValue;
    private Vector2 noiseValueRange;
    private float noiseSpeed = 1.5f;
    private string noiseSine = "_Noise_Sine";

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        shader = material.shader;

        //gets min and max of twirl sine property into valueRange.x and y
        twirlValueRange = shader.GetPropertyRangeLimits(shader.FindPropertyIndex(twirlSine));
        //set default value to the range min
        twirlValue = twirlValueRange.x;

        noiseValueRange = shader.GetPropertyRangeLimits(shader.FindPropertyIndex(noiseSine));
        noiseValue = noiseValueRange.x;
    }

    // Update is called once per frame
    void Update()
    {
        Twirl();
        Noise();

    }

    void Twirl()
    {
        //once twirlValue gets to top, twirlForward is false and it value goes down
        if (twirlForward)
        {
            if (twirlValue >= twirlValueRange.x)
            {
                //adds to twirlValue every frame and sets it into property
                twirlValue += Time.deltaTime * twirlSpeed;
                material.SetFloat(twirlSine, twirlValue);
                if (twirlValue >= twirlValueRange.y)
                {
                    twirlForward = false;

                    twirlValue = twirlValueRange.y;
                    material.SetFloat(twirlSine, twirlValue);
                }
            }
        }

        else
        {
            if (twirlValue <= twirlValueRange.y)
            {
                //adds to twirlValue every frame and sets it into property
                twirlValue -= Time.deltaTime * twirlSpeed;
                material.SetFloat(twirlSine, twirlValue);
                if (twirlValue <= twirlValueRange.x)
                {
                    twirlForward = true;

                    twirlValue = twirlValueRange.x;
                    material.SetFloat(twirlSine, twirlValue);
                }
            }
        }
    }

    void Noise()
    {
        //once noiseValue gets to top, noiseForward is false and it value goes down
        if (noiseForward)
        {
            if (noiseValue >= noiseValueRange.x)
            {
                //adds to twirlValue every frame and sets it into property
                noiseValue += Time.deltaTime * noiseSpeed;
                material.SetFloat(noiseSine, noiseValue);
                if (noiseValue >= noiseValueRange.y)
                {
                    noiseForward = false;

                    noiseValue = noiseValueRange.y;
                    material.SetFloat(noiseSine, noiseValue);
                }
            }
        }

        else
        {
            if (noiseValue <= noiseValueRange.y)
            {
                //adds to twirlValue every frame and sets it into property
                noiseValue -= Time.deltaTime * noiseSpeed;
                material.SetFloat(noiseSine, noiseValue);
                if (noiseValue <= noiseValueRange.x)
                {
                    noiseForward = true;

                    noiseValue = noiseValueRange.x;
                    material.SetFloat(noiseSine, noiseValue);
                }
            }
        }

    }
}
