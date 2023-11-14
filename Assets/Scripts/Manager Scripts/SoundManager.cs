using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    [SerializeField] private GameObject playerRef;

    private float volume = 1f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        playerRef = GameObject.FindGameObjectWithTag(TagReferencesScript.PLAYERTAG);
    }

    // Start is called before the first frame update
    private void Start()
    {
        playerRef.GetComponent<PlayerJump>().OnJumpPerformed += PlayerJump_OnJumpPerformed;
        playerRef.GetComponentInChildren<PlayerBasicShooting>().OnBaseShootPerformed += BasicShooting_OnBaseShootPerformed;
        playerRef.GetComponentInChildren<PlayerBasicShooting>().OnUpgradedShootPerformed += BasicShooting_OnUpgradedShootPerformed;
        //playerRef.GetComponentInChildren<SpecialWeaponManagerScript>().gameObject.GetComponentInChildren<FlareShotScript>()
        SpecialWeaponManagerScript.instance.gameObject.GetComponentInChildren<FlareShotScript>().OnFlareShotPerformed += FlareShot_OnFlameShotPerformed;
        SpecialWeaponManagerScript.instance.gameObject.GetComponentInChildren<SparkGrenadeScript>().OnGrenadeThrowPerformed += SparkGrenade_OnGrenadeThrowPerformed;
        playerRef.GetComponentInChildren<PlayerTongueCounter>().OnTongueCounterPerformed += TongueCounter_OnTongueCounterPerformed;

        playerRef.GetComponent<PlayerHealth>().OnPlayerDamaged += PlayerHealth_OnPlayerDamaged;
    }

    private void PlayerHealth_OnPlayerDamaged(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.hurt, playerRef.transform.position, volume);
    }

    private void TongueCounter_OnTongueCounterPerformed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.tongueCounter, playerRef.transform.position, volume);
    }

    private void SparkGrenade_OnGrenadeThrowPerformed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.grenadeThrow, playerRef.transform.position, volume);
    }

    private void FlareShot_OnFlameShotPerformed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.flareShot, playerRef.transform.position, volume);
    }

    private void BasicShooting_OnUpgradedShootPerformed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.upgradedShoot, playerRef.transform.position, volume);
    }

    private void BasicShooting_OnBaseShootPerformed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.baseShoot, playerRef.transform.position, volume);
    }

    private void PlayerJump_OnJumpPerformed(object sender, System.EventArgs e)
    {

        //PlayerJump playerJump = sender as PlayerJump;
        PlaySound(audioClipRefsSO.jump, playerRef.transform.position, volume);
        
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultipler = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultipler * volume);
    }


}
