using System;
using Unity.Netcode;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private ulong firingShipNetworkId;
    public SpriteRenderer spriteRenderer;
    private float speed;
    private float maxRange;
    private float damage;
    private float penetration;
    private Vector2 startPosition;
    private bool onlyVisual;


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
        if (!collision.CompareTag("Unit")) return;

        if (collision.gameObject.TryGetComponent<NetworkObject>(out NetworkObject networkObject))
        {
            if (networkObject.NetworkObjectId == firingShipNetworkId)
            {
                return; // Ignore collisions with the firing ship
            }
        }

        if (onlyVisual)
        {
            Impact();
        }
        else if (collision.TryGetComponent(out DamageController damageController))
        {
            /*damageController.ProjectileImpact(
                transform.position,
                damage,
                penetration
            );*/

            Impact();
        }
    }

    public void SetOnlyVisual(bool value)
    {
        onlyVisual = value;
    }

    public void SetParameters(
        bool onlyVisual,
        ulong firingShipNetworkId,
        Vector2 spawnPosition,
        Vector2 direction,
        Sprite sprite,
        float speed,
        float maxRange,
        float damage,
        float penetration
    )
    {
        transform.position = spawnPosition;
        transform.up = direction;
        spriteRenderer.sprite = sprite;

        this.onlyVisual = onlyVisual;
        this.firingShipNetworkId = firingShipNetworkId;
        this.startPosition = spawnPosition;
        this.speed = speed;
        this.maxRange = maxRange;
        this.damage = damage;
        this.penetration = penetration;
    }
}
