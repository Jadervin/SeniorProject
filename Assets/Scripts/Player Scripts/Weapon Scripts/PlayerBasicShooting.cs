using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public enum BasicShootingUpgradeStates
{
    BASE,
    TRISHOT,

}


public class PlayerBasicShooting : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject longBullet;
    /*[SerializeField]*/ private float timeSinceShooting = 0f;
    [SerializeField] private float cooldownTimeMax = .3f;
    //[SerializeField] private GameInput gameInput;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private bool canShoot = true;
    [SerializeField] private bool shotBullet;

    [SerializeField] private BasicShootingUpgradeStates basicShootingState;

    [Header("Tri Shot Variables")]
    [SerializeField] private List<Transform> shootPoints = new List<Transform>();



    public event EventHandler OnBaseShootPerformed;
    public event EventHandler OnUpgradedShootPerformed;
    
    //public event EventHandler OnShootPerformed;
    //public event EventHandler OnShootStopped;
   


    // Start is called before the first frame update
    void Start()
    {
        GameInput.Instance.OnShootPressed += GameInput_OnShootPressed;
        if (SaveSystem.SaveFileCheck() == false)
        {
            basicShootingState = BasicShootingUpgradeStates.BASE;
        }
    }

    private void GameInput_OnShootPressed(object sender, System.EventArgs e)
    {
        if(canShoot == true && MovementLimiter.instance.playerActionStates == PlayerActionStates.NOACTION)
        {

            if (basicShootingState == BasicShootingUpgradeStates.BASE)
            {
                BasicShoot();

            }

            if(basicShootingState == BasicShootingUpgradeStates.TRISHOT)
            {
                TriShotShoot();
            }


            canShoot = false;
            
            //OnShootPerformed?.Invoke(this, EventArgs.Empty);

            shotBullet = !canShoot;
            MovementLimiter.instance.IsBasicShooting();
        }
       
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if (canShoot == false)
        {
            timeSinceShooting += Time.deltaTime;

            if (timeSinceShooting > cooldownTimeMax)
            {
                timeSinceShooting = 0;
                canShoot = true;
                
                //OnShootStopped?.Invoke(this, EventArgs.Empty);

                shotBullet = !canShoot;
                MovementLimiter.instance.IsNotDoingAnything();
            }
        }
        
    }

    private void BasicShoot()
    {
        GameObject temp;
            
        temp = Instantiate(bullet, shootPoint.transform.position, shootPoint.transform.rotation);

        OnBaseShootPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void TriShotShoot()
    {
        foreach (Transform shootPoint in shootPoints)
        {
            GameObject temp;

            temp = Instantiate(longBullet, shootPoint.transform.position, shootPoint.transform.rotation);

            OnUpgradedShootPerformed?.Invoke(this, EventArgs.Empty);
        }

       /* GameObject temp1;
        GameObject temp2;
        GameObject temp3;

        temp1 = Instantiate(bullet, shootPoints[0].transform.position, shootPoint.transform.rotation);
        temp2 = Instantiate(bullet, shootPoints[1].transform.position, shootPoint.transform.rotation);
        temp3 = Instantiate(bullet, shootPoints[2].transform.position, shootPoint.transform.rotation);*/
    }

    public void UpgradeBaseWeapon()
    {
        basicShootingState = BasicShootingUpgradeStates.TRISHOT;
    }

    public BasicShootingUpgradeStates GetBaseShootingState()
    {
        return basicShootingState;
    }

    public void SetBaseShootingState(BasicShootingUpgradeStates baseShootState)
    {
        basicShootingState = baseShootState;
    }

    public bool GetShotBullet()
    {
        return shotBullet;
    }

}
