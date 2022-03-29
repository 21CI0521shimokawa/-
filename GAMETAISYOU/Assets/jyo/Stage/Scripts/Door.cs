using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : StageGimmick
{
    [Header("Door Script")]
    [Tooltip("�ړ���"), SerializeField] Vector2 moveTo = new Vector2(0, 2);
    [Tooltip("�ړ����x"), SerializeField] float moveSpeed = 5f;

    Vector2 m_normalPos;//�����ʒu

    void Start()
    {
        //�I�u�W�F�N�g���Q�[���}�l�[�W���[�̃��X�g�ɓo�^����
        GameManager.Instance.RegisterDoor(this.gameObject); 
        m_normalPos = transform.position;
    }

    void Update()
    {
        Movement();
    }

    //��Ԃɂ��ړ��ݒ�
    void Movement()
    {
        if (_IsOpen)
            transform.position = Vector3.MoveTowards(transform.position, m_normalPos + moveTo, moveSpeed * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, m_normalPos, 5 * Time.deltaTime);
    }


    //-------------------------------------------------------
    //�N������
    public override void Open()
    {
        _IsOpen = !_IsOpen;
    }
    //�I������
    public override void Close()
    {
        _IsOpen = !_IsOpen;
    }
}
