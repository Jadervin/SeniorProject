using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed;
    public float despawnTime;

    float timeAlive = 0;

    public LayerMask groundLayer;
    public LayerMask wallLayer;

    public const string WALLTAG = "Wall";
    public const string GROUNDTAG = "Ground";


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
        if(collision.gameObject.layer == groundLayer || collision.gameObject.layer == wallLayer)
        {
            Destroy(this.gameObject);
        }
    }
}
