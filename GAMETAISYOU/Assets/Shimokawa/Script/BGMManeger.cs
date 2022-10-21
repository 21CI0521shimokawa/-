using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;

public class BGMManeger : MonoBehaviour
{
    private static BGMManeger instance; //�O���ň������߂�static�ϐ��ɐݒ�
    public static bool _IsTitlePlay; //���݃^�C�g����BGM������Ă��邩�̔���(timeline�Ŏg�p)
    [SerializeField] AudioClip[] BGMs; //BGM���i�[�����ϐ�
    [SerializeField] AudioSource BGMAudios; //�����𗬂��ϐ�

    private void Awake() //�V���O���g��
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
        BGMAudios.Play(); //�ŏ��ɐݒ肳��Ă���Clip���Đ�
        this.UpdateAsObservable()
            .Subscribe(_ =>
        {
            ChangeBGM(); //�V�[�����ɉ����𔻒肵����̃V�[���ŉ����ύX
        });
    }

    private void ChangeBGM() //�V�[����ύX���锻��֐�
    {
        string SceneName = SceneManager.GetActiveScene().name;// ���݂�scene���擾
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
            Destroy();//��pBGM�ɕύX�̂��ߔj��
        }
    }
    public void  Destroy()
    {
        Destroy(gameObject);
    }
}
