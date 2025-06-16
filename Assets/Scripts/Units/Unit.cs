using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class Unit : NetworkBehaviour
{
    
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float turnSpeed;
    [SerializeField] private Vector2 destination;
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
            Transform child = transform.GetChild(0);
            child.gameObject.SetActive(true);

        }
        else
        {
            Transform child = transform.GetChild(0);
            child.gameObject.SetActive(false);
        }
    }

    public bool IsSelected
    {
        get { return isSelected; }
        set { isSelected = value; }
    }

    public void SetDestination(Vector2 newDestination)
    {
        Debug.Log($"Setting destination for {gameObject.name} to {newDestination}");
        SetDestinationServerRpc(newDestination);
    }

    [ServerRpc]
    private void SetDestinationServerRpc(Vector2 newDestination)
    {
        Debug.Log($"SERVER: Setting destination for {gameObject.name} to {newDestination}");
        destination = newDestination;
    }
}
