using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    //public static float _TimeSpeed = 1.0f;

    public float _SlowdownFactor = 0.05f; //スローモーションの速度（正常時は1）
    public float _SlowTimeDuration = 0.2f; //正常速度までの回復時間

    public bool _IsNormalSpeed = true;

    [Header("デバッグ用")]
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
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f); //フレーム更新速度は0~1
            Time.fixedDeltaTime = Mathf.Clamp(Time.fixedDeltaTime, 0f, 0.01f); //通常時の物理演算は60フレーム(固定値)、フレームの時間間隔は(1/60)s
        }
    }


    public void DoSlowMotion()
    {
        _Text.text = "Slow Motion"; 
        Time.timeScale = _SlowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f; //物理演算の時間調整
    }

    //public void DoNormalMotion()
    //{
    //    Time.timeScale = 1.0f;
    //    Time.fixedDeltaTime = Time.timeScale * .02f;
    //}
}
