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
    private bool jumpInput;
    public bool isGrounded;
    [SerializeField] private float force = 10;
    [SerializeField] private float inAirForce = 5;
    [SerializeField] private float jumpForce = 50;


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

        if (horizontalInput > 0)
        {
            if (isGrounded)
            {
                player.AddForce(transform.right * force * horizontalInput);
            }

            else if(isGrounded == false)
            {
                player.AddForce(transform.right * inAirForce * horizontalInput);
            }
            
        }
        else if(horizontalInput < 0)
        {
            if (isGrounded)
            {
                player.AddForce(transform.right * force * horizontalInput);
            }

            //player has less control when in the air
            else if (isGrounded == false)
            {
                player.AddForce(transform.right * inAirForce * horizontalInput);
            }
        }

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
                Jump();
                jumpInput = false;
            }
        }
        else if (isGrounded && jumpInput)
        {
            Jump();
            jumpInput = false;
        }
    }

    void Jump()
    {
        player.AddForce(transform.up * jumpForce);
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
