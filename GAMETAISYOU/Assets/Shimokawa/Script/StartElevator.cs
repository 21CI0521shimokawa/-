using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class StartElevator : MonoBehaviour
{
    #region SerializeField
    [SerializeField, Tooltip("�ڕW�n�_�̐ݒ�")] Vector3 DestinationPos;
    [SerializeField,Tooltip("���ݒn")] Vector3 NowPositon;
    [SerializeField, Tooltip("�G���x�[�^�[�̃X�s�[�h")] float StartElevatorSpeed;
    [SerializeField, Tooltip("�J�A�j���[�V����")] Animator StartElevatorAnimator;
    [SerializeField, Tooltip("�X���C���̑���ێ擾")] SlimeController SlimeController;
    [SerializeField, Tooltip("�I�[�f�B�I�\�[�X�擾")] AudioSource StartElevatorAudioSource;
    [SerializeField, Tooltip("�G���x�[�^�[�̊J��SE�擾")] AudioClip OpenSE;
    #endregion

    void Start()
    {
        SlimeController._ifOperation = false;
        NowPositon = this.transform.position;
        this.UpdateAsObservable()
            .TakeWhile(_ => NowPositon.y < DestinationPos.y)
            .Subscribe(_ =>
            {
                NowPositon = transform.position;
                transform.Translate(0, StartElevatorSpeed * Time.deltaTime, 0);
            },
            () =>
            {
                SlimeController._ifOperation = true;
                PlaySE(OpenSE);
                StartElevatorAnimator.SetTrigger("Open");
            });

    }
    #region public function
    public void PlaySE(AudioClip audio) //SE�����Đ�����
    {
        if (StartElevatorAudioSource != null)
        {
            StartElevatorAudioSource.PlayOneShot(audio);
        }
    }
    #endregion
}