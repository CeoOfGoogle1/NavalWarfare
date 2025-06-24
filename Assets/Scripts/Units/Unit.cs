using Unity.Netcode;
using UnityEngine;

public class Unit : NetworkBehaviour
{

    [SerializeField] private GameObject borderBox;
    [SerializeField] public float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] public float turnSpeed;
    [SerializeField] private bool isPlane;
    [SerializeField] private Vector2 destination;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject[] turrets;
    [SerializeField] public float visionRadius;
    [SerializeField] public float cloudVisionRadius;
    private float currentSpeed;
    private bool isDecelerating = false;
    private bool isSelected;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isSelected = false;

        destination = Vector2.zero; // Initialize destination to the unit's current position

        rb = GetComponent<Rigidbody2D>();
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
        if (destination != Vector2.zero || isDecelerating)
        {
            Vector2 position2d = rb.position;
            Vector2 direction = destination - position2d;

            if (!isDecelerating && direction.magnitude < 1f && !isPlane)
            {
                isDecelerating = true;
            }

            if (isDecelerating)
            {
                // Decelerate
                currentSpeed -= acceleration * Time.deltaTime;
                currentSpeed = Mathf.Max(currentSpeed, 0f);
                rb.linearVelocity = transform.up * currentSpeed;

                if (currentSpeed == 0f)
                {
                    isDecelerating = false;
                    destination = Vector2.zero;
                    rb.linearVelocity = Vector2.zero; // Stop the unit when it reaches the destination
                }
            }
            else
            {
                // Accelerate
                currentSpeed += acceleration * Time.deltaTime;
                currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
                rb.linearVelocity = transform.up * currentSpeed;

                // Rotate towards the destination
                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, turnSpeed * Time.deltaTime);
                rb.MoveRotation(angle);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
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
