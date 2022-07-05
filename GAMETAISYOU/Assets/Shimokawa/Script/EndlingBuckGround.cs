using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EndlingBuckGround : MonoBehaviour
{
    #region SerializeField
    [SerializeField, Tooltip("�w�i�I�u�W�F�N�g�̈ړ��X�s�[�h")] float Speed;
    [SerializeField, Tooltip("�܂�Ԃ��n�_�ݒ�")] float LimitPosition;
    [SerializeField, Tooltip("���X�^�[�g�n�_�ݒ�")] Vector2 RestartPosition;
    #endregion

    void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                transform.Translate(Speed * Time.deltaTime, 0, 0);
                if (transform.position.x > LimitPosition)
                {
                    transform.position = RestartPosition;
                }

            });
    }
}
