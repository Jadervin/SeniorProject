using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialWeaponEnergyUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private SpecialWeaponScript specialWeaponScript;

    // Start is called before the first frame update
    void Start()
    {
        specialWeaponScript.OnEnergyChanged += SpecialWeaponScript_OnEnergyChanged;
    }

    private void SpecialWeaponScript_OnEnergyChanged(object sender, SpecialWeaponScript.OnEnergyChangedEventArgs e)
    {
        barImage.fillAmount = e.energyNormalized;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
