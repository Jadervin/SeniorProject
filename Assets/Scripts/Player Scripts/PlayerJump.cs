using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    //public static PlayerJump Instance { get; private set; }
    
    [Header("Components")]

    [SerializeField] 
    private GameInput gameInput;

    [SerializeField]
    public Rigidbody2D body;

    [SerializeField]
    private PlayerGround ground;
    [HideInInspector] private Vector2 velocity;
    //private CharacterJuice juice;


    [Header("Jumping Stats")]

    //Maximum jump height
    [SerializeField, Range(2f, 7.5f)]
    [Tooltip("Maximum jump height")] private float jumpHeight = 7.3f;

    //How long it takes to reach that height before coming back down
    [SerializeField, Range(0.2f, 1.25f)]
    [Tooltip("How long it takes to reach that height before coming back down")] private float timeToJumpApex;

    //Gravity multiplier to apply when going up
    
    [Range(0f, 5f)]
    [SerializeField]
    [Tooltip("Gravity multiplier to apply when going up")] private float upwardMovementMultiplier = 1f;

    //Gravity multiplier to apply when coming down
    
    [Range(1f, 10f)]
    [SerializeField]
    [Tooltip("Gravity multiplier to apply when coming down")] private float downwardMovementMultiplier = 6.17f;
    
    //How many times can you jump in the air?
    [SerializeField, Range(0, 1)]
    [Tooltip("How many times can you jump in the air?")] private int maxAirJumps = 0;

    [Header("Options")]

    //Should the character drop when you let go of jump?
    [SerializeField]
    [Tooltip("Should the character drop when you let go of jump?")] private bool variableJumpHeight;

    //Gravity multiplier when you let go of jump
    [SerializeField, Range(1f, 10f)]
    [Tooltip("Gravity multiplier when you let go of jump")] private float jumpCutOff;

    //The fastest speed the character can fall
    [SerializeField]
    [Tooltip("The fastest speed the character can fall")] private float fallSpeedLimit;

    //How long should coyote time last?
    [SerializeField, Range(0f, 0.3f)]
    [Tooltip("How long should coyote time last?")] private float coyoteTime = 0.15f;

    //How far from ground should we cache your jump?
    [SerializeField, Range(0f, 0.3f)]
    [Tooltip("How far from ground should we cache your jump?")] private float jumpBuffer = 0.15f;


    [SerializeField] private float defaultBodyGravityScale = 0;

    [Range(0.7f, 1f)]
    [SerializeField] private float jumpSpeedDivider = .8f;


    [Header("Calculations")]
    [SerializeField] private float jumpSpeed;
    private float defaultGravityScale;
    [SerializeField] private float gravMultiplier;
    //[SerializeField] private int jumpCounter = 0;

    [Header("Current State")]
    [SerializeField] private bool canJumpAgain = false;
    [SerializeField] private bool desiredJump;
    [SerializeField] private bool pressingJump;
    private float jumpBufferCounter;
    private float coyoteTimeCounter = 0;
    
    
    [SerializeField] private bool onGround;
    private bool currentlyJumping;



    //private int jumpsPerformedDEBUG = 0;
    private bool firstPhysicsSet;

    //For Camera Manager
    //[Header("Camera Components")]
    private float fallSpeedYDampingChangeThreshold;




    void Awake()
    {
        //Instance = this;

        //Find the character's Rigidbody and ground detection and juice scripts

        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<PlayerGround>();
        //juice = GetComponentInChildren<characterJuice>();
        defaultGravityScale = 1f;
    }

   

    private void Start()
    {

        gameInput.OnJumpPressed += GameInput_OnJumpPressed;
        gameInput.OnJumpRelease += GameInput_OnJumpReleased;

        fallSpeedYDampingChangeThreshold = CameraManager.instance.GetFallSpeedYDampingChangeThreshold();


    }

    

    private void GameInput_OnJumpPressed(object sender, System.EventArgs e)
    {
        desiredJump = true;
        pressingJump = true;
    }


    private void GameInput_OnJumpReleased(object sender, System.EventArgs e)
    {
        pressingJump = false;
    }




    void Update()
    {
        setPhysics();



        if (firstPhysicsSet == false)
        {
            SetDefaultBodyGravityScale();
        }



        //Check if we're on ground, using Kit's Ground script
        onGround = ground.GetOnGround();


        //SetBodyGravityScale();

        //Jump buffer allows us to queue up a jump, which will play when we next hit the ground
        if (jumpBuffer > 0)
        {
            //Instead of immediately turning off "desireJump", start counting up...
            //All the while, the DoAJump function will repeatedly be fired off
            if (desiredJump)
            {
                jumpBufferCounter += Time.deltaTime;

                if (jumpBufferCounter > jumpBuffer)
                {
                    //If time exceeds the jump buffer, turn off "desireJump"
                    desiredJump = false;
                    jumpBufferCounter = 0;
                }
            }
        }

        //If we're not on the ground and we're not currently jumping, that means we've stepped off the edge of a platform.
        //So, start the coyote time counter...
        if (!currentlyJumping && !onGround)
        {
            coyoteTimeCounter += Time.deltaTime;
        }
        else
        {
            //Reset it when we touch the ground, or jump
            coyoteTimeCounter = 0;
        }



        //if we are falling past a certain speed threshold
        if(body.velocity.y < fallSpeedYDampingChangeThreshold && !CameraManager.instance.IsLerpingYDamping && !CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }

        //if we are standing still or moving up
        if (body.velocity.y >= 0f && !CameraManager.instance.IsLerpingYDamping && CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }

    }

    private void setPhysics()
    {
        //Determine the character's gravity scale, using the stats provided. Multiply it by a gravMultiplier, used later
        Vector2 newGravity = new Vector2(0, (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex));
        body.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravMultiplier;
    }

    private void FixedUpdate()
    {
        //Get velocity from Kit's Rigidbody 
        velocity = body.velocity;

        //Keep trying to do a jump, for as long as desiredJump is true
        if (desiredJump)
        {
            DoAJump();
            body.velocity = velocity;


            //Debug.Log("Jumps Performed " + jumpsPerformedDEBUG + ": " + "Body velocity: " + body.velocity);

            //Skip gravity calculations this frame, so currentlyJumping doesn't turn off
            //This makes sure you can't do the coyote time double jump bug
            return;
        }
        calculateGravity();

    }

    private void calculateGravity()
    {
        //We change the character's gravity based on her Y direction

        //If Kit is going up...
        if (body.velocity.y > 0.01f)
        {
            if (onGround)
            {
                //Don't change it if Kit is stood on something (such as a moving platform)
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                //If we're using variable jump height...)
                if (variableJumpHeight)
                {
                    //Apply upward multiplier if player is rising and holding jump
                    if (pressingJump && currentlyJumping)
                    {
                        gravMultiplier = upwardMovementMultiplier;
                    }
                    //But apply a special downward multiplier if the player lets go of jump
                    else
                    {
                        gravMultiplier = jumpCutOff;
                    }
                }
                else
                {
                    gravMultiplier = upwardMovementMultiplier;
                }
            }
        }

        //Else if going down...
        else if (body.velocity.y < -0.01f)
        {

            if (onGround)
            //Don't change it if Kit is stood on something (such as a moving platform)
            {
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                //Otherwise, apply the downward gravity multiplier as Kit comes back to Earth
                gravMultiplier = downwardMovementMultiplier;
            }

        }
        //Else not moving vertically at all
        else
        {
            if (onGround)
            {
                currentlyJumping = false;
            }

            gravMultiplier = defaultGravityScale;
            
        }

        //Set the character's Rigidbody's velocity
        //But clamp the Y variable within the bounds of the speed limit, for the terminal velocity assist option
        body.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -fallSpeedLimit, 100));
    }

    private void DoAJump()
    {
        //Create the jump, provided we are on the ground, in coyote time, or have a double jump available
        if (onGround || (coyoteTimeCounter > 0.03f && coyoteTimeCounter < coyoteTime) || canJumpAgain)
        {
           
            
            //Test this later to see if this fixes problem
            calculateGravity();


            //jumpsPerformedDEBUG++;


            desiredJump = false;
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;

            //If we have double jump on, allow us to jump again (but only once)
            canJumpAgain = (maxAirJumps == 1 && canJumpAgain == false);

            /*
            //Debug.Log("Jumps Performed " + jumpsPerformedDEBUG + ": " + "Gravity Scale: " + body.gravityScale);
            //ResetBodyGravityScale();
            */


            //Determine the power of the jump, based on our gravity and stats
            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * defaultBodyGravityScale * jumpHeight);

            /*
            //Debug.Log("Jumps Performed " + jumpsPerformedDEBUG + ": " + "Physics2D Gravity: " + Physics2D.gravity.y);
            //Debug.Log("Jumps Performed " + jumpsPerformedDEBUG + ": " + "Gravity Scale: " + body.gravityScale + "\nPrevious Graivty Scale: " + defaultBodyGravityScale);
            //Debug.Log("Jumps Performed " + jumpsPerformedDEBUG + ": " + "Gravity Mult: " + gravMultiplier);
            //Debug.Log("Jumps Performed " + jumpsPerformedDEBUG + ": " + "Jump Speed Calc: " + jumpSpeed);
            */

            //If Kit is moving up or down when she jumps (such as when doing a double jump), change the jumpSpeed;
            //This will ensure the jump is the exact same strength, no matter your velocity.
            
            /*
            //Debug.Log("velocity.y: " + velocity.y);
            //Debug.Log("Jumps Performed " + jumpsPerformedDEBUG + ": " + "velocity: " + velocity.ToString() + "\nBody.velocity:" + body.velocity.ToString());
            */

            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);

                //Debug.Log("Jumps Performed " + jumpsPerformedDEBUG + ": " + "Jump Speed Calc after Velocity.y is greater than 0: " + jumpSpeed);

            }
            else if (velocity.y < 0f)
            {
                //Maybe the problem lies with this equation


                //Find a multiplier that will allow the jump to be the same as the first jump
                jumpSpeed /= jumpSpeedDivider;


                //Debug.Log("Jumps Performed " + jumpsPerformedDEBUG + ": " + "Jump Speed Calc after multipler: " + jumpSpeed);


                jumpSpeed += Mathf.Abs(velocity.y);

                

                //Debug.Log("Jumps Performed " + jumpsPerformedDEBUG + ": " + "Jump Speed Calc after Velocity.y is less than 0: " + jumpSpeed);
            }

            //Apply the new jumpSpeed to the velocity. It will be sent to the Rigidbody in FixedUpdate;
            velocity.y = jumpSpeed + velocity.y;
            
            //Debug.Log("Jumps Performed " + jumpsPerformedDEBUG + ": " + "Velocity after Jump Calculations: " + velocity.y + ", " + "JumpSpeed: " + jumpSpeed);


            currentlyJumping = true;

            /*
             * if (juice != null) {
                //Apply the jumping effects on the juice script
                juice.jumpEffects();
            }
            */
        }

        if (jumpBuffer == 0)
        {
            //If we don't have a jump buffer, then turn off desiredJump immediately after hitting jumping
            desiredJump = false;
        }
    }



    private void SetDefaultBodyGravityScale()
    {
        defaultBodyGravityScale = body.gravityScale;
        firstPhysicsSet = true;
        
    }

    private void ResetBodyGravityScale()
    {
        body.gravityScale = defaultBodyGravityScale;
    }
}