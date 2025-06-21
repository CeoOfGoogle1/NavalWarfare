using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;
    public float penetration;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            
        }
    }
}
