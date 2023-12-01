using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public List<GameObject> colliderList = new List<GameObject>();
    public LayerMask EnemyLayerMask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!colliderList.Contains(collision.gameObject) && collision.gameObject.CompareTag(TagReferencesScript.ENEMYTAG))
        {
            colliderList.Add(collision.gameObject);
            Debug.Log("Added " + gameObject.name);
            Debug.Log("GameObjects in list: " + colliderList.Count);
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!colliderList.Contains(collision.gameObject) && collision.gameObject.CompareTag(TagReferencesScript.ENEMYTAG))
        {
            colliderList.Add(collision.gameObject);
            Debug.Log("Added " + gameObject.name);
            Debug.Log("GameObjects in list: " + colliderList.Count);
        }
    }
*/
    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (!colliderList.Contains(collision.gameObject) && collision.gameObject.CompareTag(TagReferencesScript.ENEMYTAG))
        {
            colliderList.Add(collision.gameObject);
            Debug.Log("Added " + gameObject.name);
            Debug.Log("GameObjects in list: " + colliderList.Count);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!colliderList.Contains(collision.gameObject) && collision.gameObject.CompareTag(TagReferencesScript.ENEMYTAG))
        {
            colliderList.Add(collision.gameObject);
            Debug.Log("Added " + gameObject.name);
            Debug.Log("GameObjects in list: " + colliderList.Count);
        }
    }*/

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        MyCollisions();
    }


    void MyCollisions()
    {
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, EnemyLayerMask);
        int i = 0;
        //Check when there is a new collider coming into contact with the box
        while (i < hitColliders.Length)
        {
            //Output all of the collider names
            Debug.Log("Hit : " + hitColliders[i].name + i);
            //Increase the number of Colliders in the array
            i++;
        }
    }


    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
