using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerScript : SpecialWeaponEntityScript
{
    [SerializeField] private ParticleSystem flamethrowerParticle;


    [SerializeField] private bool isHoldingButton;


    [SerializeField, Range(0, 5)]
    private float energyDecreaseTimeMax = .5f;

    public event EventHandler<OnFlamethrowerStateChangedEventArgs> OnFlamethrowerStateChanged;

    public class OnFlamethrowerStateChangedEventArgs : EventArgs
    {
        public bool isActivated;
    }


    // Start is called before the first frame update
    new private void Start()
    {
        GameInput.Instance.OnSpecialShootPressed += GameInput_OnSpecialShootPressed;
        GameInput.Instance.OnSpecialShootRelease += GameInput_OnSpecialShootRelease;

        flamethrowerParticle.gameObject.SetActive(false);
        specialWeaponManagerScript = FindAnyObjectByType<SpecialWeaponManagerScript>();

        weaponEnergyCost = specialWeaponSOReference.specialWeaponEnergyCost;
    }


    private void GameInput_OnSpecialShootPressed(object sender, System.EventArgs e)
    {
        //if the [player has enough weapon energy, and this is the current weapon, and is not doing anything else
        if (specialWeaponManagerScript.GetCurrentWeaponEnergy() >= weaponEnergyCost && specialWeaponManagerScript.GetCurrentSpecialWeapon()  == this.gameObject
            && MovementLimiter.instance.playerActionStates == PlayerActionStates.NOACTION)
        {
            specialWeaponManagerScript.DecreaseCurrentWeaponEnergy(weaponEnergyCost);
            isHoldingButton = true;
            OnFlamethrowerStateChanged?.Invoke(this, new OnFlamethrowerStateChangedEventArgs
            {
                isActivated = isHoldingButton,
            });
            
        }
        
    }


    private void GameInput_OnSpecialShootRelease(object sender, System.EventArgs e)
    {
        isHoldingButton = false;
        OnFlamethrowerStateChanged?.Invoke(this, new OnFlamethrowerStateChangedEventArgs
        {
            isActivated = isHoldingButton,
        });
    }

   
    // Update is called once per frame
    void Update()
    {
        if (isHoldingButton == true && specialWeaponManagerScript.GetCurrentWeaponEnergy() >= weaponEnergyCost)
        {

            FlamethrowerShoot();
            MovementLimiter.instance.IsSpecialShooting();
            timeSinceShooting += Time.deltaTime;



            if (timeSinceShooting > energyDecreaseTimeMax)
            {
                timeSinceShooting = 0;
                specialWeaponManagerScript.DecreaseCurrentWeaponEnergy(weaponEnergyCost);

            }
        }
        else
        {
            TurnOffFlamethrower();
            MovementLimiter.instance.IsNotDoingAnything();
        }

    }



    private void FlamethrowerShoot()
    {
        flamethrowerParticle.gameObject.SetActive(true);
        flamethrowerParticle.Play();
    }

    private void TurnOffFlamethrower()
    {
        flamethrowerParticle.Stop();
        flamethrowerParticle.gameObject.SetActive(false);
        isHoldingButton = false;

        OnFlamethrowerStateChanged?.Invoke(this, new OnFlamethrowerStateChangedEventArgs
        {
            isActivated = isHoldingButton,
        });

    }
}
