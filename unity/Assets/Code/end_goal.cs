using UnityEngine;

public class end_goal : MonoBehaviour
{

    public Color GizmosColor = new Color(.5f, 10f, 0.5f, 0.2f);

    [SerializeField]
    game_master master;

    // Start is called before the first frame update
    void Start()
    {

    }
    void OnDrawGizmos()
    {
        Gizmos.color = GizmosColor;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Rigidbody2D rb = collision.attachedRigidbody;

            float dirX = 0;
            float dirY = 0;

            dirX = Random.Range(-rb.velocity.x, rb.velocity.x);
            dirY = Random.Range(.1f, Mathf.Abs(rb.velocity.y));

            Vector2 dir = new Vector2(dirX, dirY);

            rb.AddForce(collision.attachedRigidbody.velocity * -1 + dir, ForceMode2D.Impulse);

            return;
        }
        else
        {
            master.hasWon();
        }
    }
}
