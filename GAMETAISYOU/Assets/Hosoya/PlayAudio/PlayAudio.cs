using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    static AudioSource audioSource_;

    struct PlayAudioData
    {
        public AudioClip audioClip;
        public float length;
        public float playTime;
    }

    //�Đ����Ă鉹�f�[�^���X�g
    static List<PlayAudioData> playAudioList_;

    // Start is called before the first frame update
    void Start()
    {
        audioSource_ = GetComponent<AudioSource>();
        playAudioList_ = new List<PlayAudioData>();
    }

    // Update is called once per frame
    void Update()
    {
        //�폜����v�f�̔ԍ����X�g
        List<int> deleteNumbers = new List<int>();

        //���Ԃ𑝂₷
        for(int i = 0; i < playAudioList_.Count; ++i)
        {
            PlayAudioData audioData = playAudioList_[i];

            audioData.playTime += Time.unscaledDeltaTime;

            //���̒������Đ����Ԃ̂ق�������������폜
            if (audioData.playTime >= audioData.length)
            {
                deleteNumbers.Add(i);
            }

            playAudioList_[i] = audioData;
        }

        //�v�f�̍폜 �t���ɏ���
        for(int i = deleteNumbers.Count - 1; i >= 0; --i)
        {
            playAudioList_.RemoveAt(deleteNumbers[i]);
        }
    }

    public static void PlaySE(AudioClip _audioClip)
    {
        PlayAudioData audioData = new PlayAudioData();
        audioData.audioClip = _audioClip;
        audioData.length = _audioClip.length;
        audioData.playTime = 0.0f;

        playAudioList_.Add(audioData);

        if (audioSource_ != null)
        {
            audioSource_.PlayOneShot(audioData.audioClip);
        }
        else
        {
            Debug.LogWarning("AudioSource��������܂���");
        }
    }
}
