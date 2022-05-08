using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSE : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource audioSourceSoundStretch;

    [SerializeField] AudioClip soundJump;
    [SerializeField] AudioClip soundTearoff;
    [SerializeField] AudioClip soundMove;
    [SerializeField] AudioClip soundLanding;

    [SerializeField] float soundJumpVolume;
    [SerializeField] float soundTearoffVolume;
    [SerializeField] float soundMoveVolume;
    [SerializeField] float soundLandingVolume;

    bool playsoundJump;
    bool playsoundTearoff;
    bool playsoundMove;
    bool playsoundLanding;

    bool PlaysoundStretch; 
    bool isPlaysoundStretch;    //���Đ����Ă��邩�ǂ���

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

        if(PlaysoundStretch)
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
            Debug.Log("��ԉ�");
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
            Debug.Log("������鉹");
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
            Debug.Log("������");
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
            Debug.Log("���n��");
        }
    }

    public void _PlayStretchSE(float pitch_)
    {
        audioSourceSoundStretch.pitch = pitch_;

        PlaysoundStretch = true;

        //Debug.Log("�������΂� " + pitch_);
    }

    public void _StopStretchSE()
    {
        PlaysoundStretch = false;
    }
}
