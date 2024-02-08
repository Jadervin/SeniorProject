using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareShotScript : SpecialWeaponEntityScript
{
    //[SerializeField] private GameObject projectile;
    [SerializeField] private ParticleSystem flareShotParticle;
    [SerializeField] private PlayerMovement playerMove;

    public event EventHandler OnFlareShotPerformed;


    // Start is called before the first frame update
    new private void Start()
    {
        GameInput.Instance.OnSpecialShootPressed += GameInput_OnSpecialShootPressed;
        flareShotParticle.GetComponent<SpecialWeaponParticleScript>().SetCooldownTime(cooldownTimeMax);

        specialWeaponManagerScript = FindAnyObjectByType<SpecialWeaponManagerScript>();
        weaponEnergyCost = specialWeaponSOReference.specialWeaponEnergyCost;
        timeSinceShooting = 0f;
    }

    private void GameInput_OnSpecialShootPressed(object sender, System.EventArgs e)
    {
        if (canSpecialShoot == true && specialWeaponManagerScript.GetCurrentWeaponEnergy() >= weaponEnergyCost && specialWeaponManagerScript.GetCurrentSpecialWeapon() == this.gameObject
            && MovementLimiter.instance.playerActionStates == PlayerActionStates.NOACTION)
        {
            specialWeaponManagerScript.DecreaseCurrentWeaponEnergy(weaponEnergyCost);
            canSpecialShoot = false;
            MovementLimiter.instance.IsSpecialShooting();
            FlareShot();
            
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpecialShoot == false)
        {
            timeSinceShooting += Time.deltaTime;
            

            if (timeSinceShooting > timeToBasicShootMax)
            {
                //timeSinceShooting = 0;
                MovementLimiter.instance.IsNotDoingAnything();

            }

            if (timeSinceShooting > cooldownTimeMax)
            {
                timeSinceShooting = 0;
                canSpecialShoot = true;
                flareShotParticle.gameObject.SetActive(true);
                
            }
        }


        if(playerMove.GetIsFacingRight() == true)
        {
            flareShotParticle.GetComponent<ParticleSystemRenderer>().flip = new Vector3(1, 0, 0);
        }
        else
        {
            flareShotParticle.GetComponent<ParticleSystemRenderer>().flip = new Vector3(0, 0, 0);
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

        OnFlareShotPerformed?.Invoke(this, EventArgs.Empty);


        //Instantiate(projectile, projectile.transform.position, projectile.transform.rotation);

    }
}
