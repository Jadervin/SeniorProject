using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgradeCollectableScript : Collectables
{
    [SerializeField] private int healthUpgradeIncrease = 5;


    public override void Interact()
    {
        
        player.gameObject.GetComponent<PlayerHealth>().IncreaseMaxHealth(healthUpgradeIncrease);
    }
}
