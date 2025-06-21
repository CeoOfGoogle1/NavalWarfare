using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Animations;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float turretCooldown = 5f;
    [SerializeField] private float rotationSpeed = 60f; // Degrees per second
    [SerializeField] Transform[] bulletSpawnPoints;

    Transform target;
    float initialVelocity = 10f; //what the fuck
    private Vector3 previousTargetPosition;
    private Vector3 targetVelocity;
    private Vector3 previousTurretPosition;
    private Vector3 turretVelocity;
    [SerializeField] private float shootTimer;


    void Update()
    {
        Aim();
        ShootTimer();
    }

    private void ShootTimer()
    {
        if (!NetworkManager.Singleton.IsServer) return;

        if (target == null) return;

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Debug.Log("Shooting Timer is up");
            shootTimer = turretCooldown;

            foreach (Transform spawnPoint in bulletSpawnPoints)
            {
                SpawnBullet(spawnPoint);
            }
        }
    }

    private void Aim()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position;
        Vector3 turretPosition = transform.position;
        targetVelocity = (targetPosition - previousTargetPosition) / Time.deltaTime;
        previousTargetPosition = targetPosition;
        turretVelocity = (turretPosition - previousTurretPosition) / Time.deltaTime;
        previousTurretPosition = turretPosition;

        Vector3 relativeVelocity = targetVelocity - turretVelocity;
        float distance = Vector3.Distance(turretPosition, targetPosition);

        float timeToTarget = distance / initialVelocity;
        Vector3 predictedPosition = targetPosition + relativeVelocity * timeToTarget;
        Vector2 aimDirection = (predictedPosition - turretPosition).normalized;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle - 90), Time.deltaTime * rotationSpeed); // Smooth rotation towards the target
    }

    private void SpawnBullet(Transform spawnPoint)
    {
        if (!NetworkManager.Singleton.IsServer) return;

        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, this.transform.rotation);
        bullet.GetComponent<Bullet>().SetOnlyVisual(false);
        bullet.GetComponent<Renderer>().enabled = false;

        GameObject visualBullet = Instantiate(bulletPrefab, spawnPoint.position, this.transform.rotation);
        bullet.GetComponent<Bullet>().SetOnlyVisual(true);

        ObjectSpawnManager.Instance.SpawnVisualBulletClientRpc(spawnPoint.position, this.transform.rotation);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
