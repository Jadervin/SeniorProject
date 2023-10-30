using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{

    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button closeButton;

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
    }


    // Start is called before the first frame update
    void Start()
    {
        PauseManager.instance.OnGameUnpaused += PauseManager_OnGameUnpaused;
        Hide();
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
}
