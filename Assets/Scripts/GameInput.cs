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
    public event EventHandler OnShootPressed;
    public event EventHandler OnTongueCounterPressed;
    public event EventHandler OnSpecialShootPressed;
    public event EventHandler OnSpecialShootRelease;
    public event EventHandler OnSpecialWeaponSwitch;


    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();

        playerInputActions.Player.Enable();

        playerInputActions.Player.Jump.started += Jump_started;

        playerInputActions.Player.Jump.canceled += Jump_canceled;

        playerInputActions.Player.Shoot.performed += Shoot_performed;

        playerInputActions.Player.TongueCounter.performed += TongueCounter_performed;

        playerInputActions.Player.SpecialShoot.performed += SpecialShoot_performed;

        playerInputActions.Player.SpecialShoot.canceled += SpecialShoot_canceled;
        playerInputActions.Player.SpecialWeaponSwitching.started += SpecialWeaponSwitching_started;

    }

    

    private void OnDestroy()
    {
        playerInputActions.Player.Jump.started -= Jump_started;

        playerInputActions.Player.Jump.canceled -= Jump_canceled;

        playerInputActions.Player.Shoot.performed -= Shoot_performed;

        playerInputActions.Player.TongueCounter.performed -= TongueCounter_performed;
        playerInputActions.Player.SpecialShoot.performed -= SpecialShoot_performed;
        playerInputActions.Player.SpecialShoot.canceled -= SpecialShoot_canceled;
        playerInputActions.Player.SpecialWeaponSwitching.started -= SpecialWeaponSwitching_started;

        playerInputActions.Dispose();
    }

    

    private void TongueCounter_performed(InputAction.CallbackContext obj)
    {
        if (MovementLimiter.instance.characterCanMove)
        {
            OnTongueCounterPressed?.Invoke(this, EventArgs.Empty);
        }
    }
    private void Shoot_performed(InputAction.CallbackContext obj)
    {
        if (MovementLimiter.instance.characterCanMove && MovementLimiter.instance.characterCanBasicShoot)
        {
            OnShootPressed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SpecialShoot_performed(InputAction.CallbackContext obj)
    {
        if (MovementLimiter.instance.characterCanMove && MovementLimiter.instance.characterCanSpecialShoot)
        {
            OnSpecialShootPressed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SpecialShoot_canceled(InputAction.CallbackContext obj)
    {

        if (MovementLimiter.instance.characterCanMove && MovementLimiter.instance.characterCanSpecialShoot)
        {
            OnSpecialShootRelease?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SpecialWeaponSwitching_started(InputAction.CallbackContext obj)
    {
        OnSpecialWeaponSwitch?.Invoke(this, EventArgs.Empty);
        Debug.Log("Switch. Now Read Value: " + playerInputActions.Player.SpecialWeaponSwitching.ReadValue<float>());
    }


    //When we press the jump button, tell the script that we desire a jump.
    //Also, use the started and canceled contexts to know if we're currently holding the button
    private void Jump_started(InputAction.CallbackContext context)
    {
        if (MovementLimiter.instance.characterCanMove)
        {
            //When the player holds the button, invoke event
            OnJumpPressed?.Invoke(this, EventArgs.Empty);

        }
    }


    private void Jump_canceled(InputAction.CallbackContext context)
    {
        if (MovementLimiter.instance.characterCanMove)
        {
            //When the player stops holding the button, invoke event
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
