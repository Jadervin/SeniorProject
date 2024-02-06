using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator playerAnimator;
    [SerializeField] private PlayerMovement playerMove;
    [SerializeField] private PlayerBasicShooting playerBaseShoot;


    private const string IS_WALKING = "isWalking";
    private const string IS_SHOOTING = "isShooting";

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }



    // Start is called before the first frame update
    void Start()
    {
        /*
        playerBaseShoot.OnShootPerformed += PlayerBaseShoot_OnShootPerformed;
        playerBaseShoot.OnShootStopped += PlayerBaseShoot_OnShootStopped;*/
    }
/*
    private void PlayerBaseShoot_OnShootPerformed(object sender, System.EventArgs e)
    {
        playerAnimator.SetBool(IS_SHOOTING, true);
    }

    private void PlayerBaseShoot_OnShootStopped(object sender, System.EventArgs e)
    {
        playerAnimator.SetBool(IS_SHOOTING, false);
    }

    */

    // Update is called once per frame
    void Update()
    {
        playerAnimator.SetBool(IS_WALKING, playerMove.GetIsWalking());
        playerAnimator.SetBool(IS_SHOOTING, playerBaseShoot.GetShotBullet());
    }
}
