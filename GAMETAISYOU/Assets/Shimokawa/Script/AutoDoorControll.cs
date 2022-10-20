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
    [SerializeField, Tooltip("�h�A�J��SE")] AudioClip DoorSE;
    [SerializeField, Tooltip("�h�A�̊J�A�j���[�V����")] Animator AutoDoorAnimator;
    [SerializeField, Tooltip("�ړ��C�[�W���O�w��")] Ease EaseType;
    private const int DoTime = 1; //�Փ˂�����̏�������x�������s���邽�߂̕ϐ�

    #region �X�e�[�g�Ǘ�
    private enum AutoDoorState { MOVE, WAIT };
    AutoDoorState NowState;
    #endregion

    /// <summary>
    /// �Q�[�����n�܂鎞�Ɉ�x����Start�֐�����Ă΂��֐�
    /// </summary>
    private void Awake()
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
                if (NowState == AutoDoorState.MOVE) //���݂̃X�e�[�g��MOVE����������s
                {
                    this.transform.DOMoveY(MoveTo.y, AutoDoorMoveTime) //MoveTo.y�̈ʒu�܂�AutoDoorMoveTime�̑����ňړ�
                        .OnStart(() =>
                        {
                            PlayAudio.PlaySE(DoorSE); //�ړ��J�n����Ƃ���DoorSE���Đ�
                            AutoDoorAnimator.SetBool("Start", true); //�ړ��J�n����Ƃ���Start�A�j���[�V�������Đ�
                        })
                        .OnComplete(() =>
                        {
                            AutoDoorAnimator.SetBool("Start", false); //�ړ�������������Start�A�j���[�V�������~�߂�
                            NowState = AutoDoorState.WAIT; //�ړ�������������X�e�[�g��WAIT�ɕύX
                        })
                    .SetEase(EaseType);
                }
            });
    }
}