using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLogoStay : MonoBehaviour
{
    [SerializeField] GameObject startLogo; //PressAnyKeyイメージ画像
    [SerializeField] GameObject timeLine; //オープニングムービーを流しているオブジェクト
    [SerializeField] AudioClip startSE; //ゲームスタートSE
    [SerializeField] float stayTime; //スライムが移動完了するまでの時間を保持する変数

    /// <summary>
    /// ゲームが始まる時に一度だけStart関数より先呼ばれる関数
    /// </summary>
    private void Awake()
    {
        ObjectDorw(startLogo, false);
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
        if (timeLine.activeSelf&&StartLogo.activeSelf)
        {
            PlayAudio.PlaySE(startSE);
            ObjectDorw(startLogo, false);
        }
    }

    /// <summary>
    /// スライムが移動完了したらタイトルロゴを消す関数
    /// </summary>
    /// <returns></returns>
    private IEnumerator Stay()
    {
        yield return new WaitForSeconds(stayTime);//スライムが落ち着くまで待つ
        ObjectDorw(startLogo, true);
    }

    /// <summary>
    /// オブジェクト描画判定
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
