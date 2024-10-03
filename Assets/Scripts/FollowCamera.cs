using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] float cameraOffsetX = 10f;
    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.position = new Vector3 (player.transform.position.x + cameraOffsetX, player.transform.position.y, 0);
    }
}
