using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float knockbackStrength = 16;
    [SerializeField] private float delayTime = .3f;
    [SerializeField] private PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth.OnPlayerKnockbackAction += PlayerHealth_OnPlayerKnockbackAction;
    }

    private void PlayerHealth_OnPlayerKnockbackAction(object sender, PlayerHealth.OnKnockbackEventArgs e)
    {
        StopAllCoroutines();

        //Debug.Log(transform.position);
        //Debug.Log(e.collidedGameObject.transform.position);

        //Debug.Log(transform.position - e.collidedGameObject.transform.position);

        Vector2 knockbackDirection = (transform.position - e.collidedGameObject.transform.position).normalized;
        Debug.Log(knockbackDirection);
        rb.AddForce(knockbackDirection * knockbackStrength, ForceMode2D.Impulse);
        //MovementLimiter.instance.OnKnockbackBegin();
        StartCoroutine(Reset());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delayTime);
        rb.velocity = Vector2.zero;
        //MovementLimiter.instance.OnKnockbackDone();
    }
}
