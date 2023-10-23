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
    //private float timeAlive = 0;
    public string ENEMYTAG = "Enemy";

    [SerializeField] private LayerMask whatDestroysObject;
    [SerializeField] private Rigidbody2D rb;
    public BulletTypes bulletTypes;

    [Header("Base Bullet Variables")]
    [SerializeField] private int bulletDamage = 5;


    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();

        if (bulletTypes == BulletTypes.BASE)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag(ENEMYTAG))
        {
            collision.gameObject.GetComponent<EnemyScript>().DamageHealth(bulletDamage);
            Destroy(this.gameObject);
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
}
