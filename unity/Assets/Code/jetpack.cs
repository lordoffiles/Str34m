using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class jetpack : MonoBehaviour
{
    [SerializeField]
    public float maxFuel = 100;

    [SerializeField]
    float startingFuel = 100;

    [SerializeField]
    Image innerfuelBar;

    private float currentFuel = 0;

    public float CurrentFuel { get => currentFuel; set => currentFuel = value; }

    // Start is called before the first frame update
    void Start()
    {
        CurrentFuel = startingFuel;
    }

    // Update is called once per frame
    void Update()
    {
        innerfuelBar.transform.localScale = new Vector3(
            innerfuelBar.transform.localScale.x * currentFuel /100, 
            innerfuelBar.transform.localScale.y, 
            innerfuelBar.transform.localScale.z);

    }
}
