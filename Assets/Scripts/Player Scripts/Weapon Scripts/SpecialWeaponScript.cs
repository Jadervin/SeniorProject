using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpecialWeaponScript : MonoBehaviour
{
    [SerializeField] private GameObject specialWeaponScriptParent;
    [SerializeField] private GameObject spawnedSpecialWeapon;

    [SerializeField] private int maxWeaponEnergy = 10;
    [SerializeField] private int currentWeaponEnergy;


    [SerializeField] private GameInput gameInput;

    [SerializeField] private List<SpecialWeaponSO> specialWeaponSOList = new List<SpecialWeaponSO>();
    [SerializeField] private SpecialWeaponSO currentSpecialWeapon;
    [SerializeField] private int currentWeaponIndex = 0;
    

    public event EventHandler<OnEnergyChangedEventArgs> OnEnergyChanged;

    public class OnEnergyChangedEventArgs:EventArgs
    {
        public float energyNormalized;
    }

    //public static SpecialWeaponScript Instance { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        gameInput = FindAnyObjectByType<GameInput>();

        gameInput.OnSpecialWeaponSwitch += Instance_OnSpecialWeaponSwitch;
        currentSpecialWeapon = specialWeaponSOList[0];
        currentWeaponEnergy = maxWeaponEnergy;

        SpawnWeapon();

    }

    private void Instance_OnSpecialWeaponSwitch(object sender, EventArgs e)
    {
        ChangeWeapons();
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

        OnEnergyChanged?.Invoke(this, new OnEnergyChangedEventArgs
        {
            energyNormalized = (float)currentWeaponEnergy / maxWeaponEnergy
        });
    }

    public void RechargeEnergy()
    {
        currentWeaponEnergy = maxWeaponEnergy;

        OnEnergyChanged?.Invoke(this, new OnEnergyChangedEventArgs
        {
            energyNormalized = (float)currentWeaponEnergy / maxWeaponEnergy
        });
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


    public void ChangeWeapons()
    {
        if(gameInput.GetSpecialWeaponSwitchDirection() > Mathf.Epsilon)
        {

            DeleteWeapon();
            currentWeaponIndex++;

            if(currentWeaponIndex > specialWeaponSOList.Count - 1)
            {
                currentWeaponIndex = 0;
            }

            currentSpecialWeapon = specialWeaponSOList[currentWeaponIndex];


            SpawnWeapon();


        }
        else
        {
            DeleteWeapon();
            currentWeaponIndex--;

            if (currentWeaponIndex < 0)
            {
                currentWeaponIndex = 1;
            }

            currentSpecialWeapon = specialWeaponSOList[currentWeaponIndex];


            SpawnWeapon();
        }
    }

    public void SpawnWeapon()
    {
        spawnedSpecialWeapon = Instantiate(specialWeaponSOList[currentWeaponIndex].specialWeaponGO, transform.position, transform.rotation);

        spawnedSpecialWeapon.transform.parent = specialWeaponScriptParent.transform;
    }

    public void DeleteWeapon()
    {
        Destroy(spawnedSpecialWeapon);
    }
}
