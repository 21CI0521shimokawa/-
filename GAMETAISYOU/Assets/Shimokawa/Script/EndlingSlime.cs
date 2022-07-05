using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EndlingSlime : MonoBehaviour
{
    #region SerializeField
    [SerializeField,Tooltip("�X���C���̈ړ����x")]float MoveSpeed;
    [SerializeField,Tooltip("�G���f�B���OBGM�֐��̎擾")] PlayBGM PlayBGM;
    #endregion

    private bool IsMove = true;
    private bool StopOnce;

    void Start()
    {
        this.UpdateAsObservable()
           .Subscribe(_ =>
           {
               if (IsMove)
               {
                   transform.Translate(MoveSpeed * Time.deltaTime, 0, 0);
               }
               if (!StopOnce&&transform.position.x <= -1)
               {
                   StopOnce = true;
                   IsMove = false;
               }
           });
    }
    #region public function
    public void Move() //�A�j���[�V�����C�x���g�ŎQ�Ƃ����֐�
    {
        IsMove = true;
        MoveSpeed = -1.2f;
    }

    public void ToTitle()//�A�j���[�V�����C�x���g�ŎQ�Ƃ����֐�
    {
        FadeManager.Instance.LoadScene("Title", 4f);
        PlayBGM._FadeOutStart();
    }
    #endregion
}
