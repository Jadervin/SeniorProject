using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDeleter : MonoBehaviour
{
    [SerializeField] private float spawnTime;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(spawnTime);
        Destroy(gameObject);
    }
}
