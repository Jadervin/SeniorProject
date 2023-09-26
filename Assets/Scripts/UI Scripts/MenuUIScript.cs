using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIScript : MonoBehaviour
{
    [SerializeField] private Button retryButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        retryButton.onClick.AddListener(() => {
            //Click
            Loader.Load(Loader.GameScenes.MainScene);
        });


        quitButton.onClick.AddListener(() => {
            //Click
            Application.Quit();
        });

        Time.timeScale = 1f;
    }


    
}
