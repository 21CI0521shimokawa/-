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

    //再生してる音データリスト
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
        //削除する要素の番号リスト
        List<int> deleteNumbers = new List<int>();

        //時間を増やす
        for(int i = 0; i < playAudioList_.Count; ++i)
        {
            PlayAudioData audioData = playAudioList_[i];

            audioData.playTime += Time.unscaledDeltaTime;

            //音の長さより再生時間のほうが長かったら削除
            if (audioData.playTime >= audioData.length)
            {
                deleteNumbers.Add(i);
            }

            playAudioList_[i] = audioData;
        }

        //要素の削除 逆順に処理
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
            Debug.LogWarning("AudioSourceが見つかりません");
        }
    }
}
