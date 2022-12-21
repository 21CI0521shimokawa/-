using UnityEngine;
using SceneDefine;

public class EndlingSlime : MonoBehaviour
{
    [SerializeField, Tooltip("�X���C���̈ړ����x")] float moveSpeed;
    [SerializeField, Tooltip("�X���C���̈ړ��ĊJ������̑��x")] float resumeMoveSpeed;
    [SerializeField, Tooltip("�G���f�B���OBGM�֐��̎擾")] PlayBGM playBGM;
    [SerializeField, Tooltip("�V�[���ڍs���̎���")] float fadeTime;
    private bool isMove = true;
    private bool stopOnce;

    /// <summary>
    /// ���݂��Ă����疈�t���[���Ă΂��֐�
    /// </summary>
    void Update()
    {
        if (isMove)
        {
            //IsMove��true��������MoveSpeed�̑����ō��Ɉړ�
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        }
            //StopOnce��false�ŃX���C�������̈ʒu�܂œ���������
        if (!stopOnce && transform.position.x <= -1)
        {
            //StopOnce��true�ɕύX
            stopOnce = true;
            //IsMove��false�ɂ��ăX���C���̓������~�߂�
            isMove = false;
        }
    }

    /// <summary>
    /// �A�j���[�V�����C�x���g�ŎQ�Ƃ����ړ��ĊJ�֐�
    /// </summary>
    public void ResumeMove()
    {
        isMove = true;
        moveSpeed = resumeMoveSpeed;
    }

    /// <summary>
    /// �A�j���[�V�����C�x���g�ŎQ�Ƃ����^�C�g���ɖ߂�֐�
    /// </summary>
    public void ToTitle()
    {
        SceneManagement.LoadNextScene(fadeTime);
        playBGM._FadeOutStart();
    }
}
