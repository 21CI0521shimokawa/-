using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;

public class BGMManeger : MonoBehaviour
{
    private static BGMManeger instance;
    public static bool _IsTitlePlay;
    [SerializeField] AudioClip[] BGMs;
    [SerializeField] AudioSource BGMAudios;

    private void Awake() //シングルトン
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        BGMAudios.Play();
        this.UpdateAsObservable()
            .Subscribe(_ =>
        {
            ChangeBGM();
        });
    }
    private void ChangeBGM()
    {
        string SceneName = SceneManager.GetActiveScene().name;// 現在のsceneを取得
        if (SceneName == "TGS-1")
        {
            BGMAudios.clip = BGMs[4];
            if (BGMAudios.isPlaying == false)
            {
                BGMAudios.Play();
            }
        }
        if (SceneName == "Title")
        {
            BGMAudios.clip = BGMs[0];
            if (_IsTitlePlay)
            {
                BGMAudios.Play();
            }
        }
        else if (SceneName == "S2-1")
        {
            BGMAudios.clip = BGMs[1];
            if (BGMAudios.isPlaying == false)
            {
                BGMAudios.Play();
            }
        }
        else if (SceneName == "S3-1")
        {
            BGMAudios.clip = BGMs[2];
            if (BGMAudios.isPlaying == false)
            {
                BGMAudios.volume = 0.1f;
                BGMAudios.Play();
            }
        }
        else if (SceneName == "S4-1"||SceneName=="TGS-2")
        {
            Destroy();//専用BGMに変更のため破棄
        }
    }
    public void  Destroy()
    {
        Destroy(gameObject);
    }
}
