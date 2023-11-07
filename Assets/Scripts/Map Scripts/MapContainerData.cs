using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapContainerData : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera roomCamera;
    [SerializeField] private string roomName;


    public bool hasBeenRevealed { get; set; }


    public CinemachineVirtualCamera GetRoomCamera() 
    { 
        return roomCamera; 
    }

    public string GetRoomName()
    {
        return roomName;
    }
}
