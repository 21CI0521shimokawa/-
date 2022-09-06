using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineAudioManager : MonoBehaviour
{
    public AudioClip _SlimeSEClip;
    public AudioClip _JumpSEClip;
    public AudioClip _ShakeSEClip;
    public AudioClip _GlassSEClip;
    public AudioClip _RockSEClip;
    public float _SEVolume = 0.8f;
    public float _AmbientVolume = 0.8f;
    AudioSource _SESource;
    AudioSource _AmbientSource;

    void Awake() {
        _SESource = gameObject.AddComponent<AudioSource>();
        _AmbientSource = gameObject.AddComponent<AudioSource>();
        _SESource.volume = _SEVolume;
        _AmbientSource.volume = _AmbientVolume;
        //BGMMeneger.SetActive(true);
    }

    public void PlaySlimeSEAudio()
    {
        _SESource.Stop();
        _SESource.PlayOneShot(_SlimeSEClip);
    }

    public void PlayJumpSEAudio()
    {
        _SESource.Stop();
        _SESource.PlayOneShot(_JumpSEClip);
    }

    public void PlayShakeSEAudio()
    {
        _AmbientSource.Stop();
        _AmbientSource.PlayOneShot(_ShakeSEClip);
    }

    public void PlayGlassSEAudio()
    {
        _SESource.Stop();
        _SESource.PlayOneShot(_GlassSEClip);
    }

    public void PlayRockSEAudio()
    {
        _AmbientSource.Stop();
        _AmbientSource.PlayOneShot(_RockSEClip);
    }

    public void StartGame()
    {
        FadeManager.Instance.ToStage();
    }
}
