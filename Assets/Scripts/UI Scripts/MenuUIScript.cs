using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIScript : MonoBehaviour
{

    [SerializeField] private Button playButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button startMenuButton;



    private void Awake()
    {
        if (playButton != null)
        {
            playButton.onClick.AddListener(() =>
            {
                //Click
                Loader.Load(Loader.GameScenes.GameScene);
            });
        }

        if (retryButton != null)
        {
            retryButton.onClick.AddListener(() =>
            {
                //Click
                Loader.Load(Loader.GameScenes.GameScene);
            });
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(() =>
            {
                //Click
                Application.Quit();
            });
        }

        if (startMenuButton != null)
        {
            startMenuButton.onClick.AddListener(() =>
            {
                //Click
                Loader.Load(Loader.GameScenes.StartScene);
            });
        }

        Time.timeScale = 1f;
    }


    
}
