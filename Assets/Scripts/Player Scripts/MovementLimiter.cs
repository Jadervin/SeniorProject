using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLimiter : MonoBehaviour
{
    
    public static MovementLimiter instance;

    [SerializeField] private bool _initialCharacterCanMove = true;
    public bool characterCanMove;

    [SerializeField] private bool _initialCharacterCanShoot = true;
    public bool characterCanShoot;

    private void OnEnable()
    {
        instance = this;
    }

    private void Start()
    {
        characterCanMove = _initialCharacterCanMove;
        characterCanShoot = _initialCharacterCanShoot;
    }

    public void OnKnockbackBegin()
    {
        characterCanMove = false;
    }

    public void OnKnockbackDone()
    {
        characterCanMove = true;
    }

    public void OnDeathManager()
    {
        characterCanMove = false;
    }


}
