using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour
{
    public event EventHandler<OnKnockbackEventArgs> OnEnemyKnockbackAction;
    public class OnKnockbackEventArgs : EventArgs
    {
        public GameObject collidedGameObject;
    }

    [SerializeField] private EnemyScript enemy;
    [SerializeField] private int damage;

    public static event EventHandler OnAnyEnemyCountered;


    public static void ResetStaticData()
    {
        OnAnyEnemyCountered = null;

    }


    public int GetDamage()
    {
        return damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagReferencesScript.TONGUE_COUNTER_TAG))
        {
            //Knockback enemy
            OnEnemyKnockbackAction?.Invoke(this, new OnKnockbackEventArgs
            {
                collidedGameObject = collision.gameObject
            });

            //enemyState = EnemyStates.STUNNED;

            OnAnyEnemyCountered?.Invoke(this, EventArgs.Empty);
            enemy.StunEnemy();
        }
    }
}
