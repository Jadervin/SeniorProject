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
            GameSceneManager.Instance.TogglePauseGame();
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        GameSceneManager.Instance.OnGamePaused += GameSceneManager_OnGamePaused;
        GameSceneManager.Instance.OnGameUnpaused += GameSceneManager_OnGameUnpaused;
        Hide();
    }

    private void GameSceneManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GameSceneManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void Show()
    {
        gameObject.SetActive(true);


    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
