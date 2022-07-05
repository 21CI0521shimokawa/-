using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndrollControll : MonoBehaviour
{
    //�@�e�L�X�g�̃X�N���[���X�s�[�h
    [SerializeField]
    private float textScrollSpeed = 30;
    //�@�e�L�X�g�̐����ʒu
    [SerializeField]
    private float limitPosition = 730f;
    //�@�G���h���[�����I���������ǂ���
    private bool isStopEndRoll;
    //�@�V�[���ړ��p�R���[�`��
    private Coroutine endRollCoroutine;

    //�Ō�ɕ\������e�L�X�g
    [SerializeField]
    private GameObject LastText;
    // Update is called once per frame
    void Update()
    {

        //�@�G���h���[���p�e�L�X�g�����~�b�g���z����܂œ�����
        if (transform.position.y <= limitPosition)
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
