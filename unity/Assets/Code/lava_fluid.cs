using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lava_fluid : MonoBehaviour
{
    [SerializeField]
    float springness = 0.02f;
    [SerializeField]
    float damping = 0.04f;
    [SerializeField]
    float spread = 0.05f;
    [SerializeField]
    float lineRendererWidth = .1f;
    [SerializeField]
    int edgeNumBias = 5;
    [SerializeField]
    float nodeMass = 1;
    [SerializeField]
    float particleLifeTimeBias = 0.07f;
    [SerializeField]
    float particleStartSpeedBias = 8;
    


    [SerializeField]
    GameObject splash;
    [SerializeField]
    Material mat;
    [SerializeField]
    GameObject waterMesh;
    
    const float z_offset = -1f;

    float baseHeight;
    float left;
    float bottom;



    float[] xs;
    float[] ys;
    float[] accels;
    float[] velos;
    LineRenderer lrFluidTop;

    GameObject[] meshObjects;
    GameObject[] colliders;

    Mesh[] meshes;

    public Color GizmosColor = new Color(10f, 0.5f, 0.5f, 0.2f);


    // Start is called before the first frame update
    void Start()
    {
        spawn(transform.position.x - transform.localScale.x/2,
            transform.position.x + transform.localScale.x,
            transform.position.y - transform.localScale.y/2,
            transform.position.y + transform.localScale.y);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = GizmosColor;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawn(float left, float width, float bottom, float height)
    {
        int edgesNeeded = Mathf.RoundToInt(width) * edgeNumBias;
        int nodeCount = edgesNeeded + 1;

        lrFluidTop = gameObject.AddComponent<LineRenderer>();
        lrFluidTop.material = mat;
        lrFluidTop.material.renderQueue = 1000;
        lrFluidTop.positionCount = nodeCount;
        lrFluidTop.startWidth = lineRendererWidth;
        lrFluidTop.endWidth = lineRendererWidth;

        xs = new float[nodeCount];
        ys = new float[nodeCount];
        velos = new float[nodeCount];
        accels = new float[nodeCount];

        meshObjects = new GameObject[edgesNeeded];
        meshes = new Mesh[edgesNeeded];
        colliders = new GameObject[edgesNeeded];

        baseHeight = height;
        this.bottom = bottom;
        this.left = left;



        // creer ligne pour le top de l'eau.
        for(int i = 0; i < nodeCount; i++)
        {
            ys[i] = height;
            xs[i] = left + width * i / edgesNeeded;
            accels[i] = 0;
            velos[i] = 0;
            lrFluidTop.SetPosition(i, new Vector3(xs[i], ys[i], z_offset));
        }

        // creer les mesh pour chaque poly qui relie 2 nodes au fond de l'eau.
        for( int i = 0; i < edgesNeeded; i++)
        {
            meshes[i] = new Mesh();
            Vector3[] verts = new Vector3[4];
            verts[0] = new Vector3(xs[i], ys[i], z_offset);
            verts[1] = new Vector3(xs[i + 1], ys[i + 1], z_offset);
            verts[2] = new Vector3(xs[i], this.bottom, z_offset);
            verts[3] = new Vector3(xs[i+1], this.bottom, z_offset);

            Vector2[] uvs = new Vector2[4];
            uvs[0] = new Vector2(0, 1);
            uvs[1] = new Vector2(1, 1);
            uvs[2] = new Vector2(0, 0);
            uvs[3] = new Vector2(1, 0);

            int[] tris = new int[6] { 0, 1, 3, 3, 2, 0 };

            meshes[i].vertices = verts;
            meshes[i].uv = uvs;
            meshes[i].triangles = tris;

            meshObjects[i] = Instantiate(waterMesh, Vector3.zero, Quaternion.identity) as GameObject;
            meshObjects[i].GetComponent<MeshFilter>().mesh = meshes[i];
            meshObjects[i].transform.parent = transform;

            colliders[i] = new GameObject();
            colliders[i].name = "water collision trigger " + i;
            colliders[i].tag = "water_collider";
            colliders[i].AddComponent<BoxCollider2D>();
            colliders[i].transform.parent = transform;
            // la position d'un collider est le point le plus a gauche + la largeur d'une seule de mesh 
            // * sa position dans la liste decallee de .5 le tout divise par le nombre de edges
            colliders[i].transform.position = new Vector3(left + width * (i + 0.5f) / edgesNeeded, height - 0.5f, 0);
            // pas besoin que ca soit tres haut. juste pour les objets qui entrent la premiere fois dans la lave.
            colliders[i].transform.localScale = new Vector3(width / edgesNeeded, 1, 1);
            colliders[i].GetComponent<BoxCollider2D>().isTrigger = true;
            colliders[i].AddComponent<WaterDetector>();

        }

        
    }

    // methode pour mettre a jour les meshes apres qu'on ait calcule les nouvelles
    // positions pour chacun des points
    void updateMeshes()
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            Vector3[] verts = new Vector3[4];
            verts[0] = new Vector3(xs[i], ys[i], z_offset);
            verts[1] = new Vector3(xs[i + 1], ys[i + 1], z_offset);
            verts[2] = new Vector3(xs[i], this.bottom, z_offset);
            verts[3] = new Vector3(xs[i + 1], this.bottom, z_offset);

            meshes[i].vertices = verts;
        }
    }

    void FixedUpdate()
    {
        // update chaque point du line renderer
        for (int i = 0; i < xs.Length; i++)
        {
            // loi de hooke pour les ressorts. la surface agit comme un ressort qui
            // bounce de haut en bas
            float force = springness * (ys[i] - baseHeight) + velos[i] * damping;
            accels[i] = -force / nodeMass;
            ys[i] += velos[i];
            velos[i] += accels[i];
            lrFluidTop.SetPosition(i, new Vector3(xs[i], ys[i], z_offset));
        }

        // arrays pour avoir la difference avec les nodes voisins 
        // afin de faire de la propagation.
        float[] leftDispl = new float[xs.Length];
        float[] rightDispl = new float[xs.Length];

        // batch pour performance
        for (int batch = 0; batch < 8; batch++)
        {
            // update les displacement pour chaque node
            for (int i = 0; i < xs.Length; i++)
            {
                // pas de displ a gauche pour le node le plus a gauche
                if (i > 0)
                {
                    // displace le node d'a cote
                    leftDispl[i] = spread * (ys[i] - ys[i - 1]);
                    velos[i - 1] += leftDispl[i];
                }
                // pas de displ pour le node le plus a droite
                if (i < xs.Length - 1)
                {
                    rightDispl[i] = spread * (ys[i] - ys[i + 1]);
                    velos[i + 1] += rightDispl[i];
                }

            }

            // update tous les nodes apres avoir calcule le displ
            for (int i = 0; i < xs.Length; i++)
            {
                if (i > 0)
                {
                    ys[i - 1] += leftDispl[i];
                }
                if (i < xs.Length - 1)
                {
                    ys[i + 1] += rightDispl[i];
                }
            }
        }
        updateMeshes();
    }

    // x est le point ou on veut verifier si et ou faire un splash
    public void Splash(float x, float velo)
    {
        if (velo == 0)
            return;
        if (x >= xs[0] && x <= xs[xs.Length - 1])
        {
            x -= xs[0];
            int index = Mathf.RoundToInt((xs.Length - 1) * (x / (xs[xs.Length - 1] - xs[0])));
            velos[index] += velo;

            float absVelo = Mathf.Abs(velo);

            float lifetime = 0.93f + absVelo * particleLifeTimeBias;
            splash.GetComponent<ParticleSystem>().startSpeed = particleStartSpeedBias + 2 * Mathf.Pow(absVelo, 0.5f);
            splash.GetComponent<ParticleSystem>().startSpeed = particleStartSpeedBias + 1 + 2 * Mathf.Pow(absVelo, 0.5f);
            splash.GetComponent<ParticleSystem>().startLifetime = lifetime;

            // un peu sous la surface et -5 en z pour que ca soit en avant de tout.
            Vector3 position = new Vector3(xs[index], ys[index] - 0.35f, -5);
            Quaternion rotation = Quaternion.LookRotation(new Vector3(xs[Mathf.FloorToInt(xs.Length / 2)], baseHeight + 8, 5) - position);


            GameObject go = Instantiate(splash, position, rotation) as GameObject;
            Destroy(go, lifetime + 0.3f);


        }
    }
}
