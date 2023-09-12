using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGround : MonoBehaviour
{
    private bool onGround;

    [Header("Collider Settings")]

    //The length of the ground-checking collider
    [SerializeField] private float groundLength = 0.95f;

    //The distance between the ground-checking colliders
    [SerializeField] private Vector3 colliderOffset;

    [Header("Layer Masks")]
    //Which layers are read as the ground
    [SerializeField] private LayerMask groundLayer;


    private void Update()
    {
        //Determine if the player is stood on objects on the ground layer, using a pair of raycasts. This only works if the boxCollider2D is turned on as this script is not meant to act as a collider, just a way to detect if the player is collided with the ground.
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
    }

    private void OnDrawGizmos()
    {
        //Draw the ground colliders on screen for debug purposes
        if (onGround) 
        { 
            Gizmos.color = Color.green; 
        } 
        else 
        { 
            Gizmos.color = Color.red; 
        }

        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }

    //Send ground detection to other scripts
    public bool GetOnGround() { return onGround; }

    private void OnCollisionEnter(Collision other)
    {

    }
}
