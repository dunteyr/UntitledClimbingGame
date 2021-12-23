using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public AbilityMenuScript abilityMenu;
    private HandScriptForReal handScript;
    private PlayerHealth playerHealth;

    public Rigidbody2D player;
    private CapsuleCollider2D playerCollider;
    public CircleCollider2D ground_Check;

    private CompositeCollider2D terrainCollider;

    private float colliderHeight;
    private Vector2 topOfPlayer;
    private Vector2 bottomOfPlayer;
    private bool isOverlapping = false;
    private Vector3 playerRotation;
    public float torque = 0.5f;

    private float horizontalInput;
    public bool jumpInput;
    public bool isGrounded;
    public bool takeFallDamage = false;
    public bool playerMovable = true;

    [SerializeField] private float force = 18;
    [SerializeField] private float inAirForce = 6;
    [SerializeField] public float defaultJumpForce = 700;
    [SerializeField] public float grabJumpForce = 550;
    [SerializeField] private float maxSpeed = 13;
    [SerializeField] private float fallDamageThreshold = 24;
    [SerializeField] public float fallDeathThreshold = 35;


    // Start is called before the first frame update
    void Start()
    {
        handScript = GetComponentInChildren<HandScriptForReal>();
        abilityMenu = GameObject.FindWithTag("Menu").GetComponent<AbilityMenuScript>();
        playerHealth = GetComponent<PlayerHealth>();
        playerCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if(Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true;
        }
    }

    void FixedUpdate()
    {
        //playerMovable becomes false in SetRagdoll so that player cant add forces to limp body
        if (playerMovable)
        {
            MovePlayer();

            ManageJump();
        } 
    }

    private void ManageJump()
    {
        //if player jumps while in midair
        if (isGrounded == false && jumpInput)
        {
            //doesnt jump if Infinite Jump is off
            if (abilityMenu.infiniteJumpActive == false)
            {
                jumpInput = false;
            }
            //does jump if Infinite Jump is on
            else if (abilityMenu.infiniteJumpActive)
            {
                Jump(defaultJumpForce);
                jumpInput = false;
            }
        }
        else if (isGrounded && jumpInput)
        {
            Jump(defaultJumpForce);
            jumpInput = false;
        }
    }

    public void Jump(float jumpForce)
    {
        //force is relative to player's "up" not the worlds "up"
        player.AddForce(Vector3.up * jumpForce);
    }

    private void MovePlayer()
    {
        
        //only accept input and add force to player if he is slower than max speed
        if(player.velocity.magnitude < maxSpeed)
        {
            if (horizontalInput > 0)
            {
                if (isGrounded)
                {
                    player.AddForce(transform.right * force * horizontalInput);
                }

            }
            else if (horizontalInput < 0)
            {
                if (isGrounded)
                {
                    player.AddForce(transform.right * force * horizontalInput);
                } 
            }
        }

        //whether faster than max speed or not if hes in the air he has slight right-left control
        if (isGrounded == false)
        {
            player.AddForce(transform.right * inAirForce * horizontalInput);
        }
    }

    //called in HandSriptForReal when letting go from terrain
    IEnumerator FixPlayerRotation()
    {
        playerRotation = transform.eulerAngles;

        //while the player is rotated between 60 and 120 degrees
        while (playerRotation.z > 60 && playerRotation.z < 120)
        {
            //gets rotation and adds torque based on which side player is rotated to
            playerRotation = transform.eulerAngles;
            player.AddTorque(torque * -1, ForceMode2D.Impulse);

            yield return null;
        }

        //same as other loop but for the other side
        while(playerRotation.z < 300 && playerRotation.z > 240)
        {
            playerRotation = transform.eulerAngles;
            player.AddTorque(torque, ForceMode2D.Impulse);

            yield return null;
        }

        //after the player rotation is out of those zones, set the rotation to upright instantly and stop the coroutine
        playerRotation = transform.eulerAngles;
        //desired angle is 0 degrees so rotate z axis by -playerRotation
        transform.Rotate(0.0f, 0.0f, playerRotation.z * -1, Space.World);
        
        player.constraints = RigidbodyConstraints2D.FreezeRotation;

        yield break;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        FallDamage(collision);
    }

    void FallDamage(Collision2D collision)
    {
        //when the collision speed is over the fall damage min
        if (collision.relativeVelocity.magnitude >= fallDamageThreshold)
        {
            float collisionSpeed = collision.relativeVelocity.magnitude;

            //difference between the min speed to get hurt and the death speed (both changeable)
            float damageRange = fallDeathThreshold - fallDamageThreshold;
            //how much over the min speed the collision speed was
            float speedOverage = collisionSpeed - fallDamageThreshold;

            //divide the range of min damage to death damage and the number over the min to find the percent overage
            float damageRatio = speedOverage / damageRange;
            //convert the fraction to a number between 1 and 100 for the DamagePlayer function
            float damagePercentage = damageRatio * 100;

            //apply that percent overage to the players health in the form of damage
            playerHealth.DamagePlayer(damagePercentage, true);
        }
    }

    public void SetRagdoll(bool ragdollOn)
    {
        if (ragdollOn)
        {
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

            //turn on the players hingejoint that is attached to the hand
            HingeJoint2D playerToHandJoint = GetComponent<HingeJoint2D>();
            playerToHandJoint.enabled = true;

            //if ragdoll is on while player is dead, dont let the player add forces to the body
            if (playerHealth.playerIsDead) { playerMovable = false; }

            if (handScript.handRigidBody == null)
            {
                Debug.LogWarning("Ragdoll was turned on but handRigidBody is null and cant be turned to dynamic");
            }

            else if (handScript.handRigidBody != null)
            {
                handScript.handRigidBody.bodyType = RigidbodyType2D.Dynamic;
                handScript.handCollider.isTrigger = false;
                handScript.handControl = false;
                handScript.handRigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

                //if the hand is grabbing something while ragdolled its mass needs to stay at 1 
                if (handScript.isGrabbing) { handScript.handRigidBody.mass = 1; }
                //otherwise it should be very light so it doesnt pull player around
                else { handScript.handRigidBody.mass = 0.01f; }

            }
        }

        else if(ragdollOn == false)
        {
            StartCoroutine(FixPlayerRotation());

            //turn off the players hingejoint that is attached to the hand
            HingeJoint2D playerToHandJoint = GetComponent<HingeJoint2D>();
            playerToHandJoint.enabled = false;
            if (playerHealth.playerIsDead == false) { playerMovable = true; }

            if (handScript.handRigidBody == null)
            {
                Debug.LogWarning("Ragdoll was turned off but handRigidBody is null and cant be turned to Kinematic");
            }

            else if (handScript.handRigidBody != null)
            {
                handScript.handRigidBody.bodyType = RigidbodyType2D.Kinematic;
                //resets the original weight of hand
                handScript.handRigidBody.mass = 1f;
                handScript.handCollider.isTrigger = true;
                handScript.handControl = true;
                handScript.handRigidBody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
            }
        }
    }

    private void CheckOverlap()
    {
        Debug.Log("Overlap check triggered");
        terrainCollider = GameObject.FindGameObjectWithTag("Ground").GetComponent<CompositeCollider2D>();

        colliderHeight = playerCollider.size.y;
        topOfPlayer = transform.position + new Vector3(0, colliderHeight/2, 0);
        bottomOfPlayer = transform.position - new Vector3(0, colliderHeight / 2, 0);

        if (terrainCollider.OverlapPoint(bottomOfPlayer + new Vector2 (0, 0.05f)))
        {
            isOverlapping = true;
        }

        //fixes the overlap if there is one
        if (isOverlapping)
        {
            FixOverlap();
            isOverlapping = false;
        }  
    }

    private void FixOverlap()
    {
        //get a point on the composite collider perimeter closest to top of player
        Vector2 topClo = terrainCollider.ClosestPoint(topOfPlayer);

        //subtract the distance of the top of player to the comp collider from the player height to find how much of player is overlapped
        float nonOverlap = topOfPlayer.y - topClo.y;
        float overlap = colliderHeight - nonOverlap;

        //add the overlap (plus a small amount just in case) to the player vertical position to get him out of the terrain
        Vector3 positionChange = new Vector3(0, overlap + 0.05f, 0);
        transform.SetPositionAndRotation(transform.position + positionChange, transform.rotation);
    }
}
