using System;
using Unity.Netcode;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool isOnlyVisual = false;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime = 5f;
    private float lifetimeTimer;
    public float damage;
    public float penetration;

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;

        lifetimeTimer += Time.deltaTime;
        if (lifetimeTimer >= lifetime)
        {
            Impact();
        }
    }

    private void Impact()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet collided with: " + collision.name);
        if (!collision.CompareTag("Unit")) return;

        if (collision.GetComponent<NetworkBehaviour>().IsOwner) return;

        if (!isOnlyVisual)
        {
            // Deal damage to the unit
            Debug.Log("DEALING DAMAGE");
        }

        Destroy(gameObject);

    }

    public void SetOnlyVisual(bool value)
    {
        isOnlyVisual = value;
    }
}
