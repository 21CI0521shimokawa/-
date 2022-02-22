using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("�Ή��ԍ�")]
    public int _Number;

    [SerializeField]Vector3 m_normalPos;�@//�����ʒu
    [SerializeField]bool isOpen;

    void Start()
    {
        //�I�u�W�F�N�g���Q�[���}�l�[�W���[�̃��X�g�ɓo�^����
        GameManager.Instance.RegisterDoor(this.gameObject); 
        m_normalPos = transform.position;
        isOpen = false;
    }

    void Update()
    {
        Movement();
    }

    //��Ԃɂ��ړ��ݒ�
    void Movement()
    {
        if (isOpen)
            transform.position = Vector3.MoveTowards(transform.position, m_normalPos + new Vector3(0, 2, 0), 5 * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, m_normalPos, 5 * Time.deltaTime);
    }


    //-------------------------------------------------------
    //�N������
    public void Open()
    {
        isOpen = true;
    }
    //�I������
    public void Close()
    {
        isOpen = false;
    }
}
