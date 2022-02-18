using UnityEngine;

public class WaterDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if ( collision.gameObject.GetComponent<player_controller>().canDie == true)
            {
                GetComponentInParent<game_master>().hasLost();

                Destroy(collision.gameObject, 1.0f);
            }
        }
        
        if (collision.attachedRigidbody != null &&
            collision.tag != "water_collider" &&
            Mathf.Abs(collision.attachedRigidbody.velocity.y) > 10)
        {
            transform.parent.GetComponent<lava_fluid>().Splash(collision.transform.position.x, collision.attachedRigidbody.velocity.y * collision.attachedRigidbody.mass / 40f);
            //Destroy(collision.gameObject, 0.1f);
        }

        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
