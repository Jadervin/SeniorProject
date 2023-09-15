using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float despawnTime;
    [SerializeField] private int bulletDamage = 5;
    private float timeAlive = 0;
    public string ENEMYTAG = "Enemy";


   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;

        timeAlive += Time.deltaTime;

        if (timeAlive > despawnTime)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag(ENEMYTAG))
        {
            collision.gameObject.GetComponent<EnemyScript>().DamageHealth(bulletDamage);
        }
        
        Destroy(this.gameObject);


        
    }
}
