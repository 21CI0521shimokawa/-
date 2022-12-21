using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using System.Linq;
using Cysharp.Threading.Tasks;
using System;

public class AutoDoorController : ClimbingMachine
{
    [SerializeField, Tooltip("�I�[�g�h�A�̊J��SE")] AudioClip autoDoorSE;

    /// <summary>
    /// �Q�[�����n�܂鎞�Ɉ�x�����Ă΂��֐�
    /// </summary>
    void Start()
    {
        Moving();
    }

    /// <summary>
    /// �I�[�g�h�A�̓����̐S����
    /// </summary>
    protected override void Moving()
    {
        TriggerEnterDirector
                       //�Փ˂����I�u�W�F�N�g�̃^�O��
                       .Select(collison => collison.tag)
                       //"Slime"��������
                       .Where(tag => tag == SubjectObjectTag)
                       //���݂̃X�e�[�g��MOVE����������s
                       .Where(state => NowState == ClimbingState.MOVE)
                       //��x�������s
                       .Take(DoTime)
                       .Subscribe(Do =>
                       {
                           //UpMoveValue.y�̈ʒu�܂�MovingTime�̑����ňړ�
                           ClimbingTween = transform.DOMoveY(UpMoveValue.y, MovingTime).SetEase(EaseType);

                           ClimbingTween
                           .OnStart(() =>
                           {
                               //�ړ��J�n����Ƃ���AutoDoorSE���Đ�
                               PlayAudio.PlaySE(autoDoorSE);
                               //�ړ��J�n����Ƃ���Start�A�j���[�V�������Đ�
                               ClimbingMachineAnimator.SetBool("Start", true);
                           })
                           .OnComplete(() =>
                           {
                               //�ړ�������������Start�A�j���[�V�������~�߂�
                               ClimbingMachineAnimator.SetBool("Start", false);
                               //�ړ�������������X�e�[�g��WAIT�ɕύX
                               NowState = ClimbingState.WAIT;
                           });
                       });
    }
}