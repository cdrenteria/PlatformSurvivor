using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public Rigidbody2D rb;
    private Vector2 currentPosition;
    private float inputx;
    private float inputy;
    private float movementSpeed;
    private float jumpforce;
    private float fallForce;
    private float hangTime;
    private float coyoteTime;
    private float jumpBuffer;

    private CapsuleCollider2D playerCollider;

    public AudioSource jumpSFX;
    public Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        hangTime = .5f;
        playerCollider = GetComponent<CapsuleCollider2D>();
        coyoteTime = 0f;
        jumpBuffer = 0f;
        jumpSFX = GetComponent<AudioSource>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        movementSpeed = 8f;
        jumpforce = 15f;
        fallForce = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        DecrementTimedActions();
        jumpDetection();
        moveInputDetection();
    }

    private void DecrementTimedActions()
    {
        if (coyoteTime > 0)
        {
            coyoteTime -= Time.deltaTime;
        } else
        {
            coyoteTime = 0f;
        }

        if (jumpBuffer > 0)
        {
            jumpBuffer -= Time.deltaTime;
        } else
        {
            jumpBuffer = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (!isGrounded() && inputx == 0)
        {
            hangTime -= Time.deltaTime;
        }
        else if (!isGrounded() && inputx == 0 && hangTime <= 0f)
        {
            rb.velocity = Vector2.down * fallForce;
        }
    }

    private void jumpDetection()
    {
        //inputy has to be greater than zero to work with descending platforms which use the space bar as well
        //If the player inputs jump and input is not negative and they are grounded then jump
        if ( Input.GetKeyDown(KeyCode.Space) && CanJump() && inputy >= 0 || Input.GetKeyDown(KeyCode.W) && CanJump() && inputy >= 0)
        {
            Jump();
        } 
        //If the player inputs jump, input y is not negative, and coyote time remains, then jump
        else if (Input.GetKeyDown(KeyCode.Space) && CanJump() && inputy >= 0 || Input.GetKeyDown(KeyCode.W) && CanJump() && inputy >= 0)
        {
            Jump();
            coyoteTime = 0f;
        } 
        //If the player has a buffered jump and is grounded jump
        else if (isGrounded() && jumpBuffer > 0f)
        {
            jumpBuffer = 0f;
            Jump();
        } 
        //if the player inputs a jump and is not grounded and input y is not negative then buffer a jump for .1f seconds
        else if (Input.GetKeyDown(KeyCode.Space) && !isGrounded() && inputy >= 0 || Input.GetKeyDown(KeyCode.W) && !isGrounded() && inputy >= 0)
        {
            jumpBuffer = .15f;
        }
    }
    private bool CanJump()
    {
        //check for grounded and not presseing 's'
        if (isGrounded() && Time.timeScale == 1)
        {
            return true;
        } else if (coyoteTime > 0 && Time.timeScale == 1)
        {
            return true;
        } 
        return false;
    }
    private void Jump()
    {
        //prevent double jumps by setting coyote time to zero when jumping
        coyoteTime = 0f;
        jumpSFX.PlayOneShot(jumpSFX.clip);
        currentPosition = transform.position;
        rb.AddForce(Vector2.up * jumpforce , ForceMode2D.Impulse);
    }

    private bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCollider.bounds.min, -transform.up, .05f);

        if (hit.collider != null)
        {
            coyoteTime = .15f;
            return true;
        } 
        else
        {
            return false;
        }
    }
        private void moveInputDetection()
    {
        inputx = Input.GetAxisRaw("Horizontal");
        inputy = Input.GetAxisRaw("Vertical");
        Move(new Vector2(inputx, rb.velocity.y));
    }

    private void Move(Vector2 dir)
    {
        currentPosition = transform.position;
        if (dir.x != 0 || dir.y != 0)
        {
            rb.velocity = new Vector2(dir.x * movementSpeed, dir.y) ;
        }

        if (dir.x != 0 )
        {
            moveDirection = dir;
        }

    }
}
