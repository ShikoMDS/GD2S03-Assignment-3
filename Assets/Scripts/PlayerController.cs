using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool isGrounded = false;

    private Rigidbody2D rb;

    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Smooth movement based on input states
        if (isMovingLeft && !isMovingRight)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
        else if (isMovingRight && !isMovingLeft)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseManager.instance.TogglePause();
        }
    }

    // Called when the left button is pressed
    public void MoveLeft()
    {
        isMovingLeft = true;
    }

    // Called when the right button is pressed
    public void MoveRight()
    {
        isMovingRight = true;
    }

    // Called when the left button is released
    public void StopLeft()
    {
        isMovingLeft = false;
    }

    // Called when the right button is released
    public void StopRight()
    {
        isMovingRight = false;
    }

    // Called when the jump button is pressed
    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
