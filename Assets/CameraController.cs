using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float speed; 
    [SerializeField] float scaleSpeed;
    [SerializeField] float minScale;
    [SerializeField] float maxScale;

    public void Move()
    {
        this.transform.position += new Vector3(InputManager.Instance.GetMovementVector().x, InputManager.Instance.GetMovementVector().y, 0) * speed * Time.deltaTime;
    }

    public void Scale()
    {
        //this.GetComponent<Camera>().orthographicSize -= InputManager.Instance.GetScaleVector() * scaleSpeed * Time.deltaTime;
        //Mathf.Clamp(this.GetComponent<Camera>().orthographicSize, 1f, 20f);

        float scaleChange = InputManager.Instance.GetScaleVector() * scaleSpeed * Time.deltaTime;
        float newScale = Mathf.Clamp(GetComponent<Camera>().orthographicSize - scaleChange, minScale, maxScale);
        GetComponent<Camera>().orthographicSize = newScale;
    }

    void Update()
    {
        Move();
        Scale();
    }

}
