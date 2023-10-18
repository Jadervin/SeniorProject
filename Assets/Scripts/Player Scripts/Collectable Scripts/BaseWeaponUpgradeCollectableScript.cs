using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseWeaponUpgradeCollectableScript : Collectables
{
    public override void Interact()
    {
        player.gameObject.GetComponentInChildren<PlayerBasicShooting>().UpgradeBaseWeapon();
    }

   
}
