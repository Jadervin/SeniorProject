using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareShotScript : MonoBehaviour
{
    //[SerializeField] private GameObject projectile;
    [SerializeField] private ParticleSystem flareShotParticle;
    /*[SerializeField]*/ private float timeSinceShooting = 0f;
    [SerializeField] private float timeToBasicShootMax = .2f;
    [SerializeField] private float cooldownTimeMax = 1f;
    //[SerializeField] private GameInput gameInput;
    //[SerializeField] private Transform shootPoint;
    [SerializeField] private bool canSpecialShoot = true;
    [SerializeField] private float energyUsed = 1f;


    // Start is called before the first frame update
    void Start()
    {
        GameInput.Instance.OnSpecialShootPressed += GameInput_OnSpecialShootPressed;
        flareShotParticle.GetComponent<FlareShotParticleScript>().SetCooldownTime(cooldownTimeMax);


    }

    private void GameInput_OnSpecialShootPressed(object sender, System.EventArgs e)
    {
        if (canSpecialShoot == true)
        {
            FlareShot();
            canSpecialShoot = false;
            MovementLimiter.instance.characterCanBasicShoot = false;
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
