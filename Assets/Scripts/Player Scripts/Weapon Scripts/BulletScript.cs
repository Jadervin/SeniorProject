using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum BulletTypes
{
    BASE,
    GRENADE
}


public class BulletScript : MonoBehaviour
{
    [Header("Universal Bullet Variables")]
    [SerializeField] private float speed;
    
    //private float timeAlive = 0;
    public string ENEMYTAG = "Enemy";
    [SerializeField] private int bulletDamage = 5;

 
    public BulletTypes bulletType;
    public GameObject bulletSprite;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private Collider2D bulletCollider;

    [SerializeField] private LayerMask whatDestroysObject;


    [Header("Base Bullet Variables")]
    [SerializeField] private float despawnTime;


    [Header("Grenade Bullet Variables")]
    [SerializeField] private GameObject blastRadiusPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        //blastRadiusGO = GetComponentInChildren<AfterShockScript>().gameObject;
        //bulletCollider = GetComponent<Collider2D>();
        if (bulletType == BulletTypes.BASE)
        {
            SetDestroyTime();
        }

        

        SetVelocity();
    }

    // Update is called once per frame
    void Update()
    {
/*        transform.position += transform.right * speed * Time.deltaTime;

        timeAlive += Time.deltaTime;

        if (timeAlive > despawnTime)
        {
            Destroy(this.gameObject);
        }*/
    }

    private void FixedUpdate()
    {
        if (bulletType == BulletTypes.GRENADE)
        {
            transform.right = rb.velocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag(ENEMYTAG))
        {
            collision.gameObject.GetComponent<EnemyScript>().DamageHealth(bulletDamage);

            if (bulletType == BulletTypes.GRENADE)
            {
                SpawnAftershock();
            }
            Destroy(this.gameObject);
            
            //Destroy(this.gameObject);
        }

/*        if (collision.gameObject.CompareTag(GROUNDTAG))
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag(WALLTAG))
        {
            Destroy(this.gameObject);
        }*/


        if ((whatDestroysObject.value & (1 << collision.gameObject.layer)) > 0)
        {
            if (bulletType == BulletTypes.GRENADE)
            {
                SpawnAftershock();
            }
            Destroy(this.gameObject);
            
        }

    }

    private void SetVelocity()
    {
        rb.velocity = transform.right * speed;
    }


    private void SetDestroyTime()
    {
        Destroy(this.gameObject, despawnTime);
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float GetRBGravity()
    {
        return rb.gravityScale;
    }


    private void SpawnAftershock()
    {
        GameObject temp = Instantiate(blastRadiusPrefab, this.transform.position, this.transform.rotation);
    }
}
