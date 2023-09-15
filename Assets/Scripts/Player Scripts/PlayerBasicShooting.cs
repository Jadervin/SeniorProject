using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasicShooting : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    private float timeSinceShooting = 0f;
    [SerializeField] private float cooldownTimerMax = 1f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform shootPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
