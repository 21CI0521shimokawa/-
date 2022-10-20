using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EndlingBuckGround : MonoBehaviour
{
    [SerializeField, Tooltip("�w�i�I�u�W�F�N�g�̈ړ��X�s�[�h")] float MoveSpeed;
    [SerializeField, Tooltip("�܂�Ԃ��n�_�ݒ�")] float LimitPosition;
    [SerializeField, Tooltip("���X�^�[�g�n�_�ݒ�")] Vector2 RestartPosition;

    void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                transform.Translate(MoveSpeed * Time.deltaTime, 0, 0); //����Speed�̑����ňړ�
                if (transform.position.x > LimitPosition)
                {
                    transform.position = RestartPosition; //���̈ʒu�ɂȂ�����ŏ��̈ʒu�ɖ߂�
                }

            });
    }
}
