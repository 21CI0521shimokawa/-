using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLogoStay : MonoBehaviour
{
    [SerializeField] GameObject StartLogo; //PressAnyKey�C���[�W�摜
    [SerializeField] GameObject TimeLine; //�I�[�v�j���O���[�r�[�𗬂��Ă���I�u�W�F�N�g
    [SerializeField] AudioClip StartSE; //�Q�[���X�^�[�gSE
    [SerializeField] float StayTime; //�X���C�����ړ���������܂ł̎��Ԃ�ێ�����ϐ�

    /// <summary>
    /// �Q�[�����n�܂鎞�Ɉ�x����Start�֐�����Ă΂��֐�
    /// </summary>
    private void Awake()
    {
        ObjectDorw(StartLogo, false);
    }

    /// <summary>
    /// �Q�[�����n�܂鎞�Ɉ�x�����Ă΂��֐�
    /// </summary>
    void Start()
    {
        StartCoroutine(Stay()); //�R���[�`���Ăяo��
    }

    /// <summary>
    /// ���t���[���Ă΂��֐�
    /// </summary>
    private void Update()
    {
        if (TimeLine.activeSelf&&StartLogo.activeSelf)
        {
            PlayAudio.PlaySE(StartSE);
            ObjectDorw(StartLogo, false);
        }
    }

    /// <summary>
    /// �X���C�����ړ�����������^�C�g�����S�������֐�
    /// </summary>
    /// <returns></returns>
    private IEnumerator Stay()
    {
        yield return new WaitForSeconds(StayTime);//�X���C�������������܂ő҂�
        ObjectDorw(StartLogo, true);
    }

    /// <summary>
    /// �I�u�W�F�N�g�`�攻��
    /// </summary>
    /// <param name="SubjectObject"></param>
    /// <param name="IsDrow"></param>
    private void ObjectDorw(GameObject SubjectObject, bool IsDrow) 
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
