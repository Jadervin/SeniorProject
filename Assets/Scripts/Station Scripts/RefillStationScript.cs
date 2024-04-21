using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillStationScript : MonoBehaviour
{
    [SerializeField] private Collider2D switchCollider;

    [SerializeField] private bool refilledEnergy = false;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private SpecialWeaponManagerScript specialWeaponScript;

    public static event EventHandler OnAnyHeathRefill;

    [SerializeField] private GameObject healingEffectPrefab;
    [SerializeField] private List<GameObject> healingEffectSpawnSpots;

    public static void ResetStaticData()
    {
        OnAnyHeathRefill = null;
    }


    // Start is called before the first frame update
    void Start()
    {
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        specialWeaponScript = FindAnyObjectByType<SpecialWeaponManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagReferencesScript.PLAYERTAG) && refilledEnergy == false)
        {
            refilledEnergy = true;
            OnAnyHeathRefill?.Invoke(this, EventArgs.Empty);

            foreach (GameObject spawnSpots in healingEffectSpawnSpots)
            {
                GameObject temp = Instantiate(healingEffectPrefab, spawnSpots.transform.position, spawnSpots.transform.rotation);
            }

            playerHealth.HealthRefill();
            specialWeaponScript.RechargeEnergy();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        refilledEnergy = false;
    }
}
