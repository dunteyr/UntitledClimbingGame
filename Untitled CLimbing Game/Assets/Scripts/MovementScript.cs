using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public AbilityMenuScript abilityMenu;

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
}
