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

    public event EventHandler OnJumpContextStarted;
    public event EventHandler OnJumpContextCanceled;
    //public event EventHandler OnJumpPerformed;



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


    private void Jump_performed(InputAction.CallbackContext context)
    {
        //Debug.Log("Suppose to jump");

        if (MovementLimiter.instance.characterCanMove)
        {

            //Debug.Log(context);
            Debug.Log("Jump " + context.phase);


            //When we press the jump button, tell the script that we desire a jump.
            //Also, use the started and canceled contexts to know if we're currently holding the button
            if (context.performed)
            {
                //Debug.Log("Calling StartJump Event");
                OnJumpContextStarted?.Invoke(this, EventArgs.Empty);
                
                //playerJump.StartedJump();
                
            }

            //else
            //{

            //    Debug.Log("Calling CancelJump Function");
            //    OnJumpContextCanceled?.Invoke(this, EventArgs.Empty);

            //    //playerJump.CanceledJump();

            //}

            if (context.canceled)
            {
                Debug.Log("Calling CancelJump Function");
                OnJumpContextCanceled?.Invoke(this, EventArgs.Empty);

                //playerJump.CanceledJump();

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
