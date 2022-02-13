using UnityEngine;

public class game_master : MonoBehaviour
{
    [SerializeField]
    Camera main_camera;
    [SerializeField]
    GameObject winScreen;
    [SerializeField]
    GameObject loseScreen;
    public void hasWon()
    {
        winScreen.SetActive(true);
    }

    public void hasLost()
    {
        main_camera.GetComponent<camera_follow>().setFollow(false);
        loseScreen.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
