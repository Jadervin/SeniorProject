using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLimiter : MonoBehaviour
{
    
    public static MovementLimiter instance;

    [SerializeField] private bool _initialCharacterCanMove = true;
    public bool characterCanMove;

    [SerializeField] private bool _initialCharacterCanBasicShoot = true;
    public bool characterCanBasicShoot;

    [SerializeField] private bool _initialCharacterCanSpecialShoot = true;
    public bool characterCanSpecialShoot;
    public bool isPlayerDead = false;

    private void OnEnable()
    {
        instance = this;
    }

    private void Start()
    {
        characterCanMove = _initialCharacterCanMove;
        characterCanBasicShoot = _initialCharacterCanBasicShoot;
        characterCanSpecialShoot = _initialCharacterCanSpecialShoot;
    }

    /*public void OnKnockbackBegin()
    {
        characterCanMove = false;
    }

    public void OnKnockbackDone()
    {
        characterCanMove = true;
    }*/

    /*private void Update()
    {
        Debug.Log(characterCanMove);
    }*/

    public void OnDeathManager()
    {
        characterCanMove = false;
        isPlayerDead = true;
        GameSceneManager.Instance.SetGameStateToDeath();
    }


}
