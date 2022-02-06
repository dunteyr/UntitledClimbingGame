using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private MovementScript moveScript;
    public Animator animator;
    public GameObject ragdoll;

    public bool animationOn;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        moveScript = GetComponent<MovementScript>();
        ragdoll = animator.gameObject;

        animationOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (animationOn)
        {
            AnimatorControl();
        }      
    }

    public void AnimatorControl()
    {
        //flips the character ragdoll when going in both directions
        if (moveScript.horizontalInput < 0)
        {
            ragdoll.transform.localScale = new Vector3(-1, 1, 1);
        }

        else if (moveScript.horizontalInput > 0)
        {
            ragdoll.transform.localScale = new Vector3(1, 1, 1);
        }

        /* --- Run ---*/
        animator.SetFloat("Run", Mathf.Abs(moveScript.horizontalInput));

        /* --- Start Jump --- */
        if (moveScript.jumpInput)
        {
            JumpAnimation(true);
        }

        /* --- Stop Jump --- */
        else if (moveScript.jumpInput == false)
        {
            JumpAnimation(false);
        }

        /* --- Falling --- */
        if (moveScript.isGrounded == false)
        {
            animator.SetBool("isFalling", true);
        }
        else { animator.SetBool("isFalling", false); }
    }

    public void JumpAnimation(bool jumpBool = true)
    {
        if (jumpBool == false)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {

            }
            else { animator.SetBool("Jump", jumpBool); }
        }

        else if (jumpBool)
        {
            animator.SetBool("Jump", jumpBool);
        }
    }

    public void HangAnimation(bool turnOn)
    {
        if (turnOn)
        {
            animator.SetBool("isHanging", true);
        }

        else
        {
            animator.SetBool("isHanging", false);
        }
    }
}
