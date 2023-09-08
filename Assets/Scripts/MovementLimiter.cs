using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLimiter : MonoBehaviour
{
    public static MovementLimiter instance;

    [SerializeField] bool _initialCharacterCanMove = true;
    public bool CharacterCanMove;

    private void OnEnable()
    {
        instance = this;
    }

    private void Start()
    {
        CharacterCanMove = _initialCharacterCanMove;
    }
}
