using UnityEngine;
using SceneDefine;

public class EndlingSlime : MonoBehaviour
{
    [SerializeField, Tooltip("スライムの移動速度")] float moveSpeed;
    [SerializeField, Tooltip("スライムの移動再開した後の速度")] float resumeMoveSpeed;
    [SerializeField, Tooltip("エンディングBGM関数の取得")] PlayBGM playBGM;
    [SerializeField, Tooltip("シーン移行時の時間")] float fadeTime;
    private bool isMove = true;
    private bool stopOnce;

    /// <summary>
    /// 存在していたら毎フレーム呼ばれる関数
    /// </summary>
    void Update()
    {
        if (isMove)
        {
            //IsMoveがtrueだったらMoveSpeedの速さで左に移動
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        }
            //StopOnceがfalseでスライムが一定の位置まで到着したら
        if (!stopOnce && transform.position.x <= -1)
        {
            //StopOnceをtrueに変更
            stopOnce = true;
            //IsMoveをfalseにしてスライムの動きを止める
            isMove = false;
        }
    }

    /// <summary>
    /// アニメーションイベントで参照される移動再開関数
    /// </summary>
    public void ResumeMove()
    {
        isMove = true;
        moveSpeed = resumeMoveSpeed;
    }

    /// <summary>
    /// アニメーションイベントで参照されるタイトルに戻る関数
    /// </summary>
    public void ToTitle()
    {
        SceneManagement.LoadNextScene(fadeTime);
        playBGM._FadeOutStart();
    }
}
