using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private Vector2 movementVector = new Vector2(0, 0);
    private float scaleVector = 0;

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

    public void Move(InputAction.CallbackContext context)
    {
        movementVector = context.ReadValue<Vector2>();
    }

    public void Scale(InputAction.CallbackContext context)
    {
        scaleVector = context.ReadValue<float>();
    }

    public Vector2 GetMovementVector()
    {
        return movementVector;
    }
    
    public float GetScaleVector()
    {
        return scaleVector;
    }
}
