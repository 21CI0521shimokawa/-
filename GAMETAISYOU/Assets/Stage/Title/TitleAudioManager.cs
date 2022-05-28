using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAudioManager : MonoBehaviour
{
    public AudioClip _SlimeClip;
    public AudioClip _WinkClip;
    AudioSource _LogoSource;

    public bool IsStart { get; set; }

    void Awake()
    {
        _LogoSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySlimeAudio()
    {
        _LogoSource.Stop();
        _LogoSource.PlayOneShot(_SlimeClip);
    }

    public void PlayWinkAudio()
    {
        if (IsStart) { return; }
        _LogoSource.Stop();
        _LogoSource.volume = 0.3f;
        _LogoSource.PlayOneShot(_WinkClip);
    }
}
