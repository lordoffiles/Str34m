using UnityEngine;

public class spawn_pieces : MonoBehaviour
{

    [SerializeField]
    GameObject lane;

    [SerializeField]
    GameObject[] pieces;

    [SerializeField]
    float spawnTimer = 5f;

    [SerializeField]
    float rocksToSpawnAtStart = 10;

    [SerializeField]
    GameObject player;

    public Color GizmosColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);


    // Start is called before the first frame update
    void Start()
    {
        float distanceWithPlayer = transform.position.y - player.transform.position.y;

        float firstTier = Mathf.Abs(distanceWithPlayer / 3);
        float secondTier = 2 * firstTier;
        float thirdTier = transform.position.y;
        
        for (int i = 0; i < Mathf.Ceil(rocksToSpawnAtStart / 3); i++)
        {
            spawnInRange(player.transform.position.y, firstTier);
        }

        for (int i = 0; i < Mathf.Ceil(rocksToSpawnAtStart / 3); i++)
        {
            spawnInRange(firstTier, secondTier);
        }

        for (int i = 0; i < Mathf.Ceil(rocksToSpawnAtStart / 3); i++)
        {
            spawnInRange(secondTier, thirdTier);
        }

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

        Vector3 origin = transform.position;
        Vector3 range = transform.localScale / 2.0f;
        Vector3 randomRange = new Vector3(Random.Range(-range.x, range.x),
                                          0,
                                          0);

        Vector3 inst_position = origin + randomRange;

        instantiateAndPlace(inst_position);

    }

    private void spawnInRange(float minY, float maxY)
    {

        Vector3 range = transform.localScale / 2.0f;

        Vector3 position = new Vector3(
            Random.Range(-range.x, range.x),
            Random.Range(minY, maxY),
            0);

        instantiateAndPlace(position);

    }

    private void instantiateAndPlace(Vector3 pos)
    {
        GameObject piece = pieces[Random.Range(0, pieces.Length)];

        GameObject inst = Instantiate(piece);

        inst.transform.position = pos;
        inst.tag = "Rock";
    
    }
}
