using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentSpecialWeaponUI : MonoBehaviour
{
    [SerializeField] private SpecialWeaponManagerScript specialWeaponManagerScript;
    [SerializeField] private Image sWImage;

    // Start is called before the first frame update
    void Start()
    {
        specialWeaponManagerScript = FindAnyObjectByType<SpecialWeaponManagerScript>();
        specialWeaponManagerScript.OnCurrentSWChanged += SpecialWeaponManagerScript_OnCurrentSWChanged;
    }

    private void SpecialWeaponManagerScript_OnCurrentSWChanged(object sender, SpecialWeaponManagerScript.OnCurrentSWChangedEventArgs e)
    {
        sWImage.sprite = e.sWSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
