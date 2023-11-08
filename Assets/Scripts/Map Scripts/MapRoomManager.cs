using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRoomManager : MonoBehaviour
{
    public static MapRoomManager instance;


    [SerializeField] private MapContainerData[] rooms;

    /*[SerializeField] */private List<string> openRoomNamesList;
    /*[SerializeField] */private string[] openRoomNames;

    private string[] openRoomNamesFromSave = new string[0];

    [SerializeField] private List<string> openRoomNamesListFromSave = new List<string>();




    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        
    }


    void Start()
    {

        rooms = GetComponentsInChildren<MapContainerData>(true);
/*
        Debug.Log("Amount of Rooms From Save: " + openRoomNamesFromSave.Length);

        for (int i = 0; i < openRoomNamesFromSave.Length; i++)
        {
            Debug.Log("Rooms From Save: " + openRoomNamesFromSave[i]);
        }
*/
        openRoomNamesList = new List<string>();


        RevealRoom();

        if (openRoomNamesFromSave.Length > 0 || openRoomNamesFromSave != null)
        {
            EnableSavedRooms();
        }
    }

    public void RevealRoom()
    {
        CinemachineVirtualCamera currentCamera = CameraManager.instance.GetCurrentCamera();

        for(int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].GetRoomCamera() == currentCamera && rooms[i].hasBeenRevealed == false) 
            {
                rooms[i].gameObject.SetActive(true);
                rooms[i].hasBeenRevealed = true;


                openRoomNamesList.Add(rooms[i].GetRoomName());

                openRoomNames = new string[openRoomNamesList.Count];


                for (int j = 0; j < openRoomNames.Length; j++)
                {
                    openRoomNames[j] = openRoomNamesList[j];

                }

                return;
            }
        }
    }


    public string[] GetOpenRoomNames()
    {
        return openRoomNames;
    }

    public void SetOpenRoomNames(string[] names)
    {
        openRoomNamesFromSave = names;

        for(int i = 0; i < openRoomNamesFromSave.Length; i++)
        {
            openRoomNamesListFromSave.Add(names[i]);
        }


        Debug.Log("Amount of Rooms From Save: " + openRoomNamesFromSave.Length);
        for (int i = 0; i < openRoomNamesFromSave.Length; i++)
        {
            Debug.Log("Rooms From Save: " + openRoomNamesFromSave[i]);
        }

    }

    public void EnableSavedRooms()
    {
        for(int i = 0; i < openRoomNamesFromSave.Length; i++)
        {
            for (int j = 0; j < rooms.Length; j++)
            {
                if (openRoomNamesFromSave[i] == rooms[j].GetRoomName())
                {
                    rooms[j].gameObject.SetActive(true);
                    rooms[j].hasBeenRevealed = true;



                    openRoomNamesList.Add(rooms[j].GetRoomName());

                    
                }
            }

        }

        openRoomNames = new string[openRoomNamesList.Count];


        for (int k = 0; k < openRoomNames.Length; k++)
        {
            openRoomNames[k] = openRoomNamesList[k];

        }

    }
}
