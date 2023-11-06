using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRoomManager : MonoBehaviour
{
    public static MapRoomManager instance;


    /*[SerializeField] */private MapContainerData[] rooms;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }


        rooms = GetComponentsInChildren<MapContainerData>(true);

        RevealRoom();
    }

    public void RevealRoom()
    {
        CinemachineVirtualCamera currentCamera = CameraManager.instance.GetCurrentCamera();

        for(int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i].roomCamera == currentCamera && rooms[i].hasBeenRevealed == false) 
            {
                rooms[i].gameObject.SetActive(true);
                rooms[i].hasBeenRevealed = true;

                return;
            }
        }
    }
}
