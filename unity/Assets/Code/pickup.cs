using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{

    [SerializeField]
    public int amount = 50;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().AddTorque(Random.Range(-.1f, .1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
