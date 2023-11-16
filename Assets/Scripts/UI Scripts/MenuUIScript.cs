using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MenuUIScript : MonoBehaviour
{

    [Header("Audio")]
    [SerializeField] private float volume = 1f;
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    [SerializeField] private float buttonTimer = 1f;

    [Header("Buttons")]
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
        volume = PlayerPrefs.GetFloat(TagReferencesScript.PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);

        if (playButton != null)
        {
            playButton.onClick.AddListener(() =>
            {
                //Click
                if (SaveSystem.SaveFileCheck() == true)
                {
                    playButton.GetComponentInChildren<AudioSource>().PlayOneShot(audioClipRefsSO.menu, volume);
                    startNewGamePopupGO.gameObject.SetActive(true);
                }
                else
                {
                    playButton.GetComponentInChildren<AudioSource>().PlayOneShot(audioClipRefsSO.menu, volume);
                    StartCoroutine(ButtonTimerBeforeSceneSwitch());
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
                    retryButton.GetComponentInChildren<AudioSource>().PlayOneShot(audioClipRefsSO.menu, volume);
                    StartCoroutine(ButtonTimerBeforeSceneSwitch());
                    SaveSystem.SetGameStartState(SaveSystem.GameStartStates.LOADGAME);
                    Loader.Load(Loader.GameScenes.GameScene);
                }
                else
                {
                    retryButton.GetComponentInChildren<AudioSource>().PlayOneShot(audioClipRefsSO.menu, volume);
                    StartCoroutine(ButtonTimerBeforeSceneSwitch());
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
                quitButton.GetComponentInChildren<AudioSource>().PlayOneShot(audioClipRefsSO.menu, volume);
                StartCoroutine(ButtonTimerBeforeSceneSwitch());
                Application.Quit();
            });
        }

        if (menuButton != null)
        {
            menuButton.onClick.AddListener(() =>
            {
                //Click
                menuButton.GetComponentInChildren<AudioSource>().PlayOneShot(audioClipRefsSO.menu, volume);
                StartCoroutine(ButtonTimerBeforeSceneSwitch());
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
                    loadButton.GetComponentInChildren<AudioSource>().PlayOneShot(audioClipRefsSO.menu, volume);
                    noSaveDataPopupGO.gameObject.SetActive(true);
                }

                else
                {
                    loadButton.GetComponentInChildren<AudioSource>().PlayOneShot(audioClipRefsSO.menu, volume);
                    StartCoroutine(ButtonTimerBeforeSceneSwitch());
                    SaveSystem.SetGameStartState(SaveSystem.GameStartStates.LOADGAME);
                    Loader.Load(Loader.GameScenes.GameScene);
                }
            });
        }

        if (closeLoadPopupButton != null)
        {
            closeLoadPopupButton.onClick.AddListener(() =>
            {
                closeLoadPopupButton.GetComponentInChildren<AudioSource>().PlayOneShot(audioClipRefsSO.menu, volume);
                noSaveDataPopupGO.gameObject.SetActive(false);
               
            });
        }

        if (startNewGameButton != null)
        {
            startNewGameButton.onClick.AddListener(() =>
            {
                startNewGameButton.GetComponentInChildren<AudioSource>().PlayOneShot(audioClipRefsSO.menu, volume);
                StartCoroutine(ButtonTimerBeforeSceneSwitch());
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
                closeNewGamePopupButton.GetComponentInChildren<AudioSource>().PlayOneShot(audioClipRefsSO.menu, volume);
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

    private IEnumerator ButtonTimerBeforeSceneSwitch()
    {
        yield return new WaitForSeconds(buttonTimer);
    }
    
}
