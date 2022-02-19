using UnityEngine;

public class spawn_pieces : MonoBehaviour
{

    [SerializeField]
    GameObject lane;

    [SerializeField]
    GameObject[] pieces;

    [SerializeField]
    float spawnTimer = 5f;

    public Color GizmosColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("spawn", 0, spawnTimer);
        gameObject.tag = "Rock";
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = GizmosColor;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }

    private void spawn()
    {

        GameObject piece = pieces[Random.Range(0, pieces.Length)];

        GameObject inst = Instantiate(piece);

        Vector3 origin = transform.position;
        Vector3 range = transform.localScale / 2.0f;
        Vector3 randomRange = new Vector3(Random.Range(-range.x, range.x),
                                          origin.y,
                                          0);

        Vector3 inst_position = origin + randomRange;

        inst.transform.position = inst_position;

    }
}
