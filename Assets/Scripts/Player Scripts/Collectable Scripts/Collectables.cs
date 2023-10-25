using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectables : MonoBehaviour
{
    [SerializeField] protected GameObject player;
    

    // Start is called before the first frame update
    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagReferencesScript.PLAYERTAG).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            Interact();
            Destroy(this.gameObject);
        }
    }

    public abstract void Interact();
}
