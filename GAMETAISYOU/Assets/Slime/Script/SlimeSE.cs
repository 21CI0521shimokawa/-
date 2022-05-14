using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSE : MonoBehaviour
{
    [SerializeField] SlimeController slimeController;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource audioSourceSoundStretch;

    [SerializeField] AudioClip soundJump;
    [SerializeField] AudioClip soundTearoff;
    [SerializeField] AudioClip soundMove;
    [SerializeField] AudioClip soundLanding;
    [SerializeField] AudioClip soundBuzzer;

    [SerializeField] float soundJumpVolume;
    [SerializeField] float soundTearoffVolume;
    [SerializeField] float soundMoveVolume;
    [SerializeField] float soundLandingVolume;
    [SerializeField] float soundBuzzerVolume;

    bool playsoundJump;
    bool playsoundTearoff;
    bool playsoundMove;
    bool playsoundLanding;
    bool playsoundBuzzer;

    bool PlaysoundStretch; 
    bool isPlaysoundStretch;    //今再生しているかどうか

    // Start is called before the first frame update
    void Start()
    {
        flagReset();

        PlaysoundStretch = false;
        isPlaysoundStretch = false;
    }

    // Update is called once per frame
    void Update()
    {
        flagReset();
        ChangePitch();


        if (PlaysoundStretch)
        {
            if (!isPlaysoundStretch)
            {
                isPlaysoundStretch = true;
                audioSourceSoundStretch.Play();
            }
        }
        else
        {
            if (isPlaysoundStretch)
            {
                isPlaysoundStretch = false;
                audioSourceSoundStretch.Stop();
            }          
        }
    }

    void flagReset()
    {
        playsoundJump = false;
        playsoundTearoff = false;
        playsoundMove = false;
        playsoundLanding = false;
        playsoundBuzzer = false;
    }

    public void _PlayJumpSE()
    {
        if (!playsoundJump)
        {
            playsoundJump = true;

            if (soundJump)
            {
                audioSource.PlayOneShot(soundJump, soundJumpVolume);
            }
            Debug.Log("飛ぶ音");
        }
    }

    public void _PlayTearoffSE()
    {
        if (!playsoundTearoff)
        {
            playsoundTearoff = true;

            if (soundTearoff)
            {
                audioSource.PlayOneShot(soundTearoff, soundTearoffVolume);
            }
            Debug.Log("ちぎれる音");
        }
    }

    public void _PlayMoveSE()
    {
        if (!playsoundMove)
        {
            playsoundMove = true;

            if (soundMove)
            {
                audioSource.PlayOneShot(soundMove, soundMoveVolume);
            }
            Debug.Log("歩く音");
        }
    }

    public void _PlayLandingSE()
    {
        if (!playsoundLanding)
        {
            playsoundLanding = true;

            if (soundLanding)
            {
                audioSource.PlayOneShot(soundLanding, soundLandingVolume);
            }
            Debug.Log("着地音");
        }
    }

    public void _PlayBuzzerSE()
    {
        if (!playsoundBuzzer)
        {
            playsoundBuzzer = true;

            if (soundBuzzer)
            {
                audioSource.pitch = 1.0f;
                audioSource.PlayOneShot(soundBuzzer, soundBuzzerVolume);
                ChangePitch();
            }
            Debug.Log("ブザー音");
        }
    }

    public void _PlayStretchSE(float pitch_)
    {
        audioSourceSoundStretch.pitch = pitch_;

        PlaysoundStretch = true;

        //Debug.Log("引き延ばす " + pitch_);
    }

    public void _StopStretchSE()
    {
        PlaysoundStretch = false;
    }


    void ChangePitch()
    {
        float sizeMin = 1.0f;
        float sizeMax = 5.0f;

        float pitchMin = 0.5f;
        float pitchMax = 2.0f;


        float buf = (slimeController._scaleNow - sizeMin) / sizeMax;

        float newPitch = Mathf.Lerp(pitchMax, pitchMin, buf);

        //float newPitch = pitchMax - (pitchMax - pitchMin) * buf;

        audioSource.pitch = newPitch;
    }
}
