using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance { get; private set; }

    [SerializeField] private List<AudioClip> soundtrackList;

    [SerializeField] private AudioSource audioSource;


    [SerializeField] private float volume = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        volume = PlayerPrefs.GetFloat(TagReferencesScript.PLAYER_PREFS_MUSIC_VOLUME, 1f);
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
        audioSource.volume = volume;
    }


    public void ChangeVolume(float sliderValue)
    {
        volume = sliderValue;

        /*if (volume > 1f)
        {
            volume = 0f;
        }*/
        PlayerPrefs.SetFloat(TagReferencesScript.PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }


}
