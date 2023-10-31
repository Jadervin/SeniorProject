using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillStationScript : MonoBehaviour
{
    [SerializeField] private Collider2D switchCollider;

    [SerializeField] private bool refilledEnergy = false;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private SpecialWeaponManagerScript specialWeaponScript;



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

            playerHealth.HealthRefill();
            specialWeaponScript.RechargeEnergy();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        refilledEnergy = false;
    }
}
