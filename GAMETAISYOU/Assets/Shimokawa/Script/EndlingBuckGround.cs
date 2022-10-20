using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EndlingBuckGround : MonoBehaviour
{
    [SerializeField, Tooltip("背景オブジェクトの移動スピード")] float MoveSpeed;
    [SerializeField, Tooltip("折り返し地点設定")] float LimitPosition;
    [SerializeField, Tooltip("リスタート地点設定")] Vector2 RestartPosition;

    void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                transform.Translate(MoveSpeed * Time.deltaTime, 0, 0); //左にSpeedの速さで移動
                if (transform.position.x > LimitPosition)
                {
                    transform.position = RestartPosition; //一定の位置になったら最初の位置に戻る
                }

            });
    }
}
