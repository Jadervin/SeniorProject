using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem flamethrowerParticle;

    public SpecialWeaponScript specialWeaponScript;
    [SerializeField] private bool isHoldingButton;
    [SerializeField] private float timeSinceShooting = 0f;

    [SerializeField, Range(0, 5)]
    private float energyDecreaseTimeMax = .5f;


    [SerializeField] private int weaponEnergyCost = 2;
    [SerializeField] private SpecialWeaponSO specialWeaponSOReference;


    // Start is called before the first frame update
    void Start()
    {
        GameInput.Instance.OnSpecialShootPressed += GameInput_OnSpecialShootPressed;
        GameInput.Instance.OnSpecialShootRelease += GameInput_OnSpecialShootRelease;

        flamethrowerParticle.gameObject.SetActive(false);
        specialWeaponScript = FindAnyObjectByType<SpecialWeaponScript>();

        weaponEnergyCost = specialWeaponSOReference.specialWeaponEnergyCost;
    }


    private void GameInput_OnSpecialShootPressed(object sender, System.EventArgs e)
    {
        if (specialWeaponScript.GetCurrentWeaponEnergy() >= weaponEnergyCost)
        {
            isHoldingButton = true;
            
        }
        
    }


    private void GameInput_OnSpecialShootRelease(object sender, System.EventArgs e)
    {
        isHoldingButton = false;
    }

   
    // Update is called once per frame
    void Update()
    {
        if (isHoldingButton == true && specialWeaponScript.GetCurrentWeaponEnergy() >= weaponEnergyCost)
        {

            FlamethrowerShoot();
            MovementLimiter.instance.characterCanBasicShoot = false;
            timeSinceShooting += Time.deltaTime;



            if (timeSinceShooting > energyDecreaseTimeMax)
            {
                timeSinceShooting = 0;
                specialWeaponScript.DecreaseCurrentWeaponEnergy(weaponEnergyCost);

            }
        }
        else
        {
            TurnOffFlamethrower();
            MovementLimiter.instance.characterCanBasicShoot = true;
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
        
    }
}
