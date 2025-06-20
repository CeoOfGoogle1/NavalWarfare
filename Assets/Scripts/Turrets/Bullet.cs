using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool isOnlyVisual = false;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime = 5f;
    private float lifetimeTimer;

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;

        lifetimeTimer += Time.deltaTime;
        if (lifetimeTimer >= lifetime)
        {
            Sink();
        }
    }

    private void Sink()
    {
        Destroy(gameObject);
        // Optionally, you can add a sinking animation or effect here
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Unit")) return;

        Destroy(gameObject);

        if (isOnlyVisual) return;

        //Deal Damage
        Debug.Log("DEALING DAMAGE");
    }

    public void SetOnlyVisual(bool value)
    {
        isOnlyVisual = value;
    }
}
