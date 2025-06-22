using System;
using Unity.Netcode;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private float speed;
    private float maxRange;
    private float damage;
    private float penetration;
    private Vector2 startPosition;
    private bool onlyVisual;

    public static void SpawnProjectile(
        Vector2 spawnPosition,
        Vector2 direction,
        Sprite sprite,
        float speed,
        float maxRange,
        float damage,
        float penetration
    )
    {
        GameObject obj = new GameObject("Projectile");
        obj.transform.position = spawnPosition;
        obj.transform.up = direction;

        var proj = obj.AddComponent<ProjectileController>();
        proj.speed = speed;
        proj.maxRange = maxRange;
        proj.damage = damage;
        proj.penetration = penetration;
        proj.startPosition = spawnPosition;

        var sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        proj.spriteRenderer = sr;

        var col = obj.AddComponent<CircleCollider2D>();
        col.isTrigger = true;

        var rb = obj.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.linearVelocity = direction * speed;
    }


    void Update()
    {
        if (Vector2.Distance(startPosition, transform.position) >= maxRange)
        {
            Impact();
            return;
        }
    }

    private void Impact()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<NetworkBehaviour>().IsOwner)
        {
            Destroy(gameObject);
            return;
        }

        if (collision.TryGetComponent(out DamageController ship))
        {
            ship.ProjectileImpact(
                transform.position,
                damage,
                penetration
            );
        }

        Destroy(gameObject);
    }
    
    public void SetOnlyVisual(bool value)
    {
        onlyVisual = value;
    }
}
