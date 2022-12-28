using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLogoStay : MonoBehaviour
{
    [SerializeField] GameObject startLogo; //PressAnyKey�C���[�W�摜
    [SerializeField] GameObject timeLine; //�I�[�v�j���O���[�r�[�𗬂��Ă���I�u�W�F�N�g
    [SerializeField] AudioClip startSE; //�Q�[���X�^�[�gSE
    [SerializeField] float stayTime; //�X���C�����ړ���������܂ł̎��Ԃ�ێ�����ϐ�

    /// <summary>
    /// �Q�[�����n�܂鎞�Ɉ�x����Start�֐�����Ă΂��֐�
    /// </summary>
    private void Awake()
    {
        ObjectDorw(startLogo, false);
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
        if (timeLine.activeSelf&&StartLogo.activeSelf)
        {
            PlayAudio.PlaySE(startSE);
            ObjectDorw(startLogo, false);
        }
    }

    /// <summary>
    /// �X���C�����ړ�����������^�C�g�����S�������֐�
    /// </summary>
    /// <returns></returns>
    private IEnumerator Stay()
    {
        yield return new WaitForSeconds(stayTime);//�X���C�������������܂ő҂�
        ObjectDorw(startLogo, true);
    }

    /// <summary>
    /// �I�u�W�F�N�g�`�攻��
    /// </summary>
    /// <param name="subjectObject"></param>
    /// <param name="isDrow"></param>
    private void ObjectDorw(GameObject subjectObject, bool isDrow) 
    {
        switch (isDrow)
        {
            case true:
                subjectObject.SetActive(true);
                break;
            case false:
                subjectObject.SetActive(false);
                break;
        }
    }
}
