using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using SceneDefine;

public class ElevatorController : ClimbingMachine
{
    [SerializeField, Tooltip("�J�����̒ǐՊǗ�")] GameObject cameraActive;
    [SerializeField, Tooltip("�V�[���ڍs���̎���")] float fadeTime;
    [SerializeField, Tooltip("�G���x�[�^�[�̊J��SE")] AudioClip openSE;
    private const int cloneSlimeDieTime = 20; //���􂳂ꂽ�X���C�����e�X���C���ɖ߂鎞�Ԃ��w��
    private const float delayTime = 1f; //�A�j���[�V�������Đ����Ă��瓮���������߂�DelayTime���w��

    /// <summary>
    /// �Q�[�����n�܂鎞�Ɉ�x�����Ă΂��֐�
    /// </summary>
    void Start()
    {
        Moving();
    }

    /// <summary>
    /// �G���x�[�^�[�̓����̐S����
    /// </summary>
    protected override void Moving()
    {
        TriggerEnterDirector
                         //�Փ˂����I�u�W�F�N�g�̃^�O��
                         .Select(collison => collison.tag)
                         //"Slime"��������
                         .Where(tag => tag == SubjectObjectTag)
                         //���݂̃X�e�[�g��MOVE����������s
                         .Where(_State => NowState == ClimbingState.MOVE)
                         //��x�������s
                         .Take(DoTime)
                         .Subscribe(async collision =>
                         {
                             //DoorSE���Đ�
                             PlayAudio.PlaySE(openSE);
                             //�G���x�[�^�[������A�j���[�V�������I���܂őҋ@
                             var WaitAnimEnd = await IsEndNowAnimation();
                             //�A�j���[�V�������I�����Ă���UpMoveValue.y�̒n�_�܂�delayTime�̑����ňړ����C�[�W���O�^�C�v���w��
                             ClimbingTween = transform.DOMoveY(UpMoveValue.y, MovingTime).SetDelay(delayTime).SetEase(EaseType);

                             ClimbingTween
                                  .OnStart(() =>
                                  {
                                      // Elevator�N�����ɕ��􂵂Ă���q���X���C����S���W��
                                      ElevatorStart();
                                      //���s���鎞�ɃA�j���[�V�����Đ�
                                      ClimbingMachineAnimator.SetBool("Start", true);
                                  })
                                  .OnComplete(() =>
                                  {
                                      //�ړ�����������X�e�[�g��WAIT�ɕύX
                                      NowState = ClimbingState.WAIT;
                                      //�V�[���ڍs
                                      SceneManagement.LoadNextScene(fadeTime);
                                  });
                         });
    }

    /// <summary>
    /// Elevator�N�����ɕ��􂵂Ă���q���X���C����S���W��
    /// </summary>
    public void ElevatorStart()
    {
        //�V�[���ɑ��݂��Ă��镪�􂵂Ă���q���X���C����S�Ď擾
        GameObject[] slimeObjects = GameObject.FindGameObjectsWithTag(SubjectObjectTag);
        //�V�[����ɑ��݂��Ă���X���C���I�u�W�F�N�g���擾
        foreach (GameObject objcet in slimeObjects)
        {
            //�q���X���C�����e�X���C���ɖ߂鎞�Ԃ��w�肷�鎖�ŏW��������
            objcet.GetComponent<SlimeController>().liveTime = cloneSlimeDieTime;
        }
        //�J�����̒ǐՂ��~������
        cameraActive.SetActive(false);
        //�G���x�[�^�[��Close�A�j���[�V�����Đ�
        ClimbingMachineAnimator.SetTrigger("Close");
    }

}