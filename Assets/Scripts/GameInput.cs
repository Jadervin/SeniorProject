using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInputActions playerInputActions;

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


    private void Jump_performed(InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }


    public float GetXMovement()
    {
        float xInput
        = playerInputActions.Player.Move.ReadValue<float>();

        Debug.Log(xInput);

        return xInput;

    }







}
