using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public AbilityMenuScript abilityMenu;
    private HandScriptForReal handScript;

    public Rigidbody2D player;
    public CircleCollider2D ground_Check;
    private float horizontalInput;
    public bool jumpInput;
    public bool isGrounded;
    [SerializeField] private float force = 18;
    [SerializeField] private float inAirForce = 6;
    [SerializeField] public float defaultJumpForce = 700;
    [SerializeField] public float grabJumpForce = 550;
    [SerializeField] private float maxSpeed = 13;


    // Start is called before the first frame update
    void Start()
    {
        handScript = GetComponentInChildren<HandScriptForReal>();
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

        //if player jumps while in midair
        if(isGrounded == false && jumpInput)
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
        Debug.Log("Jumped, Jump Force: " + jumpForce);
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
}
