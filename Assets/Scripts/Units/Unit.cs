using UnityEngine;
using UnityEngine.UIElements;

public class Unit : MonoBehaviour
{
    [SerializeField] public bool isSelected;
    [SerializeField] public float maxSpeed;
    [SerializeField] public float acceleration;
    [SerializeField] public float turnSpeed;
    [SerializeField] public Vector2 destination;
    private float currentSpeed;
    private bool isDecelerating = false;

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
}
