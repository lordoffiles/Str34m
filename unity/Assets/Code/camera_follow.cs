using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_follow : MonoBehaviour
{

    [SerializeField]
    GameObject player;

    [SerializeField]
    float motionSmoothing = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 bPosition = player.transform.position;
        bPosition.z = 0;
        transform.position = Vector3.Lerp(new Vector3(transform.position.x, transform.position.y, transform.position.z) , new Vector3(bPosition.x, bPosition.y, transform.position.z), motionSmoothing);
    }
}
