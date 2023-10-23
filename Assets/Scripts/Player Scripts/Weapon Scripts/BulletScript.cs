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
    [SerializeField] private float despawnTime;
    [SerializeField] private float afterShockDespawnTime;
    //private float timeAlive = 0;
    public string ENEMYTAG = "Enemy";
    [SerializeField]
    private Collider2D bulletCollider;

    [SerializeField] private LayerMask whatDestroysObject;
    [SerializeField] private Rigidbody2D rb;
    public BulletTypes bulletType;
    public GameObject bulletSprite;

    [Header("Base Bullet Variables")]
    [SerializeField] private int bulletDamage = 5;

    [Header("Grenade Bullet Variables")]
    [SerializeField] private GameObject blastRadiusGO;

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

        if(blastRadiusGO != null)
        {
            blastRadiusGO.SetActive(false);
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

            if (bulletType == BulletTypes.BASE)
            {
                Destroy(this.gameObject);
            }

            else if (bulletType == BulletTypes.GRENADE)
            {
                bulletCollider.enabled = false;

                bulletSprite.SetActive(false);
                rb.velocity = Vector3.zero;
                rb.gravityScale = 0f;
                blastRadiusGO.SetActive(true);
                Destroy(this.gameObject, afterShockDespawnTime);

            }
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
            if (bulletType == BulletTypes.BASE)
            {
                Destroy(this.gameObject);
            }

            else if (bulletType == BulletTypes.GRENADE)
            {
                bulletCollider.enabled = false;


                bulletSprite.SetActive(false);
                rb.velocity = Vector3.zero;
                rb.gravityScale = 0f;
                blastRadiusGO.SetActive(true);
                Destroy(this.gameObject, afterShockDespawnTime);

            }
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
}
