using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool isGrounded = false;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Debug movement using arrow keys or A/D keys
        float horizontal = Input.GetAxis("Horizontal");
        Vector2 velocity = rb.velocity;
        velocity.x = horizontal * moveSpeed;
        rb.velocity = velocity;

        Debug.Log("Horizontal Input: " + horizontal); // Log input values

        // Jump with Space key if grounded
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Set isGrounded to true when player touches the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
