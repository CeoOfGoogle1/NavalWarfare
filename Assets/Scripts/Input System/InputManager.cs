using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private Vector2 movementVector = new Vector2(0, 0);
    private float scaleVector = 0;
    private bool shiftPressed = false;
    

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

    public void ShiftClick(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            shiftPressed = true;
        }

        if(context.canceled)
        {
            shiftPressed = false;
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


    public bool IsShiftPressed()
    {
        return shiftPressed;
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
