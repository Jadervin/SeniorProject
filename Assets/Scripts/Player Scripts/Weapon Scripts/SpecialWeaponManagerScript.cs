using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpecialWeaponManagerScript : MonoBehaviour
{
    //[SerializeField] private GameObject specialWeaponScriptParent;
    //[SerializeField] private GameObject spawnedSpecialWeapon;

    [SerializeField] private int maxWeaponEnergy = 10;
    [SerializeField] private int currentWeaponEnergy;


    [SerializeField] private GameInput gameInput;

    //[SerializeField] private List<SpecialWeaponSO> specialWeaponSOList = new List<SpecialWeaponSO>();

    [SerializeField] private List<GameObject> specialWeaponList = new List<GameObject>();
    [SerializeField] private GameObject currentSpecialWeapon;


    //[SerializeField] private SpecialWeaponSO currentSpecialWeapon;
    [SerializeField] private int currentWeaponIndex = 0;

    public bool switchedWeapon;

    [SerializeField] private float timeSinceSwitch = 0f;
    [SerializeField] private float cooldownTimeMax = .5f;


    public event EventHandler<OnEnergyChangedEventArgs> OnEnergyChanged;

    public class OnEnergyChangedEventArgs:EventArgs
    {
        public float energyNormalized;
        public int energy;
    }

    public event EventHandler<OnCurrentSWChangedEventArgs> OnCurrentSWChanged;

    public class OnCurrentSWChangedEventArgs : EventArgs
    {
        public Sprite sWSprite;

    }

    //public static SpecialWeaponScript Instance { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        currentWeaponIndex = 0;
        gameInput = FindAnyObjectByType<GameInput>();

        gameInput.OnSpecialWeaponSwitch += Instance_OnSpecialWeaponSwitch;
        currentSpecialWeapon = specialWeaponList[0];
        currentWeaponEnergy = maxWeaponEnergy;

        foreach (var weapon in specialWeaponList)
        {
            weapon.SetActive(false);
        }

        EnableWeapon();

        //SpawnWeapon();

    }

    private void Instance_OnSpecialWeaponSwitch(object sender, EventArgs e)
    {
        ChangeWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        if (switchedWeapon == true)
        {
            timeSinceSwitch += Time.deltaTime;

            if (timeSinceSwitch > cooldownTimeMax)
            {
                timeSinceSwitch = 0;
                switchedWeapon = false;
                

            }
        }
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
            energyNormalized = (float)currentWeaponEnergy / maxWeaponEnergy,
            energy = currentWeaponEnergy
        });
    }

    public void IncreaseCurrentWeaponEnergy(int weaponEnergy)
    {
        currentWeaponEnergy += weaponEnergy;

        OnEnergyChanged?.Invoke(this, new OnEnergyChangedEventArgs
        {
            energyNormalized = (float)currentWeaponEnergy / maxWeaponEnergy,
            energy = currentWeaponEnergy
        });
    }

    public void RechargeEnergy()
    {
        currentWeaponEnergy = maxWeaponEnergy;

        OnEnergyChanged?.Invoke(this, new OnEnergyChangedEventArgs
        {
            energyNormalized = (float)currentWeaponEnergy / maxWeaponEnergy,
            energy = currentWeaponEnergy
        });
    }

    public void IncreaseMaxWeaponEnergy(int weaponEnergy) 
    {
        maxWeaponEnergy += weaponEnergy;
        currentWeaponEnergy = maxWeaponEnergy;

        OnEnergyChanged?.Invoke(this, new OnEnergyChangedEventArgs
        {
            energyNormalized = (float)currentWeaponEnergy / maxWeaponEnergy,
            energy = currentWeaponEnergy
        });
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
        switchedWeapon = true;
        if (gameInput.GetSpecialWeaponSwitchDirection() > Mathf.Epsilon)
        {

            DisableWeapon();
            //DeleteWeapon();
            currentWeaponIndex++;

            if(currentWeaponIndex > specialWeaponList.Count - 1)
            {
                currentWeaponIndex = 0;
            }

            currentSpecialWeapon = specialWeaponList[currentWeaponIndex];


            //SpawnWeapon();

            EnableWeapon();
        }
        else
        {
            DisableWeapon();

            //DeleteWeapon();
            currentWeaponIndex--;

            if (currentWeaponIndex < 0)
            {
                currentWeaponIndex = 1;
            }

            currentSpecialWeapon = specialWeaponList[currentWeaponIndex];


            //SpawnWeapon();


            EnableWeapon();
        }



        
    }

    public GameObject GetCurrentSpecialWeapon()
    {
        return currentSpecialWeapon;
    }

    /*
     public void SpawnWeapon()
    {
        spawnedSpecialWeapon = Instantiate(specialWeaponSOList[currentWeaponIndex].specialWeaponGO, transform.position, transform.rotation);

        spawnedSpecialWeapon.transform.parent = specialWeaponScriptParent.transform;
    }*/

    /*
     public void DeleteWeapon()
    {
        spawnedSpecialWeapon.SetActive(false);
        Destroy(spawnedSpecialWeapon);
    }*/

    public void DisableWeapon()
    {
        specialWeaponList[currentWeaponIndex].GetComponent<SpecialWeaponEntityScript>().ResetRechargeTimer();
        specialWeaponList[currentWeaponIndex].SetActive(false);
        //Destroy(spawnedSpecialWeapon);
    }
    public void EnableWeapon()
    {
        specialWeaponList[currentWeaponIndex].SetActive(true);
        //Destroy(spawnedSpecialWeapon);

        OnCurrentSWChanged?.Invoke(this, new OnCurrentSWChangedEventArgs
        {
            sWSprite = currentSpecialWeapon.GetComponent<SpecialWeaponEntityScript>().GetSpecialWeaponSOReference().swIcon

        });
        
    }

}
