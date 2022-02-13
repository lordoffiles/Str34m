using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 1f;

    [SerializeField]
    float jumpImpulse = 1f;

    [SerializeField]
    Rigidbody2D rb;

    private bool canJump = false;
    // Start is called before the first frame update
    void Start()
    {
        canJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        rb.AddForce(Vector3.right * horizontalInput * moveSpeed);

        if (Input.GetAxis("Jump") > 0)
        {
            canJump = true;
        }

    }

    private void FixedUpdate()
    {
        if(canJump)
        {
            rb.AddForce(new Vector2(0f,jumpImpulse/100), ForceMode2D.Impulse);
            canJump = false;
        }
    }
}