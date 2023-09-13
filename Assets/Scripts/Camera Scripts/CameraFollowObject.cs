using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    //For testing collision with layers
    //public LayerMask playerLayer;


    [Header("References")]
    [SerializeField] private Transform playerTransform;


    [Header("Flip Rotation Stats")]
    [Range(0.1f, 1f)]
    [SerializeField] private float flipYRotationTime = 0.5f;

    private Coroutine turnCoroutine;

    private PlayerMovement playerMove;

    [SerializeField] private bool isFacingRight;

    // Start is called before the first frame update
    private void Awake()
    {
        playerMove = playerTransform.gameObject.GetComponent<PlayerMovement>();

        isFacingRight = playerMove.GetIsFacingRight();
    }

    // Update is called once per frame
    private void Update()
    {
        //Make the camera follow object follow the player's position
        transform.position = playerTransform.position;
    }

    public void CallTurn()
    {
        //turnCoroutine = StartCoroutine(FlipYLerp());

        LeanTween.rotateY(gameObject, DetermineEndRotation(), flipYRotationTime).setEaseInOutSine();
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation;

        float elapsedTime = 0f;

        //yield return new WaitForSeconds(flipYRotationTime);
        //yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / flipYRotationTime));
        //transform.rotation = Quaternion.Euler(0f, yRotation, 0f);


        while (elapsedTime < flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;
            Debug.Log("Camera Follow Object Lerp Elasped Time: " + elapsedTime);


            //lerp the y rotation
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

        }


        yield return null;
    }


    private float DetermineEndRotation()
    {
        isFacingRight = !isFacingRight;

        if (isFacingRight)
        {
            return 180f;
        }
        else
        {
            return 0f;
        }
    }

    //For testing collision with layers
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.layer == playerLayer)
    //    {

    //    }
    //}
}
