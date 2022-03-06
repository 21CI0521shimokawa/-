using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    //public static float _TimeSpeed = 1.0f;

    public float _SlowdownFactor = 0.05f; //スローモーションの速度（正常時は1）
    public float _SlowTimeDuration = 0.2f; //正常速度までの回復時間
    public float _CountDown = 5f;

    public bool _IsNormalSpeed = true;

    public Text _Text;
    public Image _Time;

    float m_timer;

    [Header("デバッグ用")]
    [SerializeField] float deltime;
    [SerializeField] float fixeddeltime;
    [SerializeField] float timescale;

    void Start()
    {
        m_timer = _CountDown;
    }

    void Update()
    {
        deltime = Time.deltaTime;
        fixeddeltime = Time.fixedDeltaTime;
        timescale = Time.timeScale;

        _Time.fillAmount = m_timer / _CountDown;
        if (_IsNormalSpeed)
        {
            m_timer = _CountDown;
            _Text.text = "Normal";
            Time.timeScale += (1f / _SlowTimeDuration) * Time.unscaledDeltaTime;
            Time.fixedDeltaTime += (0.01f / _SlowTimeDuration) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f); //フレーム更新速度は0~1
            Time.fixedDeltaTime = Mathf.Clamp(Time.fixedDeltaTime, 0f, 0.01f); //通常時の物理演算は60フレーム(固定値)、フレームの時間間隔は(1/60)s
        }
        else
            SlowMotionCountDown();
    }

    public void DoSlowMotion()
    {
        _Text.text = "Slow Motion";
        Time.timeScale = _SlowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f; //物理演算の時間調整
    }

    void SlowMotionCountDown()
    {
        m_timer -= Time.unscaledDeltaTime;
        if(m_timer <= 0)
        {
            _IsNormalSpeed = true;
            m_timer = _CountDown;
        }
    }

    //public void DoNormalMotion()
    //{
    //    Time.timeScale = 1.0f;
    //    Time.fixedDeltaTime = Time.timeScale * .02f;
    //}
}
