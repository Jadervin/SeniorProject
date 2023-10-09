using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class SpecialWeaponSO : ScriptableObject
{
    public string specialWeaponName;
    public GameObject specialWeaponGO;
    public Sprite swIcon;
    public int specialWeaponEnergyCost;

}
