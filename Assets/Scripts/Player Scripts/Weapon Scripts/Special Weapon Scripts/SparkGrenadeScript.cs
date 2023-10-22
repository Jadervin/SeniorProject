using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SparkGrenadeScript : SpecialWeaponEntityScript
{
    [SerializeField] private bool isHoldingButton;

    new protected void Start()
    {
        GameInput.Instance.OnSpecialShootPressed += GameInput_OnSpecialShootPressed;
        GameInput.Instance.OnSpecialShootRelease += GameInput_OnSpecialShootRelease;

        specialWeaponManagerScript = FindAnyObjectByType<SpecialWeaponManagerScript>();

        weaponEnergyCost = specialWeaponSOReference.specialWeaponEnergyCost;
    }

    private void GameInput_OnSpecialShootPressed(object sender, EventArgs e)
    {
        //if the [player has enough weapon energy, and this is the current weapon, and is not doing anything else
        if (specialWeaponManagerScript.GetCurrentWeaponEnergy() >= weaponEnergyCost && specialWeaponManagerScript.GetCurrentSpecialWeapon() == this.gameObject
            && MovementLimiter.instance.playerActionStates == PlayerActionStates.NOACTION)
        {

            isHoldingButton = true;

        }

    }


    private void GameInput_OnSpecialShootRelease(object sender, EventArgs e)
    {
        isHoldingButton = false;
        specialWeaponManagerScript.DecreaseCurrentWeaponEnergy(weaponEnergyCost);
        SparkGrenade();

    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SparkGrenade()
    {

    }
}
