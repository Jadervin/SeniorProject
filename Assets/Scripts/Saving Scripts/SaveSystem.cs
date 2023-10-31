using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class SaveSystem 
{
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";

    public enum GameStartStates
    {
        BLANK,
        NEWGAME,
        LOADGAME,
    }

    public static GameStartStates gameStartState = GameStartStates.BLANK;

    public static void Initialize()
    {
        if(!Directory.Exists(SAVE_FOLDER)) 
        {
            //Create Save Folder
            Directory.CreateDirectory(SAVE_FOLDER);
        
        }

    }

    public static void Save(string saveString)
    {
        File.WriteAllText(SAVE_FOLDER + "save.json", saveString);

    }

    public static string Load()
    {
        if(SaveFileCheck() == true) 
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "save.json");
            return saveString;

        }
        else
        {
            return null;
        }

    }

    public static void DeleteSave()
    {
        File.Delete(SAVE_FOLDER + "save.json");
    }

    public static bool SaveFileCheck()
    {
        if (File.Exists(SAVE_FOLDER + "save.json"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void SetGameStartState(GameStartStates state)
    {
        gameStartState = state;
    }

    public static GameStartStates GetGameStartState()
    {
        return gameStartState;
    }
}
