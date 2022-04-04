using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip NextTurn;
    public AudioClip Splat;
    public AudioClip HoverClick;
    public AudioClip Shield;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (Instance == null) Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySplat()
    {
        if (audioSource.clip == Splat && audioSource.isPlaying) return;
        audioSource.clip = Splat;
        audioSource.Play();
    }

    public void PlayNextTurn()
    {
        audioSource.clip = NextTurn;
        audioSource.Play();
    }

    public void PlayHover()
    {
        if (audioSource.isPlaying) return;
        audioSource.clip = HoverClick;
        audioSource.Play();
    }

    public void PlayShield()
    {
        audioSource.clip = Shield;
        audioSource.Play();
    }
}
