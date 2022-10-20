using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EndlingSlime : MonoBehaviour
{
    [SerializeField,Tooltip("スライムの移動速度")]float MoveSpeed;
    [SerializeField, Tooltip("エンディングBGM関数の取得")] PlayBGM PlayBGM;
    private bool IsMove = true;
    private bool StopOnce;

    /// <summary>
    /// ゲームが始まる時に一度だけ呼ばれる関数
    /// </summary>
    void Start()
    {
        this.UpdateAsObservable()
           .Subscribe(_ =>
           {
               if (IsMove)
               {
                   transform.Translate(MoveSpeed * Time.deltaTime, 0, 0); //IsMoveがtrueだったらMoveSpeedの速さで左に移動
               }
               if (!StopOnce&&transform.position.x <= -1) //StopOnceがfalseでスライムが一定の位置まで到着したら
               {
                   StopOnce = true; //StopOnceをtrueに変更
                   IsMove = false; //IsMoveをfalseにしてスライムの動きを止める
               }
           });
    }

    /// <summary>
    /// アニメーションイベントで参照される移動再開関数
    /// </summary>
    public void Move()
    {
        IsMove = true;
        MoveSpeed = -1.2f;
    }

    /// <summary>
    /// アニメーションイベントで参照されるタイトルに戻る関数
    /// </summary>
    public void ToTitle()
    {
        FadeManager.Instance.LoadScene("Title", 4f);
        PlayBGM._FadeOutStart();
    }
}
