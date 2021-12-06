using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public AbilityMenuScript abilityMenu;

    public GameObject playerObject;
    public MovementScript movementScript;
    public List<Collision2D> collisionList = new List<Collision2D>();

    // Start is called before the first frame update
    void Start()
    {
        movementScript = playerObject.GetComponent<MovementScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //OnCollisionEnter2D only reads one collision at a time
        //so when touching multiple objects one of them could trigger the exit function and mess everything up
        //a list is used to keep track of collisions
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Object")
        {
            movementScript.isGrounded = true;
            collisionList.Add(collision);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //for every collider in the list of collisions compare tag to current collision
        for(int i = 0; i < collisionList.Count; i++)
        {
            if (collision.gameObject.tag == collisionList[i].gameObject.tag)
            {
                //remove collision from list
                collisionList.Remove(collisionList[i]);
            }
        }

        //if the player isnt colliding with anything then he isnt on the ground
        if(collisionList.Count == 0)
        {
            movementScript.isGrounded = false;
        }
    }
}
