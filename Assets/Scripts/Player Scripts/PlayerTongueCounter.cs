using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTongueCounter : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D polygonCollider;
    [SerializeField] private SpriteRenderer sprite;
    //[SerializeField] private GameInput gameInput;
    [SerializeField] private float tongueCounterTimer = 0.2f;
    [SerializeField] private float cooldownTime = 1f;
    private bool canCounter = true;
    private float timeSinceCounter = 0f;


    // Start is called before the first frame update
    void Start()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        polygonCollider.enabled = false;
        sprite.enabled = false;
        GameInput.Instance.OnTongueCounterPressed += GameInput_OnTongueCounterPressed;
    }

    private void GameInput_OnTongueCounterPressed(object sender, System.EventArgs e)
    {
        if (canCounter == true)
        {
            canCounter = false;
            MovementLimiter.instance.characterCanBasicShoot = false;
            MovementLimiter.instance.characterCanSpecialShoot = false;
            StartCoroutine(TongueCounter());
            

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canCounter == false)
        {
            timeSinceCounter += Time.deltaTime;

            if (timeSinceCounter > cooldownTime)
            {
                timeSinceCounter = 0;
                canCounter = true;
                

            }
        }
    }

    private IEnumerator TongueCounter()
    {
        polygonCollider.enabled = true;
        sprite.enabled = true;
        yield return new WaitForSeconds(tongueCounterTimer);
        polygonCollider.enabled = false;
        sprite.enabled = false;
        MovementLimiter.instance.characterCanBasicShoot = true;
        MovementLimiter.instance.characterCanSpecialShoot = true;

    }
}
