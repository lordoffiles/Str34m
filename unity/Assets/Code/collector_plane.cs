using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collector_plane : MonoBehaviour
{

    [SerializeField]
    float riseSpeed = 10f;
    [SerializeField]
    int timeBeforeStart = 5;

    private bool holdUpdate = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("wait");
        holdUpdate = false;
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(timeBeforeStart);
    }

    // Update is called once per frame
    void Update()
    {
        if (!holdUpdate)
        {
            transform.Translate(0, riseSpeed * Time.deltaTime, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Rock")
        {
            Destroy(collision.gameObject);
        }
    }

}
