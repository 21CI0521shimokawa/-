using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLogoStay : MonoBehaviour
{
    [SerializeField] GameObject StartLogo; //PressAnyKeyイメージ画像
    [SerializeField] GameObject TimeLine; //オープニングムービーを流しているオブジェクト
    [SerializeField] AudioClip StartSE; //ゲームスタートSE
    [SerializeField] float StayTime; //スライムが移動完了するまでの時間を保持する変数
    
    private void Awake()
    {
        ObjectDorw(StartLogo, false);
    }

    void Start()
    {
        StartCoroutine(Stay()); //コルーチン呼び出し
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
        yield return new WaitForSeconds(StayTime);//スライムが落ち着くまで待つ
        ObjectDorw(StartLogo, true);
    }

    private void ObjectDorw(GameObject SubjectObject, bool IsDrow) //オブジェクト描画判定
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
