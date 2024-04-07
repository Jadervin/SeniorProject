using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TutorialCheck : MonoBehaviour
{

    public static TutorialCheck instance {  get; private set; }

    [SerializeField] private bool playedTutorial = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        
    }


    public bool GetPlayedTutorialCheck()
    {
        return playedTutorial;
    }

    public void SetPlayedTutorialCheck(bool check)
    {
        playedTutorial = check;
    }

}
