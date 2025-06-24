using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class Unit : NetworkBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject borderBox;
    [SerializeField] private GameObject[] turrets;

    [Header("Unit Settings")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float turnSpeed;
    
    [Header("Vision Settings")]
    [SerializeField] private float visionRadius;
    [SerializeField] private float cloudVisionRadius;

    private Vector2 destination;
    private Transform target;
    private float currentSpeed;
    private bool isDecelerating = false;
    private bool isSelected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isSelected = false;

        destination = Vector2.zero; // Initialize destination to the unit's current position
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        Draw();

        AimTurrets();
    }

    private void AimTurrets()
    {
        if (target == null) return;

        foreach (GameObject turret in turrets)
        {
            turret.GetComponent<TurretController>().SetTarget(target);
        }
    }

    void Move()
    {
        if (!IsServer) return;

        if (destination != Vector2.zero || isDecelerating)
            {
                Vector2 direction = destination - (Vector2)transform.position;

                if (!isDecelerating && direction.magnitude < 1f)
                {
                    isDecelerating = true;
                }

                if (isDecelerating)
                {
                    // Decelerate
                    currentSpeed -= acceleration * Time.deltaTime;
                    currentSpeed = Mathf.Max(currentSpeed, 0f);
                    transform.position += transform.up * currentSpeed * Time.deltaTime;

                    if (currentSpeed == 0f)
                    {
                        isDecelerating = false;
                        destination = Vector2.zero;
                    }
                }
                else
                {
                    // Accelerate
                    currentSpeed += acceleration * Time.deltaTime;
                    currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
                    transform.position += transform.up * currentSpeed * Time.deltaTime;

                    // Rotate towards the destination
                    float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                    Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                }
            }
    }

    private void Draw()
    {
        if (isSelected == true)
        {
            borderBox.SetActive(true);
        }
        else
        {
            borderBox.gameObject.SetActive(false);
        }
    }

    public void SetDestination(Vector2 newDestination)
    {
        SetDestinationServerRpc(newDestination);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetDestinationServerRpc(Vector2 newDestination)
    {
        destination = newDestination;
    }

    public void SetTarget(Transform target)
    {
        var netObj = target.GetComponent<NetworkObject>();
        if (netObj != null)
        {
            SetTargetServerRpc(netObj.NetworkObjectId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetTargetServerRpc(ulong targetNetworkObjectId)
    {
        NetworkObject targetNetObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[targetNetworkObjectId];
        if (targetNetObj != null)
        {
            target = targetNetObj.transform;

            SetTargetClientRpc(targetNetworkObjectId);
        }
    }

    [ClientRpc]
    public void SetTargetClientRpc(ulong targetNetworkObjectId)
    {
        NetworkObject targetNetObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[targetNetworkObjectId];
        if (targetNetObj != null)
        {
            target = targetNetObj.transform;
        }
    }

    public bool IsSelected
    {
        get => isSelected;
        set => isSelected = value;
    }

    public float VisionRadius
    {
        get => visionRadius;
        set => visionRadius = value;
    }
    
    public float CloudVisionRadius
    {
        get => cloudVisionRadius;
        set => cloudVisionRadius = value;
    }

    public float MaxSpeed
    {
        get => maxSpeed;
        set => maxSpeed = value;
    }

    public float TurnSpeed
    {
        get => turnSpeed;
        set => turnSpeed = value;
    }
}