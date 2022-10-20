using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

public class ElevatorControll : MonoBehaviour
{
    [SerializeField, Tooltip("�h�A�̊J�X�s�[�h")] float Speed;
    [SerializeField, Tooltip("�h�A�̈ړ�����")] float ElevatorMoveTime;
    [SerializeField, Tooltip("�ړ���")] Vector3 MoveTo = new Vector3(0, 4);
    [SerializeField, Tooltip("�h�A�J��SE")] AudioClip DoorSE;
    [SerializeField, Tooltip("�h�A�̊J�A�j���[�V����")] Animator ElevatorAnimator;
    [SerializeField, Tooltip("�ړ��C�[�W���O�w��")] Ease EaseType;
    [SerializeField, Tooltip("���݂̃V�[���̖��O")] Scene SceneName;
    [SerializeField, Tooltip("�X���C���I�u�W�F�N�g�̎擾")] SlimeController ClonSlime;
    [SerializeField, Tooltip("���̎擾")] Rigidbody2D ElevatorRigidbody;
    [SerializeField, Tooltip("�J�����̒ǐՊǗ�")] GameObject CameraActive;
    private const int DoTime = 1; //�Փ˂�����̏�������x�������s���邽�߂̕ϐ�
    private const float DelayTime = 1f; //�A�j���[�V�������Đ����Ă��瓮���������߂�DelayTime���w��

    #region �X�e�[�g�Ǘ�
    private enum ElevatorState { MOVE, WAIT };
    ElevatorState NowState;
    #endregion

    /// <summary>
    /// �Q�[�����n�܂鎞�Ɉ�x����Start�֐�����Ă΂��֐�
    /// </summary>
    void Awake()
    {
        NowState = AutoDoorState.MOVE; //�X�e�[�g��MOVE�ɂ��Ă���
    }

    /// <summary>
    /// �Q�[�����n�܂鎞�Ɉ�x�����Ă΂��֐�
    /// </summary>
    void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Select(collison => collison.tag) //�Փ˂����I�u�W�F�N�g�̃^�O��
            .Where(tag => tag == "Slime") //"Slime"��������
            .Take(DoTime) //��x�������s
            .Subscribe(collision =>
            {
                if (NowState == ElevatorState.MOVE) //���݂̃X�e�[�g��MOVE����������s
                {
                    this.transform.DOMoveY(MoveTo.y, ElevatorMoveTime).SetDelay(DelayTime) //DelayTime�̎��ԑ҂��Ă���Moveto.y�̒n�_�܂�ElevatorMoveTime�̑����ňړ�
                        .OnStart(() =>
                        {
                            PlayAudio.PlaySE(DoorSE); //DoorSE���Đ�
                            ElevatorAnimator.SetBool("Start", true); //���s���鎞�ɃA�j���[�V�����Đ�
                        })
                        .OnComplete(() =>
                        {
                            ElevatorAnimator.SetBool("Start", false); //�ړ�����������ړ��I��
                            ElevatorRigidbody.constraints = RigidbodyConstraints2D.FreezePosition; //�ړ����������炻���ɗ��܂�
                            NowState = ElevatorState.WAIT; //�ړ�����������X�e�[�g��WAIT�ɕύX
                            SceneChange(); //��x�����V�[���ڍs�������Ăяo��
                        })
                    .SetEase(EaseType);
                }
            });
    }

    /// <summary>
    /// Elevator�N�����ɕ��􂵂Ă���q���X���C����S���W��
    /// </summary>
    public void ElevatorStart()
    {
        GameObject[] SlimeObjects = GameObject.FindGameObjectsWithTag("Slime"); //�V�[���ɑ��݂��Ă��镪�􂵂Ă���q���X���C����S�Ď擾
        foreach (GameObject Obj in SlimeObjects) //�V�[����ɑ��݂��Ă���X���C���I�u�W�F�N�g���擾
        {
            Obj.GetComponent<SlimeController>().liveTime = 20; //�q���X���C�����e�X���C���ɖ߂鎞�Ԃ��w�肷�鎖�ŏW��������
        }
        CameraActive.SetActive(false); //�J�����̒ǐՂ��~������
        ElevatorAnimator.SetTrigger("Close"); //�G���x�[�^�[��Close�A�j���[�V�����Đ�
    }

    /// <summary>
    /// ���݂�scene���擾���ăV�[���ڍs
    /// </summary>
    public void SceneChange()
    {
        string SceneName = SceneManager.GetActiveScene().name; //���݂̃V�[���̖��O���擾
        if (SceneName == "Title")
        {
            FadeManager.Instance.LoadScene("S0-1", FadeTime);
        }
        if (SceneName == "S0-1")
        {
            FadeManager.Instance.LoadScene("S0-2", FadeTime);
        }
        else if (SceneName == "S0-2")
        {
            FadeManager.Instance.LoadScene("S0-3", FadeTime);
        }
        else if (SceneName == "S0-3")
        {
            FadeManager.Instance.LoadScene("S1-1", FadeTime);
        }
        else if (SceneName == "S1-1")
        {
            FadeManager.Instance.LoadScene("S1-2", FadeTime);
        }
        else if (SceneName == "S1-2")
        {
            FadeManager.Instance.LoadScene("S1-3", FadeTime);
        }
        else if (SceneName == "S1-3")
        {
            FadeManager.Instance.LoadScene("S2-1", FadeTime);
        }
        else if (SceneName == "S2-1")
        {
            FadeManager.Instance.LoadScene("S2-2", FadeTime);
        }
        else if (SceneName == "S2-2")
        {
            FadeManager.Instance.LoadScene("S2-3", FadeTime);
        }
        else if (SceneName == "S2-3")
        {
            FadeManager.Instance.LoadScene("S2-4", FadeTime);
        }
        else if (SceneName == "S2-4")
        {
            FadeManager.Instance.LoadScene("S3-1", FadeTime);
        }
        else if (SceneName == "S3-1")
        {
            FadeManager.Instance.LoadScene("S3-2", FadeTime);
        }
        else if (SceneName == "S3-2")
        {
            FadeManager.Instance.LoadScene("S3-3", FadeTime);
        }
        else if (SceneName == "S3-3")
        {
            FadeManager.Instance.LoadScene("S3-4", FadeTime);
        }
        else if (SceneName == "S3-4")
        {
            FadeManager.Instance.LoadScene("S4-1", FadeTime);
        }
        else if (SceneName == "TGS-1")
        {
            FadeManager.Instance.LoadScene("TGS-2", FadeTime);
        }
    }
}