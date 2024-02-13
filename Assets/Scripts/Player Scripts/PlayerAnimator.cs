using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator playerAnimator;
    [SerializeField] private PlayerMovement playerMove;
    [SerializeField] private PlayerBasicShooting playerBaseShoot;
    [SerializeField] private PlayerJump playerJump;
    [SerializeField] private PlayerGround playerGround;
    [SerializeField] private PlayerTongueCounter playerTongueCounter;
    [SerializeField] private SparkGrenadeScript grenadeThrow;

    private const string IS_WALKING = "isWalking";
    private const string IS_SHOOTING = "isShooting";
    private const string IS_JUMPING = "isJumping";
    private const string ON_GROUND = "onGround";
    private const string IS_COUNTERING = "isCountering";
    private const string LEDGEGRABBED = "ledgeGrabbed";
    private const string ISHOLDINGTHROW = "isHoldingThrow";



    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }



    // Start is called before the first frame update
    void Start()
    {
        playerJump.OnLedgeGrabPerformed += PlayerJump_OnLedgeGrabPerformed;
        playerTongueCounter.OnTongueCounterPerformed += PlayerTongueCounter_OnTongueCounterPerformed;
    }

    private void PlayerTongueCounter_OnTongueCounterPerformed(object sender, System.EventArgs e)
    {
        playerAnimator.SetTrigger(IS_COUNTERING);
    }

    private void PlayerJump_OnLedgeGrabPerformed(object sender, System.EventArgs e)
    {
        playerAnimator.SetTrigger(LEDGEGRABBED);
    }



    // Update is called once per frame
    void Update()
    {
        playerAnimator.SetBool(IS_WALKING, playerMove.GetIsWalking());
        playerAnimator.SetBool(IS_SHOOTING, playerBaseShoot.GetShotBullet());
        playerAnimator.SetBool(IS_JUMPING, playerJump.GetCurrentlyJumping());
        playerAnimator.SetBool(ON_GROUND, playerGround.GetOnGround());
        playerAnimator.SetBool(ISHOLDINGTHROW, grenadeThrow.GetIsHoldingButton());

        //playerAnimator.SetBool(IS_COUNTERING, playerTongueCounter.GetIsCountering());


    }
}
