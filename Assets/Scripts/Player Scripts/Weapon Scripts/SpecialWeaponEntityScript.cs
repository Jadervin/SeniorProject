using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialWeaponEntityScript : MonoBehaviour
{
    [SerializeField] protected float timeSinceShooting = 0f;
    [SerializeField] protected float timeToBasicShootMax = .2f;
    [SerializeField] protected float cooldownTimeMax = 1f;
    [SerializeField] protected bool canSpecialShoot = true;
    [SerializeField] protected SpecialWeaponSO specialWeaponSOReference;
    [SerializeField] protected int weaponEnergyCost = 3;

    public SpecialWeaponManagerScript specialWeaponManagerScript;

    // Start is called before the first frame update
    protected void Start()
    {
        specialWeaponManagerScript = FindAnyObjectByType<SpecialWeaponManagerScript>();
        weaponEnergyCost = specialWeaponSOReference.specialWeaponEnergyCost;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetRechargeTimer()
    {
        if (specialWeaponManagerScript.switchedWeapon == true)
        {
            timeSinceShooting = 0f;
            canSpecialShoot = true;
        }
    }

    public SpecialWeaponSO GetSpecialWeaponSOReference()
    {
        return specialWeaponSOReference;
    }
}
