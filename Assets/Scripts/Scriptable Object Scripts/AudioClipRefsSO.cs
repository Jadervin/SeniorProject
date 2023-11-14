using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class AudioClipRefsSO : ScriptableObject
{
    [Header("Player Sounds")]
    public AudioClip jump;
    public AudioClip baseShoot;
    public AudioClip upgradedShoot;
    public AudioClip grenadeThrow;
    //public AudioClip aftershock;
    //public AudioClip flamethrower;
    public AudioClip flareShot;
    public AudioClip healing;
    public AudioClip hurt;
    public AudioClip itemGet;
    public AudioClip tongueCounter;

    [Header("Enemy Sounds")]
    public AudioClip dash;
    public AudioClip slash;
}
