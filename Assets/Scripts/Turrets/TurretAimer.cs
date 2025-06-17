using UnityEditor;
using UnityEngine;

public class TurretAimer : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform turret;
    [SerializeField] float initialVelocity;
    private Vector3 previousTargetPosition;
    private Vector3 targetVelocity;
    private Vector3 previousTurretPosition;
    private Vector3 turretVelocity;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject orb;

    private void Start()
    {
        if (target == null)
            previousTargetPosition = target.position;
        if (turret == null)
            previousTurretPosition = turret.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = target.position;
        Vector3 turretPosition = turret.position;
        targetVelocity = (targetPosition - previousTargetPosition) / Time.deltaTime;
        previousTargetPosition = targetPosition;
        turretVelocity = (turretPosition - previousTurretPosition) / Time.deltaTime;
        previousTurretPosition = turretPosition;

        Vector3 relativeVelocity = targetVelocity - turretVelocity;
        float distance = Vector3.Distance(turretPosition, targetPosition);

        float timeToTarget = distance / initialVelocity;
        Vector3 predictedPosition = targetPosition + relativeVelocity * timeToTarget;
        Vector2 aimDirection = (predictedPosition - turretPosition).normalized;

        orb.transform.position = predictedPosition;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        turret.rotation = Quaternion.Euler(0, 0, angle - 90);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = Instantiate(bulletPrefab, turret.position, turret.rotation);
        }
    }
}
