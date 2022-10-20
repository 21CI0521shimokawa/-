using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using System.Linq;

public class AutoDoorControll : MonoBehaviour
{
    [SerializeField, Tooltip("ドアの開閉スピード")] float Speed;
    [SerializeField, Tooltip("移動量"),] Vector3 MoveTo = new Vector3(0, 4);
    [SerializeField, Tooltip("ドアの移動時間")] float AutoDoorMoveTime;
    [SerializeField, Tooltip("ドア開閉SE")] AudioClip DoorSE;
    [SerializeField, Tooltip("ドアの開閉アニメーション")] Animator AutoDoorAnimator;
    [SerializeField, Tooltip("移動イージング指定")] Ease EaseType;
    private const int DoTime = 1; //衝突した後の処理を一度だけ実行するための変数

    #region ステート管理
    private enum AutoDoorState { MOVE, WAIT };
    AutoDoorState NowState;
    #endregion

    /// <summary>
    /// ゲームが始まる時に一度だけStart関数より先呼ばれる関数
    /// </summary>
    private void Awake()
    {
        NowState = AutoDoorState.MOVE; //ステートをMOVEにしておく
    }

    /// <summary>
    /// ゲームが始まる時に一度だけ呼ばれる関数
    /// </summary>
    void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Select(collison => collison.tag) //衝突したオブジェクトのタグが
            .Where(tag => tag == "Slime") //"Slime"だったら
            .Take(DoTime) //一度だけ実行
            .Subscribe(collision =>
            {
                if (NowState == AutoDoorState.MOVE) //現在のステートがMOVEだったら実行
                {
                    this.transform.DOMoveY(MoveTo.y, AutoDoorMoveTime) //MoveTo.yの位置までAutoDoorMoveTimeの速さで移動
                        .OnStart(() =>
                        {
                            PlayAudio.PlaySE(DoorSE); //移動開始するときにDoorSEを再生
                            AutoDoorAnimator.SetBool("Start", true); //移動開始するときにStartアニメーションを再生
                        })
                        .OnComplete(() =>
                        {
                            AutoDoorAnimator.SetBool("Start", false); //移動が完了したらStartアニメーションを止める
                            NowState = AutoDoorState.WAIT; //移動が完了したらステートをWAITに変更
                        })
                    .SetEase(EaseType);
                }
            });
    }
}