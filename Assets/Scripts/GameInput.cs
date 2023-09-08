using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    [SerializeField] private PlayerJump playerJump;

    private PlayerInputActions playerInputActions;

    public event EventHandler OnJumpContextStarted;
    public event EventHandler OnJumpContextCanceled;
    public event EventHandler OnJumpPerformed;

    public class InputSystemEventArgs : EventArgs
    {
        InputAction.CallbackContext obj;
    }


    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();

        playerInputActions.Player.Enable();

        playerInputActions.Player.Jump.performed += Jump_performed;
        


    }

    private void OnDestroy()
    {
        playerInputActions.Player.Jump.performed -= Jump_performed;

        playerInputActions.Dispose();
    }


    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        //OnJumpPerformed?.Invoke(this, new InputSystemEventArgs<EventArgs>
        //{
            

        //});


        Debug.Log("Suppose to jump");

        if (MovementLimiter.instance.characterCanMove)
        {
            Debug.Log("Player can move");


            //When we press the jump button, tell the script that we desire a jump.
            //Also, use the started and canceled contexts to know if we're currently holding the button
            if (context.started)
            {
                //OnJumpContextStarted?.Invoke(this, EventArgs.Empty);
                Debug.Log("Calling StartJump Function");
                playerJump.StartedJump();
                
            }

            if (context.canceled)
            {
                //OnJumpContextCanceled?.Invoke(this, EventArgs.Empty);
                Debug.Log("Calling CancelJump Function");
                playerJump.CanceledJump();
                
            }
        }
        
    }


    public float GetXMovement()
    {
        float xInput
        = playerInputActions.Player.Move.ReadValue<float>();

        //Debug.Log(xInput);

        return xInput;

    }







}
