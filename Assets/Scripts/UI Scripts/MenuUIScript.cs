using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIScript : MonoBehaviour
{

    [SerializeField] private Button playButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button menuButton;

    [SerializeField] private Transform noSaveDataPopupGO;
    [SerializeField] private Button closeLoadPopupButton;

    [SerializeField] private Transform startNewGamePopupGO;
    [SerializeField] private Button startNewGameButton;
    [SerializeField] private Button closeNewGamePopupButton;


    private void Awake()
    {
        if (playButton != null)
        {
            playButton.onClick.AddListener(() =>
            {
                //Click
                if (SaveSystem.SaveFileCheck() == true)
                {
                    startNewGamePopupGO.gameObject.SetActive(true);
                }
                else
                {
                    SaveSystem.SetGameStartState(SaveSystem.GameStartStates.NEWGAME);
                    Loader.Load(Loader.GameScenes.GameScene);
                }

                
            });
        }

        if (retryButton != null)
        {
            retryButton.onClick.AddListener(() =>
            {
                //Click
                if (SaveSystem.SaveFileCheck() == true)
                {
                    SaveSystem.SetGameStartState(SaveSystem.GameStartStates.LOADGAME);
                    Loader.Load(Loader.GameScenes.GameScene);
                }
                else
                {
                    SaveSystem.SetGameStartState(SaveSystem.GameStartStates.NEWGAME);
                    Loader.Load(Loader.GameScenes.GameScene);
                }
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

        if (menuButton != null)
        {
            menuButton.onClick.AddListener(() =>
            {
                //Click
                Loader.Load(Loader.GameScenes.StartScene);
            });
        }

        if (loadButton != null)
        {
            loadButton.onClick.AddListener(() =>
            {
                //Click
                if (SaveSystem.SaveFileCheck() == false)
                {
                    noSaveDataPopupGO.gameObject.SetActive(true);
                }

                else
                {
                    SaveSystem.SetGameStartState(SaveSystem.GameStartStates.LOADGAME);
                    Loader.Load(Loader.GameScenes.GameScene);
                }
            });
        }

        if (closeLoadPopupButton != null)
        {
            closeLoadPopupButton.onClick.AddListener(() =>
            {
                
                noSaveDataPopupGO.gameObject.SetActive(false);
               
            });
        }

        if (startNewGameButton != null)
        {
            startNewGameButton.onClick.AddListener(() =>
            {

                startNewGamePopupGO.gameObject.SetActive(false);
                SaveSystem.DeleteSave();
                SaveSystem.SetGameStartState(SaveSystem.GameStartStates.NEWGAME);
                Loader.Load(Loader.GameScenes.GameScene);

            });
        }

        if (closeNewGamePopupButton != null)
        {
            closeNewGamePopupButton.onClick.AddListener(() =>
            {

                startNewGamePopupGO.gameObject.SetActive(false);

            });
        }


        if (noSaveDataPopupGO != null)
        {
            noSaveDataPopupGO.gameObject.SetActive(false);
        }

        if (startNewGamePopupGO != null)
        {
            startNewGamePopupGO.gameObject.SetActive(false);
        }

        Time.timeScale = 1f;
    }


    
}
