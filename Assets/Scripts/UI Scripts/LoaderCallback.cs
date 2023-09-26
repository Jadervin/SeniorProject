using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{
    private bool isFirstUpdate = true;

    /*
        if this loading scene has been loaded,
        load the desired scene from the Loader static class.
        The bool will reset every time the loading scene is called.
    */

    //The loading scene is to make sure the player does not have to wait on a frozen screen while the game is loading
    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;


            Loader.LoaderCallbackFunction();
        }

    }
}
