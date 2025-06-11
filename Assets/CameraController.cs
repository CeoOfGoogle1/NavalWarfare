using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera cameraMain; // Target to follow
    [SerializeField] float speed = 5f; // Speed of camera movement
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            direction += Vector3.up;
        if (Input.GetKey(KeyCode.A))
            direction += Vector3.left;
        if (Input.GetKey(KeyCode.S))
            direction += Vector3.down;
        if (Input.GetKey(KeyCode.D))
            direction += Vector3.right;
        if (Input.GetKey(KeyCode.Q))
            cameraMain.orthographicSize += speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.E))
            cameraMain.orthographicSize -= speed * Time.deltaTime;

        cameraMain.orthographicSize = Mathf.Clamp(cameraMain.orthographicSize, 1f, 20f);

        cameraMain.transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}
