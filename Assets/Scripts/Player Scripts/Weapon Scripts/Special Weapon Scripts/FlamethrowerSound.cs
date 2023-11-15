using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerSound : MonoBehaviour
{

    [SerializeField] private FlamethrowerScript flamethrower;
    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    // Start is called before the first frame update
    void Start()
    {
        flamethrower.OnFlamethrowerStateChanged += Flamethrower_OnFlamethrowerStateChanged;
        audioSource.volume = SoundManager.instance.GetVolume();

    }

    private void Flamethrower_OnFlamethrowerStateChanged(object sender, FlamethrowerScript.OnFlamethrowerStateChangedEventArgs e)
    {
        bool playSound = e.isActivated;
        if(playSound == true)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = SoundManager.instance.GetVolume();
    }
}
