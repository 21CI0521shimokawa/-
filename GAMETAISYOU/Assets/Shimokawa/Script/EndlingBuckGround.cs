using UnityEngine;

public class EndlingBuckGround : MonoBehaviour
{
    [SerializeField, Tooltip("背景オブジェクトの移動スピード")] float speed;
    [SerializeField, Tooltip("折り返し地点設定")] float limitPosition;
    [SerializeField, Tooltip("リスタート地点設定")] Vector2 restartPosition;

    /// <summary>
    /// 存在していたら毎フレーム呼ばれる関数
    /// </summary>
    void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
        if (transform.position.x > limitPosition)
        {
            transform.position = restartPosition;
        }
    }
}
