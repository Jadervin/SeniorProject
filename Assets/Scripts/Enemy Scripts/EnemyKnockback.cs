using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    //[SerializeField] private float knockbackStrength = 16;
    [SerializeField] private float delayTime = .3f;
    [SerializeField] private EnemyScript enemyScript;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyScript = GetComponent<EnemyScript>();

        //enemyScript.OnEnemyKnockbackAction += EnemyScript_OnEnemyKnockbackAction;
    }
    /*
    private void EnemyScript_OnEnemyKnockbackAction(object sender, EnemyScript.OnKnockbackEventArgs e)
    {
        //StopAllCoroutines();
        Vector2 knockbackDirection = (transform.position - e.collidedGameObject.transform.position).normalized;

        Debug.Log(knockbackDirection);

        rb.AddForce(knockbackDirection * knockbackStrength, ForceMode2D.Impulse);
        
        StartCoroutine(StunDelay());
        enemyScript.StunEnemy();
    }
    */
    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator StunDelay()
    {
        yield return new WaitForSeconds(delayTime);
        rb.velocity = Vector2.zero;
        
    }


}
