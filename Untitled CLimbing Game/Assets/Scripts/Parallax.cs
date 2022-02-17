using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject farMountains;
    private GameObject closeMountains;

    public GameObject background;
    public SpriteRenderer[] layers;

    private Vector3 camCurrentPos;
    private Vector3 camPrevPos;
    private Vector3 camPosDiff;

    private float closeMountSpd = 0.0225f;
    private float farMountSpd = 0.01125f;

    [SerializeField]
    private float closestLayerSpd = 0.5f;
    [SerializeField]
    private float farthestLayerSpd = 0.2f;

    private Vector3 leftBounds;
    private Vector3 rightBounds;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        //playerStartPos = player.transform.position;
        farMountains = GameObject.FindGameObjectWithTag("FarMountains");
        closeMountains = GameObject.FindGameObjectWithTag("CloseMountains");

        background = GameObject.FindGameObjectWithTag("Background");
        layers = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //leftmost view of cam (0 for x, half the height for y)
        leftBounds = mainCamera.ScreenToWorldPoint(new Vector3 (0, mainCamera.pixelHeight/2, 0 ));
        //rightmost view of cam (width of view for x, half of height for y)
        rightBounds = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight / 2, 0));

        ParallaxEffect();
        //OldParallax();
    }

    void OldParallax()
    {
        //find amount of movement based on player position and previous player position
        camCurrentPos = mainCamera.transform.position;
        camPosDiff = camCurrentPos - camPrevPos;

        //horizontal movement
        closeMountains.transform.position -= new Vector3(camPosDiff.x * closeMountSpd, 0, 0);
        farMountains.transform.position -= new Vector3(camPosDiff.x * farMountSpd, 0, 0);

        //vertical movement
        closeMountains.transform.position -= new Vector3(0, camPosDiff.y * closeMountSpd, 0);
        farMountains.transform.position -= new Vector3(0, camPosDiff.y * farMountSpd, 0);

        //set the previous position for the next update to use
        camPrevPos = camCurrentPos;
    }

    private void ParallaxEffect()
    {
        //find amount of movement based on cam position and previous cam position
        camCurrentPos = mainCamera.transform.position;
        camPosDiff = camCurrentPos - camPrevPos;

        float layerSpdDiff = closestLayerSpd - farthestLayerSpd; //diff between fastest and slowest layer speed

        float speedIncrement = layerSpdDiff / layers.Length; //increments of speed for layers. So farthest layer is slowest, next layer has 1 increment,
                                                             //then 2 increments, and so on to fastest layer

        for (int i = 0; i < layers.Length; i++)
        {
            //numer of increments to add for each layer. Layer 0 needs 0, layer 1 needs 1, etc...
            float layerIncrement = speedIncrement * i;

            //layer movement. each layer is moved based on the camera movement, the fastest and slowest speed, and layer position
            layers[i].transform.position -= new Vector3(camPosDiff.x * (farthestLayerSpd + layerIncrement), camPosDiff.y * (farthestLayerSpd + layerIncrement), 0);

            TileLayer(i);

        }

        //set the previous position for the next update to use
        camPrevPos = camCurrentPos;
    }

    private void TileLayer(int layer)
    {
        /*
        Since the layers are locked to the camera, the bounds.extents give the position without accounting for camera movement
        Adding the cam position gives you an accurate world position for the layer
        */

        //top right corner of layer
        Vector3 layerPosition = layers[layer].bounds.extents + mainCamera.transform.position;


        if(layerPosition.x <= rightBounds.x)
        {
            GameObject copiedLayer = Instantiate(layers[layer].gameObject);
            copiedLayer
        }

        
    }
}
