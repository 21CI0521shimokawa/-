using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    //public static float _TimeSpeed = 1.0f;

    public float _SlowdownFactor = 0.05f; //�X���[���[�V�����̑��x�i���펞��1�j
    public float _SlowTimeDuration = 0.2f; //���푬�x�܂ł̉񕜎���

    public bool _IsNormalSpeed = true;

    [Header("�f�o�b�O�p")]
    public float deltime;
    public float fixeddeltime;
    public float timescale;

    public Text _Text;


    void Update()
    {
        deltime = Time.deltaTime;
        fixeddeltime = Time.fixedDeltaTime;
        timescale = Time.timeScale;


        if (_IsNormalSpeed)
        {
            _Text.text = "Normal";
            Time.timeScale += (1f / _SlowTimeDuration) * Time.unscaledDeltaTime;
            Time.fixedDeltaTime += (0.01f / _SlowTimeDuration) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f); //�t���[���X�V���x��0~1
            Time.fixedDeltaTime = Mathf.Clamp(Time.fixedDeltaTime, 0f, 0.01f); //�ʏ펞�̕������Z��60�t���[��(�Œ�l)�A�t���[���̎��ԊԊu��(1/60)s
        }
    }


    public void DoSlowMotion()
    {
        _Text.text = "Slow Motion"; 
        Time.timeScale = _SlowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f; //�������Z�̎��Ԓ���
    }

    //public void DoNormalMotion()
    //{
    //    Time.timeScale = 1.0f;
    //    Time.fixedDeltaTime = Time.timeScale * .02f;
    //}
}
