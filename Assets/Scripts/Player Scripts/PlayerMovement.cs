using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;

    [Header("Components")]
    [SerializeField]
    private Rigidbody2D body;

    [SerializeField]
    private PlayerGround ground;

    [Header("Movement Stats")]

    //Top movement speed
    [SerializeField, Range(0f, 20f)]
    [Tooltip("Top movement speed")] private float maxSpeed = 10f;

    //How fast to reach max speed
    [SerializeField, Range(0f, 100f)]
    [Tooltip("How fast to reach max speed")] private float maxAcceleration = 52f;

    //How fast to stop after letting go
    [SerializeField, Range(0f, 100f)]
    [Tooltip("How fast to stop after letting go")] private float maxDecceleration = 52f;

    //How fast to stop when changing direction
    [SerializeField, Range(0f, 100f)]
    [Tooltip("How fast to stop when changing direction")] private float maxTurnSpeed = 80f;

    //How fast to reach max speed when in mid-air
    [SerializeField, Range(0f, 100f)]
    [Tooltip("How fast to reach max speed when in mid-air")] private float maxAirAcceleration;

    //How fast to stop in mid-air when no direction is used
    [SerializeField, Range(0f, 100f)]
    [Tooltip("How fast to stop in mid-air when no direction is used")] private float maxAirDeceleration;

    //How fast to stop when changing direction when in mid-air
    [SerializeField, Range(0f, 100f)]
    [Tooltip("How fast to stop when changing direction when in mid-air")] private float maxAirTurnSpeed = 80f;

    //Friction to apply against movement on stick
    [SerializeField]
    [Tooltip("Friction to apply against movement on stick")] private float friction;


    [Header("Options")]
    //When false, the charcter will skip acceleration and deceleration and instantly move and stop
    [SerializeField][Tooltip("When false, the charcter will skip acceleration and deceleration and instantly move and stop")] private bool useAcceleration;

    [Header("Calculations")]
    [SerializeField] private float directionX;
    private Vector2 desiredVelocity;
    [SerializeField] private Vector2 velocity;
    private float maxSpeedChange;
    private float acceleration;
    private float deceleration;
    private float turnSpeed;

    [Header("Current State")]
    [SerializeField] private bool onGround;
    [SerializeField] private bool pressingKey;
    [SerializeField] private bool isFacingRight;


    [Header("Camera Components")]
    private CameraFollowObject cameraFollowObject;
    [SerializeField] private GameObject cameraFollowGO;



    private void Awake()
    {
        //Find the character's Rigidbody and ground detection script
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<PlayerGround>();

        
    }


    private void Start()
    {
        cameraFollowObject = cameraFollowGO.GetComponent<CameraFollowObject>();
    }


    private void Update()
    {
        directionX = gameInput.GetXMovement();

        //Used to stop movement when the character is playing her death animation
        if (!MovementLimiter.instance.characterCanMove)
        {
            directionX = 0;
        }

        //Calculate's the character's desired velocity - which is the direction you are facing, multiplied by the character's maximum speed
        //Friction is not used in this game
        desiredVelocity = new Vector2(directionX, 0f) * Mathf.Max(maxSpeed - friction, 0f);

    }

    private void FixedUpdate()
    {
        //Fixed update runs in sync with Unity's physics engine

        //Get Kit's current ground status from her ground script
        onGround = ground.GetOnGround();

        //Get the Rigidbody's current velocity
        velocity = body.velocity;

        //Used to flip the player when she changes direction
        //Also tells us that we are currently pressing a direction button
        if (directionX != 0)
        {
            TurnCheck();
            pressingKey = true;
        }
        else
        {
            pressingKey = false;
        }

        //Calculate movement, depending on whether "Instant Movement" has been checked
        if (useAcceleration)
        {
            runWithAcceleration();
        }
        else
        {
            if (onGround)
            {
                runWithoutAcceleration();
            }
            else
            {
                runWithAcceleration();
            }
        }
    }


    private void TurnCheck()
    {
        if(directionX > 0 && !isFacingRight) 
        {
            Turn();
        }

        else if (directionX < 0 && isFacingRight)
        {
            Turn();
        }


    }

    private void Turn()
    {
        if(isFacingRight) 
        {
            Vector3 rotator = new Vector3 (transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;

            //turn the camera folow object
            cameraFollowObject.CallTurn();
        
        }

        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;

            //turn the camera folow object
            cameraFollowObject.CallTurn();
        }
    }


    private void runWithAcceleration()
    {
        //Set our acceleration, deceleration, and turn speed stats, based on whether we're on the ground on in the air

        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        deceleration = onGround ? maxDecceleration : maxAirDeceleration;
        turnSpeed = onGround ? maxTurnSpeed : maxAirTurnSpeed;

        if (pressingKey)
        {
            //If the sign (i.e. positive or negative) of our input direction doesn't match our movement, it means we're turning around and so should use the turn speed stat.
            if (Mathf.Sign(directionX) != Mathf.Sign(velocity.x))
            {
                maxSpeedChange = turnSpeed * Time.deltaTime;
            }
            else
            {
                //If they match, it means we're simply running along and so should use the acceleration stat
                maxSpeedChange = acceleration * Time.deltaTime;
            }
        }
        else
        {
            //And if we're not pressing a direction at all, use the deceleration stat
            maxSpeedChange = deceleration * Time.deltaTime;
        }

        //Move our velocity towards the desired velocity, at the rate of the number calculated above
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        //Update the Rigidbody with this new velocity
        body.velocity = velocity;

    }

    private void runWithoutAcceleration()
    {
        //If we're not using acceleration and deceleration, just send our desired velocity (direction * max speed) to the Rigidbody
        velocity.x = desiredVelocity.x;

        body.velocity = velocity;
    }

    public bool GetIsFacingRight()
    {
        return isFacingRight;
    }
}
