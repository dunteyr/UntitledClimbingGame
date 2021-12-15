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

    private float horizontalInput;
    public bool jumpInput;
    public bool isGrounded;
    public bool takeFallDamage = false;

    [SerializeField] private float force = 18;
    [SerializeField] private float inAirForce = 6;
    [SerializeField] public float defaultJumpForce = 700;
    [SerializeField] public float grabJumpForce = 550;
    [SerializeField] private float maxSpeed = 13;
    [SerializeField] private float fallDamageThreshold = 24;
    [SerializeField] public float fallDeathThreshold = 40;


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
        MovePlayer();

        ManageJump();

        //DetectFallDamage();
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
        player.AddForce(transform.up * jumpForce);
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
    public void fixPlayerRotation()
    {
        Vector3 playerRotation = transform.eulerAngles;

        //desired angle is 0 degrees so rotate z axis by -playerRotation
        transform.Rotate(0.0f, 0.0f, playerRotation.z * -1, Space.World);

        //add the rotation restraint to the player (stop ragdoll)
        player.constraints = RigidbodyConstraints2D.FreezeRotation;
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
            Debug.Log("Fall damage");
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
            Debug.Log("Damage Percentage: " + damagePercentage);
        }
    }
}
