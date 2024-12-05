using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement playerMovementInstance;

    [SerializeField] public float jump;
    public bool isDead = false;

    private Rigidbody2D rb;
    private bool isGrounded;

    public Animator playerAnim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovementInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jump);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            playerAnim.SetTrigger("SlideTrigger");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            isDead= true;
        }
    }
}
