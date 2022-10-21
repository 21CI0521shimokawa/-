using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLogoStay : MonoBehaviour
{
    [SerializeField] GameObject StartLogo; //PressAnyKey�C���[�W�摜
    [SerializeField] GameObject TimeLine; //�I�[�v�j���O���[�r�[�𗬂��Ă���I�u�W�F�N�g
    [SerializeField] AudioClip StartSE; //�Q�[���X�^�[�gSE
    [SerializeField] float StayTime; //�X���C�����ړ���������܂ł̎��Ԃ�ێ�����ϐ�
    
    private void Awake()
    {
        ObjectDorw(StartLogo, false);
    }

    void Start()
    {
        StartCoroutine(Stay()); //�R���[�`���Ăяo��
    }

    private void Update()
    {
        if (TimeLine.activeSelf&&StartLogo.activeSelf)
        {
            PlayAudio.PlaySE(StartSE);
            ObjectDorw(StartLogo, false);
        }
    }

    private IEnumerator Stay()
    {
        yield return new WaitForSeconds(StayTime);//�X���C�������������܂ő҂�
        ObjectDorw(StartLogo, true);
    }

    private void ObjectDorw(GameObject SubjectObject, bool IsDrow) //�I�u�W�F�N�g�`�攻��
    {
        switch (IsDrow)
        {
            case true:
                SubjectObject.SetActive(true);
                break;
            case false:
                SubjectObject.SetActive(false);
                break;
        }
    }
}
