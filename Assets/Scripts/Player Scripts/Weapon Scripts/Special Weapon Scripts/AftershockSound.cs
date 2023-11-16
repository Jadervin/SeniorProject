using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AftershockSound : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private float volumeReduction = .02f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    // Start is called before the first frame update
    void Start()
    {
        audioSource.volume = SoundManager.instance.GetVolume() * volumeReduction;
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = SoundManager.instance.GetVolume() * volumeReduction;
    }
}
