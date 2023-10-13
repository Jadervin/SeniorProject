using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sWEnergyUpgradeCollectableScript : Collectables
{
    [SerializeField] private int energyUpgradeIncrease = 10;

    public override void Interact()
    {
        player.gameObject.GetComponentInChildren<SpecialWeaponManagerScript>().IncreaseMaxWeaponEnergy(energyUpgradeIncrease);
    }
}
