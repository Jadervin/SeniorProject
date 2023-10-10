using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialWeaponEnergyUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private SpecialWeaponManagerScript specialWeaponManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        specialWeaponManagerScript = FindAnyObjectByType<SpecialWeaponManagerScript>();
        specialWeaponManagerScript.OnEnergyChanged += SpecialWeaponScript_OnEnergyChanged;
    }

    private void SpecialWeaponScript_OnEnergyChanged(object sender, SpecialWeaponManagerScript.OnEnergyChangedEventArgs e)
    {
        barImage.fillAmount = e.energyNormalized;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
