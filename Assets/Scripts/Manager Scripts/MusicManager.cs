using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance { get; private set; }

    [SerializeField] private List<AudioClip> soundtrackList;


    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == Loader.GameScenes.StartScene.ToString())
        {
            //audioSource.Stop();


            audioSource.clip = soundtrackList[0];
            audioSource.Play();
               
            //audioSource.PlayOneShot(soundtrackList[0]);
        }

        else if(scene.name == Loader.GameScenes.GameScene.ToString())
        {
            //audioSource.Stop();
            //audioSource.PlayOneShot(soundtrackList[1]);

            audioSource.clip = soundtrackList[1];
            audioSource.Play();
        }
        else if(scene.name == Loader.GameScenes.LoseScene.ToString() || scene.name == Loader.GameScenes.WinScene.ToString())
        {
            audioSource.clip = null;
            //audioSource.Stop();
        }
    }



    private void Update()
    {
        
    }
}
