using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBackgroundScroll : MonoBehaviour
{
    //[SerializeField] private Vector2 parallaxEffectMultiplier;
    [SerializeField] private Vector2 parallaxMovementNumber;
    [SerializeField] private GameObject duplicateImage;
    [SerializeField] private GameObject cameraSpawnCollider;
    [SerializeField] private GameObject cameraDeleteCollider;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void LateUpdate()
    {
        //Vector3 deltaMovement = lastCameraPosition - cameraTransform.position;
        transform.position -= new Vector3(parallaxMovementNumber.x, parallaxMovementNumber.y, transform.position.z);
        //lastCameraPosition = cameraTransform.position;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagReferencesScript.SPAWN))
        {
            Debug.Log("Checked on Trigger");
            SpawnCopyImage();

        }
        
        if (collision.gameObject.CompareTag(TagReferencesScript.DELETE))
        {
            Destroy(this.gameObject);
        }
        
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagReferencesScript.SPAWN))
        {
            Debug.Log("Checked on Collision");
            SpawnCopyImage();

        }
    }*/

    private void SpawnCopyImage()
    {
        GameObject temp = Instantiate(duplicateImage, cameraSpawnCollider.transform.position, Quaternion.identity);
        
    }

}
