using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLogoStay : MonoBehaviour
{
    [SerializeField] GameObject StartLogo; //PressAnyKeyイメージ画像
    [SerializeField] GameObject TimeLine; //オープニングムービーを流しているオブジェクト
    [SerializeField] AudioClip StartSE; //ゲームスタートSE
    [SerializeField] float StayTime; //スライムが移動完了するまでの時間を保持する変数

    /// <summary>
    /// ゲームが始まる時に一度だけStart関数より先呼ばれる関数
    /// </summary>
    private void Awake()
    {
        ObjectDorw(StartLogo, false);
    }

    /// <summary>
    /// ゲームが始まる時に一度だけ呼ばれる関数
    /// </summary>
    void Start()
    {
        StartCoroutine(Stay()); //コルーチン呼び出し
    }

    /// <summary>
    /// 毎フレーム呼ばれる関数
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
    /// スライムが移動完了したらタイトルロゴを消す関数
    /// </summary>
    /// <returns></returns>
    private IEnumerator Stay()
    {
        yield return new WaitForSeconds(StayTime);//スライムが落ち着くまで待つ
        ObjectDorw(StartLogo, true);
    }

    /// <summary>
    /// オブジェクト描画判定
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
