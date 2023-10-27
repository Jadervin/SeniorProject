using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{

    [SerializeField] private Button startMenuButton;
    [SerializeField] private Button resumeButton;

    private void Awake()
    {
        startMenuButton.onClick.AddListener(() => {
            //Click
            Loader.Load(Loader.GameScenes.StartScene);
        });


        resumeButton.onClick.AddListener(() => {
            //Click
            PauseManager.instance.TogglePauseGame();
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        PauseManager.instance.OnGamePaused += Instance_OnGamePaused;
        PauseManager.instance.OnGameUnpaused += Instance_OnGameUnpaused;
        Hide();
    }

    private void Instance_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void Instance_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }
/*
    private void PauseManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void PauseManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }
*/

    private void Show()
    {
        this.gameObject.SetActive(true);


    }

    private void Hide()
    {
        this.gameObject.SetActive(false);
    }

}
