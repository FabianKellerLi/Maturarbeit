using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] public float speedX = 800;
    [SerializeField] public float speedY = 15;
    private Animator animator;
    public float gravityIncrease = -5;
    Vector2 move;
    private BoxCollider2D boxCollider;
    public float RaycastBoxDistanceDown = 0.2f;
    
    //Layers
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    
    //Wallslide
    private bool isWallSliding;
    public float wallSlidingSpeed = 0.2f;
    
    //Walljump
    private bool isWallJumping = false;
    private float wallJumpingDirection;
    private float wallJumpingDuration = 0.1f;
    private float wallJupmingTime = 0.2f;
    private float wallJumpingCounter;
    private Vector2 wallJumpingPower = new Vector2(20, 15);




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(move.x * speedX * Time.deltaTime, rb.velocity.y);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        print(-rb.transform.localScale.x);

        

        float HorizontalInput = Input.GetAxisRaw("Horizontal");

        move = new Vector2(HorizontalInput, Input.GetAxisRaw("Vertical"));

        //Character sprite flip when moving  left/right
        if (HorizontalInput > 0.01f)
            transform.localScale = Vector3.one * 16;
        else if (HorizontalInput < -0.01f)
            transform.localScale = new Vector3(-16, 16, 1);

        //jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
        {
            Jump();
        }
          

        //Set animator parameters
        animator.SetBool("flying", HorizontalInput != 0); 
        animator.SetBool("grounded", isGrounded());


        //Fallbeschleunigung, falls Space nicht mehr gedrückt wird
        if (isGrounded() == false && !Input.GetKey(KeyCode.Space) && !onWall() && !isWallJumping)
        {
            rb.AddForce(new Vector2(0 , gravityIncrease));
        }

        wallSlide();

        //Walljump
        if (Input.GetKeyDown(KeyCode.Space) && onWall() && !isGrounded())
        {
            wallJump();
        }


    }


    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, speedY);
        animator.SetTrigger("jump");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
    }


    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, RaycastBoxDistanceDown, groundLayer);
           return raycastHit.collider != null;
    
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), RaycastBoxDistanceDown, wallLayer);
        return raycastHit.collider != null;
    
    }

    private void wallSlide()
    {
        if (onWall() && !isGrounded() && Input.GetAxisRaw("Horizontal") == Mathf.Sign(transform.localScale.x))
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    
    private void wallJump()
    {
        if (onWall())
        {
            isWallJumping = false;
            wallJumpingDirection = -Mathf.Sign(transform.localScale.x);
            wallJumpingCounter = wallJupmingTime;

            CancelInvoke(nameof(stopWallJump));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0;
        }

        Invoke(nameof(stopWallJump), wallJumpingDuration);
    }

    private void stopWallJump()
    {
        isWallJumping = false;
    }
}
