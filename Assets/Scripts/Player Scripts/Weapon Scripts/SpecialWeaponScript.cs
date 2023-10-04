using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialWeaponScript : MonoBehaviour
{
    [SerializeField] private int maxWeaponEnergy = 10;
    [SerializeField] private int currentWeaponEnergy;

    public event EventHandler<OnEnergyChangedEventArgs> OnEnergyChanged;

    public class OnEnergyChangedEventArgs:EventArgs
    {
        public float energyNormalized;
    }

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

        OnEnergyChanged?.Invoke(this, new OnEnergyChangedEventArgs
        {
            energyNormalized = (float)currentWeaponEnergy / maxWeaponEnergy
        });
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
