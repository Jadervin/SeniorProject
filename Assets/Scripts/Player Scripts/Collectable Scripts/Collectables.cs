using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectables : MonoBehaviour
{
    [SerializeField] protected GameObject player;
    [SerializeField] protected string collectableID = "";
    public static event EventHandler<OnCollectableGetEventArgs> OnCollectableGet;

    public class OnCollectableGetEventArgs : EventArgs
    {
        public GameObject collectable;
    }

    public static void ResetStaticData()
    {
        OnCollectableGet = null;
    }

    // Start is called before the first frame update
    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagReferencesScript.PLAYERTAG).gameObject;
    }


    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            Interact();

            OnCollectableGet?.Invoke(this, new OnCollectableGetEventArgs
            {
                collectable = this.gameObject
            });



            Destroy(this.gameObject);

            //this.gameObject.SetActive(false);
        }
    }

    public string GetCollectableID()
    {
        return collectableID;
    }

    public abstract void Interact();
}
