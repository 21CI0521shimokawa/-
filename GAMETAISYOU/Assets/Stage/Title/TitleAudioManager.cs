using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAudioManager : MonoBehaviour
{
    public AudioClip _SlimeClip;
    AudioSource _LogoSource;
    void Awake()
    {
        _LogoSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySlimeAudio()
    {
        _LogoSource.Stop();
        _LogoSource.PlayOneShot(_SlimeClip);
    }
}
