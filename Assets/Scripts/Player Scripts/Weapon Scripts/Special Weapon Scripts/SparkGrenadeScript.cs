using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SparkGrenadeScript : SpecialWeaponEntityScript
{
    [SerializeField] private bool isHoldingButton;
    [SerializeField] private Transform shootPoint;
    [Range(0f, 90f)]
    [SerializeField] private float zRotation = 0f;
    [SerializeField] private GameObject grenade;
    [SerializeField] private GameObject trajectoryLine;

    new protected void Start()
    {
        GameInput.Instance.OnSpecialShootPressed += GameInput_OnSpecialShootPressed;
        GameInput.Instance.OnSpecialShootRelease += GameInput_OnSpecialShootRelease;

        specialWeaponManagerScript = FindAnyObjectByType<SpecialWeaponManagerScript>();

        weaponEnergyCost = specialWeaponSOReference.specialWeaponEnergyCost;
        shootPoint.transform.Rotate(0f, 0f, zRotation, Space.Self);
        trajectoryLine.SetActive(false);
    }

    private void GameInput_OnSpecialShootPressed(object sender, EventArgs e)
    {
        //if the [player has enough weapon energy, and this is the current weapon, and is not doing anything else
        if (canSpecialShoot == true && specialWeaponManagerScript.GetCurrentWeaponEnergy() >= weaponEnergyCost && specialWeaponManagerScript.GetCurrentSpecialWeapon() == this.gameObject
            && MovementLimiter.instance.playerActionStates == PlayerActionStates.NOACTION)
        {

            isHoldingButton = true;
            MovementLimiter.instance.IsSpecialShooting();

        }

    }


    private void GameInput_OnSpecialShootRelease(object sender, EventArgs e)
    {
        if (canSpecialShoot == true && specialWeaponManagerScript.GetCurrentWeaponEnergy() >= weaponEnergyCost && specialWeaponManagerScript.GetCurrentSpecialWeapon() == this.gameObject
            && MovementLimiter.instance.playerActionStates == PlayerActionStates.SPECIALSHOOTING
            && isHoldingButton == true)
        {
            isHoldingButton = false;
            canSpecialShoot = false;
            trajectoryLine.SetActive(false);
            specialWeaponManagerScript.DecreaseCurrentWeaponEnergy(weaponEnergyCost);
            SparkGrenade();
        }

    }

    

    // Update is called once per frame
    void Update()
    {
        trajectoryLine.SetActive(isHoldingButton);

        if (canSpecialShoot == false)
        {
            timeSinceShooting += Time.deltaTime;


            if (timeSinceShooting > timeToBasicShootMax)
            {
                //timeSinceShooting = 0;
                MovementLimiter.instance.IsNotDoingAnything();

            }

            if (timeSinceShooting > cooldownTimeMax)
            {
                timeSinceShooting = 0;
                canSpecialShoot = true;
                

            }
        }
        /*
        if(isHoldingButton == true)
        {
            trajectoryLine.SetActive(true);

        }
        */
    }

    private void TrajectoryLine()
    {

    }

    private void SparkGrenade()
    {
        GameObject temp = Instantiate(grenade, shootPoint.transform.position, shootPoint.transform.rotation);
    }

    public GameObject GetGrenade()
    {
        return grenade;
    }
}
