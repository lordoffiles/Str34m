using UnityEngine;
using System.Collections;

public class camera_follow : MonoBehaviour
{

    [SerializeField]
    GameObject player;

    [SerializeField]
    float followLerpSpeed = 1f;

    private bool canFollow = true;

    [SerializeField]
    float cameraSizeStart = 15;
    float gameplaySize;
    [SerializeField]
    float sizeLerpSmoothing = 1;

    [SerializeField]
    int timeBeforeStart = 4;

    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        canFollow = false;

        transform.position = new Vector3(0, 0, -10);

        gameplaySize = cam.orthographicSize;

        cam.orthographicSize = cameraSizeStart;

        Invoke("startFollow", timeBeforeStart);

    }

    void startFollow()
    {
        canFollow = true;
    }

    public void setFollow(bool val)
    {
        canFollow = val;
    }

    private void Update()
    {
        if(canFollow)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, gameplaySize, Time.deltaTime * sizeLerpSmoothing);
        }
    }

    private void FixedUpdate()
    {
        if (canFollow) {
            
            Vector3 bPosition = player.transform.position;
            bPosition.z = 0;
            transform.position = Vector3.Lerp(
                new Vector3(transform.position.x, transform.position.y, transform.position.z), 
                new Vector3(bPosition.x, bPosition.y, transform.position.z), 
                Time.deltaTime * followLerpSpeed);

        }
    }
}
