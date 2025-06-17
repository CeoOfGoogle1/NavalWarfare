using System;
using UnityEngine;
using UnityEngine.Animations;

public class Turret : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 60f; // Degrees per second
    Transform target;
    float initialVelocity = 10f; //what the fuck
    private Vector3 previousTargetPosition;
    private Vector3 targetVelocity;
    private Vector3 previousTurretPosition;
    private Vector3 turretVelocity;

    private void Start()
    {
        if (target == null)
            previousTargetPosition = target.position; // What the fuck does this code do????????
    }

    void Update()
    {
        Aim();
        Shoot();
    }

    private void Shoot()
    {

    }

    private void Aim()
    {
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
        //transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, angle - 90), Time.deltaTime * rotationSpeed); // Smooth rotation towards the target
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
