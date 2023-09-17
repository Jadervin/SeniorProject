using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : EntityScript
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnDeath()
    {
        this.gameObject.SetActive(false);
        MovementLimiter.instance.OnDeathManager();
        //Destroy(gameObject);
    }

}
