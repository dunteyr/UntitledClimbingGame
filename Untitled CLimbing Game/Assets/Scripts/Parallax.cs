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
    public SpriteRenderer[] duplicateLayers;

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
    private int rightBounds;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        //playerStartPos = player.transform.position;
        farMountains = GameObject.FindGameObjectWithTag("FarMountains");
        closeMountains = GameObject.FindGameObjectWithTag("CloseMountains");

        background = GameObject.FindGameObjectWithTag("Background");
        layers = GetComponentsInChildren<SpriteRenderer>();
        duplicateLayers = new SpriteRenderer[layers.Length];
    }

    // Update is called once per frame
    void Update()
    {
        //leftmost view of cam (0 for x, half the height for y)
        leftBounds = mainCamera.ScreenToWorldPoint(new Vector3 (0, mainCamera.pixelHeight/2, 0 ));
        //rightmost view of cam in pixels
        rightBounds = mainCamera.pixelWidth;

        Debug.Log(mainCamera.WorldToScreenPoint(layers[0].bounds.extents) + " " + mainCamera.pixelWidth);
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

        //pixel coordinates for the top right of layer
        Vector3 layerPosition = mainCamera.WorldToScreenPoint(layers[layer].bounds.extents);

        if (layerPosition.x <= rightBounds)
        {
            //check if there is already a duplicate
            if(duplicateLayers[layer] == null)
            {
                GameObject copiedLayer = Instantiate(layers[layer].gameObject);
                copiedLayer.transform.parent = layers[layer].transform.parent;
                duplicateLayers[layer] = copiedLayer.GetComponent<SpriteRenderer>();
            }

            //setting up the position of the new layer
            Vector3 dupLayerPos = new Vector3();
            //new x pos is the original layer's x pos plus the size of the layer
            dupLayerPos.x = layers[layer].transform.position.x + layers[layer].bounds.size.x - 0.025f; //giving layers a slight overlap
            dupLayerPos.y = layers[layer].transform.position.y;
            dupLayerPos.z = layers[layer].transform.position.z;

            //set local scale
            duplicateLayers[layer].transform.localScale = new Vector3(1, 1, 1);

            //offset the new layer 
            duplicateLayers[layer].transform.position = dupLayerPos;   
        }

        else if( layerPosition.x > rightBounds)
        {
            if(duplicateLayers[layer] != null)
            {
                Destroy(duplicateLayers[layer]);
            }
        }

        
    }
}
