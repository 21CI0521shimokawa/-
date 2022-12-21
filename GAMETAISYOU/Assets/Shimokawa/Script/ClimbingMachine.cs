using System;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// �㏸�n�I�u�W�F�N�g�̊��N���X
/// </summary>
public abstract class ClimbingMachine : MonoBehaviour
{
    [SerializeField, Tooltip("�ړI�n�܂ł̃X�s�[�h")] protected float MovingTime;
    [SerializeField, Tooltip("�h�A�̏㏸�ړ���")] protected Vector3 UpMoveValue;
    [SerializeField, Tooltip("�ړ��C�[�W���O�w��")] protected Ease EaseType;
    [SerializeField, Tooltip("�Ώۂ̃Q�[���I�u�W�F�N�g�^�O")] protected string SubjectObjectTag;
    [SerializeField, Tooltip("�A�j���\�V����")] protected Animator ClimbingMachineAnimator;
    protected const int DoTime = 1; //�Փ˂�����̏�������x�������s���邽�߂̕ϐ�
    protected ClimbingState NowState = ClimbingState.MOVE; //�p����Ŏg�p����ǂݍ��ݐ�p��enum
    protected IObservable<Collider2D> TriggerEnterDirector {  get; set; } //�����蔻���IObservable�ɕϊ�
    protected Tween ClimbingTween; //�����_���ŏ����ۂɏ����������Ȃ��Ă��܂��ǐ����Ⴍ�Ȃ�̂ŏ�������؂邽�߂̕ϐ�

    /// <summary>
    /// ClimbingMachine�̃X�e�[�g
    /// </summary>
    protected enum ClimbingState
    {
        WAIT,
        MOVE
    }

    protected abstract void Moving(); //�p����ŕK����������

    /// <summary>
    /// ���݂̃A�j���[�V�������擾���I�����R�[������֐�
    /// </summary>
    /// <returns></returns>
    protected virtual async UniTask<bool> IsEndNowAnimation()
    {
        //���ݍĐ����Ă���A�j���[�V�������擾
        var NowAnimatorState = ClimbingMachineAnimator.GetCurrentAnimatorStateInfo(0);
        //�擾�����A�j���[�V�����̒������ҋ@
        await UniTask.Delay(TimeSpan.FromSeconds(NowAnimatorState.length));
        return true;
    }
}
