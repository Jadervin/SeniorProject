using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareShotParticleScript : MonoBehaviour
{
    [SerializeField] private int bulletDamage = 1;
    [SerializeField] protected bool hitWithParticle = false;
    [SerializeField] private float cooldownTimeMax = 1f;
    [SerializeField] private float timeSinceHit = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hitWithParticle == true)
        {
            timeSinceHit += Time.deltaTime;


            if (timeSinceHit > cooldownTimeMax)
            {
                timeSinceHit = 0;
               
                hitWithParticle = false;

            }
        }
    }

    public void SetCooldownTime(float cooldownTime)
    {
        cooldownTimeMax = cooldownTime;
    }

    public int GetDamage()
    { 
        return bulletDamage; 
    }

    public bool GetHitWithParticleBool()
    {
        return hitWithParticle;
    }
    public void SetHitWithParticleBool(bool hit)
    {
        hitWithParticle = hit;
    }
}
