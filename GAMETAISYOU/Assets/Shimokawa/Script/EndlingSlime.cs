using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EndlingSlime : MonoBehaviour
{
    [SerializeField,Tooltip("�X���C���̈ړ����x")]float MoveSpeed;
    [SerializeField, Tooltip("�G���f�B���OBGM�֐��̎擾")] PlayBGM PlayBGM;
    private bool IsMove = true;
    private bool StopOnce;

    /// <summary>
    /// �Q�[�����n�܂鎞�Ɉ�x�����Ă΂��֐�
    /// </summary>
    void Start()
    {
        this.UpdateAsObservable()
           .Subscribe(_ =>
           {
               if (IsMove)
               {
                   transform.Translate(MoveSpeed * Time.deltaTime, 0, 0); //IsMove��true��������MoveSpeed�̑����ō��Ɉړ�
               }
               if (!StopOnce&&transform.position.x <= -1) //StopOnce��false�ŃX���C�������̈ʒu�܂œ���������
               {
                   StopOnce = true; //StopOnce��true�ɕύX
                   IsMove = false; //IsMove��false�ɂ��ăX���C���̓������~�߂�
               }
           });
    }

    /// <summary>
    /// �A�j���[�V�����C�x���g�ŎQ�Ƃ����ړ��ĊJ�֐�
    /// </summary>
    public void Move()
    {
        IsMove = true;
        MoveSpeed = -1.2f;
    }

    /// <summary>
    /// �A�j���[�V�����C�x���g�ŎQ�Ƃ����^�C�g���ɖ߂�֐�
    /// </summary>
    public void ToTitle()
    {
        FadeManager.Instance.LoadScene("Title", 4f);
        PlayBGM._FadeOutStart();
    }
}
