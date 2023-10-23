using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLineScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SparkGrenadeScript sparkGrenade;
    [SerializeField] private BulletScript grenade;
    private LineRenderer lineRenderer;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileRBGravity;
    [SerializeField] private Transform grenadeShootPoint;




    [Header("Trajectory Line Segment/Length")]
    [SerializeField] private int segmentCount = 50;
    [SerializeField] private float curveLength = 3.5f;


    private Vector2[] segments;

    private const float TIME_CURVE_ADITION = .5f;

    

    private void Start()
    {
        //initialize segments
        segments = new Vector2[segmentCount];

        //Grab line renderer component and set its number of points
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segmentCount;

        //sparkGrenade = GetComponentInParent<SparkGrenadeScript>();

        grenade = sparkGrenade.GetGrenade().GetComponent<BulletScript>();

        projectileSpeed = grenade.GetSpeed();
        projectileRBGravity = grenade.GetRBGravity();
    }



    private void Update()
    {
        Vector2 startPos = grenadeShootPoint.position;

        segments[0] = startPos;

        lineRenderer.SetPosition(0, startPos);

        Vector2 startVelocity = transform.right * projectileSpeed;


        for(int i = 1;  i < segmentCount; i++)
        {
            float timeOffset = (i * Time.fixedDeltaTime * curveLength);

            Vector2 gravityOffset = TIME_CURVE_ADITION* Physics2D.gravity * projectileRBGravity * Mathf.Pow(timeOffset, 2);

            segments[i] = segments[0] + startVelocity * timeOffset + gravityOffset;

            lineRenderer.SetPosition(i, segments[i]);
        }


    }
}
