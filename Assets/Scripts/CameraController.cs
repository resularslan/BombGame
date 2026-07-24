using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player;
    private Camera camera;
    private float speed;
    private float min;
    private float max;
    private Vector3 newPosition;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        speed = player.GetComponent<PlayerMovement>().speed * 0.9f;
        camera = Camera.main;
        newPosition = camera.transform.position;
        float camWidth = camera.orthographicSize * 2 * camera.aspect;
        min = GridManager.Instance.bounds.xMin + (camWidth / 2);
        max = GridManager.Instance.bounds.xMax - (camWidth / 2);
    }
    void Update()
    {
        newPosition.x = player.transform.position.x;
        if (newPosition.x < min)
        {
            newPosition.x = min;
        }
        else if (newPosition.x > max)
        {
            newPosition.x = max;
        }
        camera.transform.position = Vector3.MoveTowards(camera.transform.position, newPosition, speed * Time.deltaTime);
    }
}
