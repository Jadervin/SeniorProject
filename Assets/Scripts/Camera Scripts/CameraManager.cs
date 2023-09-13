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


    private CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer framingTransposer;


    private float normalYPanAmount;

    private Vector2 startingTrackedObjectOffset;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        for(int i = 0; i < allVirtualCameras.Length; i++)
        {
            if (allVirtualCameras[i].enabled)
            {
                //set the current active camera
                currentCamera = allVirtualCameras[i];

                //set the framing transposer
                framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }


        //set the YDamping amount so it's based on the inspector value
        normalYPanAmount = framingTransposer.m_YDamping;


        //set the starting position of the tracked object offset
        startingTrackedObjectOffset = framingTransposer.m_TrackedObjectOffset;
    }


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
        }

        yield return null;
    }


}
