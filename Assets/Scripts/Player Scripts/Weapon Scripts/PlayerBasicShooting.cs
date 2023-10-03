using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerBasicShooting : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    /*[SerializeField]*/ private float timeSinceShooting = 0f;
    [SerializeField] private float cooldownTimeMax = .3f;
    //[SerializeField] private GameInput gameInput;
    [SerializeField] private Transform shootPoint;
    /*[SerializeField]*/ private bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        GameInput.Instance.OnShootPressed += GameInput_OnShootPressed;
    }

    private void GameInput_OnShootPressed(object sender, System.EventArgs e)
    {
        if(canShoot == true)
        {
            BasicShoot();
            canShoot = false;
            MovementLimiter.instance.characterCanSpecialShoot = false;
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
                MovementLimiter.instance.characterCanSpecialShoot = true;
            }
        }
        
    }

    private void BasicShoot()
    {
        GameObject temp;
            
        temp = Instantiate(bullet, shootPoint.transform.position, shootPoint.transform.rotation);

    }
}
