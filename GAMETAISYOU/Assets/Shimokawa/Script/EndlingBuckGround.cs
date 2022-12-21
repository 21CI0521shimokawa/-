using UnityEngine;

public class EndlingBuckGround : MonoBehaviour
{
    [SerializeField, Tooltip("�w�i�I�u�W�F�N�g�̈ړ��X�s�[�h")] float speed;
    [SerializeField, Tooltip("�܂�Ԃ��n�_�ݒ�")] float limitPosition;
    [SerializeField, Tooltip("���X�^�[�g�n�_�ݒ�")] Vector2 restartPosition;

    /// <summary>
    /// ���݂��Ă����疈�t���[���Ă΂��֐�
    /// </summary>
    void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
        if (transform.position.x > limitPosition)
        {
            transform.position = restartPosition;
        }
    }
}
