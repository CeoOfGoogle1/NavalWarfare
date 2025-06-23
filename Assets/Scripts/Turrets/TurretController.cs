using Unity.Netcode;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("Turret Settings")]
    [SerializeField] private float turretCooldown;
    [SerializeField] private int magazineSize;
    [SerializeField] private float reloadTime;
    [SerializeField] public float rotationSpeed; // Degrees per second
    [SerializeField] Transform[] bulletSpawnPoints;
    private float[] cooldownTimers;
    private float[] reloadTimers;
    private float[] currentMagazineSizes;
    private float[] staggerOffsets;
    [SerializeField] private float baseInaccuracy;
    [SerializeField] private float initialInaccuracy;
    [SerializeField] private float progressiveAccuracy;
    Transform target;
    private float currentInaccuracy;
    private Transform lastTarget;
    private Vector3 previousTargetPosition;
    private Vector3 targetVelocity;
    private Vector3 turretVelocity;
    private Vector3 previousTurretPosition;

    [Header("Bullet Settings")]
    public Sprite bulletSprite;
    public float speed;
    public float maxRange;
    public float damage;
    public float penetration;

    void Start()
    {
        cooldownTimers = new float[bulletSpawnPoints.Length];
        reloadTimers = new float[bulletSpawnPoints.Length];
        currentMagazineSizes = new float[bulletSpawnPoints.Length];
        for (int i = 0; i < bulletSpawnPoints.Length; i++)
        {
            cooldownTimers[i] = turretCooldown;
            reloadTimers[i] = reloadTime;
            currentMagazineSizes[i] = magazineSize;
        }

        currentInaccuracy = initialInaccuracy;
        lastTarget = null;
    }

    void Update()
    {
        Aim();
        ShootTimer();
    }

    private void ShootTimer()
    {
        if (!NetworkManager.Singleton.IsServer) return;
        if (target == null) return;

        for (int i = 0; i < bulletSpawnPoints.Length; i++)
        {
            if (currentMagazineSizes[i] <= 0)
            {
                reloadTimers[i] -= Time.deltaTime;
                if (reloadTimers[i] <= 0f)
                {
                    reloadTimers[i] = reloadTime;
                    currentMagazineSizes[i] = magazineSize; // Reload the magazine
                }
                continue; // Skip firing if reloading
            }

            cooldownTimers[i] -= Time.deltaTime;
            if (cooldownTimers[i] <= Random.value && currentMagazineSizes[i] > 0)
            {
                cooldownTimers[i] = turretCooldown;
                Fire(bulletSpawnPoints[i].position);
                currentMagazineSizes[i]--;

                if (currentMagazineSizes[i] <= 0)
                {
                    reloadTimers[i] = reloadTime;
                }
            }
        }
    }

    private void Aim()
    {
        if (target == null) return;

        if (lastTarget != target)
        {
            currentInaccuracy = initialInaccuracy;
            lastTarget = target;
        }
        else
        {
            currentInaccuracy -= progressiveAccuracy * Time.deltaTime;
            currentInaccuracy = Mathf.Max(currentInaccuracy, 0f);
        }

        Vector3 targetPosition = target.position;
        Vector3 turretPosition = transform.position;
        targetVelocity = (targetPosition - previousTargetPosition) / Time.deltaTime;
        previousTargetPosition = targetPosition;
        turretVelocity = (turretPosition - previousTurretPosition) / Time.deltaTime;
        previousTurretPosition = turretPosition;

        Vector3 relativeVelocity = targetVelocity;
        float distance = Vector3.Distance(turretPosition, targetPosition);

        float timeToTarget = distance / speed;
        Vector3 predictedPosition = targetPosition + relativeVelocity * timeToTarget;
        Vector2 aimDirection = (predictedPosition - turretPosition).normalized;

        float inaccuracyAngle = Random.Range(-currentInaccuracy, currentInaccuracy);
        Quaternion inaccuracyRotation = Quaternion.Euler(0, 0, inaccuracyAngle);
        aimDirection = inaccuracyRotation * aimDirection;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle - 90), Time.deltaTime * rotationSpeed); // Smooth rotation towards the target
    }

    private void Fire(Vector2 spawnPosition)
    {
        if (!NetworkManager.Singleton.IsServer) return;

        // Calculate inaccuracy
        float inaccuracyAngle = Random.Range(-baseInaccuracy, baseInaccuracy);
        Quaternion inaccuracyRotation = Quaternion.Euler(0, 0, inaccuracyAngle);
        Vector2 inaccuracyDirection = inaccuracyRotation * transform.up;

        ObjectSpawnManager.SpawnProjectile(
            false, // Only visual bullets are not used in this case
            this.gameObject.GetComponentInParent<NetworkObject>().NetworkObjectId,
            spawnPosition,
            inaccuracyDirection,
            bulletSprite,
            speed,
            maxRange,
            damage,
            penetration
        );

        ObjectSpawnManager.Instance.SpawnVisualProjectileClientRpc(
            true,
            this.gameObject.GetComponentInParent<NetworkObject>().NetworkObjectId,
            spawnPosition,
            inaccuracyDirection,
            "big_shell",
            speed,
            maxRange,
            damage,
            penetration
        );
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
