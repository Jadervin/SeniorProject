using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialWeaponScript : MonoBehaviour
{
    [SerializeField] protected int maxWeaponEnergy = 10;
    [SerializeField] protected int currentWeaponEnergy;
    //public static SpecialWeaponScript Instance { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        currentWeaponEnergy = maxWeaponEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecreaseCurrentWeaponEnergy(int weaponEnergy)
    {
        currentWeaponEnergy -= weaponEnergy;

        if(currentWeaponEnergy < 0)
        {
            currentWeaponEnergy = 0;
        }
    }

    public void IncreaseCurrentWeaponEnergy(int weaponEnergy)
    {
        currentWeaponEnergy += weaponEnergy;
    }

    public void IncreaseMaxWeaponEnergy(int weaponEnergy) 
    {
        maxWeaponEnergy += weaponEnergy;
    }

    public int GetCurrentWeaponEnergy()
    {
        return currentWeaponEnergy;
    }

    public int GetMaxWeaponEnergy()
    {
        return maxWeaponEnergy;
    }
}
