using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using DG.Tweening;
using UniRx.Triggers;

public class StartElevator : MonoBehaviour
{
    [SerializeField, Tooltip("�ڕW�n�_�̐ݒ�")] Vector3 DestinationPositon;
    [SerializeField, Tooltip("�G���x�[�^�[�̃X�s�[�h")] float StartElevatorSpeed;
    [SerializeField, Tooltip("�J�A�j���[�V����")] Animator StartElevatorAnimator;
    [SerializeField, Tooltip("�X���C���̑���ێ擾")] SlimeController SlimeController;
    [SerializeField, Tooltip("�G���x�[�^�[�̊J��SE�擾")] AudioClip OpenSE;

    /// <summary>
    /// �Q�[�����n�܂鎞�Ɉ�x�����Ă΂��֐�
    /// </summary>
    void Start()
    {
        SlimeController._ifOperation = false; //�G���x�[�^�[���J���܂�Slime�̈ړ����~
        this.transform.DOMoveY(DestinationPositon.y, StartElevatorSpeed)
        .OnComplete(() =>
        {
            SlimeController._ifOperation = true; //�ړ�����������Slime�̈ړ����ĊJ
            PlayAudio.PlaySE(OpenSE); //�ړ�����������OpenSE���Đ�
            StartElevatorAnimator.SetTrigger("Open"); //�ړ�����������Open�A�j���[�V�����Đ�
        });
    }
}