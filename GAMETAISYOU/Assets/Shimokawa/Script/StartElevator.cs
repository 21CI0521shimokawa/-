using UnityEngine;
using DG.Tweening;

public class StartElevator : ClimbingMachine
{
    [SerializeField, Tooltip("�X���C���̑���ێ擾")] SlimeController slimeController;
    [SerializeField, Tooltip("�G���x�[�^�[�̊J��SE�擾")] AudioClip openSE;

    /// <summary>
    /// �Q�[�����n�܂鎞�Ɉ�x�����Ă΂��֐�
    /// </summary>
    void Start()
    {
        Moving();
    }
    protected override void Moving()
    {
        //�G���x�[�^�[���J���܂�Slime�̈ړ����~
        slimeController._ifOperation = false;
        transform.DOMoveY(UpMoveValue.y, MovingTime)
            �@�@ .OnComplete(() =>
               {
                   // �ړ�����������Slime�̈ړ����ĊJ
                   slimeController._ifOperation = true;
                   //�ړ�����������OpenSE���Đ�
                   PlayAudio.PlaySE(openSE);
                   //�ړ�����������Open�A�j���[�V�����Đ�
                   ClimbingMachineAnimator.SetTrigger("Open"); 
               });
    }
}