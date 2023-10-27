using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerActionStates
{
    NOACTION,
    BASESHOOTING,
    SPECIALSHOOTING,
    COUNTERING
}

public class MovementLimiter : MonoBehaviour
{
    
    public static MovementLimiter instance;

    public PlayerActionStates playerActionStates;

    [SerializeField] private bool _initialCharacterCanMove = true;
    public bool characterCanMove;

    [SerializeField] private bool _initialCharacterCanBasicShoot = true;
    public bool characterCanBasicShoot;

    [SerializeField] private bool _initialCharacterCanSpecialShoot = true;
    public bool characterCanSpecialShoot;


    [SerializeField] private bool _initialCharacterCanCounter = true;
    public bool characterCanCounter;

    [SerializeField] private bool _initialCharacterIsDead = false;
    public bool CharacterIsDead = false;

    private void OnEnable()
    {
        instance = this;
    }

    private void Start()
    {
        characterCanMove = _initialCharacterCanMove;
        characterCanBasicShoot = _initialCharacterCanBasicShoot;
        characterCanSpecialShoot = _initialCharacterCanSpecialShoot;
        characterCanCounter = _initialCharacterCanCounter;
        CharacterIsDead = _initialCharacterIsDead;
    }

    /*public void OnKnockbackBegin()
    {
        characterCanMove = false;
    }

    public void OnKnockbackDone()
    {
        characterCanMove = true;
    }*/

    private void Update()
    {
        switch (playerActionStates)
        {
            case PlayerActionStates.NOACTION:
                characterCanBasicShoot = true;
                characterCanSpecialShoot = true;
                characterCanCounter = true;
                break;
            case PlayerActionStates.BASESHOOTING:
                characterCanSpecialShoot = false;
                characterCanCounter = false;
                break;
            case PlayerActionStates.SPECIALSHOOTING:
                characterCanBasicShoot = false;
                characterCanCounter = false;
                break;
            case PlayerActionStates.COUNTERING: 
                characterCanSpecialShoot= false;
                characterCanBasicShoot = false;
                break;
            default: 
                break;

        }
    }

    public void IsBasicShooting()
    {
        playerActionStates = PlayerActionStates.BASESHOOTING;
    }

    public void IsSpecialShooting()
    {
        playerActionStates = PlayerActionStates.SPECIALSHOOTING;
    }

    public void IsCountering()
    {
        playerActionStates = PlayerActionStates.COUNTERING;
    }
    public void IsNotDoingAnything()
    {
        playerActionStates = PlayerActionStates.NOACTION;

    }
    

    public void OnDeathManager()
    {
        characterCanMove = false;
        CharacterIsDead = true;
        GameSceneManager.Instance.SetGameState(GameStates.GameOver);
    }


}
