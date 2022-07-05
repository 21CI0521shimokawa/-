using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EndlingBuckGround : MonoBehaviour
{
    #region SerializeField
    [SerializeField, Tooltip("背景オブジェクトの移動スピード")] float Speed;
    [SerializeField, Tooltip("折り返し地点設定")] float LimitPosition;
    [SerializeField, Tooltip("リスタート地点設定")] Vector2 RestartPosition;
    #endregion

    void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                transform.Translate(Speed * Time.deltaTime, 0, 0);
                if (transform.position.x > LimitPosition)
                {
                    transform.position = RestartPosition;
                }

            });
    }
}
