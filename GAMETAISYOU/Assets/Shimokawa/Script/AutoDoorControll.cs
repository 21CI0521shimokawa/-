using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using System.Linq;

public class AutoDoorControll : MonoBehaviour
{
    [SerializeField, Tooltip("�h�A�̊J�X�s�[�h")] float Speed;
    [SerializeField, Tooltip("�ړ���"),] Vector3 MoveTo = new Vector3(0, 4);
    [SerializeField, Tooltip("�h�A�̈ړ�����")] float AutoDoorMoveTime;
    [SerializeField, Tooltip("�I�[�f�B�I�\�[�X")] AudioSource AudioSource = null;
    [SerializeField, Tooltip("�h�A�J��SE")] AudioClip DoorSE;
    [SerializeField, Tooltip("�h�A�̊J�A�j���[�V����")] Animator AutoDoorAnimator;
    [SerializeField, Tooltip("�ړ��C�[�W���O�w��")] Ease EaseType;

    #region �X�e�[�g�Ǘ�
    private enum AutoDoorState { MOVE, WAIT };
    AutoDoorState NowState;
    #endregion

    private void Awake()
    {
        NowState = AutoDoorState.MOVE;
    }

    void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Select(collison => collison.tag)
            .Where(tag => tag == "Slime")
            .Take(1)
            .Subscribe(collision =>
            {
                if (NowState == AutoDoorState.MOVE)
                {
                    this.transform.DOMoveY(MoveTo.y, AutoDoorMoveTime)
                        .OnStart(() =>
                        {//���s�J�n���̃R�[���o�b�N
                            PlaySE(DoorSE);
                            AutoDoorAnimator.SetBool("Start", true);
                        })
                        .OnComplete(() =>
                        {//���s�������̃R�[���o�b�N
                            AutoDoorAnimator.SetBool("Start", false);
                            NowState = AutoDoorState.WAIT;
                        })
                    .SetEase(EaseType);
                }
            });
    }
    public void PlaySE(AudioClip audio) //SE�����Đ�
    {
        if (AudioSource != null)
        {
            AudioSource.PlayOneShot(audio);
        }
    }

}