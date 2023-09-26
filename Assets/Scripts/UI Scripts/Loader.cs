using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public enum GameScenes
    {
        MainMenuScene,
        GameScene,
        LoseScene,
        LoadingScene
    }

    private static GameScenes targetScene;


    public static void LoadSceneWithLoading(GameScenes targetScene)
    {
        Loader.targetScene = targetScene;

        SceneManager.LoadScene(GameScenes.LoadingScene.ToString());




    }

    public static void Load(GameScenes targetScene)
    {
        Loader.targetScene = targetScene;

        SceneManager.LoadScene(targetScene.ToString());




    }

    public static void LoaderCallbackFunction()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
