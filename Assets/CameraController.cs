using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera camera; // Target to follow
    [SerializeField] float speed = 6f; // Speed of camera movement

    private Vector2 inputVector; // Input vector for camera movement

    public void Move()
    {

    }
    
    void Update()
    {
        // Check if the camera is assigned
        if (camera == null)
        {
            Debug.LogError("Camera is not assigned in the CameraController.");
            return;
        }

        // Calculate the new position based on input
        Vector3 movement = new Vector3(inputVector.x, 0, inputVector.y) * speed * Time.deltaTime;

        // Move the camera
        camera.transform.position += movement;

        // Optionally, you can also make the camera look at a target or adjust its rotation here
    }

}
