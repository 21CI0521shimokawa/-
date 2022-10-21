using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    [SerializeField] PlayBGM PlayBGM;
    [SerializeField] float FadeTime;

    /// <summary>
    /// "Slime"�̃^�O�����Ă���I�u�W�F�N�g�ɏՓ˂�����
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Slime")
        {
            FadeManager.Instance.LoadScene("GameClear", FadeTime); //GameClear�V�[���Ɉڍs
            PlayBGM._FadeOutStart(); //BGM���t�F�[�h�A�E�g������
            collision.gameObject.GetComponent<SlimeController>()._ifOperation = false; //�S�[���n�_�ɓ��B������X���C���̍s�����~�߂�
        }
    }
}
