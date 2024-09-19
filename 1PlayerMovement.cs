using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speedX = 300;
    public float speedY = 10;
    public Animator animator;
    public bool grounded;

    Vector2 move;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(move.x * speedX * Time.deltaTime, rb.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        float HorizontalInput = Input.GetAxisRaw("Horizontal");

        move = new Vector2(HorizontalInput, Input.GetAxisRaw("Vertical"));

        //Character sprite flip when moving  left/right
        if (HorizontalInput > 0.01f)
            transform.localScale = Vector3.one * 16;
        else if (HorizontalInput < -0.01f)
            transform.localScale = new Vector3(-16, 16, 1);

        //jump
        if (Input.GetKey(KeyCode.Space) && grounded)
            Jump();

        //Set animator parameters
        animator.SetBool("flying", HorizontalInput != 0); 
        animator.SetBool("grounded", grounded);

    }


    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, speedY);
        animator.SetTrigger("jump");
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            grounded = true;
    }
}