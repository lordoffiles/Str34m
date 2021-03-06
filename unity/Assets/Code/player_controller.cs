using UnityEngine;

public class player_controller : MonoBehaviour
{
    [SerializeField]
    public bool canDie = true;
    [SerializeField]
    float moveSpeed = 10f;

    [SerializeField]
    float maxVelocity = 15;

    private float moveDirection = 1;

    [SerializeField]
    float jumpHeight = 5f;
    [SerializeField]
    float gravityScale = 3.5f;
    [SerializeField]
    float fallingGravityScale = 5f;
    
    

    [SerializeField]
    Rigidbody2D rb;

    jetpack jp;
    [SerializeField]
    float fuelUsagePerFrame = .5f;
    [SerializeField]
    float jetpackVerticalForce = 1;
    [SerializeField]
    GameObject fireEffectLeft;
    [SerializeField]
    GameObject fireEffectRight;

    private bool jumpUsed = false;

    private bool isLookingRight = true;

    private bool hasRockParent = false;

    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        jp = GetComponent<jetpack>();
        rb.freezeRotation = true;

        rb.gravityScale = gravityScale;

        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = Input.GetAxis("Horizontal");

        if (moveDirection > 0 || moveDirection < 0) {
            isLookingRight = (moveDirection > 0);
        }

        // tourner le bonhomme du bon bord quand il bouge.
        if(moveDirection != 0)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = !isLookingRight;

        }

        // sauter
        if (Input.GetAxis("Jump") != 0 && !jumpUsed)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            jumpUsed = true;
            source.Play();
        }

        // jetpack
        if (Input.GetAxis("Fire1") != 0 && jp.CurrentFuel - fuelUsagePerFrame > 0)
        {
            // pour permettre de combattre la gravite plus forte 
            if (rb.velocity.y < 0)
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jetpackVerticalForce * 2);
            else
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jetpackVerticalForce);

            if (isLookingRight)
            {
                fireEffectLeft.SetActive(true);
                fireEffectRight.SetActive(false);
            } else
            {
                fireEffectRight.SetActive(true);
                fireEffectLeft.SetActive(false);
            }

            jp.CurrentFuel = jp.CurrentFuel - fuelUsagePerFrame;

            
        } else
        {
            fireEffectRight.SetActive(false);
            fireEffectLeft.SetActive(false);
        }

        if (rb.velocity.y >= 0)
        {
            rb.gravityScale = gravityScale;
        } else
        {
            rb.gravityScale = fallingGravityScale;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "pickup")
        {
            jp.addFuel(collision.gameObject.GetComponent<pickup>().amount);
            Destroy(collision.gameObject);
            //play sound collect
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Rock")
        {
            jumpUsed = false;
        }
    }

    private void FixedUpdate()
    {   
        Vector2 velo = new Vector2(
            moveDirection * moveSpeed,
            rb.velocity.y);
        rb.velocity = Vector2.ClampMagnitude(velo, maxVelocity);
    }
}
