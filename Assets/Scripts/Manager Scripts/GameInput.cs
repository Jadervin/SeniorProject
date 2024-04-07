using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    private PlayerInputActions playerInputActions;

    public event EventHandler OnJumpPressed;
    public event EventHandler OnJumpRelease;
    public event EventHandler OnShootPressed;
    public event EventHandler OnTongueCounterPressed;
    public event EventHandler OnSpecialShootPressed;
    public event EventHandler OnSpecialShootRelease;
    public event EventHandler OnSpecialWeaponSwitch;
    public event EventHandler OnPausePressed;
    public event EventHandler OnMapPressed;


    public enum Bindings
    {
        Move_Left,
        Move_Right,
        Jump,
        Shoot,
        Special_Shoot,
        Tongue_Counter,
        SWSwitch_Left,
        SWSwitch_Right,
        Pause,
        Map,
        Gamepad_Jump,
        Gamepad_Shoot,
        Gamepad_Special_Shoot,
        Gamepad_Tongue_Counter,
        Gamepad_SWSwitch_Left,
        Gamepad_SWSwitch_Right,
        Gamepad_Pause,
        Gamepad_Map
    }

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }


        playerInputActions.Player.Enable();

        playerInputActions.Player.Jump.started += Jump_started;

        playerInputActions.Player.Jump.canceled += Jump_canceled;

        playerInputActions.Player.Shoot.performed += Shoot_performed;

        playerInputActions.Player.TongueCounter.performed += TongueCounter_performed;

        playerInputActions.Player.SpecialShoot.performed += SpecialShoot_performed;

        playerInputActions.Player.SpecialShoot.canceled += SpecialShoot_canceled;
        playerInputActions.Player.SpecialWeaponSwitching.started += SpecialWeaponSwitching_started;

        playerInputActions.Player.Pause.performed += Pause_performed;
        playerInputActions.Player.Map.performed += Map_performed;

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
        playerInputActions.Player.Pause.performed -= Pause_performed;
        playerInputActions.Player.Map.performed -= Map_performed;

        playerInputActions.Dispose();
    }


    private void Map_performed(InputAction.CallbackContext obj)
    {
        if (GameSceneManager.Instance.GetGameState() == GameStates.GamePlaying || GameSceneManager.Instance.GetGameState() == GameStates.MapOpen
            /*GameSceneManager.Instance.GetGameState() != GameStates.GameOver && GameSceneManager.Instance.GetGameState() != GameStates.Paused && GameSceneManager.Instance.GetGameState() != GameStates.Rebinding && GameSceneManager.Instance.GetGameState() != GameStates.Saving*/)
        {
            OnMapPressed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        if(GameSceneManager.Instance.GetGameState() == GameStates.GamePlaying || GameSceneManager.Instance.GetGameState() == GameStates.Paused
            /*GameSceneManager.Instance.GetGameState() != GameStates.GameOver && GameSceneManager.Instance.GetGameState() != GameStates.MapOpen && GameSceneManager.Instance.GetGameState() != GameStates.Rebinding && GameSceneManager.Instance.GetGameState() != GameStates.Saving*/)
        {
            OnPausePressed?.Invoke(this, EventArgs.Empty);
        }
    }


    private void TongueCounter_performed(InputAction.CallbackContext obj)
    {
        if (MovementLimiter.instance.characterCanMove && GameSceneManager.Instance.GetGameState() == GameStates.GamePlaying)
        {
            OnTongueCounterPressed?.Invoke(this, EventArgs.Empty);
        }
    }
    private void Shoot_performed(InputAction.CallbackContext obj)
    {
        if (MovementLimiter.instance.characterCanMove && MovementLimiter.instance.characterCanBasicShoot && GameSceneManager.Instance.GetGameState() == GameStates.GamePlaying)
        {
            OnShootPressed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SpecialShoot_performed(InputAction.CallbackContext obj)
    {
        if (MovementLimiter.instance.characterCanMove && MovementLimiter.instance.characterCanSpecialShoot && GameSceneManager.Instance.GetGameState() == GameStates.GamePlaying)
        {
            OnSpecialShootPressed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SpecialShoot_canceled(InputAction.CallbackContext obj)
    {

        if (MovementLimiter.instance.characterCanMove && MovementLimiter.instance.characterCanSpecialShoot && GameSceneManager.Instance.GetGameState() == GameStates.GamePlaying)
        {
            OnSpecialShootRelease?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SpecialWeaponSwitching_started(InputAction.CallbackContext obj)
    {
        if (GameSceneManager.Instance.GetGameState() == GameStates.GamePlaying)
        {
            OnSpecialWeaponSwitch?.Invoke(this, EventArgs.Empty);
        }
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

    public float GetSpecialWeaponSwitchDirection()
    {
        float specialWeaponSwitchDirection = playerInputActions.Player.SpecialWeaponSwitching.ReadValue<float>();

        return specialWeaponSwitchDirection;
    }

    public string GetBindingText(Bindings binding)
    {
        switch(binding)
        {
            default:
            case Bindings.Move_Left:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
                
            case Bindings.Move_Right:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
                
            case Bindings.Jump:
                return playerInputActions.Player.Jump.bindings[0].ToDisplayString();

            case Bindings.Shoot:
                return playerInputActions.Player.Shoot.bindings[0].ToDisplayString();

            case Bindings.Special_Shoot:
                return playerInputActions.Player.SpecialShoot.bindings[0].ToDisplayString();

            case Bindings.Tongue_Counter:
                return playerInputActions.Player.TongueCounter.bindings[0].ToDisplayString();

            case Bindings.SWSwitch_Left:
                return playerInputActions.Player.SpecialWeaponSwitching.bindings[1].ToDisplayString();

            case Bindings.SWSwitch_Right:
                return playerInputActions.Player.SpecialWeaponSwitching.bindings[2].ToDisplayString();

            case Bindings.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();

            case Bindings.Map:
                return playerInputActions.Player.Map.bindings[0].ToDisplayString();

            case Bindings.Gamepad_Jump:
                return playerInputActions.Player.Jump.bindings[1].ToDisplayString();

            case Bindings.Gamepad_Shoot:
                return playerInputActions.Player.Shoot.bindings[1].ToDisplayString();

            case Bindings.Gamepad_Special_Shoot:
                return playerInputActions.Player.SpecialShoot.bindings[1].ToDisplayString();

            case Bindings.Gamepad_Tongue_Counter:
                return playerInputActions.Player.TongueCounter.bindings[1].ToDisplayString();

            case Bindings.Gamepad_SWSwitch_Left:
                return playerInputActions.Player.SpecialWeaponSwitching.bindings[4].ToDisplayString();

            case Bindings.Gamepad_SWSwitch_Right:
                return playerInputActions.Player.SpecialWeaponSwitching.bindings[5].ToDisplayString();

            case Bindings.Gamepad_Pause:
                return playerInputActions.Player.Pause.bindings[1].ToDisplayString();

            case Bindings.Gamepad_Map:
                return playerInputActions.Player.Map.bindings[1].ToDisplayString();

            
                
        }
    }



    public void RebindBinding(Bindings binding, Action onActionRebound)
    {
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Bindings.Move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;

            case Bindings.Move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;

            case Bindings.Jump:
                inputAction = playerInputActions.Player.Jump;
                bindingIndex = 0;
                break;

            case Bindings.Shoot:
                inputAction = playerInputActions.Player.Shoot;
                bindingIndex = 0;
                break;

            case Bindings.Special_Shoot:
                inputAction = playerInputActions.Player.SpecialShoot;
                bindingIndex = 0;
                break;

            case Bindings.Tongue_Counter:
                inputAction = playerInputActions.Player.TongueCounter;
                bindingIndex = 0;
                break;

            case Bindings.SWSwitch_Left:
                inputAction = playerInputActions.Player.SpecialWeaponSwitching;
                bindingIndex = 1;
                break;

            case Bindings.SWSwitch_Right:
                inputAction = playerInputActions.Player.SpecialWeaponSwitching;
                bindingIndex = 2;
                break;

            case Bindings.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;

            case Bindings.Map:
                inputAction = playerInputActions.Player.Map;
                bindingIndex = 0;
                break;

            case Bindings.Gamepad_Jump:
                inputAction = playerInputActions.Player.Jump;
                bindingIndex = 1;
                break;

            case Bindings.Gamepad_Shoot:
                inputAction = playerInputActions.Player.Shoot;
                bindingIndex = 1;
                break;

            case Bindings.Gamepad_Special_Shoot:
                inputAction = playerInputActions.Player.SpecialShoot;
                bindingIndex = 1;
                break;

            case Bindings.Gamepad_Tongue_Counter:
                inputAction = playerInputActions.Player.TongueCounter;
                bindingIndex = 1;
                break;

            case Bindings.Gamepad_SWSwitch_Left:
                inputAction = playerInputActions.Player.SpecialWeaponSwitching;
                bindingIndex = 4;
                break;

            case Bindings.Gamepad_SWSwitch_Right:
                inputAction = playerInputActions.Player.SpecialWeaponSwitching;
                bindingIndex = 5;
                break;

            case Bindings.Gamepad_Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 1;
                break;

            case Bindings.Gamepad_Map:
                inputAction = playerInputActions.Player.Map;
                bindingIndex = 1;
                break;

        }


        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback =>
        {
            callback.Dispose();
            playerInputActions.Player.Enable();
            onActionRebound();
            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
        }).Start();
    }


    public PlayerInputActions GetPlayerInputActions()
    {
        return playerInputActions;
    }
}
