using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.Rendering;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera[] allVirtualCameras;

    [Header("Controls for lerping the Y Damping during player jump/fall")]
    [SerializeField] private float fallPanAmount = .25f;
    [SerializeField] private float fallYPanTime = .35f;
    [SerializeField] private float fallSpeedYDampingChangeThreshold = -15f;


    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }


    private Coroutine lerpYPanCoroutine;
    private Coroutine panCameraCoroutine;


    [SerializeField] private CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer framingTransposer;


    private float normalYPanAmount;

    private Vector2 startingTrackedObjectOffset;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        //if (SaveSystem.SaveFileCheck() == false)
        {
            for (int i = 0; i < allVirtualCameras.Length; i++)
            {
                if (allVirtualCameras[i].enabled)
                {
                    //set the current active camera
                    currentCamera = allVirtualCameras[i];

                    //MapRoomManager.instance.RevealRoom();


                    //set the framing transposer
                    framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
                }
            }


            //set the YDamping amount so it's based on the inspector value
            normalYPanAmount = framingTransposer.m_YDamping;


            //set the starting position of the tracked object offset
            startingTrackedObjectOffset = framingTransposer.m_TrackedObjectOffset;
        }
    }


    //Lerp the Y Damping

    public void LerpYDamping(bool isPlayerFalling)
    {
        lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;

        //grab the starting damping amount
        float startDampingAmount = framingTransposer.m_YDamping;
        float endDampAmount = 0f;

        //determine the end damping amount
        if(isPlayerFalling)
        {
            endDampAmount = fallPanAmount;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = normalYPanAmount;
        }


        //lerp the pan amount
        float elapsedTime = 0f;
        while(elapsedTime < fallYPanTime)
        {
            //Debug.Log("Camera Manager Lerp Elasped Time: " + elaspedTime);
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampingAmount, endDampAmount, (elapsedTime/fallYPanTime));
            framingTransposer.m_YDamping = lerpedPanAmount;


            yield return null;

        }

        IsLerpingYDamping = false;
        
    }


    public float GetFallSpeedYDampingChangeThreshold()
    {
        return fallSpeedYDampingChangeThreshold;
    }



    //For Panning the Camera

    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartPosition)
    {
        panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartPosition));

        
    }

    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPosition)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;

        //set the direction and distance if we are panning in the direction indicated by the trigger object
        if (!panToStartingPosition)
        {
            //set the direction and distance
            switch (panDirection)
            { 
                case PanDirection.UP:
                {
                    endPos = Vector2.up;
                    break;
                }
                case PanDirection.DOWN:
                {
                   endPos = Vector2.down;
                   break;
                }
                case PanDirection.LEFT:
                {
                    //Check and see if this is the correct statement once you set the enum to left
                    endPos = Vector2.left;
                    //endPos = Vector2.right;
                    break;
                }
                case PanDirection.RIGHT:
                {
                    //Check and see if this is the correct statement once you set the enum to right
                    endPos = Vector2.right;

                    //endPos = Vector2.left;
                    break;
                }
                default:
                {
                    break;
                }
                        
            }

            endPos *= panDistance;

            startingPos = startingTrackedObjectOffset;

            endPos += startingPos;

        }

        //handle the direction settings when moving back to the starting position
        else
        {
            startingPos = framingTransposer.m_TrackedObjectOffset;
            endPos = startingTrackedObjectOffset;
        }


        //handle the actual panning of the camera
        float elapsedTime = 0f;

        while(elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;

            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime / panTime));
            framingTransposer.m_TrackedObjectOffset = panLerp;
            yield return null;

        }

        
    }



    //Swap Cameras


    public void SwapCameras(CinemachineVirtualCamera cameraFromLeft, CinemachineVirtualCamera cameraFromRight, CinemachineVirtualCamera cameraFromTop, CinemachineVirtualCamera cameraFromBottom, Vector2 triggerExitDirection)
    {
        //if our current camera is the camera on the left and our trigger exit direction was on the right
        if(currentCamera == cameraFromLeft && triggerExitDirection.x > 0f)
        {
            //activate the new camera
            cameraFromRight.enabled = true; 


            //deactivate the old camera
            cameraFromLeft.enabled = false;

            //set the new camera as the current camera
            currentCamera = cameraFromRight;

            MapRoomManager.instance.RevealRoom();

            //update our composer variable
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        //if our current camera is the camera on the right and our trigger exit direction was on the left
        else if (currentCamera == cameraFromRight && triggerExitDirection.x < 0f)
        {
            //activate the new camera
            cameraFromLeft.enabled = true;


            //deactivate the old camera
            cameraFromRight.enabled = false;

            //set the new camera as the current camera
            currentCamera = cameraFromLeft;

            MapRoomManager.instance.RevealRoom();

            //update our composer variable
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        //if our current camera is the camera on the top and our trigger exit direction was on the bottom
        else if (currentCamera == cameraFromTop && triggerExitDirection.y < 0f)
        {
            //activate the new camera
            cameraFromBottom.enabled = true;


            //deactivate the old camera
            cameraFromTop.enabled = false;

            //set the new camera as the current camera
            currentCamera = cameraFromBottom;

            MapRoomManager.instance.RevealRoom();

            //update our composer variable
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        //if our current camera is the camera on the bottom and our trigger exit direction was on the top
        else if (currentCamera == cameraFromBottom && triggerExitDirection.y > 0f)
        {
            //activate the new camera
            cameraFromTop.enabled = true;


            //deactivate the old camera
            cameraFromBottom.enabled = false;

            //set the new camera as the current camera
            currentCamera = cameraFromTop;

            MapRoomManager.instance.RevealRoom();

            //update our composer variable
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }


    public int GetCurrentCameraIndex()
    {
        int index = 0;
        for (int i = 0; i < allVirtualCameras.Length; i++)
        {
            if(currentCamera == allVirtualCameras[i])
            {
                index = i; 
                break;
            }
            
        }
        return index;

    }

    public CinemachineVirtualCamera GetCurrentCamera()
    {
        return currentCamera;


    }

    public void SetCurrentCamera(int savedCurrentCameraIndex)
    {
        for (int i = 0; i < allVirtualCameras.Length; i++)
        {
            if (i == savedCurrentCameraIndex)
            {
                //set the current active camera

                currentCamera = allVirtualCameras[i];
                //allVirtualCameras[i].gameObject.SetActive(true);
                allVirtualCameras[i].enabled = true;
                //MapRoomManager.instance.RevealRoom();


                //set the framing transposer
                framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

                //set the YDamping amount so it's based on the inspector value
                normalYPanAmount = framingTransposer.m_YDamping;


                //set the starting position of the tracked object offset
                startingTrackedObjectOffset = framingTransposer.m_TrackedObjectOffset;
            }
            else
            {
                //allVirtualCameras[i].gameObject.SetActive(false);
                allVirtualCameras[i].enabled = false;


            }
        }
    }

}
