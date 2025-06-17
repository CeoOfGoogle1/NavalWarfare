using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }
}
