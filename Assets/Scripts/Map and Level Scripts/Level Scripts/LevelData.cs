using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public List<GameObject> enemyColliderList = new List<GameObject>();
    private Collider2D[] hitColliders2D;

    public LayerMask EnemyLayerMask;
    public BoxCollider2D boxCollider2D;

    public bool GotEnemies;
    public Transform origin;


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagReferencesScript.PLAYERTAG))
        {
            if(enemyColliderList.Count > 0)
            {
                foreach (GameObject enemy in enemyColliderList)
                {
                    enemy.SetActive(true);

                    enemy.gameObject.GetComponentInChildren<EnemyScript>().ResetHealth();
                    enemy.gameObject.GetComponentInChildren<EnemyScript>().ResetEnemyState();

                    if(enemy.transform.GetChild(0).TryGetComponent<GroundStationaryAttackerEnemyScript>(out GroundStationaryAttackerEnemyScript stationaryEnemy))
                    {
                        //Debug.Log("Stationary Enemy Reset");
                        stationaryEnemy.ResetEnemyState();
                    }

                    if(enemy.transform.GetChild(0).TryGetComponent<FlyingStationaryChaserEnemyScript>(out FlyingStationaryChaserEnemyScript flyingEnemy))
                    {
                        //Debug.Log("Flying Enemy Reset");
                        flyingEnemy.ResetEnemyState();
                    }
                   
                }
            }
        }   
    }

    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (GotEnemies == false)
        {
            MyCollisions();
        }
    }


    void MyCollisions()
    {
        //Use the OverlapBox to detect if there are any other colliders within this box area.
        //Use the GameObject's centre, half the size (as a radius) and rotation. This creates an invisible box around your GameObject.
        hitColliders2D = Physics2D.OverlapBoxAll(origin.transform.position, boxCollider2D.size, 360.0f, EnemyLayerMask);

        foreach (var hit in hitColliders2D)
        {
            if(hit.gameObject.TryGetComponent<EnemyScript>(out EnemyScript enemy))
            {
                if (enemy.GetEnemyID() != null)
                {
                    enemyColliderList.Add(hit.transform.parent.gameObject);
                }
            }
        }

        
        GotEnemies = true;
    }


    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
        Gizmos.DrawWireCube(origin.transform.position, boxCollider2D.size);
        
    }
}
