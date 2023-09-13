using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

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


    private CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer framingTransposer;


    private float normalYPanAmount;

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
}
