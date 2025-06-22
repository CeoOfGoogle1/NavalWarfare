using Unity.Netcode;
using UnityEngine;

public class ObjectSpawnManager : NetworkBehaviour
{

    public static ObjectSpawnManager Instance;

    private void Awake()
    {

        DontDestroyOnLoad(gameObject);

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

    [ClientRpc]
    public void SpawnVisualProjectileClientRpc(
        bool onlyVisual,
        ulong newFiringShipNetworkId,
        Vector2 spawnPosition,
        Vector2 direction,
        string spriteName,
        float speed,
        float maxRange,
        float damage,
        float penetration
    )
    {
        if (Resources.Load<Sprite>("Textures/Projectiles/" + spriteName) == null)
        {
            Debug.LogError("Sprite not found: " + spriteName);
            return;
        }

        SpawnProjectile(
            onlyVisual,
            newFiringShipNetworkId,
            spawnPosition,
            direction,
            Resources.Load<Sprite>("Textures/Projectiles/" + spriteName),
            speed,
            maxRange,
            damage,
            penetration
        );
    }

    public static void SpawnProjectile(
        bool onlyVisual,
        ulong newFiringShipNetworkId,
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

        var sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        proj.spriteRenderer = sr;

        var col = obj.AddComponent<CircleCollider2D>();
        col.isTrigger = true;

        var rb = obj.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.linearVelocity = direction * speed;

        proj.SetParameters(
            onlyVisual,
            newFiringShipNetworkId,
            spawnPosition,
            direction,
            sprite,
            speed,
            maxRange,
            damage,
            penetration
        );

        if (!onlyVisual)
        {
            sr.enabled = false;
        }
    }
}
