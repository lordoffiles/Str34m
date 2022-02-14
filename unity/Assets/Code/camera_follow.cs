using UnityEngine;

public class camera_follow : MonoBehaviour
{

    [SerializeField]
    GameObject player;

    [SerializeField]
    float motionSmoothing = 1f;

    private bool canFollow = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void setFollow(bool val)
    {
        canFollow = val;
    }

    private void FixedUpdate()
    {
        if (canFollow) {
            Vector3 bPosition = player.transform.position;
            bPosition.z = 0;
            transform.position = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(bPosition.x, bPosition.y, transform.position.z), motionSmoothing);

        }
    }
}
