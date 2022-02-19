using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rock_movement : MonoBehaviour
{
    bool playerIsNear = false;
    float velocityY = 0f;
    float velocityX = 0f;
    Rigidbody2D rb;
    float mass = 1f;
    float gravityScale = 1f;

    float movementY = 0;
    float movementX = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerIsNear = true;

            velocityY = rb.velocity.y;
            velocityX = rb.velocity.x;
            mass = rb.mass;
            gravityScale = rb.gravityScale;

            rb.bodyType = RigidbodyType2D.Static;

            collision.gameObject.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            playerIsNear = false;

            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = new Vector2(movementX, movementY);

            collision.transform.parent = null;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsNear)
        {
            // f = m*a; v = f * dT * gs;
            // j'assume que l'acceleration est 9.81m/s^2
            movementY = (velocityY) * Time.deltaTime;
            movementX = (velocityX) * Time.deltaTime; 

            transform.Translate(movementX, movementY, 0);
        }
    }
}
