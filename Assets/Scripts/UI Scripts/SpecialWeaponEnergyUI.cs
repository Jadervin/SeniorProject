using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialWeaponEnergyUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private SpecialWeaponManagerScript specialWeaponManagerScript;
    [SerializeField] private TextMeshProUGUI sWEnergyAmountText;

    // Start is called before the first frame update
    void Start()
    {
        specialWeaponManagerScript = FindAnyObjectByType<SpecialWeaponManagerScript>();
        specialWeaponManagerScript.OnEnergyChanged += SpecialWeaponScript_OnEnergyChanged;
        sWEnergyAmountText.text = specialWeaponManagerScript.GetMaxWeaponEnergy().ToString();

    }

    private void SpecialWeaponScript_OnEnergyChanged(object sender, SpecialWeaponManagerScript.OnEnergyChangedEventArgs e)
    {
        barImage.fillAmount = e.energyNormalized;
        sWEnergyAmountText.text = e.energy.ToString();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
