using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndrollController : MonoBehaviour
{
    [SerializeField] private float textScrollSpeed = 30; //�e�L�X�g�̃X�N���[���X�s�[�h
    [SerializeField] private float limitPosition = 730f; //�e�L�X�g�̐����ʒu
    [SerializeField] private GameObject LastText;//�Ō�ɕ\������e�L�X�g
    private bool isStopEndRoll; //�G���h���[�����I���������ǂ���

    void Update()
    {
        if (transform.position.y <= limitPosition) //�G���h���[���p�e�L�X�g�����~�b�g���z����܂œ�����
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + textScrollSpeed * Time.deltaTime);
        }
        else
        {
            LastText.SetActive(true);
            isStopEndRoll = true;
        }
    }
}
