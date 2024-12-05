//OBSOLETE - Use Advanced Player Controller

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRB;
    [SerializeField] public float moveSpeed = 10;
    [SerializeField] public float jump = 1;
    public bool isDead = false;

    private float xInput, zInput;
    private bool isGrounded;

    public Animator playerAnim;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Best to handle input processing
        ProcessInput();
    }

    private void FixedUpdate()
    {
        //For physics based movement - best to handle movement
        MovePlayer();
    }

    private void ProcessInput()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerRB.AddForce(Vector2.up * jump);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerAnim.SetTrigger("SlideTrigger");
        }
    }

    private void MovePlayer()
    {
        playerRB.AddForce(new Vector3(xInput, 0f, zInput) * moveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
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
        if (other.gameObject.CompareTag("Enemy"))
        {
            isDead = true;
        }
    }
}