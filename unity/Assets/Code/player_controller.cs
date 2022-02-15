using UnityEngine;

public class player_controller : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 10f;

    [SerializeField]
    float jumpAmount = 15f;

    [SerializeField]
    float jumpHeight = 5f;

    [SerializeField]
    float gravityScale = 3.5f;

    [SerializeField]
    float fallingGravityScale = 5f;

    [SerializeField]
    Rigidbody2D rb;

    private bool canJetpack = false;
    
    // Start is called before the first frame update
    void Start()
    {
        canJetpack = true;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        rb.AddForce(Vector3.right * horizontalInput * moveSpeed / 100);

        if (canJetpack == false && Input.GetAxis("Jump") > 0)
        {
            rb.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
            // rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            
        }
        if (rb.velocity.y >= 0)
        {
            rb.gravityScale = gravityScale;
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallingGravityScale;
        }

    }

    void resetCanJump()
    {
        // canJetpack = true;
    }

    private void FixedUpdate()
    {
        if (canJetpack)
        {
            rb.AddForce(new Vector2(0f, jumpImpulse / 100), ForceMode2D.Impulse);
            canJetpack = false;
        }
    }
}
