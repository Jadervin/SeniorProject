using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem 
{
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";

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
        if(File.Exists(SAVE_FOLDER + "save.json")) 
        {
            string saveString = File.ReadAllText(SAVE_FOLDER + "save.json");
            return saveString;

        }
        else
        {
            return null;
        }


    }
}
