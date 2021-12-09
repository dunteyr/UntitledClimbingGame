using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScriptForReal : MonoBehaviour
{
    public AbilityMenuScript abilityMenu;
    private MovementScript movementScript;

    private Collider2D handCollider;
    public GameObject player;
    private Vector2 playerPosition;
    private Vector2 rotationPoint;
    [SerializeField] private float distance = 1;
    [SerializeField] private float rotationPointOffset;

    private SpriteRenderer spriteRend;
    private Color defaultColor = Color.white;
    [SerializeField] private Color grabbableColor = Color.green;
    [SerializeField] private Color grabColor = Color.blue;
    public bool isGrabbing;

    private HingeJoint2D handHingeJoint;
    private HingeJoint2D terrainHingeJoint;
    private HingeJoint2D ropeHingeJoint;

    private bool leftMouseClicked;
    private bool rightMouseClicked;

    private bool handControl;
    
    void Start()
    {
        spriteRend = gameObject.GetComponent<SpriteRenderer>();
        handCollider = gameObject.GetComponent<Collider2D>();
        handControl = true;

        movementScript = player.GetComponent<MovementScript>();
    }

    private void Update()
    {
        //Getting input is more accurate in Update()
        ManageInput();
    }


    void FixedUpdate()
    {
        PointHandToMouse();

        ManageLettingGo();
    }

    private void ManageInput()
    {
        //Left Mouse Click
        if (Input.GetMouseButtonDown(0))
        {
            leftMouseClicked = true;
        }

        else if (Input.GetMouseButtonUp(0))
        {
            leftMouseClicked = false;
        }

        //Right Mouse Clicked
        if (Input.GetMouseButtonDown(1))
        {
            rightMouseClicked = true;
        }

        else if (Input.GetMouseButtonUp(1))
        {
            rightMouseClicked = false;
        }
    }

    private void PointHandToMouse()
    {
        //parameter used to turn on and off handControl with function call
        if (handControl)
        {
            Vector2 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            playerPosition = player.transform.position;
            //rotation point is the player position with the offset added
            rotationPoint = new Vector2(playerPosition.x, playerPosition.y + rotationPointOffset);

            float xPos = mousePosition.x - rotationPoint.x;
            float yPos = mousePosition.y - rotationPoint.y;

            float mag = Mathf.Sqrt(xPos * xPos + yPos * yPos);
            float xPosDir = xPos / mag;
            float yPosDir = yPos / mag;

            transform.position = new Vector3(rotationPoint.x + xPosDir * distance, rotationPoint.y + yPosDir * distance, this.transform.position.z);
        }
    }

    //Calls LetGo() function depending on certain inputs and hingejoints
    private void ManageLettingGo()
    {
        //if player right clicks while holding something (has a hingejoint component) let go of it
        if (rightMouseClicked && handHingeJoint != null)
        {
            rightMouseClicked = false;
            LetGo(handHingeJoint);
        }

        if (rightMouseClicked && terrainHingeJoint != null)
        {
            rightMouseClicked = false;
            LetGo(terrainHingeJoint);
        }

        if (rightMouseClicked && ropeHingeJoint != null)
        {
            rightMouseClicked = false;
            LetGo(ropeHingeJoint);
        }

        //if player jumps while holding terrain then let go and jump
        if (movementScript.jumpInput && terrainHingeJoint != null)
        {
            LetGo(terrainHingeJoint);
            movementScript.Jump(movementScript.grabJumpForce);
        }

        //if player jumps while holding rope then let go and jump
        if (movementScript.jumpInput && ropeHingeJoint != null)
        {
            LetGo(ropeHingeJoint);
            movementScript.Jump(movementScript.grabJumpForce);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        

        if(collision.tag == "Ground" || collision.tag == "Object" || collision.tag == "Rope")
        {
            spriteRend.color = grabbableColor;

            if (leftMouseClicked)
            {
                isGrabbing = true;
                leftMouseClicked = false;
                //turning off trigger so ontriggerstay doesnt execute while grabbing
                handCollider.isTrigger = false;
                Grab(collision);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(isGrabbing)
        {
            spriteRend.color = grabColor;
        }    
        else
        {
            spriteRend.color = defaultColor;
        }
    }

    //called in OnTriggerStay so player can grab ground
    private void Grab(Collider2D collision)
    {
        //if there is already a joint on the hand, remove it
        if(gameObject.GetComponent<HingeJoint2D>() != null)
        {
            Debug.LogWarning("There was a hingejoint on this object");
            Destroy(gameObject.GetComponent<HingeJoint2D>());
        }

        if(collision.tag == "Object")
        {
            //Get rigidbody of the object collided with
            Rigidbody2D bodyToConnect = collision.gameObject.GetComponent<Rigidbody2D>();

            //create hingejoint component on hand and edit properties
            //hingeJoint is initialized outside of Grab() so it can be referenced in "Let Go" method
            handHingeJoint = gameObject.AddComponent<HingeJoint2D>();
            handHingeJoint.autoConfigureConnectedAnchor = false;

            //both anchor and connected anchor are relative to their anchored object. so (0, 0) is center of hand and center of object
            handHingeJoint.anchor = new Vector2(0, 0);
            handHingeJoint.connectedBody = bodyToConnect;
            handHingeJoint.connectedAnchor = new Vector2(0, 0);

            //attach joint to objects rigidbody
        }
        
        //as opposed to objects, if the player grabs static terrain, the hinge joint component is added to
        //the terrain rather than the hand because the players hangs from terrain. Terrain doesn't hang from the hand
        if(collision.tag == "Ground")
        {
            //check if the Ground has a composite collider
            if(collision.GetComponent<CompositeCollider2D>() == null)
            {
                Debug.LogError("The object with tag: Ground, has no CompositeCollider to grab");
            }
            else
            {
                CompositeCollider2D terrainCollider = collision.GetComponent<CompositeCollider2D>();
                Vector2 anchorPoint = terrainCollider.ClosestPoint(gameObject.transform.position);
                Rigidbody2D handRigidBody = gameObject.GetComponent<Rigidbody2D>();

                //add hinge joint to terrain
                terrainHingeJoint = collision.gameObject.AddComponent<HingeJoint2D>();

                //configure hinge joint and attach to terrain
                terrainHingeJoint.autoConfigureConnectedAnchor = true;
                terrainHingeJoint.anchor = anchorPoint;

                //turn off hand control
                //and change hand from kinematic to dynamic so it is physics affected
                handControl = false;
                handRigidBody.bodyType = RigidbodyType2D.Dynamic;

                //turn on the players hingejoint that is attached to the hand
                HingeJoint2D playerToHandJoint = player.GetComponent<HingeJoint2D>();
                playerToHandJoint.enabled = true;

                //remove the rotation restraint from the player (ragdoll)
                player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

                //attach terrains hingejoint to hand
                terrainHingeJoint.connectedBody = handRigidBody;
                terrainHingeJoint.connectedAnchor = new Vector2(0, 0);

            }
        }

        if (collision.tag == "Rope")
        {
            //check if the Ground has a composite collider
            if (collision.GetComponent<BoxCollider2D>() == null)
            {
                Debug.LogError("The object with tag: Rope, has no BoxCollider to grab");
            }
            else
            {
                BoxCollider2D ropeCollider = collision.GetComponent<BoxCollider2D>();
                //Vector2 anchorPoint = ropeCollider.ClosestPoint(gameObject.transform.position);
                Rigidbody2D handRigidBody = gameObject.GetComponent<Rigidbody2D>();

                //add hinge joint to rope
                ropeHingeJoint = collision.gameObject.AddComponent<HingeJoint2D>();

                //configure hinge joint and attach to rope
                ropeHingeJoint.autoConfigureConnectedAnchor = true;
                ropeHingeJoint.anchor = new Vector2 (0, 0);

                //turn off hand control
                //and change hand from kinematic to dynamic so it is physics affected
                handControl = false;
                handRigidBody.bodyType = RigidbodyType2D.Dynamic;

                //turn on the players hingejoint that is attached to the hand
                HingeJoint2D playerToHandJoint = player.GetComponent<HingeJoint2D>();
                playerToHandJoint.enabled = true;

                //remove the rotation restraint from the player (ragdoll)
                player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

                //attach ropes hingejoint to hand
                ropeHingeJoint.connectedBody = handRigidBody;
                ropeHingeJoint.connectedAnchor = new Vector2(0, 0);

                Debug.Log("Anchor Point " + ropeCollider.ClosestPoint(gameObject.transform.position));
                Debug.Log("Rope Collider: " + ropeCollider);
                Debug.Log("hand position: " + gameObject.transform.localPosition);

            }
        }
    }

    private void LetGo(HingeJoint2D jointToLetGo)
    {
        spriteRend.color = defaultColor;
        isGrabbing = false;
        if(jointToLetGo == null)
        {
            Debug.LogWarning("There is no hinge joint attached yet LetGo() was executed");
        }

        else if(jointToLetGo != null)
        {
            //check if the let go was called for terrain grabbing
            //because it requires property changes of player and hand
            if(jointToLetGo == terrainHingeJoint)
            {
                Destroy(terrainHingeJoint);

                Rigidbody2D handRigidBody = gameObject.GetComponent<Rigidbody2D>();
                handRigidBody.bodyType = RigidbodyType2D.Kinematic;

                //turn off the players hingejoint that is attached to the hand
                HingeJoint2D playerToHandJoint = player.GetComponent<HingeJoint2D>();
                playerToHandJoint.enabled = false;

                //let mouse control hand again
                handControl = true;

                //makes player stand upright and constrains rotation
                player.GetComponent<MovementScript>().fixPlayerRotation();

                handCollider.isTrigger = true;
            }

            else if(jointToLetGo == ropeHingeJoint)
            {
                Destroy(ropeHingeJoint);

                Rigidbody2D handRigidBody = gameObject.GetComponent<Rigidbody2D>();
                handRigidBody.bodyType = RigidbodyType2D.Kinematic;

                //turn off the players hingejoint that is attached to the hand
                HingeJoint2D playerToHandJoint = player.GetComponent<HingeJoint2D>();
                playerToHandJoint.enabled = false;

                //let mouse control hand again
                handControl = true;

                //makes player stand upright and constrains rotation
                player.GetComponent<MovementScript>().fixPlayerRotation();

                handCollider.isTrigger = true;
            }
            else
            {
                Destroy(jointToLetGo);
                //Allow hand to grab again
                handCollider.isTrigger = true;
            }            
        }
    }
}
