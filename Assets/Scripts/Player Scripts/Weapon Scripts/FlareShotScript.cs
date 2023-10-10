using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareShotScript : MonoBehaviour
{
    //[SerializeField] private GameObject projectile;
    [SerializeField] private ParticleSystem flareShotParticle;
    [SerializeField] private float timeSinceShooting = 0f;
    [SerializeField] private float timeToBasicShootMax = .2f;
    [SerializeField] private float cooldownTimeMax = 1f;
    //[SerializeField] private GameInput gameInput;
    //[SerializeField] private Transform shootPoint;
    [SerializeField] private bool canSpecialShoot = true;
    [SerializeField] private SpecialWeaponSO specialWeaponSOReference;

    public SpecialWeaponScript specialWeaponScript;


    [SerializeField] private int weaponEnergyCost = 3;


    // Start is called before the first frame update
    void Start()
    {
        GameInput.Instance.OnSpecialShootPressed += GameInput_OnSpecialShootPressed;
        flareShotParticle.GetComponent<SpecialWeaponParticleScript>().SetCooldownTime(cooldownTimeMax);

        specialWeaponScript = FindAnyObjectByType<SpecialWeaponScript>();
        weaponEnergyCost = specialWeaponSOReference.specialWeaponEnergyCost;
        timeSinceShooting = 0f;
    }

    private void GameInput_OnSpecialShootPressed(object sender, System.EventArgs e)
    {
        if (canSpecialShoot == true && specialWeaponScript.GetCurrentWeaponEnergy() >= weaponEnergyCost)
        {
            specialWeaponScript.DecreaseCurrentWeaponEnergy(weaponEnergyCost);
            canSpecialShoot = false;
            MovementLimiter.instance.characterCanBasicShoot = false;
            FlareShot();
            
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(specialWeaponScript.switchedWeapon == true)
        {
            timeSinceShooting = 0f;
            canSpecialShoot = true;
        }
        /*
        if(this.gameObject.activeSelf == false)
        {
            timeSinceShooting = 0f;
            canSpecialShoot = true;
        }
        */

        if (canSpecialShoot == false)
        {
            timeSinceShooting += Time.deltaTime;
            

            if (timeSinceShooting > timeToBasicShootMax)
            {
                //timeSinceShooting = 0;
                MovementLimiter.instance.characterCanBasicShoot = true;

            }

            if (timeSinceShooting > cooldownTimeMax)
            {
                timeSinceShooting = 0;
                canSpecialShoot = true;
                flareShotParticle.gameObject.SetActive(true);
                
            }
        }


    }


    private void FlareShot()
    {
        /*
        projectile.transform.rotation = new Quaternion(shootPoint.transform.rotation.x, projectile.transform.rotation.y, shootPoint.transform.rotation.z, projectile.transform.rotation.w);

        GameObject temp;
        temp = Instantiate(projectile, shootPoint.transform.position, projectile.transform.rotation);
        projectile.GetComponent<ParticleSystem>().Play();
*/

        flareShotParticle.Play();


        //Instantiate(projectile, projectile.transform.position, projectile.transform.rotation);

    }
}
