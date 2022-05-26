using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : StageGimmick
{
    [Header("Door Script")]
    [Tooltip("���ԃA�j���[�^�["), SerializeField] Animator gear;
    [Tooltip("�ړ���"), SerializeField] Vector2 moveTo = new Vector2(0, 2);
    [Tooltip("�ړ����x"), SerializeField] float moveSpeed = 5f;

    [SerializeField] AudioSource audioSource;
    bool isOpenOneFlameBefore;    

    Vector2 m_normalPos;//�����ʒu

    void Start()
    {
        //�I�u�W�F�N�g���Q�[���}�l�[�W���[�̃��X�g�ɓo�^����
        GameManager.Instance.RegisterDoor(this.gameObject); 
        m_normalPos = transform.position;

        isOpenOneFlameBefore = false;
    }

    void Update()
    {
        Movement();
    }

    //��Ԃɂ��ړ��ݒ�
    void Movement()
    {
        if (_IsOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_normalPos + moveTo, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, m_normalPos + moveTo) < 1e-3)
            {
                gear.SetBool("Open", false);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, m_normalPos, 5 * Time.deltaTime);
            if (Vector3.Distance(transform.position, m_normalPos) < 1e-3)
            {
                gear.SetBool("Open", false);
            }
        }

        PlaySE();
    }

    //SE�Đ�
    void PlaySE()
    {
        if(isOpenOneFlameBefore != _IsOpen)
        {
            isOpenOneFlameBefore = _IsOpen;
            audioSource.Play();
        }
    }


    //-------------------------------------------------------
    //�N������
    public override void Open()
    {
        _IsOpen = !_IsOpen;
        gear.SetBool("Open", true);
    }
    //�I������
    public override void Close()
    {
        _IsOpen = !_IsOpen;
        gear.SetBool("Open", true);
    }
}
