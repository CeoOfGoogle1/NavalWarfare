using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class Unit : NetworkBehaviour
{

    [SerializeField] private GameObject borderBox;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float turnSpeed;
    [SerializeField] private Vector2 destination;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject[] turrets;
    [SerializeField] public float visionRadius;
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
            turret.GetComponent<Turret>().SetTarget(target);
        }
    }

    void Move()
    {
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

    public bool IsSelected
    {
        get { return isSelected; }
        set { isSelected = value; }
    }

    public float VisionRadius
    {
        get { return visionRadius; }
        set { visionRadius = value; }
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
}
