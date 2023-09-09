using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    //[SerializeField] private PlayerJump playerJump;

    private PlayerInputActions playerInputActions;

    public event EventHandler OnJumpPressed;
    public event EventHandler OnJumpRelease;
    



    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();

        playerInputActions.Player.Enable();

        playerInputActions.Player.Jump.started += Jump_started;

        playerInputActions.Player.Jump.canceled += Jump_canceled;




    }

    private void OnDestroy()
    {
        playerInputActions.Player.Jump.started -= Jump_started;

        playerInputActions.Player.Jump.canceled -= Jump_canceled;

        playerInputActions.Dispose();
    }

    //When we press the jump button, tell the script that we desire a jump.
    //Also, use the started and canceled contexts to know if we're currently holding the button

    private void Jump_started(InputAction.CallbackContext context)
    {
        if (MovementLimiter.instance.characterCanMove)
        {
            
            //Debug.Log("Jump " + context.phase);

            //When the player holds the button, invoke event
            //Debug.Log("Invoking JumpPress Event");
            OnJumpPressed?.Invoke(this, EventArgs.Empty);

        }
    }


    private void Jump_canceled(InputAction.CallbackContext context)
    {
        if (MovementLimiter.instance.characterCanMove)
        {
            
            //Debug.Log("Jump " + context.phase);


            //When the player stops holding the button, invoke event
            //Debug.Log("Invoking JumpRelease Event");
            OnJumpRelease?.Invoke(this, EventArgs.Empty);
        }

    }

    public float GetXMovement()
    {
        float xInput
        = playerInputActions.Player.Move.ReadValue<float>();

        return xInput;

    }


}
