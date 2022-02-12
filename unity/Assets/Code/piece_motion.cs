using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class piece_motion : MonoBehaviour
{
    [SerializeField]
    int fallSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        enabled = false;
        GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0f, (float)-fallSpeed/100, 0f));
    }
}
