using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLineAudioManager : MonoBehaviour
{
    public AudioClip _BGMClip;
    public AudioClip _SlimeSEClip;
    public AudioClip _ShakeSEClip;
    public AudioClip _GlassSEClip;
    public AudioClip _RockSEClip;
    public float _BGMVolume = 0.8f;
    public float _SEVolume = 0.8f;
    public float _AmbientVolume = 0.8f;
    public GameObject BGMMeneger;

    AudioSource _BGMSource;
    AudioSource _SESource;
    AudioSource _AmbientSource;

    void Awake() {
        _BGMSource = gameObject.AddComponent<AudioSource>();
        _SESource = gameObject.AddComponent<AudioSource>();
        _AmbientSource = gameObject.AddComponent<AudioSource>();
        _BGMSource.volume = _BGMVolume;
        _SESource.volume = _SEVolume;
        _AmbientSource.volume = _AmbientVolume;
        BGMMeneger.SetActive(true);
    }

   /* public void PlayBGMAudio()
    {
        _BGMSource.Stop();
        _BGMSource.clip = _BGMClip;
        _BGMSource.Play();
    }*/

    public void PlaySlimeSEAudio()
    {
        _SESource.Stop();
        _SESource.PlayOneShot(_SlimeSEClip);
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
