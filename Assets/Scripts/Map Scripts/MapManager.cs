using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    [SerializeField] private GameObject largeMapImage;

    public bool IsMapScreenOpen {  get; set; }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        CloseMapScreen();
    }

    private void Start()
    {
        GameInput.Instance.OnMapPressed += GameInput_OnMapPressed;
    }

    private void GameInput_OnMapPressed(object sender, System.EventArgs e)
    {
        if(IsMapScreenOpen == false) 
        {
            //open map screen
            GameSceneManager.Instance.SetGameState(GameStates.MapOpen);
            OpenMapScreen();


        }

        else
        {
            //close map screen
            GameSceneManager.Instance.SetGameState(GameStates.GamePlaying);
            CloseMapScreen();

        }
    }



    private void OpenMapScreen()
    {
        largeMapImage.SetActive(true);
        IsMapScreenOpen = true;
    }

    private void CloseMapScreen()
    {
        largeMapImage.SetActive(false);
        IsMapScreenOpen = false;
    }
}
