using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{
    public GameObject player;
    private Vector2 playerPosition;
    private Vector2 rotationPoint;
    [SerializeField] private float distance = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 mousePosition = Input.mousePosition;
        //mousePosition is represented in world coordinates now
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        playerPosition = player.transform.position;

        float xPos = mousePosition.x - playerPosition.x;
        float yPos = mousePosition.y - playerPosition.y;

        float angle = Mathf.Atan2(yPos, xPos) * Mathf.Rad2Deg;

        float finalX = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;
        float finalY = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;

        transform.position = new Vector3(finalX, finalY, transform.position.z);

        Debug.Log(xPos + " " + yPos + " " + angle);
    }
}
