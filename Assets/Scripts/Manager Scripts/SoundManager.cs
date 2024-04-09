using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    [SerializeField] private GameObject playerRef;

    [SerializeField] private float volume = 1f;

    /*[SerializeField]*/ private AudioSource[] currentSoundEffectsArray = new AudioSource[0];

    [SerializeField] private AudioSource audioSource;

    //[SerializeField] private List<AudioSource> currentSoundEffectsList;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        volume = PlayerPrefs.GetFloat(TagReferencesScript.PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);

        playerRef = GameObject.FindGameObjectWithTag(TagReferencesScript.PLAYERTAG);
    }

    // Start is called before the first frame update
    private void Start()
    {
        playerRef.GetComponent<PlayerJump>().OnJumpPerformed += PlayerJump_OnJumpPerformed;
        playerRef.GetComponentInChildren<PlayerBasicShooting>().OnBaseShootPerformed += BasicShooting_OnBaseShootPerformed;
        playerRef.GetComponentInChildren<PlayerBasicShooting>().OnUpgradedShootPerformed += BasicShooting_OnUpgradedShootPerformed;
        
        SpecialWeaponManagerScript.instance.gameObject.GetComponentInChildren<FlareShotScript>().OnFlareShotPerformed += FlareShot_OnFlameShotPerformed;
        SpecialWeaponManagerScript.instance.gameObject.GetComponentInChildren<SparkGrenadeScript>().OnGrenadeThrowPerformed += SparkGrenade_OnGrenadeThrowPerformed;
        playerRef.GetComponentInChildren<PlayerTongueCounter>().OnTongueCounterPerformed += TongueCounter_OnTongueCounterPerformed;

        playerRef.GetComponent<PlayerHealth>().OnPlayerDamaged += PlayerHealth_OnPlayerDamaged;
        playerRef.GetComponent<PlayerHealth>().OnPlayerDead += PlayerHealth_OnPlayerDead;

        RefillStationScript.OnAnyHeathRefill += RefillStationScript_OnAnyHeathRefill;
        Collectables.OnAnyCollectableGet += Collectables_OnAnyCollectableGet;
        GroundStationaryAttackerEnemyScript.OnAnyEnemySlash += GroundStationaryAttackerEnemyScript_OnAnyEnemySlash;
        FlyingStationaryChaserEnemyScript.OnAnyEnemySlash += FlyingStationaryChaserEnemyScript_OnAnyEnemySlash;
        EnemyScript.OnAnyEnemyDash += EnemyScript_OnAnyEnemyDash;
        EnemyScript.OnAnyEnemyDefeated += EnemyScript_OnAnyEnemyDefeated;

        BossEnemyScript.OnAnyBossDash += BossEnemyScript_OnAnyBossDash;
        BossEnemyScript.OnAnyBossShoot += BossEnemyScript_OnAnyBossShoot;
        BossEnemyScript.OnAnyBossEnemyDefeated += BossEnemyScript_OnAnyBossEnemyDefeated;
        BossEnemyScript.OnAnyBossActivation += BossEnemyScript_OnAnyBossActivation;

        PlanetTotemScript.OnPlayerStartingWorld += PlanetTotemScript_OnPlayerStartingWorld;
        PlanetTotemScript.OnPlayerHasAllArtifacts += PlanetTotemScript_OnPlayerHasAllArtifacts;

        audioSource.spatialBlend = 0f;


    }

    

    private void Update()
    {
        audioSource.volume = volume;

        if(GameSceneManager.Instance.GetGameState() == GameStates.Paused)
        {
            currentSoundEffectsArray = FindObjectsOfType<AudioSource>();
            for(int i = 0; i < currentSoundEffectsArray.Length; i++)
            {
                currentSoundEffectsArray[i].Pause();
            }
        }
        else
        {
            if (currentSoundEffectsArray.Length > 0)
            {
                for (int i = 0; i < currentSoundEffectsArray.Length; i++)
                {
                    currentSoundEffectsArray[i].UnPause();
                }

                currentSoundEffectsArray = new AudioSource[0];
            }
        }
    }


    private void PlanetTotemScript_OnPlayerHasAllArtifacts(object sender, System.EventArgs e)
    {
        PlanetTotemScript planetTotem = sender as PlanetTotemScript;
        PlaySound(audioClipRefsSO.planetTotemUnlocking, planetTotem.transform.position, volume);
    }

    private void PlanetTotemScript_OnPlayerStartingWorld(object sender, PlanetTotemScript.OnSendingArtifactNumberEventArgs e)
    {
        PlanetTotemScript planetTotem = sender as PlanetTotemScript;
        PlaySound(audioClipRefsSO.planetTotemActivation, planetTotem.transform.position, volume);
    }

    private void BossEnemyScript_OnAnyBossEnemyDefeated(object sender, BossEnemyScript.OnBossEnemyDefeatedEventArgs e)
    {
        BossEnemyScript bossEnemy = sender as BossEnemyScript;
        PlaySound(audioClipRefsSO.enemyDeath, bossEnemy.transform.position, volume);
    }

    private void EnemyScript_OnAnyEnemyDefeated(object sender, EnemyScript.OnEnemyDefeatedEventArgs e)
    {
        EnemyScript enemy = sender as EnemyScript;
        PlaySound(audioClipRefsSO.enemyDeath, enemy.transform.position, volume);
    }

    private void BossEnemyScript_OnAnyBossShoot(object sender, System.EventArgs e)
    {
        BossEnemyScript bossEnemy = sender as BossEnemyScript;
        PlaySound(audioClipRefsSO.baseShoot, bossEnemy.transform.position, volume);
    }

    private void BossEnemyScript_OnAnyBossDash(object sender, System.EventArgs e)
    {
        BossEnemyScript bossEnemy = sender as BossEnemyScript;
        PlaySound(audioClipRefsSO.dash, bossEnemy.transform.position, volume);
    }

    private void BossEnemyScript_OnAnyBossActivation(object sender, System.EventArgs e)
    {
        BossEnemyScript bossEnemy = sender as BossEnemyScript;
        PlaySound(audioClipRefsSO.bossEnemyActivation, bossEnemy.transform.position, volume);
    }

    private void EnemyScript_OnAnyEnemyDash(object sender, System.EventArgs e)
    {
        EnemyScript dashEnemy = sender as EnemyScript;
        PlaySound(audioClipRefsSO.dash, dashEnemy.transform.position, volume);
    }

    private void FlyingStationaryChaserEnemyScript_OnAnyEnemySlash(object sender, System.EventArgs e)
    {
        FlyingStationaryChaserEnemyScript flyingSlashEnemy = sender as FlyingStationaryChaserEnemyScript;
        PlaySound(audioClipRefsSO.slash, flyingSlashEnemy.transform.position, volume);
    }

    private void GroundStationaryAttackerEnemyScript_OnAnyEnemySlash(object sender, System.EventArgs e)
    {
        GroundStationaryAttackerEnemyScript slashEnemy = sender as GroundStationaryAttackerEnemyScript;
        PlaySound(audioClipRefsSO.slash, slashEnemy.transform.position, volume);
    }

    private void Collectables_OnAnyCollectableGet(object sender, Collectables.OnCollectableGetEventArgs e)
    {
        Collectables collectable = sender as Collectables;
        PlaySound(audioClipRefsSO.itemGet, collectable.transform.position, volume);
    }

    private void RefillStationScript_OnAnyHeathRefill(object sender, System.EventArgs e)
    {
        RefillStationScript refillStation = sender as RefillStationScript;
        PlaySound(audioClipRefsSO.healing, refillStation.transform.position, volume);
    }

    private void PlayerHealth_OnPlayerDamaged(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.hurt, playerRef.transform.position, volume);
    }


    private void PlayerHealth_OnPlayerDead(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.playerDeath, playerRef.transform.position, volume);
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
        PlaySound(audioClipRefsSO.upgradedShoot, playerRef.transform.position, 0.3f);
    }

    private void BasicShooting_OnBaseShootPerformed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.baseShoot, playerRef.transform.position, volume);
    }

    private void PlayerJump_OnJumpPerformed(object sender, System.EventArgs e)
    {

        //PlayerJump playerJump = sender as PlayerJump;
        //PlaySound(audioClipRefsSO.jump, playerRef.transform.position, volume);
        PlayFromArrayOfSounds(audioClipRefsSO.jumpSounds, playerRef.transform.position);

    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultipler)
    {
        if (audioClip == null)
        {
            Debug.LogError($"{nameof(audioClip)} is null on {gameObject.name}", this);
            Debug.Log($"{nameof(audioClip)} is null on {gameObject.name}", this);
        }

        if (float.IsNaN(volumeMultipler))
        {
            Debug.LogError($"{nameof(volumeMultipler)} is null on {gameObject.name}", this);
            Debug.Log($"{nameof(volumeMultipler)} is null on {gameObject.name}", this);
        }

        if (float.IsNaN(volume))
        {
            Debug.LogError($"{nameof(volume)} is null on {gameObject.name}", this);
            Debug.Log($"{nameof(volume)} is null on {gameObject.name}", this);
        }

        audioSource.PlayOneShot(audioClip, volumeMultipler * volume);
        //AudioSource.PlayClipAtPoint(audioClip, position, volumeMultipler * volume);
        


    }

    private void PlayFromArrayOfSounds(List<AudioClip> audioClipArray, Vector3 position)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Count)], position, volume);

    }

    public void PlaySaveSound()
    {
        PlaySound(audioClipRefsSO.saving, playerRef.transform.position, volume);
    }

    public void ChangeVolume(float sliderValue)
    {
        volume = sliderValue;

        /*if (volume > 1f)
        {
            volume = 0f;
        }*/
        PlayerPrefs.SetFloat(TagReferencesScript.PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }

    public AudioClipRefsSO GetAudioClipRefsSO()
    {
        return audioClipRefsSO;
    }
}
