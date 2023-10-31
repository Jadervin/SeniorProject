using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameInput;

public class OptionsUI : MonoBehaviour
{

    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button closeButton;
    [SerializeField] private Transform pressToRebindKeyTransform;


    [Header("Key Binding Buttons")]
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button shootButton;
    [SerializeField] private Button specialShootButton;
    [SerializeField] private Button tongueCounterButton;
    [SerializeField] private Button sWSwitchLeftButton;
    [SerializeField] private Button sWSwitchRightButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button mapButton;

    [SerializeField] private Button gamepadJumpButton;
    [SerializeField] private Button gamepadShootButton;
    [SerializeField] private Button gamepadSpecialShootButton;
    [SerializeField] private Button gamepadTongueCounterButton;
    [SerializeField] private Button gamepadSWSwitchLeftButton;
    [SerializeField] private Button gamepadSWSwitchRightButton;
    [SerializeField] private Button gamepadPauseButton;
    [SerializeField] private Button gamepadMapButton;


    [Header("Key Binding Button Texts")]
    [SerializeField] private TextMeshProUGUI moveLeftButtonText;
    [SerializeField] private TextMeshProUGUI moveRightButtonText;
    [SerializeField] private TextMeshProUGUI jumpButtonText;
    [SerializeField] private TextMeshProUGUI shootButtonText;
    [SerializeField] private TextMeshProUGUI specialShootButtonText;
    [SerializeField] private TextMeshProUGUI tongueCounterButtonText;
    [SerializeField] private TextMeshProUGUI sWSwitchLeftText;
    [SerializeField] private TextMeshProUGUI sWSwitchRightButtonText;
    [SerializeField] private TextMeshProUGUI pauseButtonText;
    [SerializeField] private TextMeshProUGUI mapButtonText;


    [SerializeField] private TextMeshProUGUI gamepadJumpButtonText;
    [SerializeField] private TextMeshProUGUI gamepadShootButtonText;
    [SerializeField] private TextMeshProUGUI gamepadSpecialShootButtonText;
    [SerializeField] private TextMeshProUGUI gamepadTongueCounterButtonText;
    [SerializeField] private TextMeshProUGUI gamepadSWSwitchLeftButtonText;
    [SerializeField] private TextMeshProUGUI gamepadSWSwitchRightButtonText;
    [SerializeField] private TextMeshProUGUI gamepadPauseButtonText;
    [SerializeField] private TextMeshProUGUI gamepadMapButtonText;


     private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        closeButton.onClick.AddListener(() => {
            //Click
            PauseManager.instance.TogglePauseGame();

            Hide();
        });

        moveLeftButton.onClick.AddListener(() => {
            //Click
            RebindBinding(Bindings.Move_Left);
        });

        moveRightButton.onClick.AddListener(() => {
            //Click
            RebindBinding(Bindings.Move_Right);
        });


        jumpButton.onClick.AddListener(() => {
            //Click
            RebindBinding(Bindings.Jump);
        });

        shootButton.onClick.AddListener(() => {
            //Click
            RebindBinding(Bindings.Shoot);
        });

        specialShootButton.onClick.AddListener(() => {
            //Click
            RebindBinding(Bindings.Special_Shoot);
        });

        tongueCounterButton.onClick.AddListener(() => {
            //Click
            RebindBinding(Bindings.Tongue_Counter);
        });

        sWSwitchLeftButton.onClick.AddListener(() => {
            //Click
            RebindBinding(Bindings.SWSwitch_Left);
        });


        sWSwitchRightButton.onClick.AddListener(() => {
            //Click
            RebindBinding(Bindings.SWSwitch_Right);
        });

        pauseButton.onClick.AddListener(() => {
            //Click
            RebindBinding(Bindings.Pause);
        });

        mapButton.onClick.AddListener(() => {
            //Click
            RebindBinding(Bindings.Map);
        });

        gamepadJumpButton.onClick.AddListener(() => {
            //Click
            RebindBinding(Bindings.Gamepad_Jump);
        });


        gamepadShootButton.onClick.AddListener(() => {
            //Click
            RebindBinding(Bindings.Gamepad_Shoot);
        });

        gamepadSpecialShootButton.onClick.AddListener(() => {
            //Click
            RebindBinding(Bindings.Gamepad_Special_Shoot);
        });

        gamepadTongueCounterButton.onClick.AddListener(() => {
            //Click
            RebindBinding(Bindings.Gamepad_Tongue_Counter);
        });

        gamepadSWSwitchLeftButton.onClick.AddListener(() => {
            //Click
            RebindBinding(Bindings.Gamepad_SWSwitch_Left);
        });

        gamepadSWSwitchRightButton.onClick.AddListener(() => {
            //Click
            RebindBinding(Bindings.Gamepad_SWSwitch_Right);
        });

        gamepadPauseButton.onClick.AddListener(() => {
            //Click
            RebindBinding(Bindings.Gamepad_Pause);
        });

        gamepadMapButton.onClick.AddListener(() => {
            //Click
            RebindBinding(Bindings.Gamepad_Map);
        });

    }


    // Start is called before the first frame update
    void Start()
    {
        PauseManager.instance.OnGameUnpaused += PauseManager_OnGameUnpaused;
        UpdateVisual();
        Hide();
        HidePressToRebindKey();
    }

    private void PauseManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);

        closeButton.Select();
    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }


    private void ShowPressToRebindKey()
    {

        pressToRebindKeyTransform.gameObject.SetActive(true);
        GameSceneManager.Instance.SetGameState(GameStates.Rebinding);


    }

    private void HidePressToRebindKey()
    {

        pressToRebindKeyTransform.gameObject.SetActive(false);

    }

    private void UpdateVisual()
    {
        moveLeftButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Left);
        moveRightButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Move_Right);
        jumpButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Jump);
        shootButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Shoot);
        specialShootButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Special_Shoot);
        tongueCounterButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Tongue_Counter);
        sWSwitchLeftText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.SWSwitch_Left);
        sWSwitchRightButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.SWSwitch_Right);
        pauseButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Pause);
        mapButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Map);


        gamepadJumpButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Gamepad_Jump);
        gamepadShootButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Gamepad_Shoot);
        gamepadSpecialShootButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Gamepad_Special_Shoot);
        gamepadTongueCounterButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Gamepad_Tongue_Counter);
        gamepadSWSwitchLeftButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Gamepad_SWSwitch_Left);
        gamepadSWSwitchRightButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Gamepad_SWSwitch_Right);
        gamepadPauseButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Gamepad_Pause);
        gamepadMapButtonText.text = GameInput.Instance.GetBindingText(GameInput.Bindings.Gamepad_Map);
    }


    private void RebindBinding(Bindings binding)
    {

        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding,()=> {

            HidePressToRebindKey();
            UpdateVisual();
            GameSceneManager.Instance.SetGameState(GameStates.Paused);


        });



    }
}
