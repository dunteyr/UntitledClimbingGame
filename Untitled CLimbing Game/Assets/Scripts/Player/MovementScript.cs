using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class MovementScript : MonoBehaviour
{
    public AbilityMenuScript abilityMenu;
    private HandScriptForReal handScript;
    private PlayerHealth playerHealth;
    private CameraPlayerBehavior camScript;
    private PlayerAnimation animScript;
    private CheckpointManager checkpointMngr;

    public Rigidbody2D player;
    private CapsuleCollider2D playerCollider;
    public CircleCollider2D ground_Check;
    public HingeJoint2D playerToHandJoint;

    public GameObject ragdoll;
    private GameObject[] limbs;
    [SerializeField] private Quaternion[] defaultLimbRot;
    [SerializeField] private Vector3[] defaultLimbPos;

    private float handBreakForce = 20;
    public Collision2D lastFallCollision;

    private CompositeCollider2D terrainCollider;

    private float colliderHeight;
    private Vector2 topOfPlayer;
    private Vector2 bottomOfPlayer;
    private bool isOverlapping = false;
    private Vector3 playerRotation;
    public float torque = 0.5f;

    public float horizontalInput;
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
    public float deathForceAmount = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        handScript = GetComponentInChildren<HandScriptForReal>();
        abilityMenu = GameObject.FindWithTag("Menu").GetComponent<AbilityMenuScript>();
        playerHealth = GetComponent<PlayerHealth>();
        animScript = GetComponent<PlayerAnimation>();
        camScript = GetComponent<CameraPlayerBehavior>();
        checkpointMngr = GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointManager>();

        playerCollider = GetComponent<CapsuleCollider2D>();
        playerToHandJoint = GetComponent<HingeJoint2D>();

        ragdoll = GetComponentInChildren<Animator>().gameObject;
        limbs = GameObject.FindGameObjectsWithTag("Limb");
        defaultLimbRot = new Quaternion[limbs.Length];
        defaultLimbPos = new Vector3[limbs.Length];
        //Sets the limbs' default positions
        InitializeLimbPositions();
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
        /*
        //without specific case for over max speed, high framerates will cause jitteriness in player movement
        else if(player.velocity.magnitude >= maxSpeed)
        {

        }
        */

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
            //store the collision info in case its needed for player death force
            lastFallCollision = collision;

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

    public void SetRagdoll(bool ragdollOn, bool ragdollLetGo = false)
    {
        if (ragdollOn)
        {
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

            //turn on the players hingejoint that is attached to the hand
            playerToHandJoint.enabled = true;

            //if ragdoll is on while player is dead, dont let the player add forces to the body
            if (playerHealth.playerIsDead)
            {
                playerMovable = false;
                animScript.animationOn = false;
                ragdoll.transform.SetParent(null);
                //make the hand invisible and turn off collider
                handScript.gameObject.layer = 8;
                handScript.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                //player.gameObject.SetActive(false);

                //Sets all the limbs to be ragdolled (Affected by gravity and non animated)
                ConfigureLimbs(ragdollOn);
                //applies force to ragdoll (collision data is the collision of the last fall damage)
                DeathForce(lastFallCollision);

                //allow hand to be ripped off if player is dead
                playerToHandJoint.breakForce = handBreakForce;
            }

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

                //this is for when you want to let go of something on ragdoll
                if (ragdollLetGo)
                {
                    if (handScript.isGrabbing)
                    {
                        if (handScript.terrainHingeJoint != null)
                        {
                            handScript.LetGo(handScript.terrainHingeJoint, true);
                        }
                        else if (handScript.handHingeJoint != null)
                        {
                            handScript.LetGo(handScript.handHingeJoint, true);
                        }
                        else if (handScript.ropeHingeJoint != null)
                        {
                            handScript.LetGo(handScript.ropeHingeJoint, true);
                        }
                    }
                }

                //if the hand is grabbing something while ragdolled its mass needs to stay at 1 
                if (handScript.isGrabbing) { handScript.handRigidBody.mass = 1; }
                //otherwise it should be very light so it doesnt pull player around
                else { handScript.handRigidBody.mass = 0.01f; }

            }
        }

        else if(ragdollOn == false)
        {
            //player.gameObject.SetActive(true);
            StartCoroutine(FixPlayerRotation());

            //if the hand is still attached to the body
            if (playerToHandJoint != null)
            {
                //turn off the players hingejoint that is attached to the hand
                playerToHandJoint.enabled = false;
                if (playerHealth.playerIsDead == false)
                {
                    playerMovable = true;
                    animScript.animationOn = true;
                    //make the hand visible and turn on collider
                    handScript.gameObject.layer = 0;
                    handScript.gameObject.GetComponent<CircleCollider2D>().enabled = true;

                    //Sets every limb to non ragdoll
                    ConfigureLimbs(false);

                    //make sure hand cant be ripped off
                    playerToHandJoint.breakForce = Mathf.Infinity;
                }

                if (handScript.handRigidBody == null)
                {
                    Debug.LogWarning("Ragdoll was turned off but handRigidBody is null and cant be turned to Kinematic");
                }

                else if (handScript.handRigidBody != null)
                {
                    //sets non ragdoll hand properties
                    ConfigureHand();
                }
            }

            //if the hand is not attached to the body
            else
            {
                ReattachHand();
            }
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
                    limbs[i].transform.localRotation = defaultLimbRot[i];
                    limbs[i].transform.localPosition = defaultLimbPos[i];

                }
                else { Debug.LogError("Sprite skin was null. Couldnt turn off ragdoll"); }
            }
        }
    }

    private void InitializeLimbPositions()
    {
        for (int i = 0; i < limbs.Length; i++)
        {
            defaultLimbRot[i] = limbs[i].transform.localRotation;

            defaultLimbPos[i] = limbs[i].transform.localPosition;
        }
    }

    //Player rig dettaches from player controller on ragdoll. this function reattaches them. (To respawn correctly)
    public void AttachRagToPlayer()
    {
        //sets the ragdoll parent to the player position
        ragdoll.transform.SetPositionAndRotation(player.gameObject.transform.position, player.gameObject.transform.rotation); 
        //sets the pelvis to the player position (And the ragdoll parent position)
        ragdoll.transform.Find("Pelvis").SetPositionAndRotation(player.gameObject.transform.position, player.gameObject.transform.rotation);
        //sets the ragdoll parents' parent to be the player controller
        ragdoll.transform.SetParent(player.gameObject.transform);
    }

    //This function puts a force on the player ragdoll after death using the collision of the previous fall damage. Called in SetRagdoll()
    private void DeathForce(Collision2D collision)
    {
        //Check for collision data
        if(collision == null) { Debug.Log("No collision data for death force"); }

        else
        {
            //Check if the ragdoll has been unchilded from player
            if (ragdoll.transform.parent != null)
            {
                Debug.LogError("ragdoll was given death force while still a child of player controller");
            }

            else
            {
                //Get the ragdoll pelvis
                Rigidbody2D pelvis = ragdoll.transform.Find("Pelvis").GetComponent<Rigidbody2D>();
                //Get the death force from collision data
                Vector2 deathForce = collision.relativeVelocity;
                //apply the death force to the pelvis
                pelvis.AddForce(deathForce * deathForceAmount, ForceMode2D.Impulse);
            }
        }

    }

    /* --- DEPRECATED --- */
    //Ragdoll system switched from capsule to rigged ragdoll. Hand cant dettach anymore so doesnt need reconfiguring. Can PROBABLY be deleted but will keep for now
    //Sets non ragdoll hand properties
    private void ConfigureHand()
    {
        handScript.handRigidBody.bodyType = RigidbodyType2D.Kinematic;
        //resets the original weight of hand
        handScript.handRigidBody.mass = 1f;
        handScript.handCollider.isTrigger = true;
        handScript.handControl = true;
        handScript.handRigidBody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
    }

    /* --- DEPRECATED --- */
    //Ragdoll system switched from capsule to rigged ragdoll. Hand cant dettach anymore. Can be deleted PROBABLY but will keep for now
    private void ReattachHand()
    {
        //get hand object and create an offset for the hand position
        GameObject hand = handScript.gameObject;
        Vector3 offset = new Vector3(handScript.distance, 0);

        //set the position of the hand
        hand.transform.SetPositionAndRotation(player.transform.position + offset, new Quaternion(0, 0, 0, 0));

        //add a new hinge joint
        playerToHandJoint = player.gameObject.AddComponent<HingeJoint2D>();

        //connect to hand, set anchor point, and disable (because player will spawn not grabbing anything)
        playerToHandJoint.connectedBody = handScript.handRigidBody;
        playerToHandJoint.autoConfigureConnectedAnchor = true;
        playerToHandJoint.anchor = new Vector2(0, handScript.rotationPointOffset);

        //Once hand is attached. Redo the ragdoll function
        SetRagdoll(false);

    }

    /* --- DEPRECATED --- */
    //Overlap fixing is done by the FixPlayerRotation coroutine. This function can be deleted
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

    /* --- DEPRECATED --- */
    //Overlap fixing is done by the FixPlayerRotation coroutine. This function can be deleted
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

    /* --- DEPRECATED --- */
    //This function was replaced by SetRagdoll and can PROBABLY be deleted.
    public void OldSetRagdoll(bool ragdollOn, bool ragdollLetGo = false)
    {

        /*---TURN ON RAGDOLL---*/
        if (ragdollOn)
        {
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

            //turn on the players hingejoint that is attached to the hand
            playerToHandJoint.enabled = true;

            //if ragdoll is on while player is dead, dont let the player add forces to the body
            if (playerHealth.playerIsDead)
            {
                playerMovable = false;

                //allow hand to be ripped off if player is dead
                playerToHandJoint.breakForce = handBreakForce;
            }

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

                //this is for when you want to let go of something on ragdoll
                if (ragdollLetGo)
                {
                    if (handScript.isGrabbing)
                    {
                        if (handScript.terrainHingeJoint != null)
                        {
                            handScript.LetGo(handScript.terrainHingeJoint, true);
                        }
                        else if (handScript.handHingeJoint != null)
                        {
                            handScript.LetGo(handScript.handHingeJoint, true);
                        }
                        else if (handScript.ropeHingeJoint != null)
                        {
                            handScript.LetGo(handScript.ropeHingeJoint, true);
                        }
                    }
                }

                //if the hand is grabbing something while ragdolled its mass needs to stay at 1 
                if (handScript.isGrabbing) { handScript.handRigidBody.mass = 1; }
                //otherwise it should be very light so it doesnt pull player around
                else { handScript.handRigidBody.mass = 0.01f; }

            }
        }

        /*---TURN OFF RAGDOLL---*/
        else if (ragdollOn == false)
        {
            StartCoroutine(FixPlayerRotation());

            //if the hand is still attached to the body
            if (playerToHandJoint != null)
            {
                //turn off the players hingejoint that is attached to the hand
                playerToHandJoint.enabled = false;
                if (playerHealth.playerIsDead == false)
                {
                    playerMovable = true;

                    //make sure hand cant be ripped off
                    playerToHandJoint.breakForce = Mathf.Infinity;
                }

                if (handScript.handRigidBody == null)
                {
                    Debug.LogWarning("Ragdoll was turned off but handRigidBody is null and cant be turned to Kinematic");
                }

                else if (handScript.handRigidBody != null)
                {
                    //sets non ragdoll hand properties
                    ConfigureHand();
                }
            }

            //if the hand is not attached to the body
            else
            {
                ReattachHand();
            }
        }
    }

}
